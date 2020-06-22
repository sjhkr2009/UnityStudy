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
    /// <summary>
    /// override에서 base 함수를 사용하지 않은 경우, 부모 클래스의 함수는 동작하지 않는다.
    /// </summary>
    [Button]
    void Test02()
    {
        C07_3_Player player1 = new C07_3_Knight();
        C07_3_Player player2 = new C07_3_Archor();
        C07_3_Player player3 = new C07_3_Mage();
        Debug.Log("--------------이동 함수 발동 시-----------");
        player1.Move(); // 기사의 Move()는 new로 선언했으므로 별개의 함수다. 여기선 Player로 불러왔으니 Player의 이동 함수만 실행된다.
        player2.Move(); // 궁수의 Move()는 부모 클래스를 오버라이드했다. 따라서 자식 클래스 Archor의 이동 함수만 실행된다.
        player3.Move(); // 마법사의 Move()는 부모 클래스를 오버라이드했지만, 부모 클래스의 함수를 base.Move()로 추가해 주었다. 따라서 부모 클래스와 자식 클래스의 함수가 둘 다 실행된다.
    }
    /// <summary>
    /// 클래스 간의 다형성을 이용해 부모와 자식 클래스를 오갈 수 있다. 하지만 부모 클래스를 자식으로 변화시킬 때는 변화에 성공할 수도, 실패할 수도 있는데, 이는 is나 as로 체크한다.
    /// </summary>
    [Button]
    void Test03()
    {
        C07_3_Player player = GetPlayer(1);
        bool isKnight = player is C07_3_Knight; //'변수 is 클래스'의 형태를 bool 값으로 반환한다. 변수가 해당 클래스거나 해당 클래스의 자식 클래스면 true 이다.
        if (isKnight) Debug.Log("기사 생성");
        else if (player is C07_3_Archor) Debug.Log("궁수 생성");

        C07_3_Player player2 = GetPlayer(2); //Player로 불러왔으므로 아직 Mage에만 존재하는 mp 변수는 쓸 수 없다.
        player2.Attack(); //Player 클래스에 존재하는 함수나 변수는 조작 가능하다.

        C07_3_Mage mage = player2 as C07_3_Mage; //자식 클래스로 캐스팅할 수 있다. Mage를 가지고 있지 않다면 Null Reference 에러가 뜬다.
        mage.mp = 100; //Mage로 불러왔으니 Mage에만 존재하는 변수를 조작할 수 있다.
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

    public virtual void Move()  //오버라이드를 허용하려면 virtual로 선언한다. 오버라이드 하지 않으면 그냥 이 함수가 발동된다.
                                //그렇다고 모든 함수를 virtual로 선언하지는 말 것. virtual이 성능상 좀 더 떨어진다.
    {
        Debug.Log("이동 명령 받음");
    }
    public virtual void Attack() { } //참고로 오버로드(overload)와 오버라이드(override)는 다르다. 오버로드는 같은 함수명을 인자나 반환형만 달리해서 여러 개 선언하는 것이다.
}

class C07_3_Knight : C07_3_Player
{
    public new void Move() //부모 클래스의 함수와 무관한 별개의 함수로 선언한다. 이 때도 base.Move()로 부모 함수를 포함시킬 수는 있지만, Player로 선언하고 Move()를 발동하면 이 함수는 동작하지 않는다.
    {
        Debug.Log("기사 이동");
    }
    public C07_3_Knight()
    {
        Move();
    }
    public override void Attack() //부모 클래스의 Attack()을 이 함수로 덮어씌운다. 부모 클래스의 함수가 발동되면 이 함수가 대신 실행된다.
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
    public override void Move() //부모 클래스의 함수를 발동시켰을 때, 부모 클래스의 함수도 발동시키고, 자식 클래스의 함수도 추가로 발동하고 싶다면 base.함수명() 을 넣어주면 된다.
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
