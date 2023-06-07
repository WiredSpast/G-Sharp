using GSharp.Protocol;

namespace GSharp.Extensions.parsers
{
    public class HEntityUpdate
    {
        private int index;
        private bool isController = false;

        private HPoint tile;
        private HPoint movingTo = null;

        private HSign sign;
        private HStance stance;
        private HAction action;
        private HDirection headFacing;
        private HDirection bodyFacing;

        private HEntityUpdate(HPacket packet)
        {
            index = packet.ReadInt();
            tile = new HPoint(packet.ReadInt(), packet.ReadInt(), double.Parse(packet.ReadString()));

            headFacing = (HDirection)packet.ReadInt();
            bodyFacing = (HDirection)packet.ReadInt();

            string action = packet.ReadString();
            string[] actionData = action.Split('/');

            foreach (string actionInfo in actionData)
            {
                string[] actionValues = actionInfo.Split(' ');

                if (actionValues.Length < 2) continue;
                if (string.IsNullOrEmpty(actionValues[0])) continue;

                switch (actionValues[0])
                {
                    case "flatctrl":
                        isController = true;
                        action = HAction.None.ToString();
                        break;
                    case "mv":
                        string[] values = actionValues[1].Split(',');
                        if (values.Length >= 3)
                            movingTo = new HPoint(int.Parse(values[0]), int.Parse(values[1]), double.Parse(values[2]));

                        action = HAction.Move.ToString();
                        break;
                    case "sit":
                        action = HAction.Sit.ToString();
                        stance = HStance.Sit;
                        break;
                    case "lay":
                        action = HAction.Lay.ToString();
                        stance = HStance.Lay;
                        break;
                    case "sign":
                        sign = (HSign)int.Parse(actionValues[1]);
                        action = HAction.Sign.ToString();
                        break;
                }
            }
        }

        public static HEntityUpdate[] Parse(HPacket packet)
        {
            HEntityUpdate[] updates = new HEntityUpdate[packet.ReadInt()];
            for (int i = 0; i < updates.Length; i++)
                updates[i] = new HEntityUpdate(packet);

            return updates;
        }

        public int GetIndex()
        {
            return index;
        }

        public bool IsController()
        {
            return isController;
        }

        public HPoint GetTile()
        {
            return tile;
        }

        public HPoint GetMovingTo()
        {
            return movingTo;
        }

        public HSign GetSign()
        {
            return sign;
        }

        public HStance GetStance()
        {
            return stance;
        }

        public HAction GetAction()
        {
            return action;
        }

        public HDirection GetHeadFacing()
        {
            return headFacing;
        }

        public HDirection GetBodyFacing()
        {
            return bodyFacing;
        }
    }
}
