using System;
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

    public static void ResetTransform(this Transform transform) {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }
}
