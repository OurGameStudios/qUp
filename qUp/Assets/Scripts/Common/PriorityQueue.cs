using System;
using System.Collections.Generic;
using System.Linq;

namespace Common {
    public class PriorityQueue<TPriority, TValue> where TPriority : IComparable {
        
        
        
        private SortedList<TPriority, Queue<TValue>> sortedList = new SortedList<TPriority, Queue<TValue>>();

        public void Add(TPriority priority, TValue value) {
            sortedList.TryGetValue(priority, out var queue);
            if (queue != null) {
                queue.Enqueue(value);
            } else {
                queue = new Queue<TValue>();
                queue.Enqueue(value);
                sortedList.Add(priority, queue);
            }
        }

        public TValue Pop() {
            var pop = sortedList.First();
            var value = pop.Value.Dequeue();
            if (pop.Value.Count == 0) {
                sortedList.Remove(pop.Key);
            }
            return value;
        }

        public bool IsEmpty() => sortedList.Count == 0;
    }
}
