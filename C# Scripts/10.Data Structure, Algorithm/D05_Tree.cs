using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D05_Tree : AlgorithmTest
{
    // 05. 트리 (Tree)

    // 계층적 구조를 갖는 데이터를 표현하기 위한 자료구조.
    // 그래프와 달리 정점 대신 노드를 사용하며, 간선은 방향성을 가진다. 계층적인 구조를 나타내므로 일방적인 연결관계여야 하며, 순환적인 연결이 존재해서는 안 된다.
    // 몇 가지 제약조건이 있는 그래프의 일종으로 보아도 무방함.

    // 노드(Node) : 데이터를 표현
    // 간선(Edge) : 노드의 계층 구조를 표현하기 위해 사용


    // 1. 관련 용어

    // 부모 노드,자식 노드 (parent node, child node) : A → B 라는 연결관계가 있을 때, A를 부모 노드, B를 자식 노드라고 한다.
    //                                                  부모 노드는 여러 자식을 가질 수 있으나, 자식 노드는 둘 이상의 부모 노드를 가질 수 없다. (최상위 노드는 부모 노드가 없을 수 있음)

    // 형제 노드 (sibling node) :   같은 부모 노드를 가진 자식 노드의 관계
    //                              A → B와 A → C의 관계가 있을 때, B와 C는 서로 형제 노드이다.

    // 선조 노드,자손 노드 (ancestor node, descendant node) :  둘 이상의 부모/자식 관계에 걸쳐 연결되어 있는 노드.
    //                                                          A → B → C 연결관계에서 A는 C의 선조 노드, C는 A의 자손 노드이다.

    // 루트 (root, root node) : 부모 노드를 가지지 않는 최상위 노드
    // 잎 (leaf, leaf node) : 자식 노드를 가지지 않는 최하위 노드

    // 노드의 깊이(depth) :  루트를 기준으로 몇 단계에 걸쳐 연결되어 있는지를 의미한다
    // 노드의 높이(height) : 잎을 기준으로 몇 단계에 걸쳐 올라와 있는지를 의미한다.
    //                       깊이와 높이는 각각 루트나 잎을 0으로 계산한다. A → B → C → D 의 연결관계에서 B의 깊이는 1, 높이는 2이다.

    // 트리의 높이(깊이) : 트리가 가질 수 있는 최대 높이(깊이)를 의미한다.

    // 트리의 재귀적 속성 : 트리는 일부분을 떼어 보아도 또다른 작은 트리의 모습이 된다. 따라서 트리와 관련된 연산을 할 때는 재귀함수가 자주 활용된다.
    //                       ㄴ 트리의 일부분에 해당하는 작은 트리를 '서브트리(subtree)' 라고 한다.

    //----------------------------------------------------------------------------







    // 2. 트리 구현

    // 트리도 제공되는 클래스가 따로 없어서, 직접 구현해야 한다.

    // 그래프와 달리 노드 클래스를 만들어 이를 기반으로 구현한다.
    // 그래프는 잘 변하지 않는 정적인 데이터를 바탕으로 가상의 연결관계를 표현하지만, 트리는 각 데이터의 변화가 자주 일어나고 삽입/삭제가 자주 발생하기 때문이다.
    // 따라서 가상의 정점을 가정하여 연결관계만 표기하는 것이 아닌, 실제 노드를 만들어서 연결시켜준다고 생각한다.

    [SerializeField] bool onStartTreeClear = false;

    D05_TreeNode<string> root = null;
    void MakeTree()
    {
        if (root != null) return;

        root = new D05_TreeNode<string>() { Data = "개발실" };

        {
            D05_TreeNode<string> node = new D05_TreeNode<string>() { Data = "디자인팀" };
            root.Children.Add(node);
            node.Children.Add(new D05_TreeNode<string>() { Data = "전투" });
            node.Children.Add(new D05_TreeNode<string>() { Data = "경제" });
            node.Children.Add(new D05_TreeNode<string>() { Data = "스토리" });
        }

        {
            D05_TreeNode<string> node = new D05_TreeNode<string>() { Data = "프로그래밍팀" };
            root.Children.Add(node);
            node.Children.Add(new D05_TreeNode<string>() { Data = "서버" });
            node.Children.Add(new D05_TreeNode<string>() { Data = "클라이언트" });
            {
                D05_TreeNode<string> _node = new D05_TreeNode<string>() { Data = "엔진" };
                node.Children.Add(_node);
                _node.Children.Add(new D05_TreeNode<string>() { Data = "유니티" });
                _node.Children.Add(new D05_TreeNode<string>() { Data = "언리얼" });
            }
        }

        {
            D05_TreeNode<string> node = new D05_TreeNode<string>() { Data = "아트팀" };
            root.Children.Add(node);
            node.Children.Add(new D05_TreeNode<string>() { Data = "배경" });
            node.Children.Add(new D05_TreeNode<string>() { Data = "캐릭터" });
        }
    }
    public override void TestCode01()
    {
        code1 = "트리 구현 연습 - 게임 개발팀 구조 묘사";

        if (onStartTreeClear && root != null) root = null;
        MakeTree();
        PrintTree<string>(root);
    }

    // 재귀함수 이용 - 트리 내용 출력
    void PrintTree<T>(D05_TreeNode<T> root)
    {
        Debug.Log(root.Data);                   // 루트 노드의 내용을 출력한다

        foreach (var child in root.Children)    // 노드의 각 자식마다
            PrintTree<T>(child);                // 이 작업(출력 후 자식 탐색)을 반복한다.
    }

    public override void TestCode02()
    {
        code2 = "트리 구현 연습 - 트리 높이 구하기";

        if (onStartTreeClear && root != null) root = null;
        MakeTree();
        Debug.Log("트리 높이 : " + GetHeight(root));
    }

    // 재귀함수 이용 2 - 트리 높이 구하기
    int GetHeight<T>(D05_TreeNode<T> root)
    {
        int childHeight = 0;

        if (root.Children.Count == 0) return 0;

        foreach (var child in root.Children)
            childHeight = Mathf.Max(childHeight, GetHeight(child));

        return childHeight + 1;
    }

    //----------------------------------------------------------------------------






    // 3. 힙 이론

    // 이진 검색 트리 : 하나의 노드에 두 개의 자식 노드가 연결된 형태의 반복으로, 자식 트리의 한 쪽은 부모보다 작은 값이, 다른 쪽은 부모보다 큰 값이 온다.
    //                  최상위 노드를 40으로 해서 [40 -> 35, 60], [35 -> 22, 37], [60 -> 50, 77] 과 같은 형태로 연결되어 있다고 가정할 때,
    //                  40보다 작은 값을 찾으려면 40 -> 60으로 연결된 부분은 탐색할 필요가 없다.
    //      문제점: 특정 범위의 값을 가진 노드 수가 많아지면 균형이 깨진다. 
    //              100개의 데이터가 90:10으로 나뉘면, 90개가 들어있는 쪽의 노드들을 탐색하는 것은 사실상 리스트 탐색과 비용이 다를 바가 없다. (50:50 이면 탐색 비용이 절반이지만...)

    // 힙 트리 : 이진 검색 트리의 문제점을 보완한 형태
    //              1) 부모 노드가 가진 값은 항상 자식 노드가 가진 값보다 크다.
    //              2) 마지막 레벨(가장 깊이가 큰 최하위 노드들)을 제외한 모든 레벨에 노드가 꽉 차 있어야 하고, 마지막 레벨에 노드가 있을 때는 항상 왼쪽부터 순서대로 채워야 한다.
    //                      -> 따라서 노드 개수만 알면, 트리 구조는 무조건 확정된다.
    //                      -> 따라서 배열을 이용하여 힙 구조를 바로 표현할 수 있다.  
    //                                  -> [i]번 노드의 왼쪽 자식 : [2*i + 1] 번
    //                                  -> [i]번 노드의 오른쪽 자식 : [2*i + 2] 번
    //                                  -> [i]번 노드의 부모 : [(i-1) / 2] 번

    // 힙 트리에 새로운 값 추가하기
    // 1. 새 노드가 들어갈 위치는 정해져 있다. 해당 위치에 새 노드를 추가한다.
    // 2. 새 노드와 그 부모 노드의 값을 비교해서, 새 노드가 더 크다면 자리를 바꾼다. 새 노드가 부모 노드보다 작을 때까지 반복한다.

    // 힙 트리에서 값 빼기 - 최대값 빼기
    // 1. 최대값은 힙 트리의 루트(최상위 노드)일 것이다. 우선 이를 제거한다.
    // 2. 제일 마지막에 위치한 노드를 루트로 옮긴다. 이러면 힙 트리의 구조는 유지된다.
    // 3. 부모 노드, 왼쪽 및 오른쪽 자식 노드를 비교하여, 가장 큰 값을 부모 노드로 옮긴다. 루트 노드에서 시작하여 부모 노드가 제일 크다는 결과가 나올 때까지 반복한다.


    public override void TestCode03()
    {
        code3 = "힙 트리를 이용한 우선순위 큐 구현 - 데이터 삽입/추출";

        D05_PriorityIntQueue q = new D05_PriorityIntQueue();

        q.Push(40);
        q.Push(25);
        q.Push(67);
        q.Push(70);
        q.Push(99);
        q.Push(36);
        q.Push(4);
        q.Push(13);
        q.Push(22);
        q.Push(56);
        // Tip) 만일 작은 순으로 추출하고 싶으면 Push할 때 마이너스를 붙여 -40, -25, -67, ... 등으로 입력하고, Pop으로 꺼낼 때 다시 마이너스를 붙여 원래 값으로 사용하면 된다.

        while(q.Count > 0)
        {
            Debug.Log(q.Pop());
        }
    }
    // 힙 트리는 리스트 탐색에 비해 속도가 빠르다. n개의 데이터를 log2(n) 회의 탐색만으로 찾을 수 있어서 시간 복잡도가 줄어들기 때문이다.






    //----------------------------------------------------------------------------

    // 3.1. 힙 트리 응용 - 제네릭 형식으로 만들기

    // IComparable 인터페이스를 이용하여 우선순위를 비교한다. (int에는 기본적으로 적용되어 있다)

    public override void TestCode04()
    {
        code4 = "힙 트리 응용 - 제네릭으로 3번 코드 구현, 낮은 숫자 순으로";

        D05_PriorityQueue<int> q = new D05_PriorityQueue<int>();

        q.Push(-40);
        q.Push(-25);
        q.Push(-67);
        q.Push(-70);
        q.Push(-99);
        q.Push(-36);
        q.Push(-4);
        q.Push(-13);
        q.Push(-22);
        q.Push(-56);
        // 코드 3번의 팁 적용

        while (q.Count > 0)
        {
            Debug.Log(-q.Pop());
        }
    }


    public override void TestCode05()
    {
        code5 = "힙 트리 응용 - 제네릭으로 직접 만든 클래스를 ID로 비교하기";

        D05_PriorityQueue<D05_Knight> q = new D05_PriorityQueue<D05_Knight>();

        q.Push(new D05_Knight() { Id = 20 });
        q.Push(new D05_Knight() { Id = 5 });
        q.Push(new D05_Knight() { Id = 35 });
        q.Push(new D05_Knight() { Id = 25 });
        q.Push(new D05_Knight() { Id = 40 });
        q.Push(new D05_Knight() { Id = 10 });

        while (q.Count > 0)
        {
            Debug.Log(q.Pop().Id);
        }
    }
}






class D05_TreeNode<T>
{
    public T Data { get; set; }
    public List<D05_TreeNode<T>> Children { get; set; } = new List<D05_TreeNode<T>>(); // 자식 트리들을 저장할 클래스. 계층적 구조이므로 부모 노드를 저장할 필요는 없다. 
}

// 힙 트리를 이용한 우선순위 큐 구현하기 (힙 트리의 값을 정수로 가정하고, 우선순위 가중치로 간주한다)
class D05_PriorityIntQueue
{
    List<int> heap = new List<int>();

    int ParentIndex(int child)
    {
        int parent = (child - 1) / 2;
        return parent;
    }
    int ChildIndexR(int parent)
    {
        int child = (parent * 2) + 2;
        return child;
    }
    int ChildIndexL(int parent)
    {
        int child = (parent * 2) + 1;
        return child;
    }

    /// <summary>
    /// 입력된 데이터를 노드로 추가하고, 데이터의 우선순위에 따라 위치를 조정한다.
    /// </summary>
    public void Push(int data)
    {
        heap.Add(data);

        int dataIndex = heap.Count - 1;

        while (dataIndex > 0)
        {
            if (data < heap[ParentIndex(dataIndex)]) break;

            int temp = heap[ParentIndex(dataIndex)];
            heap[ParentIndex(dataIndex)] = data;
            heap[dataIndex] = temp;

            dataIndex = ParentIndex(dataIndex);
        }
    }

    /// <summary>
    /// 가장 높은 우선순위를 갖는 최상위 노드를 반환하고, 리스트에서 제거한다.
    /// </summary>
    public int Pop()
    {
        int pop = heap[0];

        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);
        lastIndex--;

        int now = 0;

        while(true)
        {
            int next = now;
            if (ChildIndexL(now) <= lastIndex && heap[ChildIndexL(now)] > heap[next])
                next = ChildIndexL(now);

            if (ChildIndexR(now) <= lastIndex && heap[ChildIndexR(now)] > heap[next])
                next = ChildIndexR(now);

            if (next == now) break;

            int temp = heap[next];
            heap[next] = heap[now];
            heap[now] = temp;

            now = next;
        }
        
        return pop;
    }

    public int Count => heap.Count;
}






// 우선순위 큐가 int가 아닌 제네릭이라면, 데이터 우선순위를 어떻게 계산할지 문제가 생긴다.
// 이는 System에서 기본 제공되는 IComparable 인터페이스를 이용한다. 이 인터페이스를 상속받은 모든 요소에 대해 비교가 가능하다.

class D05_Knight : IComparable<D05_Knight>
{
    public int Id { get; set; }

    // 이 인터페이스는 CompareTo() 구현을 포함한다. '대상.CompareTo(비교대상)' 형태로 사용하며, -1,0,1 중 하나의 값을 반환해야 한다.
    public int CompareTo(D05_Knight other)
    {
        if (Id == other.Id) return 0;

        return Id > other.Id ? 1 : -1; //이 오브젝트의 아이디가 비교대상의 아이디보다 크면 1, 작으면 -1을 반환한다.
    }
}

//위의 int 우선순위 큐를 수정하여, 제네릭 형식으로 만들어보자.
class D05_PriorityQueue<T> where T : IComparable<T> 
{
    // 데이터는 전부 int에서 T로 바꾼다.
    List<T> heap = new List<T>();

    int ParentIndex(int child)
    {
        int parent = (child - 1) / 2;
        return parent;
    }
    int ChildIndexR(int parent)
    {
        int child = (parent * 2) + 2;
        return child;
    }
    int ChildIndexL(int parent)
    {
        int child = (parent * 2) + 1;
        return child;
    }

    public void Push(T data)
    {
        heap.Add(data);

        int dataIndex = heap.Count - 1;

        while (dataIndex > 0)
        {
            //수정된 부분:  현재 데이터가 숫자가 아니니 직접 비교가 불가능하므로, '현재 데이터.CompareTo(비교대상)' 로 비교한다.
            //              현재 데이터가 우선순위가 낮으면 -1이 반환될 것이다.
            if (data.CompareTo(heap[ParentIndex(dataIndex)]) < 0) break;

            T temp = heap[ParentIndex(dataIndex)];
            heap[ParentIndex(dataIndex)] = data;
            heap[dataIndex] = temp;

            dataIndex = ParentIndex(dataIndex);
        }
    }

    public T Pop()
    {
        T pop = heap[0];

        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);
        lastIndex--;

        int now = 0;

        while (true)
        {
            int next = now;
            //수정된 부분 :  리스트 내 값끼리 비교가 불가능하므로, 마찬가지로 CompareTo로 비교한다.
            //               앞의 int 형에서 A < B 형태였을 경우, 지금은 A.CompareTo(B) < 0 으로 바꾸면 된다.
            if (ChildIndexL(now) <= lastIndex && heap[ChildIndexL(now)].CompareTo(heap[next]) > 0)
                next = ChildIndexL(now);

            if (ChildIndexR(now) <= lastIndex && heap[ChildIndexR(now)].CompareTo(heap[next]) > 0)
                next = ChildIndexR(now);

            if (next == now) break;

            T temp = heap[next];
            heap[next] = heap[now];
            heap[now] = temp;

            now = next;
        }

        return pop;
    }

    public int Count => heap.Count;
}