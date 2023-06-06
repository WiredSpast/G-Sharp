using GSharp.Misc;
using GSharp.Protocol.Connection;
using GSharp.Protocol;
using GSharp.Services.PacketInfo;
using Microsoft.VisualBasic;
using System.Net.Sockets;
using System.Text;
using System.Linq.Expressions;
using System.Net;
using System.ComponentModel;
using GSharp.Services;
using System.Threading.Tasks.Dataflow;

namespace GSharp.Extensions
{
    public abstract class Extension : ExtensionBase
    {
        protected FlagsCheckListener flagRequestCallback = null;
        // BuildStr -> https://www.appsloveworld.com/csharp/100/924/does-c-have-anything-similar-to-javas-string-valueof
        public string BuildStr(string s)
        {
            Stack<char> stack = new Stack<char>(); // USE GENERIC HERE

            foreach (char x in s.ToCharArray())
            {
                if (x != '$')
                {
                    stack.Push(x);
                }
                else
                    stack.Pop();
            }
            return string.Join<char>("", stack);
        }

        private sealed class IncomingMessageIds
        {
            public const ushort ON_DOUBLE_CLICK = 1;
            public const ushort INFO_REQUEST = 2;
            public const ushort PACKET_INTERCEPT = 3;
            public const ushort FLAGS_CHECK = 4;
            public const ushort CONNECTION_START = 5;
            public const ushort CONNECTION_END = 6;
            public const ushort INIT = 7;
            public const ushort UPDATE_HOST_INFO = 10;
            public const ushort PACKET_TO_STRING_RESPONSE = 20;
            public const ushort STRING_TO_PACKET_RESPONSE = 21;
        }

        private sealed class OutgoingMessageIds
        {
            public const ushort EXTENSION_INFO = 1;
            public const ushort MANIPULATED_PACKET = 2;
            public const ushort REQUEST_FLAGS = 3;
            public const ushort SEND_MESSAGE = 4;
            public const ushort PACKET_TO_STRING_REQUEST = 20;
            public const ushort STRING_TO_PACKET_REQUEST = 21;
            public const ushort EXTENSION_CONSOLE_LOG = 98;
        }

        //protected FlagsCheckListener flagRequestCallback = null;

        private string[] _args;
        private volatile bool _isCorrupted = false;
        private static readonly string[] _PORT_FLAG = { "--port", "-p" };
        private static readonly string[] _FILE_FLAG = { "--filename", "-f" };
        private static readonly string[] _COOKIE_FLAG = { "--auth-token", "-c" };

        private volatile bool _delayed_init = false;
        
        private Socket? _socket;
        private static Boolean UNITY_PACKETS = false;

        // private Stream _outStream = null;

        private String GetArgument(String[] args, params String[] arg)
        {
            for(int i = 0; i < args.Length - 1; i++)
            {
                foreach(string str in arg)
                {
                    if (args[i].ToLower() == str.ToLower())
                    {
                        return args[i + 1];
                    }
                }
            }

            return "";
        }

        public Extension(string[] args)
        {
            this._args = args;

            if (getInfoAnnotations() == null)
            {
                Console.WriteLine("Extension info not found\n\n" +
                    "Usage:\n" +
                    "@ExtensionInfo ( \n" +
                    "       Title =  \"...\",\n" +
                    "       Description =  \"...\",\n" +
                    "       Version =  \"...\",\n" +
                    "       Author =  \"...\"" +
                    "\n)");
                this._isCorrupted = true;
            }

            if (GetArgument(args, _PORT_FLAG) == null)
            {
                Console.WriteLine("Don't forget to include G-Earth's port in your program parameters (-p {port})");
                this._isCorrupted = true;
            }
        }

        //public void run()
        //{
        //    if (this._isCorrupted)
        //    {
        //        return;
        //    }

        //    string[] args = Environment.GetCommandLineArgs();
        //    int port = int.Parse(GetArgument(args, _PORT_FLAG));
        //    string file = GetArgument(args, _FILE_FLAG);
        //    string cookie = GetArgument(args, _COOKIE_FLAG);

        //    Socket gEarthExtensionServer = null;

        //    try
        //    {
        //        gEarthExtensionServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        gEarthExtensionServer.NoDelay = true;
        //        gEarthExtensionServer.Connect("127.0.0.1", port);

        //        NetworkStream networkStream = new NetworkStream(gEarthExtensionServer);
        //        BinaryReader reader = new BinaryReader(networkStream);
        //        NetworkStream outputStream = new NetworkStream(gEarthExtensionServer);
        //        BinaryWriter writer = new BinaryWriter(outputStream);

        //        while (gEarthExtensionServer.Connected)
        //        {
        //            int length;
        //            try
        //            {
        //                length = reader.ReadInt32();
        //            }
        //            catch (EndOfStreamException)
        //            {
        //                break;
        //            }

        //            byte[] headerandbody = new byte[length + 4];

        //            int amountRead = 0;

        //            while (amountRead < length)
        //            {
        //                // todo
        //            }

        //            HPacket packet = new HPacket(headerandbody);
        //            packet.FixLength();

        //            if (packet.HeaderId == OutgoingMessageIds.EXTENSION_INFO)
        //            {
        //                ExtensionInfo info = getInfoAnnotations();

        //                HPacket response = new HPacket(IncomingMessageIds.INFO_REQUEST);

        //            }
        //        }
        //    }
        //    catch (IOException e)
        //    {
        //        // toodo
        //    }
        //    finally
        //    {
        //        // todo 
        //    }

        //}

        public void Run()
        {
            if (_socket != null) return;

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 9092);

            _socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _socket.Connect(remoteEP);
            ReadLoop();
        }

        private void ReadLoop()
        {
            new Thread(() =>
            {
                while (true)
                {
                    var lengthBytes = ReadSocketBytes(4);
                    var bodyBytes = ReadSocketBytes(lengthBytes.ToInt());
                    var bytes = lengthBytes.Concat(bodyBytes);
                    OnGPacket(new HPacket(bytes));
                }
            }).Start();
        }

        private byte[] ReadSocketBytes(int size)
        {
            if (_socket == null)
            {
                // somethings wrong if this happens
            }

            byte[] buffer = new byte[size];
            int received = _socket.Receive(buffer);
            if (received != size)
            {
                // somethings wrong if this happens
            }
            return buffer;
        }

        private void OnGPacket(HPacket packet)
        {
            switch (packet.HeaderId)
            {
                case IncomingMessageIds.INFO_REQUEST:
                    Console.WriteLine("INFO_REQUEST");
                    OnInfoRequestPacket();
                    break;
                case IncomingMessageIds.CONNECTION_START:
                    Console.WriteLine("CONNECTION_START");
                    OnConnectionStartPacket(packet);
                    break;
                case IncomingMessageIds.CONNECTION_END:
                    Console.WriteLine("CONNECTION_END");
                    OnConnectionEndPacket();
                    break;
                case IncomingMessageIds.FLAGS_CHECK:
                    Console.WriteLine("FLAGS_CHECK");
                    OnFlagsCheckPacket(packet);
                    break;
                case IncomingMessageIds.INIT:
                    Console.WriteLine("INIT");
                    OnInitPacket(packet);
                    break;
                case IncomingMessageIds.ON_DOUBLE_CLICK:
                    Console.WriteLine("ON_DOUBLE_CLICK");
                    OnDoubleClickPacket();
                    break;
                case IncomingMessageIds.PACKET_INTERCEPT:
                    Console.WriteLine("PACKET_INTERCEPT");
                    OnPacketInterceptPacket(packet);
                    break;
                case IncomingMessageIds.UPDATE_HOST_INFO:
                    Console.WriteLine("UPDATE_HOST_INFO");
                    OnUpdateHostInfoPacket(packet);
                    break;
                case IncomingMessageIds.PACKET_TO_STRING_RESPONSE:
                    OnPacketToStringResponsePacket(packet);
                    break;
                case IncomingMessageIds.STRING_TO_PACKET_RESPONSE:
                    OnStringToPacketResponsePacket(packet);
                    break;
            }
        }

        private void OnInfoRequestPacket()
        {
            var file = GetArgument(_args, _FILE_FLAG);
            var cookie = GetArgument(_args, _COOKIE_FLAG);
            var response = new HPacket(OutgoingMessageIds.EXTENSION_INFO);
            var annotations = getInfoAnnotations();
            response.Append(annotations.Title);
            response.Append(annotations.Author);
            response.Append(annotations.Version);
            response.Append(annotations.Description);
            response.Append(false); // onclick used
            response.Append(file != "");
            response.Append(file);
            response.Append(cookie);
            response.Append(true); // can leave
            response.Append(true); // can delete
            _socket.Send(response.Bytes);
        }

        private void OnConnectionStartPacket(HPacket packet)
        {
            String host = packet.ReadString();
            int connectionPort = packet.ReadInt();
            String hotelVersion = packet.ReadString();
            String clientIdentifier = packet.ReadString();
            HClient clientType = (HClient)Enum.Parse(typeof(HClient), packet.ReadString()); // thanks chatgpt & wiredspast
            SetPacketInfoManager(PacketInfoManager.ReadFromPacket(packet));

            Services.Constants.UNITY_PACKETS = clientType == HClient.UNITY;
            if (this._delayed_init)
            {
                InitExtension();
                this._delayed_init = false;
            }

            GetOnConnectionObservable().FireEvent(l => l.OnConnection(host, connectionPort, hotelVersion, clientIdentifier, clientType));
            OnStartConnection();
        }

        private void OnConnectionEndPacket()
        {
            onEndConnection();
        }

        private void OnFlagsCheckPacket(HPacket packet)
        {
            if (flagRequestCallback != null)
            {
                int arraysize = packet.ReadInt();
                String[] gEarthArgs = new string[arraysize];
                
                for (int i = 0; i < gEarthArgs.Length; i++)
                {
                    gEarthArgs[i] = packet.ReadString();
                }

                flagRequestCallback.act(gEarthArgs);
            }
        }

        private void OnInitPacket(HPacket packet)
        {
            this._delayed_init = packet.ReadBool();
            HostInfo hostinfo = HostInfo.FromPacket(packet);

            UpdateHostInfo(hostinfo);

            if (!this._delayed_init)
            {
                InitExtension();
            }

            WriteToConsole("Extension\"" + getInfoAnnotations().Title + "\" successfully initialized");
        }

        private void OnDoubleClickPacket()
        {
            OnClick();
        }

        private void OnPacketInterceptPacket(HPacket packet)
        {
            String stringifiedMessage = packet.ReadLongString();
            HMessage habboMessage = new HMessage(stringifiedMessage);
            Console.Write("Before: ");
            Console.WriteLine(stringifiedMessage);
            ModifyMessage(habboMessage);
            Console.Write("After: ");
            Console.WriteLine(habboMessage.Stringify());

            HPacket response = new HPacket(OutgoingMessageIds.MANIPULATED_PACKET);
            response.AppendLongString(habboMessage.Stringify());

            _socket.Send(response.Bytes);
        }

        private void OnUpdateHostInfoPacket(HPacket packet)
        {
            HostInfo hostInfo = HostInfo.FromPacket(packet);
            UpdateHostInfo(hostInfo);
        }

        private void OnPacketToStringResponsePacket(HPacket packet)
        {
            // what is this?  you wont find it in there, as far as I know only G-Python has these 2 implemented, should we just remove these, we'l worry about these later not now
            throw new NotImplementedException();
        }

        private void OnStringToPacketResponsePacket(HPacket packet)
        {
            // what is this? i cant find it
            throw new NotImplementedException();
        }

        protected abstract void InitExtension();
        protected abstract void OnStartConnection();
        protected abstract void onEndConnection();

    }
}
