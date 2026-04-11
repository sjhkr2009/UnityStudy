using System;
using System.Collections.Generic;
using System.Reflection;
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
    
    public static FieldResult TryAssignField(MonoBehaviour component, FieldInfo field, Transform root, string targetPath, out Transform foundTransform)
    {
        foundTransform = FindByPathSuffix(root, targetPath);
        if (foundTransform == null)
            return FieldResult.PathNotFound;

        var fieldType = field.FieldType;

        if (fieldType == typeof(GameObject))
        {
            field.SetValue(component, foundTransform.gameObject);
            return FieldResult.Assigned;
        }

        if (fieldType == typeof(Transform))
        {
            field.SetValue(component, foundTransform);
            return FieldResult.Assigned;
        }

        if (typeof(Component).IsAssignableFrom(fieldType))
        {
            var targetComponent = foundTransform.GetComponent(fieldType);
            if (targetComponent != null)
            {
                field.SetValue(component, targetComponent);
                return FieldResult.Assigned;
            }

            return FieldResult.ComponentNotFound;
        }

        return FieldResult.UnsupportedType;
    }
}

