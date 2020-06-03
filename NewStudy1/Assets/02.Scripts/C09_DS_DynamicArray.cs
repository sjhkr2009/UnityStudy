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
}
