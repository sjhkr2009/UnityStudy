using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C01_ComponentExample : MonoBehaviour {
    // C01_InspectorGUI.cs 에서 인스펙터를 커스텀하기 위한 예시 컴포넌트
    public GameObject myObject;
    public string myString;
    [SerializeField] private int myInt;
}
