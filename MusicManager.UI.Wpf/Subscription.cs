using System;

namespace MusicManager.UI.Wpf
{
    class Subscription
    {
        public WeakReference Target { get; private set; }
        public MulticastDelegate MulticastDelegate { get; private set; }
        public Type MessageType { get; private set; }

        public Subscription(WeakReference target, MulticastDelegate multicastDelegate, Type messageType)
        {
            Target = target;
            MulticastDelegate = multicastDelegate;
            MessageType = messageType;
        }

        public void Invoke(object message)
        {
            this.MulticastDelegate.DynamicInvoke(message);
        }

        public override string ToString()
        {
            return MessageType.Name + " - " + Target.Target.GetType().Name;
        }
    }
}
