using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D03_Stack_Queue : AlgorithmTest
{
    // 03. 스택과 큐

    //후입선출, 선입선출에 따라 들어온 순서에 의해 데이터가 나가는 순서도 정해지는 리스트.
    //중간에 있는 요소는 접근이 불가능하다.


    // 1. 스택 (Stack)

    // LIFO (Last In, First Out) - 후입선출을 따르는 리스트
    // 가장 늦게 들어온 요소가 먼저 빠져나간다.

    //구현 예시 : 팝업창 등에서 Esc를 누르면 제일 위에 뜬 창이 먼저 닫힌다.

    public override void TestCode01()
    {
        code1 = "스택";
        
        Stack<int> stack = new Stack<int>();

        stack.Push(101); //Push로 요소를 입력한다. 인덱스는 지정할 수 없다.
        stack.Push(102);
        stack.Push(103);
        stack.Push(104);
        stack.Push(105);

        int a = stack.Pop(); //Pop()으로 마지막으로 입력된 요소를 뺄 수 있다.
        int b = stack.Peek(); //Peek()으로 마지막으로 입력된 요소를 볼 수 있다. Pop과 달리 리스트에서 제거하지는 않는다.
        Debug.Log($"Pop: {a} / Peek: {b}");
    }




    // 2. 큐 (Queue)

    // FIFO (First In, First Out) - 선입선출을 따르는 리스트
    // 가장 먼저 들어온 요소가 먼저 빠져나간다.

    //구현 예시 : MMORPG 등에서 수많은 유저가 동시다발적으로 서버에 요청을 보낼 때, 먼저 들어온 순서로 반응할 수 있다.

    public override void TestCode02()
    {
        code2 = "큐";

        Queue<int> queue = new Queue<int>();

        queue.Enqueue(201); //Enqueue로 요소를 입력한다. 역시 인덱스는 지정할 수 없다.
        queue.Enqueue(202);
        queue.Enqueue(203);
        queue.Enqueue(204);
        queue.Enqueue(205);

        int a = queue.Dequeue(); //Dequque()로 리스트에서 제일 처음 입력된 요소를 뺄 수 있다.
        int b = queue.Peek(); //Peek()로 처음 입력된 요소를 볼 수 있다. Dequque와 달리 리스트에서 제거하지는 않는다.
        Debug.Log($"Dequeue: {a} / Peek: {b}");
    }




    // 리스트의 처음이나 마지막에 요소를 넣고 빼는 것은 리스트나 링크드 리스트(RemoveFirst, AddLast 등)에서도 할 수 있다.
    // 그럼에도 스택이나 큐를 사용하는 이유는 추상적으로 구현하기 편리하기 때문. '링크드 리스트로 선입선출 방식으로 구현하자'는 것보다 '큐 방식으로 하자'는 것이 의사소통에도 편하다.
    //  추가로, 리스트와 달리 인덱스를 실수로 잘못 넣을 일도 없고, 넣고 뺄 요소의 자리를 찾는 것도 상수의 시간복잡도로 구현된다.
    //  예를 들어 선입선출에서 첫 요소를 빼면 다른 요소를 리스트 앞으로 당기지 않고, 그냥 두번째 자리였던 곳을 첫 요소로 간주한다.
    
}
