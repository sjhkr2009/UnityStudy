using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 유니티 Debug 클래스가 빌드에 포함되지 않도록 하기 위한 커스텀 디버깅 클래스
/// </summary>
public static class Debug
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object msg)
    {
        UnityEngine.Debug.Log(msg);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(object msg)
    {
        UnityEngine.Debug.LogError(msg);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object msg)
    {
        UnityEngine.Debug.LogWarning(msg);
    }
}
