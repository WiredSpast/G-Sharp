using GSharp.Protocol;
using System.Collections.Concurrent;

namespace GSharp.Services.PacketInfo
{
    public class PacketInfoManager
    {
        private ConcurrentDictionary<int, List<PacketInfo>> _headerIdToMessage_incoming = new ConcurrentDictionary<int, List<PacketInfo>>();
        private ConcurrentDictionary<int, List<PacketInfo>> _headerIdToMessage_outgoing = new ConcurrentDictionary<int, List<PacketInfo>>();
        private ConcurrentDictionary<string, List<PacketInfo>> _hashToMessage_incoming = new ConcurrentDictionary<string, List<PacketInfo>>();
        private ConcurrentDictionary<string, List<PacketInfo>> _hashToMessage_outgoing = new ConcurrentDictionary<string, List<PacketInfo>>();
        private ConcurrentDictionary<string, List<PacketInfo>> _nameToMessage_incoming = new ConcurrentDictionary<string, List<PacketInfo>>();
        private ConcurrentDictionary<string, List<PacketInfo>> nameToMessage_outgoing = new ConcurrentDictionary<string, List<PacketInfo>>();
        private List<PacketInfo> _packetInfoList;

        public PacketInfoManager(List<PacketInfo> packetInfoList)
        {
            this._packetInfoList = packetInfoList;
            foreach (PacketInfo packetInfo in packetInfoList)
            {
                AddMessage(packetInfo);
            }
        }

        private void AddMessage(PacketInfo packetInfo)
        {
            if (packetInfo.GetHash() == null && packetInfo.GetName() == null) return;

            ConcurrentDictionary<int, List<PacketInfo>> headerIdToMessage =
                packetInfo.GetDestination() == HDirection.TOCLIENT
                    ? _headerIdToMessage_incoming
                    : _headerIdToMessage_outgoing;

            ConcurrentDictionary<string, List<PacketInfo>> hashToMessage =
                packetInfo.GetDestination() == HDirection.TOCLIENT
                    ? _hashToMessage_incoming
                    : _hashToMessage_outgoing;

            ConcurrentDictionary<string, List<PacketInfo>> nameToMessage =
                packetInfo.GetDestination() == HDirection.TOCLIENT
                    ? _nameToMessage_incoming
                    : nameToMessage_outgoing;

            headerIdToMessage.TryAdd(packetInfo.GetHeaderId(), new List<PacketInfo>());

            headerIdToMessage[packetInfo.GetHeaderId()].Add(packetInfo);
            if (packetInfo.GetHash() != null)
            {
                hashToMessage.TryAdd(packetInfo.GetHash(), new List<PacketInfo>());
                hashToMessage[packetInfo.GetHash()].Add(packetInfo);
            }
            if (packetInfo.GetName() != null)
            {
                nameToMessage.TryAdd(packetInfo.GetName(), new List<PacketInfo>());
                nameToMessage[packetInfo.GetName()].Add(packetInfo);
            }
        }

        public List<PacketInfo> GetAllPacketInfoFromHeaderId(HDirection direction, int headerId)
        {
            ConcurrentDictionary<int, List<PacketInfo>> headerIdToMessage =
                direction == HDirection.TOSERVER
                    ? _headerIdToMessage_outgoing
                    : _headerIdToMessage_incoming;

            return headerIdToMessage.TryGetValue(headerId, out List<PacketInfo> packetInfos)
                ? packetInfos
                : new List<PacketInfo>();
        }

        public List<PacketInfo> GetAllPacketInfoFromHash(HDirection direction, string hash)
        {
            ConcurrentDictionary<string, List<PacketInfo>> hashToMessage =
                direction == HDirection.TOSERVER
                    ? _hashToMessage_outgoing
                    : _hashToMessage_incoming;

            return hashToMessage.TryGetValue(hash, out List<PacketInfo> packetInfos)
                ? packetInfos
                : new List<PacketInfo>();
        }

        public List<PacketInfo> GetAllPacketInfoFromName(HDirection direction, string name)
        {
            ConcurrentDictionary<string, List<PacketInfo>> nameToMessage =
                direction == HDirection.TOSERVER
                    ? nameToMessage_outgoing
                    : _nameToMessage_incoming;

            return nameToMessage.TryGetValue(name, out List<PacketInfo> packetInfos)
                ? packetInfos
                : new List<PacketInfo>();
        }

        public PacketInfo GetPacketInfoFromHeaderId(HDirection direction, int headerId)
        {
            List<PacketInfo> all = GetAllPacketInfoFromHeaderId(direction, headerId);
            return all.Count == 0 ? null : all[0];
        }

        public PacketInfo GetPacketInfoFromHash(HDirection direction, string hash)
        {
            List<PacketInfo> all = GetAllPacketInfoFromHash(direction, hash);
            return all.Count == 0 ? null : all[0];
        }

        public PacketInfo GetPacketInfoFromName(HDirection direction, string name)
        {
            List<PacketInfo> all = GetAllPacketInfoFromName(direction, name);
            return all.Count == 0 ? null : all[0];
        }

        public List<PacketInfo> GetPacketInfoList()
        {
            return _packetInfoList;
        }

        public static PacketInfoManager ReadFromPacket(HPacket hPacket)
        {
            List<PacketInfo> packetInfoList = new List<PacketInfo>();
            int size = hPacket.ReadInt();

            for (int i = 0; i < size; i++)
            {
                int headerId = hPacket.ReadInt();
                string hash = hPacket.ReadString();
                string name = hPacket.ReadString();
                string structure = hPacket.ReadString();
                bool isOutgoing = hPacket.ReadBool();
                string source = hPacket.ReadString();

                packetInfoList.Add(new PacketInfo(
                    isOutgoing ? HDirection.TOSERVER : HDirection.TOCLIENT,
                    headerId,
                    hash.Equals("NULL") ? null : hash,
                    name.Equals("NULL") ? null : name,
                    structure.Equals("NULL") ? null : structure,
                    source));
            }

            return new PacketInfoManager(packetInfoList);
        }

        public void AppendToPacket(HPacket hPacket)
        {
            hPacket.Append(this._packetInfoList.Count);
            foreach (PacketInfo packetInfo in this._packetInfoList)
            {
                hPacket.Append(packetInfo.GetHeaderId());
                hPacket.Append(packetInfo.GetHash() == null ? "NULL" : packetInfo.GetHash());
                hPacket.Append(packetInfo.GetName() == null ? "NULL" : packetInfo.GetName());
                hPacket.Append(packetInfo.GetStructure() == null ? "NULL" : packetInfo.GetStructure());
                hPacket.Append(packetInfo.GetDestination() == HDirection.TOSERVER);
                hPacket.Append(packetInfo.GetSource());
            }
        }

        public static PacketInfoManager EMPTY = new PacketInfoManager(new List<PacketInfo>());
    }

}
