using System.Globalization;
using OpenQA.Selenium.DevTools.V85.Page;

namespace NaverFinance; 

public static class CustomExtension {
    public static int? ToInt32(this string numeric) {
        if (int.TryParse(numeric, NumberStyles.Any, CultureInfo.InvariantCulture, out var ret)) {
            return ret;
        }

        return null;
    }

    public static int ToInt32(this string numeric, int defaultValue)
        => numeric.ToInt32() ?? defaultValue;

    public static float? ToFloat(this string floatValue) {
        if (float.TryParse(floatValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var ret)) {
            return ret;
        }

        return null;
    }
    
    public static float ToFloat(this string floatValue, float defaultValue)
        => floatValue.ToFloat() ?? defaultValue;
}