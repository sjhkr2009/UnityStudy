using UnityEditor;
using System.IO;

public static class ScriptCreator {
    private const string EmptyInterfaceTemplate =
        @"public interface #SCRIPTNAME# {
    
}";
    private const string EmptyScriptTemplate =
        @"using System;
using System.Collections.Generic;

public class #SCRIPTNAME# {
    
}";
    private const string EmptyMonoBehaviorTemplate =
        @"using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Define;

public class #SCRIPTNAME# : MonoBehaviour {
    
}";
    
    private static string scriptPath;

    [MenuItem("Assets/Create/C# Interface", false, 64)]
    static void CreateEmptyInterface() => CreateScript(EmptyInterfaceTemplate, "IInterface.cs");
    
    [MenuItem("Assets/Create/C# Script (Empty)", false, 65)]
    static void CreateEmptyScript() => CreateScript(EmptyScriptTemplate, "MyScript.cs");
    
    [MenuItem("Assets/Create/C# Script (MonoBehaviour)", false, 66)]
    static void CreateEmptyMonoBehaviour() => CreateScript(EmptyMonoBehaviorTemplate, "MyBehaviour.cs");

    static void CreateScript(string template, string defaultFileName) {
        if (string.IsNullOrEmpty(scriptPath) || !File.Exists(scriptPath)) scriptPath = FileUtil.GetUniqueTempPathInProject();
        
        File.WriteAllText(scriptPath, template);
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(scriptPath, defaultFileName);
    }
}