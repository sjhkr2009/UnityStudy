using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class AutoAssignAttribute : PropertyAttribute
{
    public string Path { get; }

    public AutoAssignAttribute(string path)
    {
        Path = path;
    }
}

