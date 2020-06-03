using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class C05_OOP_Basic : MonoBehaviour
{
    // 05. 객체지향 프로그래밍 (Object-Oriented Programming)

    //1. 개념

    // E01의 TextRPG와 같이, 게임의 진행 과정이 코드 순으로 진행되는 것은 일종의 절차(Procedure)지향 프로그래밍.
    // 이 방식은 심플하고 직관적이지만, 프로그램이 커지고 복잡해질수록 유지보수가 힘들고 로직이 꼬일 수 있다.
        //예를 들어, 몬스터가 두 마리로 늘면? 로비에서 바로 필드로 가게 만들어달라고 기획이 변경되면? 수정하기 어려움.

    //객체(Object)는 필드 내 모든 개체를 가리킨다.    -    플레이어, 몬스터, 화살, 나무, 스킬, 보이지 않는 것들까지...
    //객체는 '속성'과 '기능'으로 분류하여 묘사할 수 있다.

        /* ex)
         * RPG에서 '기사'라는 객체
         * 속성: hp, 공격력, 현재 위치, ...
         * 기능: 이동 기능, 공격 기능, 체력이 0이 되면 죽는 기능, 이펙트 출력 기능, ...
         */

    //여기까지는 절차지향과 크게 차이가 와닿지 않으니, 객체에 해당하는 Knight 클래스를 하단에 만들어보자.

    [Button]
    void Test()
    {
        C05_Knight knight = new C05_Knight(); //새로 생성하지 않고 null로 남겨두면, 대상이 없어 접근할 수 없다.
        //게임에서 에러의 70% 이상은 이런 Null Reference Exception 에러(참조할 대상이 비어 있는 상태)에 해당한다.
        knight.hp = 100;
        knight.power = 10;

        knight.Move();
        knight.Attack();
    }







    //----------------------------------------------------------------------------

    //2.복사와 참조

    // E01에서 사용한 struct는 구조체로 복사 형식이며, Class는 참조 형식이라는 차이가 있다.
    // struct를 대입하면 해당 객체가 가진 값을 변수에 저장하지만, Class를 대입하면 해당 객체가 저장된 메모리 주소값(포인터)을 변수로 갖는다.

    // struct는 값으로 불러오면 원래의 값을 복사한 별개의 값을 가져오지만, Class는 메모리 주소를 복사해 가져오므로 원래의 객체 그대로를 참조한다.
    // 함수에서 ref를 사용하지 않아도 Class가 변수로 들어 있다면 ref를 사용한 변수처럼 원본을 참조하게 된다.

    //struct로 만든 Mage와 Class로 만든 Knight를 비교해보자.

    //객체를 매개변수로 넣으면 hp를 0으로 만드는 함수를 만든다.
    void KillKnight(C05_Knight knight)
    {
        knight.hp = 0;
        Debug.Log($"{knight.name} 사망 (체력: {knight.hp})");
    }
    void KillMage(C05_Mage mage)
    {
        mage.hp = 0;
        Debug.Log($"{mage.name} 사망 (체력: {mage.hp})");
    }

    [Button]
    void KillObjectTest()
    {
        //기사와 마법사를 생성하고, 생성한 기사와 마법사를 새로운 변수에 대입하여 하나 더 만든 뒤, 이름만 달리해 준다.
        C05_Knight knight = new C05_Knight();
        knight.name = "기사 1";
        knight.hp = 100;
        knight.power = 10;

        C05_Knight knight2 = knight;
        knight2.name = "기사 2";

        C05_Mage mage = new C05_Mage();
        mage.name = "마법사 1";
        mage.hp = 50;
        mage.power = 20;

        C05_Mage mage2 = mage;
        mage2.name = "마법사 2";

        //첫 번째 기사와 마법사를 죽인다.
        KillKnight(knight);
        KillMage(mage);

        //그 후 출력해보면, 기사는 두 번째 기사가 죽어있고 첫번째와 두번째 기사의 정보가 동일하다.
        //Class는 대입할 때 메모리 주소(포인터)를 대입하므로, 기사2는 기존에 생성된 기사1을 가리키는 값일 뿐이다. 따라서 기사1과 기사2는 하나의 객체를 가리킨다.
        //KillKnight에서 Class 변수를 가져가 변경했으니, 이는 기사1과 기사2가 가리키는 주소에 위치한 값을 실제로 변경했다. 따라서 hp도 0이 되었다.
        Debug.Log($"죽은 기사는 {knight.name} (체력: {knight.hp})이며, 살아있는 기사는 {knight2.name} (체력: {knight2.hp}) 입니다.");
        
        //반면 마법사는 mage와 mgae2가 선언될 때 각각 별개의 메모리 공간을 갖는다.
        //둘은 다른 객체이므로 마법사 1의 정보 변경(사망)은 마법사 2에게 영향을 주지 않을 뿐더러, mage의 값을 복사해서 가져간 KillMage의 영향도 받지 않아서 원본의 hp도 그대로다.
        Debug.Log($"죽은 마법사는 {mage.name} (체력: {mage.hp})이며, 살아있는 마법사는 {mage2.name} (체력: {mage2.hp}) 입니다.");
    }






    //----------------------------------------------------------------------------

    //3. 스택과 힙

    // 메모리엔 '스택(stack)'과 '힙(heap)'이라는 두 개의 공간이 있다.

    //복사 타입은 stack 영역에 본체를 갖는다. 선언될 경우 stack영역에 자신의 값을 저장한다. void, int, string 등의 변수나 함수도 여기에 해당한다.
    //참조 타입은 heap 영역에 본체를 갖고, new로 생성할 경우 heap 영역을 가리키는 주소값을 stack에 저장한다. 해당 Class 변수는 heap에 할당된 공간을 가리키는 포인터가 된다.

    //stack 영역은 함수가 시작할 때 할당되고 끝나면 사라지며 자동으로 메모리 관리가 된다.
    //heap 영역은 메모리를 할당했으면 Delete를 명령하지 않는 한 계속 남아있게 된다.
        // 하지만, C#은 언어 차원에서 heap 영역에 참조가 0인 부분이 있으면 자동으로 제거해준다. (직접 메모리 관리가 불가하다는 점은 장점인 동시에 단점이 되기도 한다)


}

class C05_Knight
{
    public string name;
    public int hp;
    public int power;

    public void Move()
    {
        Debug.Log($"{name} 이동");
    }
    public void Attack()
    {
        Debug.Log($"{name} 공격");
    }
}

struct C05_Mage
{
    public string name;
    public int hp;
    public int power;

    public void Move()
    {
        Debug.Log($"{name} 이동");
    }
    public void Attack()
    {
        Debug.Log($"{name} 공격");
    }
}