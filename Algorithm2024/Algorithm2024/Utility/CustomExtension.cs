namespace Algorithm2024; 

public static class CustomExtension {
    public static void ForEach<T>(this IEnumerable<T> container, Action<T> action) {
        foreach (var element in container) {
            action?.Invoke(element);
        }
    }

    public static int Clamp(this int num, int min, int max) {
        if (num < min) return min;
        if (num > max) return max;
        
        return num;
    }

    public static int ClampMin(this int num, int min) => num < min ? min : num;
    public static int ClampMax(this int num, int max) => num > max ? max : num;
}