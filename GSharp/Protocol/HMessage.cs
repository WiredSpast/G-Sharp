using GSharp.Misc;

namespace GSharp.Protocol
{
    public class HMessage : StringifyAble
    {
        public enum Direction
        {
            TOSERVER,
            TOCLIENT
        }

        private HPacket hPacket;
        private Direction direction;
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

        public Direction getDestination()
        {
            return direction;
        }

        public Boolean isCorrupted()
        {
            return hPacket.isCorrupted();
        }

        public HMessage(String fromString)
        {
            constructFromString(fromString);
        }

        public string Stringify()
        {
            string s = (isBlocked ? "1" : "0") + "\t" + index + "\t" + direction + "\t" + hPacket.Stringified;
            return s;
        }

        public void constructFromString(string str)
        {
            string[] parts = str.Split('\t', 4);
            this.isBlocked = parts[0] == "1";
            this.index = int.Parse(parts[1]);
            this.direction = parts[2] == "TOCLIENT" ? Direction.TOCLIENT : Direction.TOSERVER;
            HPacket p = new HPacket(new byte[0]);
            p.constructFromString(parts[3]);
            this.hPacket = p;
        }

        public void ConstructFromHMessage(HMessage message)
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
