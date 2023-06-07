using GSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public class HEntity
    {
        private int id;
        private int index;
        private HPoint tile;
        private string name;
        private string motto;
        private HGender gender;
        private HEntityType entityType;
        private string figureId;
        private string favoriteGroup = null;
        private HEntityUpdate lastUpdate = null;
        private object[] stuff = new object[0];
        private int unknown1;

        public HEntity(HPacket packet)
        {
            id = packet.ReadInt();
            name = packet.ReadString();
            motto = packet.ReadString();
            figureId = packet.ReadString();
            index = packet.ReadInt();
            tile = new HPoint(packet.ReadInt(), packet.ReadInt(), double.Parse(packet.ReadString()));

            unknown1 = packet.ReadInt();
            int entityTypeId = packet.ReadInt();
            entityType = (HEntityType)entityTypeId;

            switch (entityTypeId)
            {
                case 1:
                    stuff = new object[5];
                    string genderString = packet.ReadString();
                    gender = genderString == "M" ? HGender.Male : HGender.Female;
                    stuff[0] = packet.ReadInt();
                    stuff[1] = packet.ReadInt();
                    favoriteGroup = packet.ReadString();
                    stuff[2] = packet.ReadString();
                    stuff[3] = packet.ReadInt();
                    stuff[4] = packet.ReadBool();
                    break;
                case 2:
                    stuff = new object[12];
                    stuff[0] = packet.ReadInt();
                    stuff[1] = packet.ReadInt();
                    stuff[2] = packet.ReadString();
                    stuff[3] = packet.ReadInt();
                    stuff[4] = packet.ReadBool();
                    stuff[5] = packet.ReadBool();
                    stuff[6] = packet.ReadBool();
                    stuff[7] = packet.ReadBool();
                    stuff[8] = packet.ReadBool();
                    stuff[9] = packet.ReadBool();
                    stuff[10] = packet.ReadInt();
                    stuff[11] = packet.ReadString();
                    break;
                case 4:
                    stuff = new object[4];
                    stuff[0] = packet.ReadString();
                    stuff[1] = packet.ReadInt();
                    stuff[2] = packet.ReadString();
                    List<short> list = new List<short>();
                    for (int j = packet.ReadInt(); j > 0; j--)
                    {
                        list.Add(packet.ReadShort());
                    }
                    stuff[3] = list;
                    break;
            }
        }

        public void AppendToPacket(HPacket packet)
        {
            packet.Append(id);
            packet.Append(name);
            packet.Append(motto);
            packet.Append(figureId);
            packet.Append(index);
            packet.Append(tile.X);
            packet.Append(tile.Y);
            packet.Append(tile.Z.ToString());
            packet.Append(unknown1);
            packet.Append((int)entityType);

            switch ((int)entityType)
            {
                case 1:
                    stuff = new object[5];
                    gender = (HGender)(Object)packet.ReadString();
                    stuff[0] = packet.ReadInt();
                    stuff[1] = packet.ReadInt();
                    favoriteGroup = packet.ReadString();
                    stuff[2] = packet.ReadString();
                    stuff[3] = packet.ReadInt();
                    stuff[4] = packet.ReadBool();
                    break;
                case 2:
                    stuff = new Object[12];
                    stuff[0] = packet.ReadInt();
                    stuff[1] = packet.ReadInt();
                    stuff[2] = packet.ReadString();
                    stuff[3] = packet.ReadInt();
                    stuff[4] = packet.ReadBool();
                    stuff[5] = packet.ReadBool();
                    stuff[6] = packet.ReadBool();
                    stuff[7] = packet.ReadBool();
                    stuff[8] = packet.ReadBool();
                    stuff[9] = packet.ReadBool();
                    stuff[10] = packet.ReadInt();
                    stuff[11] = packet.ReadString();
                    break;
                case 4:
                    stuff = new Object[4];
                    stuff[0] = packet.ReadString();
                    stuff[1] = packet.ReadInt();
                    stuff[2] = packet.ReadString();

                    List<short> list = new List<short>();

                    for (int j = packet.ReadInt(); j > 0; j--)
                    {
                        list.Add(packet.ReadShort());
                    }
                    stuff[3] = list;

                    break;
            }
        }

        public static HPacket ConstructPacket(HEntity[] entities, int headerId)
        {
            HPacket packet = new HPacket(headerId.ToString());
            packet.Append(entities.Length);
            foreach (HEntity entity in entities)
            {
                entity.AppendToPacket(packet);
            }

            return packet;
        }

        public static HEntity[] Parse(HPacket packet)
        {
            HEntity[] entities = new HEntity[packet.ReadInt()];

            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new HEntity(packet);
            }

            return entities;
        }

        public bool TryUpdate(HEntityUpdate update)
        {
            if (index != update.GetIndex())
                return false;

            tile = update.GetTile();
            lastUpdate = update;
            return true;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public HPoint Tile
        {
            get { return tile; }
            set { tile = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Motto
        {
            get { return motto; }
            set { motto = value; }
        }

        public HGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public HEntityType EntityType
        {
            get { return entityType; }
            set { entityType = value; }
        }

        public string FigureId
        {
            get { return figureId; }
            set { figureId = value; }
        }

        public string FavoriteGroup
        {
            get { return favoriteGroup; }
            set { favoriteGroup = value; }
        }

        public HEntityUpdate LastUpdate
        {
            get { return lastUpdate; }
            set { lastUpdate = value; }
        }

        public object[] Stuff
        {
            get { return stuff; }
            set { stuff = value; }
        }
    }
}
