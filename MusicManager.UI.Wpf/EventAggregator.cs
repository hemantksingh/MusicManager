using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicManager.UI.Wpf
{
    public class EventAggregator : IEventAggregator
    {
        private readonly IList<Subscription> _subscriptions = new List<Subscription>();

        public void Subscribe<T>(object subscriber, Action<T> actionToPerform)
        {
            CleanUpSubscriptions();
            // Add subscriptions through weak reference to enable garbage collection of the subscribers
            // that have been disposed without unsubscribing. A strong reference would prevent the subscribers
            // from being garbage collected and perform unsolicited actions when messages are published.
            _subscriptions.Add(new Subscription(new WeakReference(subscriber), actionToPerform, typeof (T)));
        }

        public void Publish<T>(T message)
        {
            CleanUpSubscriptions();
            IEnumerable<Subscription> subscriptions = _subscriptions.Where(
                subscription => subscription.MessageType == message.GetType());

            foreach (Subscription subscription in subscriptions)
            {
                subscription.Invoke(message);
            }
        }

        public void Unsubscribe(object subscriber)
        {
            IEnumerable<Subscription> subscriptionsForSubscriber = _subscriptions.Where(
                subscription => subscription.Target == subscriber);

            RemoveSubscriptions(subscriptionsForSubscriber);
        }

        /// <summary>
        ///     Removes all the subscriptions where the subscriber has been garbage collected.
        /// </summary>
        private void CleanUpSubscriptions()
        {
            List<Subscription> subscriptionsToClean = _subscriptions.Where(q => q.Target.IsAlive == false).ToList();
            RemoveSubscriptions(subscriptionsToClean);
        }

        private void RemoveSubscriptions(IEnumerable<Subscription> subscriptionsToClean)
        {
            foreach (Subscription subscription in subscriptionsToClean.ToList())
            {
                _subscriptions.Remove(subscription);
            }
        }
    }
}