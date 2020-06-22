using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour
{
    // 자식 클래스인 오크나 고블린을 BaseMonster로 불러올 수 있다.
    // AnimalTest.cs 의 다형성 항목 참고.

    public BaseMonster[] monsters; //각 몬스터들을 상위 클래스로서 한번에 불러올 수 있다. 즉 여러 몬스터들을 하나의 변수로 관리할 수 있다.

    void Start()
    {
        for(int i = 0; i < monsters.Length; i++)
        {
            Debug.Log(monsters[i].gameObject.name); //불러온 부모 스크립트를 가진 오브젝트의 이름을 출력.
        }
    }
}
