using GSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers.stuffdata
{
    public class LegacyStuffData : StuffDataBase
    {
        public const int Identifier = 0;

        private string legacyString;

        public LegacyStuffData()
        {
        }

        public LegacyStuffData(string legacyString)
        {
            this.legacyString = legacyString ?? "";
        }

        public LegacyStuffData(int uniqueSerialNumber, int uniqueSerialSize, string legacyString)
            : base(uniqueSerialNumber, uniqueSerialSize)
        {
            this.legacyString = legacyString ?? "";
        }

        protected virtual void Initialize(HPacket packet)
        {
            if ((GetFlags() & 256) > 0)
            {
                this.SetUniqueSerialNumber(packet.ReadInt());
                this.SetUniqueSerialSize(packet.ReadInt());
            }
        }

        public virtual void AppendToPacket(HPacket packet)
        {
            if ((GetFlags() & 256) > 0) 
            {
                packet.Append(this.GetUniqueSerialNumber());
                packet.Append(this.GetUniqueSerialSize());
            }
        }

        public override string GetLegacyString()
        {
            return this.legacyString;
        }

        public override void SetLegacyString(string legacyString)
        {
            this.legacyString = legacyString;
        }
    }
}
