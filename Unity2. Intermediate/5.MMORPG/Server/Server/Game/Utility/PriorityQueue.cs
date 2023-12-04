namespace Server.Game.Utility; 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T> : IEnumerable<T> {
	private List<T> heap = new List<T>();

	public PriorityQueue(Comparer<T> comparer = null) {
		Comparision = comparer ?? Comparer<T>.Default;
	}

	public PriorityQueue(Comparison<T> comparison) {
		Comparision = Comparer<T>.Create(comparison);
	}

	public int Count => heap.Count;
	private Comparer<T> Comparision { get; }

	public void Enqueue(T data) {
		heap.Add(data);

		if (heap.Count == 1) return;

		// 힙 트리의 맨 끝에 데이터를 삽입한 후 정렬한다 
		int curIndex = heap.Count - 1;
		while (curIndex > 0) {
			int parentIndex = (curIndex - 1) / 2;

			// 부모 노드보다 우선순위가 높으면 인덱스를 바꾸고, 아니라면 중단
			if (Comparision.Compare(heap[curIndex], heap[parentIndex]) < 0) break;

			(heap[curIndex], heap[parentIndex]) = (heap[parentIndex], heap[curIndex]);
			curIndex = parentIndex;
		}
	}

	public T Dequeue() {
		T ret = heap[0];

		if (heap.Count == 1) {
			heap.Remove(ret);
			return ret;
		}

		// 마지막 데이터를 루트로 이동한 후 재정렬한다.
		int curIndex = 0;
		int lastIndex = heap.Count - 1;
		heap[curIndex] = heap[lastIndex];
		heap.RemoveAt(lastIndex);

		lastIndex--;

		while (true) {
			int leftChild = (2 * curIndex) + 1;
			int rightChild = (2 * curIndex) + 2;
			int nextIndex = curIndex;

			// 자식 노드가 나보다 크면 자리를 바꾼다. 둘 다 확인해야 하며 오른쪽 노드가 더 크므로 나중에 확인한다. 둘 다 나보다 작으면 중단한다.
			if (leftChild <= lastIndex && Comparision.Compare(heap[nextIndex], heap[leftChild]) < 0)
				nextIndex = leftChild;
			if (rightChild <= lastIndex && Comparision.Compare(heap[nextIndex], heap[rightChild]) < 0)
				nextIndex = rightChild;
			if (nextIndex == curIndex) break;

			(heap[curIndex], heap[nextIndex]) = (heap[nextIndex], heap[curIndex]);
			curIndex = nextIndex;
		}

		return ret;
	}

	public T Peek() => heap[0];

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/** 컨테이너의 모든 요소를 우선순위가 높은 것부터 정렬한 리스트를 반환합니다. 원본 PriorityQueue는 변경되지 않습니다. */
	public List<T> GetSortedList() {
		var ret = heap.ToList();
		// Comparision은 우선순위가 높은게 0번에 오는데, Sort는 기본적으로 오름차순으로 정렬된다. 따라서 Comparision의 결과를 반대로 적용한다.
		ret.Sort((e1, e2) => -Comparision.Compare(e1, e2));
		return ret;
	}

	public IEnumerator<T> GetEnumerator() {
		// 원본 배열을 수정하지 않도록 복사해서 반환한다
		var sortedHeap = GetSortedList();

		foreach (var element in sortedHeap) {
			yield return element;
		}
	}
}
