using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// CustomEditor(type) 으로 해당 타입의 컴포넌트나 스크립터블 오브젝트 등을 인스펙터에 그릴 커스텀 에디터임을 표시한다.
[CustomEditor(typeof(C_ComponentExample))]
public class C01_InspectorGUI : Editor {
    private C_ComponentExample targetRef;

    private SerializedProperty myObject;
    private SerializedProperty myString;
    private SerializedProperty myInteger;
    
    // 오브젝트가 인스펙터에 표시될 때 OnEnable이 호출된다. (다른 오브젝트를 클릭했다가 이 오브젝트를 클릭해도, 클릭할 때마다 호출됨)
    private void OnEnable() {
        // Editor.target 으로 에디터의 대상 오브젝트를 Object 타입으로 받을 수 있다.
        targetRef = (C_ComponentExample)target;
        
        // 직렬화된 target은 SerializedObject 타입으로 저장된다.
        // 이 오브젝트엔 직렬화가 가능한 public 변수와 [SerializeField] 속성이 붙은 변수가 저장되며, 이름을 통해 SerializedProperty 타입으로 가져올 수 있다.
        
        // 레퍼런스의 변수명이 바뀔수도 있으니 nameof로 찾아오는게 일반적이지만, public이 아닌 변수는 그냥 문자열로 찾는다. 
        myObject = serializedObject.FindProperty(nameof(targetRef.myObject));
        myString = serializedObject.FindProperty(nameof(C_ComponentExample.myString));
        myInteger = serializedObject.FindProperty("myInt");
    }
    
    // 인스펙터 창에 표시되는 동안 실행된다. OnGUI와 마찬가지로 다시 그릴 필요가 있을 때만 호출된다.
    public override void OnInspectorGUI() {
        GUILayout.Label("기존 인스펙터 GUI");
        base.OnInspectorGUI(); // 커스텀 에디터가 없을 때 인스펙터에 표시되는 내용들
        
        GUILayout.Space(20);
        GUILayout.Label("커스텀 인스펙터 GUI");
        
        // 원본 오브젝트를 직렬화하여 serializedObject에 값을 업데이트한다. 값이 외부에서 바뀌었을 수 있으니 수정 전에 호출해준다.
        serializedObject.Update();
        
        // PropertyField()는 타입에 따라 인스펙터에 기본적으로 표시되는 내용으로 그린다. (변수명과 그 값을 표기하고, 수정될 경우 원본 오브젝트에 적용)
        EditorGUILayout.PropertyField(myObject);
        EditorGUILayout.PropertyField(myInteger);
        
        // 수정하고 싶은 값은 PropertyField 대신 아래와 같이 커스텀할 수 있다. 
        using (new EditorGUILayout.HorizontalScope()) {
            GUI.color = Color.cyan;
            EditorGUILayout.PrefixLabel("문자열 변수 이름");
            // SerializedProperty.stringValue을 통해 해당 변수의 값을 읽어오거나 저장할 수 있다.
            // intValue, floatValue, vector3Value 등 직렬화 가능한 다양한 타입을 지원한다.
            myString.stringValue = EditorGUILayout.TextArea(myString.stringValue);
        }

        if (myInteger.intValue < 30) GUI.color = Color.red;
        else if (myInteger.intValue < 70) GUI.color = Color.yellow;
        else GUI.color = Color.green;
        
        myInteger.intValue = EditorGUILayout.IntSlider(myInteger.intValue, 0, 100);
        
        GUI.color = Color.white;
        
        // 수정된 내용은 ApplyModifiedProperties()를 호출해야 원본 오브젝트에 저장된다.
        serializedObject.ApplyModifiedProperties();
    }
}
