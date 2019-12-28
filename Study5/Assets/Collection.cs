using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    //배열: 일정 개수만큼의 방을 만들어 각각에 값을 저장. 방의 개수를 추가/삭제하려면 배열을 다시 만들어야 한다. (비용이 비싸다)
    int[] testArray = new int[10];

    //리스트: 노드 기반, 개수가 정해져있지 않아 추가/삭제가 자유로우나, 데이터가 연속적이지 않아 용량을 더 많이 먹는다. (포인터 연산이라 접근 속도가 느리지는 않음)
    List<int> testList = new List<int>();

    //링크드 리스트: 자신의 다음 값 주소뿐만 아니라, 앞뒤 값을 모두 가진 리스트
    LinkedList<int> testLinkedList = new LinkedList<int>();

    //Queue: FIFO (First In, First Out), 컬렉션에 값이 순서대로 들어가서, 뺄 때는 먼저 들어간 값부터 빠진다.
    Queue<int> testQueue = new Queue<int>();

    //Stack: LIFO (Last In, First Out), 컬렉션에 값이 순서대로 들어가서, 뺄 때는 마지막에 들어간 값부터 빠진다.
    Stack<int> testStack = new Stack<int>();

    //Dictionary: 키와 값으로 이루어지며, 키를 통해서만 값에 접근할 수 있다. (testList[5] 와 같은 방식은 사용불가)
    Dictionary<string, int> testDictionary = new Dictionary<string, int>();


    void Start()
    {
        //List
        testList.Add(1);
        testList.Add(2);
        testList.Add(4);
        testList.Insert(2, 3);
        Debug.Log($"List: {testList}"); //1,2,3,4

        //LinkedList
        testLinkedList.AddLast(2);
        testLinkedList.AddLast(3);
        testLinkedList.AddFirst(1);
        Debug.Log($"LinkedList: {testLinkedList}"); //1,2,3

        //Queue
        testQueue.Enqueue(5);
        testQueue.Enqueue(10);
        testQueue.Enqueue(15);
        testQueue.Enqueue(20);
        testQueue.Dequeue();
        Debug.Log($"Queue: {testQueue}"); //10,15,20

        //Stack
        testStack.Push(3);
        testStack.Push(6);
        testStack.Push(9);
        testStack.Push(12);
        testStack.Pop();
        Debug.Log($"Stack: {testStack}"); //3,6,9

        //Dictionary
        testDictionary.Add("1번", 1);
        testDictionary.Add("3번", 3);
        testDictionary.Add("5번", 5);
        Debug.Log($"Dictionary 3번: {testDictionary["3번"]}"); // 3

    }

    void Update()
    {
        
    }
}
