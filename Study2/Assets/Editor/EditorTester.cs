using System.Collections;
using System.Collections.Generic;
using UnityEditor; //에셋 내 Editor 폴더(폴더명 준수, 다른 폴더 내에 있는건 상관없으니 Editor 폴더면 됨)에 스크립트를 만들고, using.UnityEditor 추가해주기
using UnityEngine;

[CustomEditor((typeof(Tester)))] //Tester 스크립트에 적용되는 에디터

public class EditorTester : Editor
{
    private SerializedProperty moveSpeedProperty;
    private void OnEnable()
    {
        moveSpeedProperty = serializedObject.FindProperty("moveSpeed");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); //기본 에디터
        serializedObject.Update();

        if (GUILayout.Button("Button")) //버튼과 버튼명
        {
            Tester tester = serializedObject.targetObject as Tester;
            //tester.Function(); //public 함수 버튼으로 실행 가능
        }

        serializedObject.ApplyModifiedProperties();//적용하기
    }
}
