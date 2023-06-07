using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public enum HRelationshipStatus
    {
        NONE = 0,
        HEART = 1,
        SMILEY = 2,
        SKULL = 3
    }

    public static class HRelationshipStatusExtensions
    {
        public static HRelationshipStatus FromId(int id)
        {
            foreach (HRelationshipStatus status in Enum.GetValues(typeof(HRelationshipStatus)))
            {
                if ((int)status == id) return status;
            }
            return default(HRelationshipStatus);
        }
    }

}
