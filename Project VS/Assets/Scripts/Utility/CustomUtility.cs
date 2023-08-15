using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CustomUtility {
    public static T Clamp<T>(this T value, T min, T max) where T : IComparable {
        if (value.CompareTo(min) < 0) return min;
        if (value.CompareTo(max) > 0) return max;
        return value;
    }
    
    public static T ClampMax<T>(this T value, T max) where T : IComparable {
        if (value.CompareTo(max) > 0) return max;
        return value;
    }
    
    public static T ClampMin<T>(this T value, T min) where T : IComparable {
        if (value.CompareTo(min) < 0) return min;
        return value;
    }
    
    public static Vector2 GetRandomVector(float min, float max) {
        return new Vector2(Random.Range(min, max), Random.Range(min, max));
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
        var component = gameObject.GetComponent<T>();
        if (component) return component;

        return gameObject.AddComponent<T>();
    }

    public static Transform ResetTransform(this Transform transform) {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        return transform;
    }

    public static void ForEach<T>(this IEnumerable<T> container, Action<T> action) {
        foreach (var element in container) {
            action?.Invoke(element);
        }
    }

    public static T PickRandom<T>(this IEnumerable<T> container) {
        var count = container.Count();
        if (count <= 0) return default;
        
        var index = Random.Range(0, count);
        return container.ElementAt(index);
    }
    
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection) {
        return collection.OrderBy(_ => Random.value);
    }
}
