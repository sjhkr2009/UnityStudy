using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class C10_Generic : MonoBehaviour
{
    // 10. 일반화(Generic)

    // 여러 타입을 대입할 수 있도록 할 때 사용한다.
    [Button]
    void Test01()
    {
        //여러 타입을 소화하는 형식

        //1) 오브젝트(object)
        //      object를 사용한다. var는 컴파일러가 알아서 형식을 찾아주는 반면, object는 그 자체로 하나의 형식이다. 다른 형식으로 캐스팅도 가능하다.
        //      int, string, float 등이 object를 상속받기 때문에 가능하다.
        //      하지만 object는 int 등과 달리 참조 타입으로만 동작하므로, stack에 값을 바로 저장하지 않고 heap에 저장 후 스택에서 빼온다. 따라서 무거운 작업이라 느리다.
        //      그래서 여러 타입을 소화하게 하기 위해 모든 형식을 object로 선언하는 것은 좋은 방법이 아니다.

        object obj = 3;
        object obj2 = "Hello";
        object obj3 = obj;
        obj3 = 2f;
        int num = (int)obj;
        Debug.Log($"3, Hello, 2f = {obj}, {obj2}, {obj3}");

        //2) 일반화(제네릭, Generic)
        //      <T> 를 사용한다. 사실 T가 아니라도 상관은 없다.
        //      클래스나 함수의 이름 뒤에 <T>를 사용하여 선언하고, 사용할 때 T 자리에 형식을 입력하면 된다.
        //      대표적으로 리스트가 제네릭 형식이다. (List<T>)

        C10_MyList<int> myIntList = new C10_MyList<int>(5);
        C10_MyList<C10_Item> myItemList = new C10_MyList<C10_Item>(3);
        Debug.Log("int 리스트 내의 숫자 0 : " + myIntList.GetItem(3));

        for (int i = 0; i < myItemList.array.Length; i++)
        {
            myItemList.SetItem(new C10_Item(), i);
            myItemList.GetItem(i).name = "아이템 " + (i + 1);
        }
        Debug.Log($"세번째 아이템 : {myItemList.GetItem(2).name}");
    }
    [Button]
    void Test02()
    {
        //T가 여러개일 경우
        And<string, float>("Hello", 18f);

        //제약 조건
        C10_Monster monster = new C10_Monster();
        monster.hp = 100;
        monster.name = "몬스터";
        DebugHp(monster); //DebugHp<C10_1_Monster>(monster) 로 해도 되지만, 유추할 수 있는 경우 형식을 생략할 수 있다.

        //여러 개의 제약 조건과 생성자
        C10_Player player = CreateCreature<C10_Player>(); //T가 어떤 것인지 유추할 수 없는 경우 직접 지정해야 한다.
        player.hp = 250;
        player.name = "플레이어";
        DebugHp(player);
    }
    void And<T, K>(T a, K b) //두 개 이상의 제네릭은 <A, B, ...> 식으로 쉼표로 구분한다. 보편적으로 T를 쓰지만 < > 안에만 쓰면 다른 문자도 상관없다.
    {
        Debug.Log($"{a} and {b}");
    } 
    void DebugHp<T>(T type) where T : C10_Creature    //제네릭을 특정 형태로만 선언하고 싶다면 where T : 지정된 형식 을 사용하여 제약 조건을 지정한다.
                                                        //지정된 형식에 특정 클래스명을 적으면 해당 클래스 또는 이를 상속받은 클래스만 들어갈 수 있다.
    {
        Debug.Log($"{type.name}의 체력 : " + type.hp); //T의 형식이 C10_1_Creature나 이를 상속받은 형태로 한정되므로, C10_1_Creature의 변수 hp를 사용할 수 있다.
    }
    T CreateCreature<T>() where T : C10_Creature, new() //where에 여러 개의 조건을 지정할 수 있다. 단, new()는 마지막에 입력해야 한다.
    {
        C10_Creature creature = new T(); //new() 생성자를 제약 조건으로 지정했으므로, new T()를 사용할 수 있다.
        return (T)creature;
    }

}

class C10_MyList<T>
{
    public T[] array;
    public C10_MyList(int num)
    {
        array = new T[num];
    }
    public T GetItem(int index)
    {
        return array[index];
    }
    public void SetItem(T value, int index)
    {
        array[index] = value;
    }
}
class C10_Item { public string name; }
class C10_Creature
{
    public int hp;
    public string name;
}
class C10_Monster : C10_Creature { }
class C10_Player : C10_Creature { }