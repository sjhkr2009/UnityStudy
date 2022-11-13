using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C01_Example : MonoBehaviour {
    // SerializedObject를 이용하여 오브젝트 커스텀 에디팅을 위한 예시 컴포넌트 (C01~C03 에서 사용)
    public GameObject myObject;
    public string myString;
    [SerializeField] private int myInt;

    public C_ClassExample myClass;

    private void OnEnable() {
        // 참고: 모든 UnityEngine.Object는 hideFlags를 가지고 있다. DontSave로 지정 시 인스펙터에서 수정 중에 게임 플레이/정지해도 언로드되지 않는다.
        // 그 외에 하이어라키에서 숨기거나 수정불가하게 하는 등의 조작이 가능하다. 기본값은 None.
        gameObject.hideFlags = HideFlags.DontSave;
    }
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
