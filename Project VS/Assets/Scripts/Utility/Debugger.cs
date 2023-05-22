using UnityEngine;

/** 유니티 Debug 클래스를 캡슐화한 것으로, 추후 외부 로그 전송이나 환경에 따라 필터링하는 등의 동작이 추가될 수 있음. */
public static class Debugger {
    // ReSharper disable Unity.PerformanceAnalysis
    public static void Log(object message) {
        Debug.Log(message);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public static void Warning(object message) {
        Debug.LogWarning(message);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public static void Error(object message) {
        Debug.LogError(message);
    }
}
