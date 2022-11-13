using UnityEngine;


public static class DownloadUtility {
    public enum SizeUnits {
        Byte, KB, MB, GB
    }
    
    public static bool IsNetworkValid() {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    public static bool IsDiskSpaceEnough(long requiredSize) {
        return Caching.defaultCache.spaceFree >= requiredSize;
    }

    private static long OneGB = 1000000000;
    private static long OneMB = 1000000;
    private static long OneKB = 1000;

    public static SizeUnits GetProperByteUnit(long byteSize) {
        if (byteSize >= OneGB)
            return SizeUnits.GB;
        if (byteSize >= OneMB)
            return SizeUnits.MB;
        if (byteSize >= OneKB)
            return SizeUnits.KB;
        return SizeUnits.Byte;
    }
    
    public static long ConvertByteByUnit(long byteSize, SizeUnits unit) {
        return (long)((byteSize / (double)System.Math.Pow(1024, (long)unit)));
    }
    
    public static string GetConvertedByteString(long byteSize, SizeUnits unit, bool appendUnit = true) {
        string unitStr = appendUnit ? unit.ToString() : string.Empty;
        return $"{ConvertByteByUnit(byteSize, unit).ToString("0.00")}{unitStr}";
    }
}
