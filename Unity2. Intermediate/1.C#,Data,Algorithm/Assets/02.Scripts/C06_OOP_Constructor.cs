using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class C06_OOP_Constructor : MonoBehaviour
{
    // 06. 생성자(Constructor) : 객체 지향 프로그래밍에서 쓰이는 객체 초기화 함수.

    //클래스는 new로 생성될 때 생성자 함수를 호출한다. 따로 생성자를 적지 않아도 기본적으로 내용이 빈 생성자 함수를 갖는다. C05의 Knight와, 생성자가 있는 하단의 Knight를 비교해보자.

    [Button]
    void Test()
    {
        C06_Knight knight = new C06_Knight();
        C06_Knight knight2 = new C06_Knight("마법사");
        C06_Knight knight3 = new C06_Knight(25);
    }
}

class C06_Knight
{
    public string name;
    public int hp;
    public int power;

    public C06_Knight()
    {
        hp = 100;
        power = 10;
        name = "기사";
        Debug.Log($"{name} 생성!");
    }

    public C06_Knight(string name)
    {
        this.name = "기사";
        Debug.Log($"실제로 {this.name} 이지만 {name}인 척하는 중");
    }

    public C06_Knight(int power) : this()
    {
        Debug.Log($"{name}의 공격력은 {power} 으로 표기되어 있습니다. 실제로는 {this.power} 입니다.");
    }

    public void Move()
    {
        Debug.Log($"{name} 이동");
    }
    public void Attack()
    {
        Debug.Log($"{name} 공격");
    }
}