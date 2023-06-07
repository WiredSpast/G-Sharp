using GSharp.Extensions.parsers.stuffdata;
using GSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    // TODO
    public class HFloorItem //: IFurni
    {
        private int id;
        private int typeId;
        private HPoint tile;
        private HDirection facing;

        private int secondsToExpiration;
        private int usagePolicy;
        private int ownerId;
        private string ownerName;
        private IStuffData stuff;

        private string sizeZ;
        private int? extra;
        private string staticClass;

        public HFloorItem(HPacket packet)
        {
            id = packet.ReadInt();
            typeId = packet.ReadInt();

            int x = packet.ReadInt();
            int y = packet.ReadInt();
            facing = (HDirection)packet.ReadInt();

            tile = new HPoint(x, y, double.Parse(packet.ReadString()));

            sizeZ = packet.ReadString();
            extra = packet.ReadInt();

            // TODO
            //stuff = IStuffData.read(packet);

            secondsToExpiration = packet.ReadInt();
            usagePolicy = packet.ReadInt();

            ownerId = packet.ReadInt();

            if (typeId < 0)
            {
                staticClass = packet.ReadString();
            } else
            {
                staticClass = null;
            }
        }

        public void appendToPacket(HPacket packet)
        {
            packet.Append(id);
            packet.Append(typeId);
            packet.Append(tile.X);
            packet.Append(tile.Y);
            packet.Append((int)facing);
            packet.Append(tile.Z.ToString());
            packet.Append(sizeZ);
            packet.Append((int)extra);
            stuff.appendToPacket(packet);
            packet.Append(secondsToExpiration);
            packet.Append(usagePolicy);
            packet.Append(ownerId);

            if (typeId < 0)
            {
                packet.Append(staticClass);
            }
        }

        public static HFloorItem[] Parse(HPacket packet)
        {
            int ownersCount = packet.ReadInt();
            Dictionary<int, string> owners = new Dictionary<int, string>(ownersCount);

            for (int i = 0; i < ownersCount; i++)
                owners.Add(packet.ReadInt(), packet.ReadString());

            HFloorItem[] furniture = new HFloorItem[packet.ReadInt()];
            for (int i = 0; i < furniture.Length; i++)
            {
                HFloorItem furni = new HFloorItem(packet);
                furni.ownerName = owners[furni.ownerId];

                furniture[i] = furni;
            }
            return furniture;
        }

        public static HPacket ConstructPacket(HFloorItem[] floorItems, int headerId)
        {
            Dictionary<int, string> owners = new Dictionary<int, string>();
            foreach (HFloorItem floorItem in floorItems)
            {
                owners[floorItem.ownerId] = floorItem.ownerName;
            }

            HPacket packet = new HPacket(headerId.ToString());
            packet.Append(owners.Count);
            foreach (int ownerId in owners.Keys)
            {
                packet.Append(ownerId);
                packet.Append(owners[ownerId]);
            }

            packet.Append(floorItems.Length);
            foreach (HFloorItem floorItem in floorItems)
            {
                floorItem.appendToPacket(packet);
            }

            return packet;
        }

        public int Id
        {
            get { return id; }
        }

        public int TypeId
        {
            get { return typeId; }
        }

        public int UsagePolicy
        {
            get { return usagePolicy; }
        }

        public int OwnerId
        {
            get { return ownerId; }
        }

        public string OwnerName
        {
            get { return ownerName; }
        }

        public int SecondsToExpiration
        {
            get { return secondsToExpiration; }
        }

        public HDirection Facing
        {
            get { return facing; }
        }

        public HPoint Tile
        {
            get { return tile; }
        }

        public IStuffData Stuff
        {
            get { return stuff; }
        }

        public void SetOwnerName(string ownerName)
        {
            this.ownerName = ownerName;
        }

        public void SetId(int id)
        {
            this.id = id;
        }

        public void SetTypeId(int typeId)
        {
            this.typeId = typeId;
        }

        public void SetTile(HPoint tile)
        {
            this.tile = tile;
        }

        public void SetFacing(HDirection facing)
        {
            this.facing = facing;
        }

        public void SetSecondsToExpiration(int secondsToExpiration)
        {
            this.secondsToExpiration = secondsToExpiration;
        }

        public void SetUsagePolicy(int usagePolicy)
        {
            this.usagePolicy = usagePolicy;
        }

        public void SetOwnerId(int ownerId)
        {
            this.ownerId = ownerId;
        }

        public void SetStuff(IStuffData stuff)
        {
            this.stuff = stuff;
        }

    }
}
