using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : MonoBehaviour // 시작 부분에서 abstract class로 정의한다.
{
    // 추상 클래스: 내용도, 함수 껍데기도 담을 수 있는, 클래스와 인터페이스의 성격을 모두 가진 것.
    // 클래스와 달리 함수 이름만 적어두고 내용은 자식 클래스가 채우게 할 수 있고,
    // 인터페이스와 달리 변수나 함수의 내용도 담을 수 있다. 


    //여기서는 공격을 위한 기본적인 기능을 담아, 몬스터들이 이 스크립트를 상속받아 쓸 수 있게 한다. // -> 클래스의 성격
    //단, Attack() 함수를 각 몬스터들이 무조건 가지게 하되, 그 내용은 각자 정해서 쓰도록 만든다. // -> 인터페이스의 성격
    

    //내용을 담는 부분
    public float damage = 100f;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    //내용을 담지 않을 부분
    public abstract void Attack(); //정의하지 않은 함수 껍데기는 추상 클래스로 선언하고, 내용을 쓰지 않는다.
    //내용은 자식 클래스가 override 해서 만들면 된다.
}
