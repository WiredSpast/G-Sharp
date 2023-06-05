using GSharp.Protocol;
using System.Xml.Linq;

namespace GSharp.Services
{
    public class PacketInfo
    {
        private HDirection _destination;
        private int _headerId;
        private String _hash;
        private String _name;
        private String _structure;
        private String _source;

        public PacketInfo(HDirection destination, int headerId, string hash, string name, string structure, string source)
        {
            this._destination = destination;
            this._headerId = headerId;
            this._hash = hash;
            this._name = name;
            this._structure = structure;
            this._source = source;
        }

        public String GetName() { return this._name; }

        public String GetHash() { return this._hash; }

        public int GetHeaderId() { return this._headerId;  }

        public HDirection GetDestination() { return this._destination; }

        public String GetStructure() { return this._structure; }

        public String GetSource() { return this._source; }

        public override string ToString()
        {
            return this._headerId + ": " + "[" + this._name + "][" + this._structure + "]";
        }
    }
}
