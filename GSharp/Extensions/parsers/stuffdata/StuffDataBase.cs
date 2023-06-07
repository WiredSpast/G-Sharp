using GSharp.Protocol;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers.stuffdata
{
    // TODO
    public abstract class StuffDataBase //: IStuffData
    {
        private int flags = 0;
        private int uniqueSerialNumber = 0;
        private int uniqueSerialSize = 0;

        protected StuffDataBase()
        {
        }

        protected StuffDataBase(int uniqueSerialNumber, int uniqueSerialSize)
        {
            this.uniqueSerialNumber = uniqueSerialNumber;
            this.uniqueSerialSize = uniqueSerialSize;
            flags = 256;
        }

        protected void Initialize(HPacket packet)
        {
            if ((flags & 256) > 0)
            {
                this.uniqueSerialNumber = packet.ReadInt();
                this.uniqueSerialSize = packet.ReadInt();
            }
        }

        public void AppendToPacket(HPacket packet)
        {
            if ((flags & 256) > 0)
            {
                packet.Append(this.uniqueSerialNumber);
                packet.Append(this.uniqueSerialSize);
            }
        }

        public virtual string GetLegacyString()
        {
            return "";
        }

        public virtual void SetLegacyString(string legacyString)
        {
        }

        public void SetFlags(int flags)
        {
            this.flags = flags;
        }

        public int GetFlags()
        {
            return this.flags;
        }

        public int GetUniqueSerialNumber()
        {
            return this.uniqueSerialNumber;
        }

        public int GetUniqueSerialSize()
        {
            return this.uniqueSerialSize;
        }

        public void SetUniqueSerialNumber(int uniqueSerialNumber)
        {
            this.uniqueSerialNumber = uniqueSerialNumber;
        }

        public void SetUniqueSerialSize(int uniqueSerialSize)
        {
            this.uniqueSerialSize = uniqueSerialSize;
        }

        public virtual int GetRarityLevel()
        {
            return -1;
        }

        public int GetState()
        {
            try
            {
                return int.Parse(GetLegacyString());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public void SetState(int state)
        {
            SetLegacyString(state.ToString());
        }

        public string GetJSONValue(string key)
        {
            try
            {
                return JObject.Parse(GetLegacyString()).Value<string>(key);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
