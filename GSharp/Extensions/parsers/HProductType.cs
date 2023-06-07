using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public enum HProductType
    {
        WallItem = 'I',
        FloorItem = 'S',
        Effect = 'E',
        Badge = 'B'
    }

    public static class HProductTypeExtensions
    {
        public static HProductType FromString(string id)
        {
            foreach (HProductType t in Enum.GetValues(typeof(HProductType)))
            {
                if (t.ToString().Equals(id, StringComparison.OrdinalIgnoreCase))
                    return t;
            }
            return default(HProductType);
        }
    }

}
