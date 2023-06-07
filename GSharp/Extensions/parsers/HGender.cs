using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public enum HGender
    {
        Unisex = 'U',
        Male = 'M',
        Female = 'F'
    }

    public static class HGenderExtensions
    {
        public static HGender FromString(string text)
        {
            foreach (HGender g in Enum.GetValues(typeof(HGender)))
            {
                if (g.ToString().Equals(text, StringComparison.OrdinalIgnoreCase))
                    return g;
            }
            return default(HGender);
        }
    }

}
