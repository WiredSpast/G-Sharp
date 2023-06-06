using GSharp.Misc;
using GSharp.Protocol;
using GSharp.Services.PacketInfo;
using System;
using System.ComponentModel;
using System.Reflection;
using GSharp.Misc.listenerpattern;

namespace GSharp.Extensions
{
    // code: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/observableobject
    // https://stackoverflow.com/questions/10093180/observableobject-or-inotifypropertychanged-on-viewmodels
    public class ObservableObject<T> : INotifyPropertyChanged
    {
        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

#pragma warning disable CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class ExtensionBase
    {
        public interface MessageListener
        {
            void act(HMessage message);
        }

        public interface FlagsCheckListener
        {
            void act(String[] args);
        }

        protected readonly Dictionary<int, List<Action<HMessage>>> incomingMessageListeners = new Dictionary<int, List<Action<HMessage>>>();
        protected readonly Dictionary<int, List<Action<HMessage>>> outgoingMessageListeners = new Dictionary<int, List<Action<HMessage>>>();

        protected readonly Dictionary<string, List<Action<HMessage>>> hashOrNameIncomingListeners = new Dictionary<string, List<Action<HMessage>>>();
        protected readonly Dictionary<string, List<Action<HMessage>>> hashOrNameOutgoingListeners = new Dictionary<string, List<Action<HMessage>>>();

        volatile PacketInfoManager packetInfoManager = PacketInfoManager.EMPTY;
        // TODO: Observable hostInfo (Line 36 ExtensionBase.java)
        ObservableObject<HostInfo> observableHostInfo = new ObservableObject<HostInfo>();

        public void UpdateHostInfo(HostInfo hostInfo)
        {
            ObservableObject<HostInfo> observableHostInfo = new ObservableObject<HostInfo>();
        }

        public void Intercept(HDirection direction, int headerId, Action<HMessage> messageListener)
        {
            Dictionary<int, List<Action<HMessage>>> listeners = direction == HDirection.TOCLIENT ? incomingMessageListeners : outgoingMessageListeners;

            lock (listeners)
            {
                if (!listeners.ContainsKey(headerId))
                {
                    listeners[headerId] = new List<Action<HMessage>>();
                }

                listeners[headerId].Add(messageListener);
            }
        }

        public void Intercept(HDirection direction, string hashOrName, Action<HMessage> messageListener)
        {
            Dictionary<string, List<Action<HMessage>>> listeners = direction == HDirection.TOCLIENT ?
                hashOrNameIncomingListeners :
                hashOrNameOutgoingListeners;

            lock (listeners)
            {
                if (!listeners.ContainsKey(hashOrName))
                {
                    listeners[hashOrName] = new List<Action<HMessage>>();
                }

                listeners[hashOrName].Add(messageListener);
            }
        }

        public void Intercept(HDirection direction, Action<HMessage> messageListener)
        {
            Intercept(direction, -1, messageListener);
        }

        public void WriteToConsole(String s)
        {
            Console.WriteLine(s);
        }

        protected bool IsOnClickMethodUsed()
        {
            Type type = GetType();
            while (type != typeof(Extension))
            {
                try
                {
                    type.GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic);
            
                    return true;
                }
                // Thanks https://stackoverflow.com/questions/19005956/is-there-a-c-sharp-equivalent-for-javas-nosuchelementexception
                // for the Catch exception
                catch (InvalidOperationException)
                {
                }

                type = type.BaseType;
            }

            return false;
        }

        public void ModifyMessage(HMessage habboMessage)
        {
            HPacket habboPacket = habboMessage.GetPacket();

            Dictionary<int, List<Action<HMessage>>> listeners =
                habboMessage.getDestination() == HDirection.TOCLIENT ?
                    incomingMessageListeners :
                    outgoingMessageListeners;

            Dictionary<string, List<Action<HMessage>>> hashOrNameListeners =
                habboMessage.getDestination() == HDirection.TOCLIENT ?
                    hashOrNameIncomingListeners :
                    hashOrNameOutgoingListeners;

            HashSet<Action<HMessage>> correctListeners = new HashSet<Action<HMessage>>();

            lock (listeners)
            {
                if (listeners.ContainsKey(-1)) // Registered on all packets
                {
                    for (int i = listeners[-1].Count - 1; i >= 0; i--)
                    {
                        correctListeners.Add(listeners[-1][i]);
                    }
                }

                if (listeners.ContainsKey(habboPacket.HeaderId))
                {
                    for (int i = listeners[habboPacket.HeaderId].Count - 1; i >= 0; i--)
                    {
                        correctListeners.Add(listeners[habboPacket.HeaderId][i]);
                    }
                }
            }

            lock (hashOrNameListeners)
            {
                List<PacketInfo> packetInfos = packetInfoManager.GetAllPacketInfoFromHeaderId(habboMessage.getDestination(), habboPacket.HeaderId);

                List<string> identifiers = new List<string>();
                foreach (PacketInfo packetInfo in packetInfos)
                {
                    string name = packetInfo.GetName();
                    string hash = packetInfo.GetHash();
                    if (name != null && hashOrNameListeners.ContainsKey(name))
                    {
                        identifiers.Add(name);
                    }
                    if (hash != null && hashOrNameListeners.ContainsKey(hash))
                    {
                        identifiers.Add(hash);
                    }
                }

                foreach (string identifier in identifiers)
                {
                    for (int i = hashOrNameListeners[identifier].Count - 1; i >= 0; i--)
                    {
                        correctListeners.Add(hashOrNameListeners[identifier][i]);
                    }
                }
            }

            foreach (Action<HMessage> listener in correctListeners)
            {
                habboMessage.GetPacket().ReadIndex = 6;
                listener.Invoke(habboMessage);
            }
            habboMessage.GetPacket().ReadIndex = 6;
        }

        protected void OnClick() { }

        protected ExtensionInfo getInfoAnnotations()
        {
            return GetType().GetCustomAttributes(typeof(ExtensionInfo), true).FirstOrDefault() as ExtensionInfo;
        }

        private Observable<OnConnectionListener> onConnectionObservable = new Observable<OnConnectionListener>();

        public void OnConnect(OnConnectionListener listener)
        {
            onConnectionObservable.AddListener(listener);
        }

        public Observable<OnConnectionListener> GetOnConnectionObservable()
        {
            return onConnectionObservable;
        }

        protected void SetPacketInfoManager(PacketInfoManager packetInfoManager)
        {
            this.packetInfoManager = packetInfoManager;
        }

        public PacketInfoManager GetPacketInfoManager()
        {
            return packetInfoManager;
        }

        public HostInfo GetHostInfo()
        {
            return observableHostInfo.Value;
        }

    }
}
