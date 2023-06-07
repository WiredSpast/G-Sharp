using GSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers.stuffdata
{
    public interface IStuffData
    {
        // TODO
        //static IStuffData read(HPacket packet)
        //{
        //    int a = packet.ReadInt();
        //    StuffDataBase stuffData = null;

        //    switch (a & 255)
        //    {
        //        case LegacyStuffData.Identifier:
        //            stuffData = new LegacyStuffData();
        //            break;
        //        case MapStuffData.IDENTIFIER:
        //            stuffData = new MapStuffData();
        //            break;
        //        case StringArrayStuffData.Identifier:
        //            stuffData = new StringArrayStuffData();
        //            break;
        //        case VoteResultStuffData.Identifier:
        //            stuffData = new VoteResultStuffData();
        //            break;
        //        case EmptyStuffData.Identifier:
        //            stuffData = new EmptyStuffData();
        //            break;
        //        case IntArrayStuffData.Identifier:
        //            stuffData = new IntArrayStuffData();
        //            break;
        //        case HighScoreStuffData.Identifier:
        //            stuffData = new HighScoreStuffData();
        //            break;
        //        case CrackableStuffData.Identifier:
        //            stuffData = new CrackableStuffData();
        //            break;
        //    }

        //    if (stuffData != null)
        //    {
        //        stuffData.SetFlags(a & 65280);
        //        stuffData.initialize(packet);
        //    }
        //    else
        //    {
        //        throw new RuntimeException("Unknown stuffdata type");
        //    }

        //    return stuffData;
        //}

        void appendToPacket(HPacket packet);

        String getLegacyString();
        void setLegacyString(String legacyString);

        void setFlags(int flags);
        int getFlags();

        int getUniqueSerialNumber();
        int getUniqueSerialSize();
        void setUniqueSerialNumber(int uniqueSerialNumber);
        void setUniqueSerialSize(int uniqueSerialSize);

        int getRarityLevel();

        int getState();
        void setState(int state);

        String getJSONValue(String key);
    }
}
