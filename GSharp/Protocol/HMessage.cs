using GSharp.Misc;

namespace GSharp.Protocol
{
    public class HMessage : StringifyAble
    {
        private HPacket hPacket;
        private HDirection direction;
        private int index;

        private Boolean isBlocked;

        public int getIndex()
        {
            return index;
        }

        public void setBlocked(Boolean block)
        {
            // both dont show error but native uses the uncommented so fk it
            isBlocked = block;
            //this.isBlocked = block;
        }

        public Boolean IsBlocked
        {
            get { return isBlocked; }
        }

        public HPacket GetPacket()
        {
            return hPacket;
        }

        public HDirection getDestination()
        {
            return direction;
        }

        public Boolean IsCorrupted()
        {
            return hPacket.IsCorrupted();
        }

        public HMessage(String str)
        {
            string[] parts = str.Split('\t', 4);
            this.isBlocked = parts[0] == "1";
            this.index = int.Parse(parts[1]);
            this.direction = parts[2] == "TOCLIENT" ? HDirection.ToClient : HDirection.ToServer;
            HPacket p = new HPacket(parts[3]);
            this.hPacket = p;
        }

        public string Stringify()
        {
            string s = (isBlocked ? "1" : "0") + "\t" + index + "\t" + direction + "\t" + hPacket.Stringified;
            return s;
        }

        public HMessage(HMessage message)
        {
            this.isBlocked = message.IsBlocked;
            this.index = message.getIndex();
            this.direction = message.getDestination();
            this.hPacket = new HPacket(message.GetPacket());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is HMessage))
                return false;

            HMessage message = (HMessage)obj;

            return message.hPacket.Equals(hPacket) && direction == message.direction && index == message.index;
        }



    }
}
