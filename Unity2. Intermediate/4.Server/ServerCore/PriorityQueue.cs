using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
	public class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> heap = new List<T>();
        public int Count => heap.Count;

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

        public T Peek()
		{
            if (heap.Count == 0)
                return default(T);
            
            return heap[0];
		}
    }
}
