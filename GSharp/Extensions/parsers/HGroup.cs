using GSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public class HGroup
    {
        private int id, ownerId;
        private string name, badgeCode, primaryColor, secondaryColor;
        private bool isFavorite, hasForum;

        public HGroup(HPacket packet)
        {
            id = packet.ReadInt();
            name = packet.ReadString();
            badgeCode = packet.ReadString();
            primaryColor = packet.ReadString();
            secondaryColor = packet.ReadString();

            isFavorite = packet.ReadBool();
            ownerId = packet.ReadInt();
            hasForum = packet.ReadBool();
        }

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string BadgeCode
        {
            get { return badgeCode; }
        }

        public string PrimaryColor
        {
            get { return primaryColor; }
        }

        public string SecondaryColor
        {
            get { return secondaryColor; }
        }

        public bool IsFavorite
        {
            get { return isFavorite; }
        }

        public int OwnerId
        {
            get { return ownerId; }
        }

        public bool HasForum
        {
            get { return hasForum; }
        }
    }
}
