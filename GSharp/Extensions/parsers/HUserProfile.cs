using GSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public class HUserProfile
    {
        private int id;
        private string username;
        private string motto;
        private string figure;
        private string creationDate;
        private int achievementScore;
        private int friendCount;

        private bool isFriend;
        private bool isRequestedFriend;
        private bool isOnline;

        private HGroup[] groups;

        private int lastAccessSince;
        private bool openProfile;

        public HUserProfile(HPacket packet)
        {
            id = packet.ReadInt();
            username = packet.ReadString();
            motto = packet.ReadString();
            figure = packet.ReadString();
            creationDate = packet.ReadString();
            achievementScore = packet.ReadInt();
            friendCount = packet.ReadInt();

            isFriend = packet.ReadBool();
            isRequestedFriend = packet.ReadBool();
            isOnline = packet.ReadBool();

            groups = new HGroup[packet.ReadInt()];
            for (int i = 0; i < groups.Length; i++)
                groups[i] = new HGroup(packet);

            lastAccessSince = packet.ReadInt();
            openProfile = packet.ReadBool();
        }

        public int Id => id;

        public string Username => username;

        public string Motto => motto;

        public string Figure => figure;

        public string CreationDate => creationDate;

        public int AchievementScore => achievementScore;

        public int FriendCount => friendCount;

        public bool IsFriend => isFriend;

        public bool IsRequestedFriend => isRequestedFriend;

        public bool IsOnline => isOnline;

        public HGroup[] Groups => groups;

        public int LastAccessSince => lastAccessSince;

        public bool OpenProfile => openProfile;
    }
}
