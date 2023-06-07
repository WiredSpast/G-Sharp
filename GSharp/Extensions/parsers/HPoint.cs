using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public class HPoint
    {
        private int x;
        private int y;
        private double z;

        public HPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public HPoint(int x, int y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public double Z
        {
            get { return z; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is HPoint))
                return false;

            HPoint p = (HPoint)obj;
            return this.x == p.X && this.y == p.Y && this.z == p.Z;
        }

        public override string ToString()
        {
            return "(" + this.X + "," + this.Y + "," + this.Z + ")";
        }
    }

}
