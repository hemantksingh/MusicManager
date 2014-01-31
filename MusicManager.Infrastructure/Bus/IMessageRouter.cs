namespace MusicManager.Infrastructure.Bus
{
    public interface IMessageRouter
    {
        void Route(object message);
    }
}
