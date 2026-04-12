using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class AutoAssignUtility
{
    public enum FieldResult
    {
        Assigned,
        PathNotFound,
        ComponentNotFound,
        UnsupportedType,
    }

    private static readonly List<string> _pathBuffer = new();
    public static string GetInnerPath(Transform root, Transform target)
    {
        _pathBuffer.Clear();
        var current = target;

        while (current != null && current != root)
        {
            _pathBuffer.Add(current.name);
            current = current.parent;
        }

        _pathBuffer.Reverse();
        var path = string.Join('/', _pathBuffer);
        _pathBuffer.Clear();

        return path;
    }

    public static bool MatchesPathSuffix(string fullPath, string expectedPath)
    {
        return fullPath == expectedPath
               || fullPath.EndsWith($"/{expectedPath}", StringComparison.Ordinal);
    }

    public static Transform FindByPathSuffix(Transform root, string path)
    {
        var allTransforms = root.GetComponentsInChildren<Transform>(true);

        foreach (var child in allTransforms)
        {
            if (child == root)
                continue;

            var fullPath = GetInnerPath(root, child);

            // 'Image2' 입력 시 '~~/Image2' 는 해당되지만 '~~/MyImage2' 는 해당하지 않아야 됨.
            if (MatchesPathSuffix(fullPath, path))
                return child;
        }

        return null;
    }

    public static Transform GetTransformFromValue(object value)
    {
        return value switch
        {
            GameObject go => go.transform,
            Component comp => comp.transform,
            _ => null
        };
    }

    public static IEnumerable<FieldInfo> GetAllFields(Type type)
    {
        var current = type;
        while (current != null && current != typeof(MonoBehaviour))
        {
            var fields = current.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var field in fields)
                yield return field;
            current = current.BaseType;
        }
    }

    public static FieldResult TryAssignField(MonoBehaviour component, FieldInfo field, string targetPath, out Transform foundTransform)
    {
        var root = component.transform;
        var isSelfAssign = string.IsNullOrEmpty(targetPath);
        foundTransform = isSelfAssign ? root : FindByPathSuffix(root, targetPath);

        if (foundTransform == null)
            return FieldResult.PathNotFound;

        var fieldType = field.FieldType;
        Object resolved = null;

        if (fieldType == typeof(GameObject))
        {
            resolved = foundTransform.gameObject;
        }
        else if (fieldType == typeof(Transform))
        {
            resolved = foundTransform;
        }
        else if (typeof(Component).IsAssignableFrom(fieldType))
        {
            resolved = foundTransform.GetComponent(fieldType)
                       ?? foundTransform.GetComponentInChildren(fieldType, true);
        }
        else
        {
            return FieldResult.UnsupportedType;
        }

        if (resolved == null)
            return FieldResult.ComponentNotFound;

        var so = new SerializedObject(component);
        var sp = so.FindProperty(field.Name);
        if (sp != null)
        {
            sp.objectReferenceValue = resolved;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
        else
        {
            field.SetValue(component, resolved);
        }

        return FieldResult.Assigned;
    }
}
