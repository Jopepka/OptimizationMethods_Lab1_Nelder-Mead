using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;

namespace Nelder_Mead
{
    public class Pair
    {
        public VectorM X { get; }
        public double Y { get; }

        public Pair(VectorM x, double y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Pair p1, Pair p2)
        {
            return p1.Y == p2.Y;
        }

        public static bool operator !=(Pair p1, Pair p2)
        {
            return p1.Y != p2.Y;
        }

        public static bool operator >=(Pair p1, Pair p2)
        {
            return p1.Y >= p2.Y;
        }

        public static bool operator <=(Pair p1, Pair p2)
        {
            return p1.Y <= p2.Y;
        }

        public static bool operator >(Pair p1, Pair p2)
        {
            return p1.Y > p2.Y;
        }

        public static bool operator <(Pair p1, Pair p2)
        {
            return p1.Y < p2.Y;
        }

        public override string ToString()
        {
            string str = X.ToString();
            str += "\n" + Y;

            return str;
        }
    }
}
