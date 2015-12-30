using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework.Image
{
    public class BGRA32FormattedImage
    {
        public BGRA32FormattedImage(int w, int h, PixelData color)
        {
            size.Width = w;
            size.Height = h;
            size.OffsetX = 0;
            size.OffsetY = 0;

            data = new byte[w * h * 4];

            ClearData(color);
        }

        public byte[] data;
        public IBRectangle size = new IBRectangle();

        public void ClearData(PixelData color)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color.b;
                i++;
                data[i] = color.g;
                i++;
                data[i] = color.r;
                i++;
                data[i] = color.a;
            }
        }
    }
}
