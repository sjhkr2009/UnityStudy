using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TestObject", menuName ="TestObject", order =0)]
public class TestObject : ScriptableObject
{
    public int n;

    // 프로젝트상에 생성된 오브젝트로, 씬 상에 존재하는 오브젝트와 달리 씬이 바뀌어도 유지된다
}
