using System.Diagnostics;
using UnityEngine;

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
}
