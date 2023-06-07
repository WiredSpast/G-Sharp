using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public enum HEntityType
    {
        HABBO = 1,
        PET = 2,
        OLD_BOT = 3,
        BOT = 4
    }

    public static class HEntityTypeExtensions
    {
        private static readonly Dictionary<int, HEntityType> map = new Dictionary<int, HEntityType>();

        static HEntityTypeExtensions()
        {
            foreach (HEntityType type in System.Enum.GetValues(typeof(HEntityType)))
            {
                map[type.GetId()] = type;
            }
        }

        public static int GetId(this HEntityType entityType)
        {
            return (int)entityType;
        }

        public static HEntityType ValueOf(int id)
        {
            return map[id];
        }
    }
}
