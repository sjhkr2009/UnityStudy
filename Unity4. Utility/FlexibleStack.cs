using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

/// <summary>
/// Stack 자료구조를 유연하게 사용하기 위해, 특정 인덱스에 삽입, 특정 원소의 탐색이나 삭제, 예외처리 등을 추가한 확장된 기능을 제공합니다.
/// Push/Pop/Peek 등 스택의 API 이외의 방식으로 원소에 접근하는 것은 Stack과 마찬가지로 O(N)의 시간복잡도를 가지니 필요한 경우에만 사용하세요.
/// </summary>
public class FlexibleStack<T> : IEnumerable<T> {
    private LinkedList<T> InternalList { get; }= new LinkedList<T>();

    public int Count => InternalList.Count;

    public bool Contains(T item) => InternalList.Contains(item);

    public void Push(T item) => InternalList.AddLast(item);
    
    public T Pop(Predicate<T> condition) {
        var item = FindNode(condition);
        if (item == null) return default;

        InternalList.Remove(item);
        return item.Value;
    }
    
    public T Pop() {
        if (Count == 0) return default;
        
        var item = InternalList.Last.Value;
        InternalList.RemoveLast();
        return item;
    }
    
    public T Peek() {
        return Count == 0 ? default : InternalList.Last.Value;
    }

    public bool TryPop(out T item) {
        var result = Count > 0;
        item = Pop();
        return result;
    }
    
    public T PopOrDefault(T defaultValue) => TryPop(out var item) ? item : defaultValue;

    public bool TryPeek(out T item) {
        var result = Count > 0;
        item = Peek();
        return result;
    }
    
    public T PeekOrDefault(T defaultValue = default) => TryPeek(out var item) ? item : defaultValue;
    
    public bool Remove(T item) {
        return InternalList.Remove(item);
    }
    
    /** 스택에서 해당하는 요소를 추가적인 메모리 할당 없이 모두 제거합니다. */
    public int RemoveIf(Predicate<T> condition) {
        if (Count == 0 || condition == null) return 0;

        var currentNode = InternalList.Last;
        var removeCount = 0;
        
        // 반복문을 사용하는 대신 노드를 순회한다.
        // 반복문을 사용 시 반복 중에 리스트의 요소가 삭제되면 안 되고, 삭제할 대상을 미리 별도의 리스트로 만들어두면 불필요하게 메모리에 새 배열이 할당되기 때문.
        while (currentNode != null) {
            var nextNode = currentNode.Previous;
            if (condition.Invoke(currentNode.Value)) {
                InternalList.Remove(currentNode);
                removeCount++;
            }

            currentNode = nextNode;
        }

        return removeCount;
    }
    
    /** 스택의 맨 위에서부터 해당되는 첫 번째 요소를 찾아 반환합니다. 스택에서 제거되지는 않습니다. */
    public T Find(T item) => Find(e => e.Equals(item));
    
    /** 스택의 맨 위에서부터 condition에 해당하는 첫 번째 요소를 찾아 반환합니다. 스택에서 제거되지는 않습니다. */
    public T Find(Predicate<T> condition) {
        var node = FindNode(condition);
        return node == null ? default : node.Value;
    }

    private LinkedListNode<T> FindNode(T item) => FindNode(e => e.Equals(item));
    private LinkedListNode<T> FindNode(Predicate<T> condition) {
        if (condition == null) return default;
        
        var currentNode = InternalList.Last;
        while (currentNode != null) {
            if (condition.Invoke(currentNode.Value)) return currentNode;

            currentNode = currentNode.Previous;
        }

        return null;
    }
    
    /** 스택의 위에서 특정 순서에 있는 요소를 반환합니다. 스택에서 제거되지는 않습니다. */
    public T IndexOf(int index) {
        return InternalList.Reverse().ElementAtOrDefault(index);
    }
    
    /** 특정 순서에 요소를 삽입합니다. 스택의 제일 위에서부터 index 번째에, 없으면 맨 위에 삽입합니다. */
    public void Insert(T item, int index) {
        if (index < 0 || index >= Count) {
            InternalList.AddLast(item);
            return;
        }
        
        var targetNode = InternalList.Last;
        for (var curIndex = 0; curIndex < index; curIndex++) {
            targetNode = targetNode?.Previous;
        }

        if (targetNode == null) {
            InternalList.AddLast(item);
            return;
        }

        InternalList.AddAfter(targetNode, item);
    }
    
    /** 스택 자료구조의 특성에 맞게 나중에 Push된 요소가 먼저 실행됩니다. 역순으로 실행하길 원하면 ForEachReverse를 사용하세요. */
    public void ForEach(Action<T> action) {
        if (action == null) return;
        
        foreach (var element in InternalList.Reverse()) {
            action.Invoke(element);
        }
    }
    
    /** 스택에 먼저 들어온 순으로 순회합니다. 일반적인 스택의 작동방식과 반대 순서입니다. */
    public void ForEachReverse(Action<T> action) {
        if (action == null) return;
        
        foreach (var element in InternalList) {
            action.Invoke(element);
        }
    }
    
    public void Clear() => InternalList.Clear();

    public IEnumerator<T> GetEnumerator() => InternalList.Reverse().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => InternalList.Reverse().GetEnumerator();
}