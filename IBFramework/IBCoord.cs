using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework
{
    public class IBCoord
    {
        public double x;
        public double y;

        public IBCoord()
        {
            x = 0;
            y = 0;
        }

        public IBCoord(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public static double GetDistance(IBCoord c1, IBCoord c2)
        {
            double dist = Math.Sqrt((c1.x - c2.x) * (c1.x - c2.x) + (c1.y - c2.y) * (c1.y - c2.y));

            return dist;
        }
    }
}
