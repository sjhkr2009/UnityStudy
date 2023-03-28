using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
        var component = gameObject.GetComponent<T>();
        if (component) return component;

        return gameObject.AddComponent<T>();
    }
}
