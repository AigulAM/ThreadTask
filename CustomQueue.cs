using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadTask
{

    public class CustomQueue<T>
    {
        private object _lock = new object();

        public AutoResetEvent AutoResetEvent { get; }

        public Queue<T> Queue => _queue;

        public StatusWork Status;
        private readonly Queue<T> _queue = new Queue<T>();

        public CustomQueue(AutoResetEvent resetEvent)
        {
            AutoResetEvent = resetEvent;
        }
        public bool TryDequeue(out T result)
        {
            lock (_lock)
            {
                if (Queue.Count != 0)
                {
                    result = Queue.Dequeue();
                    return true;
                }


                result = default;
                return false;
                
                 
            }


        }

        public int Count()
        {
            return Queue.Count();
        }

        public void Enqueue(T item)
        {
            lock (_lock)
            {
                 Queue.Enqueue(item);
                 AutoResetEvent.Set();
            }


        }

        
    }
}
