using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class C09_DS_DynamicArray : MonoBehaviour
{
    // 09. 자료구조(Data Structure) -  동적 배열(Dynamic Array)

    // 배열(Array)은 값을 저장할 공간의 개수가 고정되어 있다는 단점이 있다.
    // 데이터가 많아지면 배열을 재정의해야 하고, 적다면 남는 공간이 낭비된다.

    // 동적 배열은 배열의 개수가 유동적인 배열을 뜻한다.

    
    
    
    // 1. 리스트(List)

    // 대표적인 동적 배열의 하나. 배열과 마찬가지로 참조 형식이다.

    /// <summary>
    /// 편의상 리스트 내 요소들을 string으로 나열하여 반환하는 함수를 만들어둔다. 디버그할 때 사용할 것.
    /// </summary>
    string ListToString<T>(List<T> list)
    {
        string toString = "|";
        foreach (var item in list) toString += $" {item} |";
        return toString;
    }

    [Button]
    void Test01_List()
    {
        List<int> list = new List<int>(); //배열과 달리 'List<변수 형식>' 형태의 클래스로서, 최초 생산 시 빈 상태로 생성된다.
        //list[0] = 1; 리스트는 처음에 비어 있는 상태이므로, 이런 식으로 정의하면 인덱스를 초과했다고 뜬다. 인덱스 0을 가지려면 1개의 값은 있어야 하는데 배열이 비어 있기 때문이다.

        //배열에 값 추가하기 (리스트의 값을 읽는 법은 배열과 동일하다)
        list.Add(0);
        for (int i = 1; i < 5; i++) list.Add(i);
        Debug.Log("0~4 리스트 : " + ListToString<int>(list));

        //배열에 값 삽입/삭제 (삽입/삭제는 리스트 내 요소들의 순서를 조정해야 하므로, 그리 효율적인 작업이 아니니 남용은 자제할 것.)
        list.Insert(1, 99); //인덱스 1에 99를 삽입한다. 1 이상의 인덱스를 갖는 값들은 뒤로 밀려난다.
        Debug.Log("인덱스1에 99 추가 : " + ListToString<int>(list));
        list.Remove(3); //리스트에서 해당 값 하나를 지운다. 해당 값이 여러 개 있으면 인덱스가 가장 앞쪽인 요소를 지운다. 모두 지우려면 RemoveAll 을 사용한다.
        Debug.Log("요소 3을 찾아서 삭제 : " + ListToString<int>(list));
        list.RemoveAt(0); //리스트에서 해당 인덱스를 지운다.
        Debug.Log("인덱스 0을 찾아서 삭제 : " + ListToString<int>(list));

        list.Clear(); //리스트의 모든 내용을 삭제한다.
        Debug.Log("빈 리스트 : " + ListToString<int>(list));
    }

    // RPG 게임에서 소환된 몬스터는 부여된 ID를 통해 식별하게 된다. 이 때, 리스트는 어떤 ID에 해당하는 오브젝트를 찾아내기 어렵다는 단점이 있다.
    // 생성/소멸이 반복되니 계속 인덱스가 바뀌니, 사실상 리스트 처음부터 끝까지 훑어 찾는 작업을 매번 해야 한다.
    // 이렇게 특정 요소에 접근해야 할 경우 후술할 딕셔너리를 사용한다.




    //--------------------------------------------------------------





    // 2. 딕셔너리 (Dictionary)

    // Key와 Value의 하나의 쌍을 저장하는 동적 배열.
    // 배열의 순서가 지속적으로 바뀌거나 의도적으로 뒤섞을 때, 요소에 쉽게 접근하기 위해 사용한다.

    // 딕셔너리가 빠르게 요소를 찾을 수 있는 이유는 'Hashtable' 구조이기 때문.
    // 1만개의 정보가 있을 경우, 이를 한 공간에 모두 저장하지 않고 여러 공간에 나누어 저장한다. 즉 딕셔너리는 2개 이상 공간의 집합으로 구성된다.
    // 따라서 메모리 공간을 많이 차지하는 대신, 데이터를 찾아내는 성능을 높이게 된다.

    [Button]
    void Test02_Dictionary()
    {
        Dictionary<int, C09_Monster> dic = new Dictionary<int, C09_Monster>(); //생성 시 <Key, Value> 형태로 입력한다.

        dic.Add(1, new C09_Monster(1)); //Add.(Key Value) 로 값을 추가한다. 삭제도 Remove(key)로 가능하다.

        for (int i = 2; i <= 100; i++)
        {
            dic.Add(i, new C09_Monster(i));
        }
        C09_Monster monster54 = dic[54];                    //딕셔너리는 '딕셔너리[key]'로 불러올 수 있지만, 해당 키가 없으면 에러가 난다.
        Debug.Log($"54번 몬스터 : {monster54.name}");

        C09_Monster monster73;
        bool found = dic.TryGetValue(73, out monster73);    //따라서 TryGetValue(key, out 저장할 변수) 를 통해 불러온다. 못찾았으면 false를 반환한다.
        if (found) Debug.Log($"73번 몬스터 : {monster73.name}");
        else Debug.Log($"정보를 찾지 못했습니다. ({nameof(monster73)})");

        C09_Monster monster150;
        found = dic.TryGetValue(150, out monster150);
        if (found) Debug.Log($"150번 몬스터 : {monster150.name}");
        else Debug.Log($"정보를 찾지 못했습니다. ({nameof(monster150)})");
    }

}

class C09_Monster
{
    public string name;
    public C09_Monster(int count)
    {
        name = "몬스터 " + count;
    }
}
