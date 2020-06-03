using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class C07_OOP_Class : MonoBehaviour
{
    // 07. 클래스의 특징
    
    // 객체지향 프로그래밍의 대표적인 세 가지 특징은 '상속성, 다형성, 은닉성'이다.
    [Button]
    void Test01()
    {
        C07_1_Knight knight = new C07_1_Knight();
        knight.Move();

        C07_1_Mage mage = new C07_1_Mage();

        C07_1_CustomClass01 customClass1 = new C07_1_CustomClass01();
        C07_1_CustomClass01 customClass2 = new C07_1_CustomClass01("어둠 살수");
    }
    [Button]
    void Test02()
    {
        C07_3_Player player1 = new C07_3_Knight();
        C07_3_Player player2 = new C07_3_Archor();
        C07_3_Player player3 = new C07_3_Mage();
    }
    [Button]
    void Test03()
    {
        C07_3_Player player = GetPlayer(1);
        bool isKnight = player is C07_3_Knight;
        if (isKnight) Debug.Log("기사 생성");
        else if (player is C07_3_Archor) Debug.Log("궁수 생성");

        C07_3_Player player2 = GetPlayer(2);
        player2.Attack();

        C07_3_Mage mage = player2 as C07_3_Mage;
        mage.mp = 100;
        Debug.Log($"마법사의 마나: {mage.mp}");
    }

    C07_3_Player GetPlayer(int type)
    {
        C07_3_Player player = null;
        switch (type)
        {
            case 0:
                player = new C07_3_Knight();
                break;
            case 1:
                player = new C07_3_Archor();
                break;
            case 2:
                player = new C07_3_Mage();
                break;
        }

        return player;
    }
}

//1. 상속성

//여러 클래스들이 동일한 특성을 가질 때, 한 클래스에 공통 특성을 서술하고, 여러 다른 클래스가 이를 상속받게 할 수 있다.
//이 때 상속하는 클래스를 '부모 클래스' 또는 '기반 클래스', 
//상속받는 클래스를 '자식 클래스' 또는 '파생 클래스'라고 한다.

class C07_1_Player
{
    public int id;
    public string name;
    public int hp;
    public int power;

    public C07_1_Player()
    {
        Debug.Log("----------------------------------");
        Debug.Log("플레이어 생성");
    }
    public C07_1_Player(string name)
    {
        Debug.Log("----------------------------------");
        this.name = name;
        Debug.Log($"'{this.name}'을 생성했습니다.");
    }
    public void Move()
    {
        Debug.Log($"{name} 이동");
    }
}

class C07_1_Mage : C07_1_Player
{
    public C07_1_Mage()
    {
        name = "마법사";
        Debug.Log($"플레이어의 직업은 {name} 입니다");
    }
}

class C07_1_Knight : C07_1_Player { public C07_1_Knight() { name = "전사"; } }

class C07_1_CustomClass01 : C07_1_Player
{
    int mp = 300;
    public C07_1_CustomClass01() : base("히든 직업 1")
    {
        hp = 300;
        power = 50;
        mp = 100;
        Debug.Log($"'{name}'은 높은 공격력과 체력을 갖습니다(Hp: {hp}, 공격력: {power})");
    }
    public C07_1_CustomClass01(string name)
    {
        base.name = name;
        Debug.Log($"플레이어는 히든 직업 {base.name}으로 전직했습니다. {this.mp}의 마나를 갖습니다.");
    }
}



// 2. 은닉성

// 각 클래스의 보안 상태를 설정할 수 있다.
// 예를 들어, 자동차는 핸들, 페달, 전기장치, 엔진 등의 특성을 갖는데, 페달은 외부에서 조작 가능해야 하고 엔진은 일반 사용자가 접근하지 못하게 숨겨야 한다.

//프로젝트가 커질수록 여러 사람이 공동으로 작업하게 되는데, 개별 함수를 일일이 확인하기보다 함수명만 보고 기능을 예측하는 경우가 많다.
//이 때 프로젝트에 중요한 영향을 미치는 정보나 함수는 다른 곳에서 마음대로 가져다 쓸 수 없게 해야 한다.


//접근 한정자를 통해 은닉성을 제어할 수 있다.

//public : 외부에서 자유롭게 가져다 쓸 수 있음
//protected : 해당 클래스를 상속받은 경우에만 외부에서 사용할 수 있음
//private(기본값) : 선언된 클래스 외부에서 사용 불가

class C07_2_Player
{
    public string name;
    private int _id;

    public void SetId(int id) //이렇게 private 변수를 public 함수를 통해 변경하면, 외부에서 변경한 지점을 찾을 때 함수에 break point를 찍고 편리하게 디버깅할 수 있다.
    {
        _id = id; //private 함수는 this.변수명으로 대입하기보다, _변수명 또는 멤버 변수라는 의미로 m_변수명 으로 표기하는 것이 선호된다.
    }
}




// 3. 다형성

// 부모 클래스를 통해 변수를 선언하거나 함수를 사용하면, 하나의 변수로 여러 자식 클래스들을 제어할 수 있다.

class C07_3_Player
{
    public int hp;
    public int power;

    public virtual void Move()
    {
        Debug.Log("이동 명령 받음");
    }
    public virtual void Attack() { }
}

class C07_3_Knight : C07_3_Player
{
    public new void Move() //
    {
        Debug.Log("기사 이동");
    }
    public C07_3_Knight()
    {
        Move();
    }
    public override void Attack()
    {
        Debug.Log("기사 공격");
    }
}
class C07_3_Archor : C07_3_Player
{
    public override void Move()
    {
        Debug.Log("궁수 이동");
    }
    public C07_3_Archor()
    {
        Move();
    }
    public override void Attack()
    {
        Debug.Log("궁수 공격");
    }
}
class C07_3_Mage : C07_3_Player
{
    public int mp;
    public override void Move()
    {
        base.Move();
        Debug.Log("마법사 이동");
    }
    public C07_3_Mage()
    {
        Move();
    }
    public override void Attack()
    {
        Debug.Log("마법사 공격");
    }
}
