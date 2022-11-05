using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventManager))]
public class EventManagerColorEditor : Editor {
    private static Color selectedColor;
    private static string inputText;
    
    const int ColorStartPos = 7; // "<color=".Length
    const int ColorCodeLength = 7; // "#000000".Length
    const int ColorPrefixLength = 15; // "<color=#000000>".Length
    const int ColorPostfixLength = 8; // "</color>.Length
    const int ColorTotalLength = ColorPrefixLength + ColorPostfixLength;
    
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        EditorGUILayout.LabelField("색상");
        selectedColor = EditorGUILayout.ColorField(selectedColor);
        if (GUILayout.Button("적용하기")) {
            GUIUtility.systemCopyBuffer = $"<color=#{ColorUtility.ToHtmlStringRGB(selectedColor)}></color>";
            if (TryToColoredString(inputText, out var content, out _)) {
                inputText = $"<color=#{ColorUtility.ToHtmlStringRGB(selectedColor)}>{content}</color>";
            } else {
                inputText = $"<color=#{ColorUtility.ToHtmlStringRGB(selectedColor)}>{inputText}</color>";
            }
        }
        EditorGUILayout.LabelField("텍스트");
        inputText = EditorGUILayout.TextArea(inputText);
        
        GUI.color = Color.green;
        if (TryToColoredString(inputText, out var inputTextRich, out var textColor)) {
            EditorGUILayout.LabelField($"Color Code: {ColorUtility.ToHtmlStringRGB(textColor)}");
            GUI.color = textColor;
            EditorGUILayout.TextArea($"Text: {inputTextRich}");
        } else {
            if (EditorGUIUtility.textFieldHasSelection) {
                // 얘는 GUILayout.TextArea 에서만 동작하고 EditorGUI 기반의 에디터에선 동작하지 않아서, 리플렉션을 사용하기로 함
                //TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                TextEditor editor = typeof(EditorGUI)
                    .GetField("activeEditor", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) as TextEditor;
                if(!string.IsNullOrEmpty(editor?.SelectedText)) EditorGUILayout.LabelField($"Selected: {editor.SelectedText}");
            }
            EditorGUILayout.LabelField($"Is Not Colored Text");
        }
        GUI.color = Color.white;
    }

    bool TryToColoredString(string htmlColoredString, out string content, out Color color) {
        content = htmlColoredString;
        color = Color.white;
        
        if (string.IsNullOrEmpty(htmlColoredString)) return false;
        
        var matchInfo = Regex.Match(htmlColoredString, @"(<color=#[0-9a-fA-F]{6}>)((.|\n)*?)(<[/]color>)");
        if (!matchInfo.Success) return false;
        
        var colorValue = matchInfo.Value.Substring(ColorStartPos, ColorCodeLength);
        if (!ColorUtility.TryParseHtmlString(colorValue, out color)) return false;

        content = matchInfo.Value.Substring(ColorPrefixLength, matchInfo.Value.Length - ColorTotalLength);
        return true;
    }
}
