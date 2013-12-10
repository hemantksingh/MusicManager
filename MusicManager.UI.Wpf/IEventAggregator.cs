using System;

namespace MusicManager.UI.Wpf
{
    public interface IEventAggregator
    {
        void Subscribe<T>(object subscriber, Action<T> actionToPerform);
        void Publish<T>(T message);
        void Unsubscribe(object subscriber);
    }
}