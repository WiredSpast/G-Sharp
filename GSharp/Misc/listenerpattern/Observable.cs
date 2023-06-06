namespace GSharp.Misc.listenerpattern
{
    public class Observable<Listener>
    {
        private Action<Listener> defaultConsumer = null;

        public Observable(Action<Listener> defaultConsumer)
        {
            this.defaultConsumer = defaultConsumer;
        }

        public Observable()
            : this(listener => { })
        {
        }

        private List<Listener> listeners = new List<Listener>();

        public void AddListener(Listener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(Listener listener)
        {
            listeners.Remove(listener);
        }

        public void FireEvent(Action<Listener> consumer)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                consumer(listeners[i]);
            }
        }

        public void FireEvent()
        {
            FireEvent(defaultConsumer);
        }
    }
}
