using System.Collections.Generic;

namespace MusicManager.Infrastructure.Bus
{
    public interface IBus : IUnitOfWork
    {
        void Publish(object message);
        void Publish(IEnumerable<object> messages);
    }
}
