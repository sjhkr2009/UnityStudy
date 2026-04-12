using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class AutoAssignAttribute : PropertyAttribute
{
    public string Path { get; }
    public bool Optional { get; }

    public AutoAssignAttribute() : this(string.Empty, false) { }
    public AutoAssignAttribute(string path, bool optional = false)
    {
        Path = path;
        Optional = optional;
    }
}
