using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class C11_Interface : MonoBehaviour
{
    // 11. 인터페이스
    
    // 상속받는 클래스에게 특정 동작을 강요하는 방법


    //1. 추상 클래스와 함수

    // 기존의 상속 구조에서, 자식 클래스가 부모 클래스의 특정 함수를 깜빡하고 오버라이드하지 않아도 문법상 문제가 없다.
    // abstract로 추상화하면 대상을 상속하는 곳에서 오버라이드를 강제할 수 있다.

    //그러나 C++에선 다중 상속을 지원하지만, C#에서는 하나의 클래스만 상속 가능하다.
    //이는 클래스 A를 두 자식 B,C가 물려받고, 다른 클래스 D가 B,C를 동시에 상속받을 경우, A에 있는 함수를 B,C가 모두 가지고 있어서 D가 어떤 것을 실행(또는 오버라이드)해야 할 지 모르는 경우 때문.
    //(속칭 죽음의 다이아몬드 문제)

    [Button]
    void Test01()
    {
        C11_Ork ork = new C11_Ork();
        ork.Shout();
    }

    //--------------------------------------------------------------------------------




    //2. 인터페이스

    // 추상 클래스와 달리 어떤 함수도 본문을 선언할 수 없지만, 다중 상속이 가능하다.
    // 구현해야 할 요소를 선언하기만 하고, 본문은 전적으로 상속받는 곳에서 정의한다.

    [Button]
    void Test02()
    {
        C11_Ork ork = new C11_Ork();
        ork.Move();
    }

    // 이는 상속 대상이 공통된 이름의 함수를 갖는 것을 이용하여, 다음과 같이 응용할 수 있다.
    [Button]
    void Test03()
    {
        C11_Ork ork = new C11_Ork();
        C11_Player player = new C11_Player();
        DoMove(ork);
        DoMove(player);
    }
    //IMoveable 인터페이스를 통해 움직임을 명령하는 함수를 만들면, 움직일 수 있는 모든 개체의 이동 명령을 이 함수를 통해 제어할 수 있다.
    void DoMove(C11_Moveable moveable)
    {
        Debug.Log("이동 인터페이스 동작...");
        moveable.Move();
    }
}

abstract class C11_Monster //추상 클래스는 new로 생성할 수 없다. 즉 상속 전용이다.
{
    public abstract void Shout(); //추상 함수는 내용을 선언할 수 없다. 즉 상속받는 곳에서 오버라이드하여 본문을 선언해야 한다. (그렇지 않으면 에러)
    public virtual void Dead() { }
}
class C11_Ork : C11_Monster, C11_Moveable
{
    public void Move()
    {
        Debug.Log("오크 이동");
    }

    public override void Shout() //abstract 함수는 반드시 본문을 정의해야 한다.
    {
        Debug.Log("우어어어");
    }

}
class C11_Player : C11_Moveable
{
    public void Move()
    {
        Debug.Log("플레이어 이동");
    }
}
interface C11_Moveable // 인터페이스는 주로 약자인 I를 앞에 붙여준다. (IList, ICollection 등)
{
    void Move(); //인터페이스는 본문을 정의할 수 없고, 함수를 선언하기만 한다.
}