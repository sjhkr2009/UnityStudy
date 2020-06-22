using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class C14_Lambda : MonoBehaviour
{
    // 14. 람다(람다식, Lambda)

    // Lambda : 일회용 함수를 만드는데 사용하는 문법

    // 1. 기본 형태 : 무명 함수의 간결화


    List<C14_Item> items = new List<C14_Item>();

    [Button]
    void Test01()
    {
        // 람다와는 무관하나, 생성 후 변수를 설정할수도 있지만, 생성 시 변수를 {} 안에서 지정해줄 수 있다. 여러 개면 쉼표로 구분한다.
        items.Add(new C14_Item() { ItemType = C14_ItemType.Weapon, ItemRarity = C14_ItemRarity.Epic, name = "에픽 무기" });
        items.Add(new C14_Item() { ItemType = C14_ItemType.Armor, ItemRarity = C14_ItemRarity.Unique, name = "유니크 방어구" });
        items.Add(new C14_Item() { ItemType = C14_ItemType.Accessory, ItemRarity = C14_ItemRarity.Legendary, name = "레전더리 악세서리" });

        //델리게이트 사용
        Debug.Log("무기 탐색 : " + FindItem(IsWeapon).name);
        Debug.Log("유니크 아이템 탐색 : " + FindItem(IsUnique).name);


        //무명 함수 만들기
        Debug.Log("에픽 아이템 탐색 : " + FindItem(delegate (C14_Item item) { return item.ItemRarity == C14_ItemRarity.Epic; }).name);
        Debug.Log("악세서리 탐색 : " + FindItem(delegate (C14_Item i) { return i.ItemType == C14_ItemType.Accessory; }).name);


        //람다식
        Debug.Log("레전더리 아이템 탐색 : " + FindItem((C14_Item i) => { return i.ItemRarity == C14_ItemRarity.Legendary; }).name);
        Debug.Log("방어구 탐색 : " + FindItem((C14_Item i) => { return i.ItemType == C14_ItemType.Armor; }).name);

        items.Clear(); //테스트 실행 시마다 리스트를 추가하지 않도록 리스트를 비워준다.
    }



    //---------------------------------------------------------------------------------
    //설명

    //아이템을 찾는 함수 : 람다 미사용 시
    C14_Item FindWeapon()
    {
        foreach (var item in items)
        {
            if (item.ItemType == C14_ItemType.Weapon) return item;
        }
        return null;
    }
    C14_Item FindUniqueItem()
    {
        foreach (var item in items)
        {
            if (item.ItemRarity == C14_ItemRarity.Unique) return item;
        }
        return null;
    }
    //이런 식으로 여러 함수를 일일이 만드는 것은 코드가 반복적으로 길어진다.
    //레어 아이템을 찾거나 악세서리를 찾는 등, 아이템이 가진 정보나 찾으려는 정보마다 함수를 추가해야 한다. 함수 당 길이가 길어질수록 복잡해진다.
    //이를 해결하기 위해 두 가지 방법을 생각할 수 있다.

    // 1) 기존의 델리게이트 활용
    delegate bool ItemSelector(C14_Item item);
    C14_Item FindItem(ItemSelector itemSelector)
    {
        foreach (var item in items)
        {
            if (itemSelector(item)) return item;
        }
        return null;
    }
    bool IsWeapon(C14_Item item) { return item.ItemType == C14_ItemType.Weapon; }
    bool IsUnique(C14_Item item) { return item.ItemRarity == C14_ItemRarity.Unique; }
    // 델리게이트를 받는 함수 하나를 만들고, 델리게이트에 조건을 집어넣어 원하는 값을 찾는다.
    // 함수를 직접 구현하는 것보단 간결하지만, 새로운 아이템을 탐색하고자 할 때마다 조건을 추가해야 하는 점은 여전히 번거롭다.



    // 2) 무명 함수(익명 함수, Anonymous Function) 만들기

    // 1번에서는 조건을 추가해놓고 FindItem에 인자로 넣었다면, 여기서는 조건의 선언 없이 FindItem에 바로 함수를 만들어 넣는다.
    // FindItem(delegate(인자) { 일회용 함수 }); 형태로 사용한다.
    // 인자와 반환형은 선언된 델리게이트와 동일해야 한다. 위의 Test01() 참고.

    // C# 초기에 사용하던 방식.


    // 3) 람다식

    // 2번을 좀 더 간결하게 표현한 것.
    // (인자) => { 일회용 함수 } 형태로 표현한다. 위의 Test01() 참고.


    //---------------------------------------------------------------------------------






    // 2. 제네릭과의 응용

    delegate Return MyFunction<Parameter, Return>(Parameter item); //델리게이트의 인자와 반환형을 제네릭으로 설정한다.
    // 인자와 반환형이 1개인 함수는 전부 이 델리게이트를 통해 넘겨줄 수 있다.

    delegate Return MyFunction<Return>();
    delegate Return MyFunction<P1, P2, Return>(P1 item, P2 selector);
    // 델리게이트도 오버로드가 가능하므로, 인자가 여러 개인 함수에 동시에 활용하게 할 수도 있다.

    //다만 이런 함수는 이미 C#에 만들어져 있다. 이를 선언된 델리게이트처럼 쓰면 된다.
    public event Func<bool> myFunc; //반환형이 있는 경우, Func<인자, 인자, 인자, ..., 반환형>
    public event Action myFunc2; //반환형이 없는 경우, Action<인자, 인자, 인자, ...> 인자가 없는 경우 <> 생략하고 그냥 Action

    C14_Item FindItem2(MyFunction<C14_Item, bool> itemSelector)
    {
        foreach (var item in items)
        {
            if (itemSelector(item)) return item;
        }
        return null;
    }

    // 찾는 아이템의 타입 번호를 받아서, 해당 형태의 아이템을 찾아주는 함수 만들기
    C14_Item FindItem3(MyFunction<C14_Item, int> itemSelector, int selector)
    {
        foreach (var item in items)
        {
            if (itemSelector(item) == selector) return item;
        }
        return null;
    }

    [Button]
    void Test02()
    {
        items.Add(new C14_Item() { ItemType = C14_ItemType.Weapon, ItemRarity = C14_ItemRarity.Epic, name = "에픽 무기" });
        items.Add(new C14_Item() { ItemType = C14_ItemType.Armor, ItemRarity = C14_ItemRarity.Unique, name = "유니크 방어구" });
        items.Add(new C14_Item() { ItemType = C14_ItemType.Accessory, ItemRarity = C14_ItemRarity.Legendary, name = "레전더리 악세서리" });

        C14_Item item = FindItem2((C14_Item i) => { return i.ItemRarity == C14_ItemRarity.Legendary; });
        Debug.Log($"레전더리 아이템 : {item.name}");

        MyFunction<C14_Item, int> selector = (C14_Item i) => //여러 줄일 경우 중괄호를 펼쳐서 함수처럼 적을 수도 있다.
        {
            return (int)i.ItemType; //아이템 타입의 enum 값을 정수로 반환한다.
        }; //selector를 선언하는 명령문이므로, 중괄호 끝에 세미콜론을 빼먹지 말 것.

        Debug.Log($"무기 : { FindItem3(selector, 0).name}");
        Debug.Log($"방어구 : { FindItem3(selector, 1).name}");
        Debug.Log($"악세서리 : { FindItem3(selector, 2).name}");
        // 각각을 탐색하는 함수를 일일이 만들 필요가 없어진다.

        items.Clear(); //테스트 실행 시마다 리스트를 추가하지 않도록 리스트를 비워준다.
    }



}

enum C14_ItemRarity
{
    Normal,
    Rare,
    Epic,
    Unique,
    Legendary
}
enum C14_ItemType
{
    Weapon,
    Armor,
    Accessory
}
class C14_Item
{
    public C14_ItemType ItemType;
    public C14_ItemRarity ItemRarity;
    public string name;
}