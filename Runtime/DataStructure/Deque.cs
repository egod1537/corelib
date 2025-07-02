using System.Collections.Generic;

namespace Corelib.Utils
{
    public class Deque<T>
    {
        private LinkedList<T> list = new();

        public void PushFront(T item) => list.AddFirst(item);
        public void PushBack(T item) => list.AddLast(item);

        public T PopFront()
        {
            T value = list.First.Value;
            list.RemoveFirst();
            return value;
        }

        public T PopBack()
        {
            T value = list.Last.Value;
            list.RemoveLast();
            return value;
        }

        public int Count => list.Count;
        public T Front => list.First.Value;
        public T Back => list.Last.Value;
    }
}