using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D02_Lists : AlgorithmTest
{
    // 02. 선형 자료구조

    // 자료구조는 크게 선형과 비선형으로 구분된다.

    // 선형 자료구조 : 자료를 하나씩 순차적으로 나열한 형태
    //  - 배열(Array), 동적 배열(List), 연결 리스트(LinkedList), 스택(Stack), 큐(Queue) 등
    // 비선셩 자료구조 : 하나의 자료 뒤에 다수의 자료가 올 수 있는 형태
    //  - 트리(tree), 그래프(graph) 등




    // 1. 배열(Array)
    // - 호텔로 비유하면 사용할 방의 개수를 고정해서 계약하고, 연속된 방으로 배정 받아 사용하는 형태. 방의 개수는 절대 변경할 수 없다.
    // 장점: 연속된 방

    // 문제점: 방의 개수 추가/감소 불가  ------  하지만 타일맵을 만드는 등 개수의 추가/감소나 중간 삽입/삭제가 불필요할 경우 배열을 사용하는게 가장 좋을 수 있다.

    public int[] data1 = new int[25];




    //-----------------------------------------------------------------------------------------



    // 2. 동적 배열(List)
    // - 호텔로 비유하면 사용할 방의 개수를 유동적으로 계약. 101~103호를 쓰다가 한 방이 더 필요하면 104호를 추가로 계약하거나, 반대로 취소할 수 있다.
    // 장점: 유동적인 계약

    // 문제점1: 101~103호를 계약했다가 한 방이 더 필요한데 104호가 차 있다면? -> 다른 층에 자리를 잡아 201~204호를 계약한다.
    //  이사 비용은 어떻게? -> 동적 배열은 실제 사용할 방보다 여유분을 두고, 1.5~2배 많은 방을 예약하여 이사 횟수를 최소화한다.
    //      but. 그럼에도 예약분이 가득 차서 이사 비용이 발생할 수 있다.
    // 문제점2: 중간 삽입/삭제가 불가능하다
    //  101~104호를 계약하다가 102호를 취소해야 한다면? -> 103,104호를 102,103호로 옮겨야 한다.
    //  101~104호에 추가로 방을 예약하려는데 102호에 들어오고 싶어하면? -> 102,103,104호가 한 칸씩 옮겨야 한다.

    public List<int> data2 = new List<int>();
    public D02_MyList<int> data2_1 = new D02_MyList<int>();

    public override void TestCode01()
    {
        code1 = "동적 배열(List) - 데이터 추가와 삭제";

        data2.Add(101);
        data2.Add(102);
        data2.Add(103);
        data2.Add(104);
        data2.Add(105);

        int temp = data2[2];
        data2.RemoveAt(2);
    }
    public override void TestCode02()
    {
        code2 = "동적 배열 직접 구현 - 데이터 추가와 삭제";

        data2_1.Add(101);
        data2_1.Add(102);
        data2_1.Add(103);
        data2_1.Add(104);
        data2_1.Add(105);

        int temp = data2[2];
        data2_1.RemoveAt(2);
    }

    //      참고: C++에서는 동적 배열을 Vector, 연결 리스트를 List라는 이름을 사용한다.

    //-----------------------------------------------------------------------------------------






    // 3. 연결 리스트(Linked List)
    // - 호텔로 비유하면 여러 명이 각각 원하는 방을 계약하여 사용한다. 단, 각 방에는 순서가 있어서, 이전 방과 다음 방 하나에 한하여 워프할 수 있는 장치가 있어 이동 비용은 없다고 가정한다.
    // 장점: 중간 추가/삭제 이점
    //  이전 방과 다음 방은 언제든 재지정할 수 있어서, 중간에 다른 요소를 끼워넣어도 이사 비용이 발생하지 않는다.

    // 문제점: N번째 방을 바로 찾을 수 없다. 3번째 방을 찾으려면 103호로 바로 가면 되던 이전과 달리, 여기서는 첫 번째 방에 가서 워프를 두 번 해야 세번째 방의 호수를 알아낼 수 있다.
    //          ------  하지만 보안상 특정 값을 바로 가져오지 못 하게 할 때는 유용하다

    // 데이터 추가 및 삭제에 상수의 시간복잡도를 갖는다. 인덱서를 지원하지 않는 것도 구조상 특정 인덱스의 값을 찾는 것은 시간복잡도가 O(n)이 되기 때문.

    public LinkedList<int> data3 = new LinkedList<int>();
    public D02_MyLinkedList<int> data3_1 = new D02_MyLinkedList<int>();

    public override void TestCode03()
    {
        code3 = "연결 리스트 - 데이터 추가와 삭제";

        LinkedListNode<int> node0 = data3.AddLast(101); //마우스를 대 보면 각 Add 함수가 LinkedListNode 형식을 반환하는 형식임을 알 수 있다. (List의 경우는 void 함수)
        data3.AddLast(102);
        LinkedListNode<int> node1 = data3.AddLast(103); //해당 형식으로 반환형을 받을 수 있다.
        data3.AddLast(104);
        LinkedListNode<int> node2 = data3.AddLast(105);
        
        data3.Remove(node0); //연결 리스트는 인덱스를 갖지 않으므로 LinkedListNode로 값의 주소를 저장해 두었다가 삭제할 수 있다.
        data3.Remove(node1);
        data3.Remove(node2);
    }

    public override void TestCode04()
    {
        code4 = "연결 리스트 직접 구현 - 데이터 추가와 삭제";

        D02_Room<int> node0 = data3_1.AddLast(101);
        data3_1.AddLast(102);
        D02_Room<int> node1 = data3_1.AddLast(103);
        data3_1.AddLast(104);
        D02_Room<int> node2 = data3_1.AddLast(105);

        data3_1.Remove(node0);
        data3_1.Remove(node1);
        data3_1.Remove(node2);
    }

    //-----------------------------------------------------------------------------------------
}










// 배열을 바탕으로 동적 배열 직접 구현해보기
// Add와 인덱서의 시간 복잡도는 O(1), 삭제는 O(n)
public class D02_MyList<T>
{
    //기본적으로 1개짜리 배열을 만든다.
    const int DefaultSize = 1;
    T[] data = new T[DefaultSize];

    // 사용중인 데이터 개수. 처음엔 빈 배열일테니 0개.
    public int count = 0;
    // 예약된 데이터 개수. (사용중인 데이터 개수 + 추가로 사용할 수 있게 확보된 공간의 개수)
    public int capacity => data.Length;

    //data 자체는 내부 배열이고, 리스트 클래스를 인덱서로 접근할 수 있게 한다.
    public T this[int index]
    {
        get => data[index];
        set => data[index] = value;
    }

    /// <summary>
    /// 배열에 요소를 추가하는 함수. for문이 있기는 하지만, 2배씩 공간을 늘리는 특성상 실행되는 경우가 적으므로 시간 복잡도 계산 때는 예외적으로 상수 취급한다. 즉 시간 복잡도는 O(1)
    /// </summary>
    public void Add(T item)
    {
        // 사용중인 데이터 개수가 예약된 공간과 같거나 많다면, 공간을 늘려준다.
        if(count >= capacity)
        {
            //사용중인 데이터의 두 배까지 수용가능한 새로운 배열을 만들고, 기존 배열의 요소들을 옮긴 후, 새로운 배열로 기존 배열을 대체한다.
            T[] newArray = new T[count * 2];
            for (int i = 0; i < count; i++)
                newArray[i] = data[i];

            data = newArray;
        }
        // 앞의 if문이 실행되었든 아니든 배열에 빈 자리가 있을테니, 새로운 값을 추가하고 사용중인 데이터 개수가 1개 증가했음을 표시한다.
        data[count] = item;
        count++;
    }

    /// <summary>
    /// 배열의 인덱스로 요소를 제거하는 함수. index에 따라 for문 시행횟수가 달라지지만, 시간 복잡도를 계산할 때는 최악의 경우를 상정하여 0부터 돈다고 가정한다. 즉 시간 복잡도는 O(n)
    /// </summary>
    public void RemoveAt(int index)
    {
        // 제거 대상의 뒤에 있는 모든 요소들을 한 칸씩 앞으로 옮긴다. 5번 요소 제거 시 5번 자리에 6번을, 6번 자리에 7번을 차례로 대입한다.
        // 배열의 개수가 10개일 경우 [9]가 마지막일 것이고, 따라서 8번 자리에 9번을 대입하는 것이 마지막 시행이 될 것이다. 따라서 '(배열 개수 - 1) 보다 작을 것'을 반복문의 조건으로 넣는다.
        for (int i = index; i < count - 1; i++)
            data[i] = data[i + 1];

        // 다 옮겼으면 배열의 마지막 요소는 삭제하고, 카운트를 하나 줄인다.
        data[count - 1] = default(T);
        count--;
    }

}




//연결 리스트 직접 구현해보기
// 모든 함수의 시간 복잡도 O(1)

//연결 리스트를 구성할 방을 구현한다. 이 Room이 LinkedListNode의 역할을 한다. 각 방은 자신의 값과 자신 앞뒤의 주소값을 갖는다.
public class D02_Room<T>
{
    //방은 인덱스 없이, 자신의 앞과 뒤에 있는 방의 주소를 갖는다. Room은 참조 형식(클래스)이므로 next와 prev는 다른 Room의 주소(노드) 두 개를 갖고 있음을 의미한다.
    public D02_Room<T> next;
    public D02_Room<T> prev;
    public T data;
}
public class D02_MyLinkedList<T>
{
    public int count = 0;
    public D02_Room<T> first = null;
    public D02_Room<T> last = null;

    public D02_Room<T> AddLast(T data)
    {
        D02_Room<T> newRoom = new D02_Room<T>();
        newRoom.data = data;

        if (first == null) first = newRoom;
        if(last != null)
        {
            last.next = newRoom;
            newRoom.prev = last;
        }

        last = newRoom;
        count++;
        return newRoom;
    }
    public void Remove(D02_Room<T> room)
    {
        if (room != first) room.prev.next = room.next;
        else first = room.next;

        if (room != last) room.next.prev = room.prev;
        else last = room.prev;

        count--;
    }
}