using GSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public class HFriend
    {
        private int id;
        private string name;
        private HGender gender;
        private bool online;
        private bool followingAllowed;
        private string figure;
        private int categoryId;
        private string motto;
        private string realName;
        private string facebookId;
        private bool persistedMessageUser;
        private bool vipMember;
        private bool pocketHabboUser;
        private HRelationshipStatus relationshipStatus;

        private string categoryName = null;

        public HFriend(HPacket packet)
        {
            id = packet.ReadInt();
            name = packet.ReadString();
            gender = packet.ReadInt() == 0 ? HGender.Female : HGender.Male;
            online = packet.ReadBool();
            followingAllowed = packet.ReadBool();
            figure = packet.ReadString();
            categoryId = packet.ReadInt();
            motto = packet.ReadString();
            realName = packet.ReadString();
            facebookId = packet.ReadString();
            persistedMessageUser = packet.ReadBool();
            vipMember = packet.ReadBool();
            pocketHabboUser = packet.ReadBool();
            relationshipStatus = (HRelationshipStatus)packet.ReadShort();
        }
        public void AppendToPacket(HPacket packet)
        {
            packet.Append(id);
            packet.Append(name);
            packet.Append(gender == HGender.Female ? 0 : 1);
            packet.Append(online);
            packet.Append(followingAllowed);
            packet.Append(figure);
            packet.Append(categoryId);
            packet.Append(motto);
            packet.Append(realName);
            packet.Append(facebookId);
            packet.Append(persistedMessageUser);
            packet.Append(vipMember);
            packet.Append(pocketHabboUser);
            packet.Append((short)((int)relationshipStatus));
        }

        public static HFriend[] ParseFromFragment(HPacket packet)
        {
            packet.ReadIndex = 14;
            // int packetCount
            // int packetIndex
            HFriend[] friends = new HFriend[packet.ReadInt()];

            for (int i = 0; i < friends.Length; i++)
            {
                friends[i] = new HFriend(packet);
            }

            return friends;
        }

        public static HFriend[] ParseFromUpdate(HPacket packet)
        {
            packet.ReadIndex = 6;
            int categoryCount = packet.ReadInt();
            Dictionary<int, string> categories = new Dictionary<int, string>();

            for (int i = 0; i < categoryCount; i++)
            {
                categories[packet.ReadInt()] = packet.ReadString();
            }

            int friendCount = packet.ReadInt();
            List<HFriend> friends = new List<HFriend>();
            for (int i = 0; i < friendCount; i++)
            {
                if (packet.ReadInt() != -1)
                {
                    friends.Add(new HFriend(packet));
                }
                else
                {
                    packet.ReadInt();
                }
            }

            foreach (HFriend friend in friends)
            {
                friend.categoryName = categories.GetValueOrDefault(friend.categoryId, null);
            }

            return friends.ToArray();
        }

        public static int[] GetRemovedFriendIdsFromUpdate(HPacket packet)
        {
            packet.ReadIndex = 6;
            int categoryCount = packet.ReadInt();
            for (int i = 0; i < categoryCount; i++)
            {
                packet.ReadInt();
                packet.ReadString();
            }

            int friendCount = packet.ReadInt();
            List<int> removedIds = new List<int>();
            for (int i = 0; i < friendCount; i++)
            {
                if (packet.ReadInt() != -1)
                {
                    new HFriend(packet);
                }
                else
                {
                    removedIds.Add(packet.ReadInt());
                }
            }

            return removedIds.ToArray();
        }

        public static HPacket[] ConstructFragmentPackets(HFriend[] friends, int headerId)
        {
            int packetCount = (int)Math.Ceiling((double)friends.Length / 100);

            HPacket[] packets = new HPacket[packetCount];

            for (int i = 0; i < packetCount; i++)
            {
                packets[i] = new HPacket(headerId.ToString());
                packets[i].Append(packetCount);
                packets[i].Append(i);
                packets[i].Append(i == packetCount - 1 ? friends.Length % 100 : 100);

                for (int j = i * 100; j < friends.Length && j < (j + 1) * 100; j++)
                {
                    friends[j].AppendToPacket(packets[i]);
                }
            }

            return packets;
        }

        public static HPacket ConstructUpdatePacket(HFriend[] friends, int headerId)
        {
            Dictionary<int, string> categories = new Dictionary<int, string>();
            foreach (HFriend friend in friends)
            {
                if (friend.categoryName != null)
                    categories[friend.categoryId] = friend.categoryName;
            }

            HPacket packet = new HPacket(headerId.ToString());
            packet.Append(categories.Count);
            foreach (int categoryId in categories.Keys)
            {
                packet.Append(categoryId);
                packet.Append(categories[categoryId]);
            }

            packet.Append(friends.Length);
            foreach (HFriend friend in friends)
            {
                friend.AppendToPacket(packet);
            }

            return packet;
        }

        public int Id { get => id; set => id = value; }

        public string Name { get => name; set => name = value; }

        public HGender Gender { get => gender; set => gender = value; }

        public bool Online { get => online; set => online = value; }

        public bool FollowingAllowed { get => followingAllowed; set => followingAllowed = value; }

        public string Figure { get => figure; set => figure = value; }

        public int CategoryId { get => categoryId; set => categoryId = value; }

        public string Motto { get => motto; set => motto = value; }

        public string RealName { get => realName; set => realName = value; }

        public string FacebookId { get => facebookId; set => facebookId = value; }

        public bool PersistedMessageUser { get => persistedMessageUser; set => persistedMessageUser = value; }

        public bool VipMember { get => vipMember; set => vipMember = value; }

        public bool PocketHabboUser { get => pocketHabboUser; set => pocketHabboUser = value; }

        public HRelationshipStatus RelationshipStatus { get => relationshipStatus; set => relationshipStatus = value; }

        public string CategoryName { get => categoryName; set => categoryName = value; }
    }
}
