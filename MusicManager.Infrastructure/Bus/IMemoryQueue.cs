using System;

namespace MusicManager.Infrastructure.Bus
{
    internal interface IMemoryQueue
    {
        void Put(object item);
        void Pop(Action<object> popAction);
    }
}