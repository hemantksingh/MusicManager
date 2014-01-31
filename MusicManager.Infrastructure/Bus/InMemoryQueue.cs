using System;
using System.Collections.Generic;

namespace MusicManager.Infrastructure.Bus
{
    class InMemoryQueue : IMemoryQueue
    {
        private readonly Queue<object> _itemQueue;
        private readonly Queue<Action<object>> _listenerQueue;

        public InMemoryQueue()
        {
            _itemQueue = new Queue<object>(32);
            _listenerQueue = new Queue<Action<object>>(32);
        }

        public void Put(object item)
        {
            if (_listenerQueue.Count == 0)
            {
                _itemQueue.Enqueue(item);                
                return;
            }
            Action<object> listener = _listenerQueue.Dequeue();
            listener(item);
        }
        
        public void Pop(Action<object> popAction)
        {
            if (_itemQueue.Count == 0)
            {
                _listenerQueue.Enqueue(popAction);
                return;
            }
            object item = _itemQueue.Dequeue();
            popAction(item);
        }
    }
}
