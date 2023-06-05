using GSharp.Protocol;
using System.Xml.Linq;

namespace GSharp.Services.PacketInfo
{
    public class PacketInfo
    {
        private HDirection _destination;
        private int _headerId;
        private string _hash;
        private string _name;
        private string _structure;
        private string _source;

        public PacketInfo(HDirection destination, int headerId, string hash, string name, string structure, string source)
        {
            _destination = destination;
            _headerId = headerId;
            _hash = hash;
            _name = name;
            _structure = structure;
            _source = source;
        }

        public string GetName() { return _name; }

        public string GetHash() { return _hash; }

        public int GetHeaderId() { return _headerId; }

        public HDirection GetDestination() { return _destination; }

        public string GetStructure() { return _structure; }

        public string GetSource() { return _source; }

        public override string ToString()
        {
            return _headerId + ": " + "[" + _name + "][" + _structure + "]";
        }
    }
}
