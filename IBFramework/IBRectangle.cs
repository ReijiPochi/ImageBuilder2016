using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework
{
    public class IBRectangle
    {
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public IBRectangle()
        {
            OffsetX = 0;
            OffsetY = 0;
            Width = 0;
            Height = 0;
        }

        public IBRectangle(double w, double h)
        {
            OffsetX = 0;
            OffsetY = 0;
            Width = w;
            Height = h;
        }

        public IBRectangle(double w, double h, double offsetX, double offsetY)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;
            Width = w;
            Height = h;
        }
    }
}
