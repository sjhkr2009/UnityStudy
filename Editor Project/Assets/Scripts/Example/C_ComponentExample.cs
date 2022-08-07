using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ComponentExample : MonoBehaviour {
    // SerializedObject를 이용하여 오브젝트 커스텀 에디팅을 위한 예시 컴포넌트 (C01~C04 에서 사용)
    public GameObject myObject;
    public string myString;
    [SerializeField] private int myInt;

    public C_ClassExample myClass;
}

[Serializable]
public class C_ClassExample {
    public enum Type {
        TypeA,
        TypeB
    }
    public int level;
    public string name;
    public float hp;
    public Type type;
}
