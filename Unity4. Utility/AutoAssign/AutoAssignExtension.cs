using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public static class AutoAssignExtension
{
    private class FieldEntry
    {
        public FieldInfo Field;
        public string SuffixPath;
        public string ResolvedPath;
        public bool Resolved;
    }

    private static readonly Dictionary<Type, FieldEntry[]> FieldCache = new();

    public static T AutoAssign<T>(this T component) where T : MonoBehaviour
    {
        component.CheckAutoAssign();
        return component;
    }

    public static void CheckAutoAssign(this MonoBehaviour component)
    {
        var entries = GetCachedFields(component.GetType());
        if (entries.Length == 0)
            return;

        var root = component.transform;

        foreach (var entry in entries)
        {
            var value = entry.Field.GetValue(component);
            if (value is Object obj && obj != null)
                continue;

            var resolved = Resolve(root, entry);
            if (resolved != null)
                entry.Field.SetValue(component, resolved);
        }
    }

    private static FieldEntry[] GetCachedFields(Type type)
    {
        if (FieldCache.TryGetValue(type, out var cached))
            return cached;

        var list = new List<FieldEntry>();
        var current = type;
        while (current != null && current != typeof(MonoBehaviour))
        {
            var fields = current.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var field in fields)
            {
                var att = field.GetCustomAttribute<AutoAssignAttribute>();
                if (att != null)
                    list.Add(new FieldEntry { Field = field, SuffixPath = att.Path });
            }
            current = current.BaseType;
        }

        var result = list.ToArray();
        FieldCache[type] = result;
        return result;
    }

    private static Object Resolve(Transform root, FieldEntry entry)
    {
        if (string.IsNullOrEmpty(entry.SuffixPath))
            return ResolveAt(root, entry.Field.FieldType);

        var target = FindTarget(root, entry);
        return ResolveAt(target, entry.Field.FieldType);
    }

    private static Object ResolveAt(Transform target, Type fieldType)
    {
        if (target == null)
            return null;

        if (fieldType == typeof(GameObject))
            return target.gameObject;

        if (fieldType == typeof(Transform))
            return target;

        if (typeof(Component).IsAssignableFrom(fieldType))
            return target.GetComponent(fieldType) ?? target.GetComponentInChildren(fieldType, true);

        return null;
    }

    private static Transform FindTarget(Transform root, FieldEntry entry)
    {
        if (entry.Resolved)
            return entry.ResolvedPath != null ? root.Find(entry.ResolvedPath) : null;

        entry.Resolved = true;

        var found = FindByPathSuffix(root, entry.SuffixPath);
        if (found == null)
            return null;

        entry.ResolvedPath = GetRelativePath(root, found);
        return found;
    }

    private static Transform FindByPathSuffix(Transform root, string path)
    {
        var segments = path.Split('/');

        foreach (var child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child == root)
                continue;

            if (MatchesPath(root, child, segments))
                return child;
        }

        return null;
    }

    private static bool MatchesPath(Transform root, Transform target, string[] segments)
    {
        var current = target;
        for (int i = segments.Length - 1; i >= 0; i--)
        {
            if (current == null || current == root)
                return false;

            if (current.name != segments[i])
                return false;

            current = current.parent;
        }
        return true;
    }

    private static string GetRelativePath(Transform root, Transform target)
    {
        var parts = new List<string>();
        var current = target;
        while (current != null && current != root)
        {
            parts.Add(current.name);
            current = current.parent;
        }
        parts.Reverse();
        return string.Join("/", parts);
    }
}
