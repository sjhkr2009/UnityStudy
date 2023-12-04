using System;
using System.Collections.Generic;

namespace Server.Game.Utility; 

public static class Extensions {
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
        if (enumerable == null || action == null) return null;
        
        foreach (var element in enumerable) {
            action.Invoke(element);
        }

        return enumerable;
    }
}