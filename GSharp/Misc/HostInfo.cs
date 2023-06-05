using GSharp.Protocol;

namespace GSharp.Misc
{
    public class HostInfo
    {
        private String _packetLogger;
        private String _version;
        private Dictionary<string, string> attributes;

        public HostInfo(String  packetLogger, String version, Dictionary<String, String> attributes)
        {
            this._packetLogger = packetLogger;
            this._version = version;
            this.attributes = attributes;
        }

        public static HostInfo FromPacket(HPacket packet)
        {
            string packetLogger = packet.ReadString();
            string version = packet.ReadString();
            int attributeCount = packet.ReadInt();
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            for(int i = 0; i < attributeCount; i++)
            {
                string key = packet.ReadString();
                string value = packet.ReadString();
                attributes[key] = value;
            }


            return new HostInfo(packetLogger, version, attributes);
        }

        public void appendToPacket(HPacket packet)
        {
            packet.Append(this._packetLogger);
            packet.Append(this._version);
            packet.Append(this.attributes.Count);

            foreach(string key in attributes.Keys)
            {
                packet.Append(key.ToString());
                packet.Append(attributes[key].ToString());
            }
        }

        public String GetPacketLogger()
        {
            return this._packetLogger;
        }

        public string GetVersion()
        {
            return this._version;
        }

        public Dictionary<string, string> GetAttributes()
        {
            return attributes;
        }


    }
}
