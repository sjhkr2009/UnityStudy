using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C12_Property : MonoBehaviour
{
    // 12. 프로퍼티(Property)

    // 변수에 값을 직접 대입하지 않고, 함수를 통해 접근하게 하는 방법. 변수를 함수처럼 이용할 수 있으며 디버그로 추적이 가능하다.
    // 객체지향의 은닉성과 관련이 있다.

    // 1. 원리 (Get함수와 Set함수를 이용한 은닉성, C++방식)
    [Button]
    void Test01()
    {
        C12_Player_nonProperty player = new C12_Player_nonProperty();
        player.SetHp(50);
        Debug.Log("플레이어 체력 : " + player.GetHp());
    }

    // 2. 프로퍼티 사용 시
    [Button]
    void Test02()
    {
        C12_Player_Property player = new C12_Player_Property();
        player.Hp = 50;
        Debug.Log("플레이어 체력 : " + player.Hp);
    }

    // 3. 응용
    private int hp;
    public int Hp
    {
        set => hp = value; //get이나 set 중 하나만 입력해서 읽기 또는 쓰기만 가능하게 만들 수 있다.
        //입력할 내용이 한 줄이면 {} 대신 => 을 사용할 수 있다.
    }

    private string _name;
    public string Name
    {
        get => _name; //=> 사용 시 return을 생략할 수 있다.
        set
        {
            _name = value;
            Debug.Log($"이름이 {value} (으)로 변경되었습니다."); //값을 읽고 쓰는 것 외에 다른 동작도 입력 가능하다.
        }
    }
    public int power { get; set; } //아래 'C12_Player_nonProperty'처럼 값을 간접적으로 읽고 쓰게 하는(은닉성) 용도로만 사용한다면, 괄호 안에 get; set;만 쓰면 된다.
    //이 경우 값을 읽고 쓸 다른 private 변수를 선언할 필요도 없으며, 내부적으로 같은 이름의 private 변수를 만들어 그곳에 값을 읽고 쓰게 된다.
    //get과 set 중 하나로만 사용하고 싶다면 {get;} 또는 {set;}만 써도 된다. 둘 중 하나는 private으로 선언하려면 {get; private set;} 과 같이 쓰면 된다.

    [Button]
    void Test03()
    {
        Name = "태사단";
        power = 30;
        Debug.Log("플레이어 체력 : " + power);
    }

}

class C12_Player_nonProperty
{
    private int hp;                                 //변수의 직접 조작을 막아두고
    public int GetHp() { return hp; }               //값을 받아오는건 Get함수 또는 Getter로
    public void SetHp(int value) { hp = value; }    //값의 입력은 Set함수 또는 Setter로 한다. C++은 프로퍼티가 없어서 이 방식이 정석이다.
}

class C12_Player_Property
{
    private int hp;             //변수의 직접 조작은 막아두고
    public int Hp
    {
        get { return hp; }      //get에는 이 변수 호출 시 실행할 동작을 입력하고, 돌려줄 값을 return으로 적는다.
        set { hp = value; }     //set에는 입력된 값을 어떻게 처리할지 적는다. 입력된 값은 value 이다.
    }
}