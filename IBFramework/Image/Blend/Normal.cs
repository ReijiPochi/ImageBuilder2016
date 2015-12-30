using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBFramework.Image.Pixel;

namespace IBFramework.Image.Blend
{
    public class Normal : BlendMode
    {
        public override void Blend(PixcelImage source, BGRA32FormattedImage trg)
        {
            throw new NotImplementedException();
        }

        public override void Blend(SingleColorImage source, BGRA32FormattedImage trg)
        {
            int trgSizeW = (int)trg.size.Width;
            int trgSizeH = (int)trg.size.Height;

            if ((int)source.size.OffsetX > trgSizeW || (int)source.size.OffsetY > trgSizeH)
                return;



            int strX = 0, strY = 0;

            if ((int)source.size.OffsetX < 0)
                strX = 0;
            else if ((int)source.size.OffsetX > (int)trg.size.Width)
                strX = (int)trg.size.Width;
            else
                strX = (int)source.size.OffsetX;

            if ((int)source.size.OffsetY < 0)
                strY = 0;
            else if ((int)source.size.OffsetY > (int)trg.size.Height)
                strY = (int)trg.size.Height;
            else
                strY = (int)source.size.OffsetY;


            int maxX = 0, maxY = 0;

            if ((int)source.size.OffsetX + (int)source.size.Width < 0)
                maxX = 0;
            else if ((int)source.size.OffsetX + (int)source.size.Width > (int)trg.size.Width)
                maxX = (int)trg.size.Width;
            else
                maxX = (int)source.size.OffsetX + (int)source.size.Width;

            if ((int)source.size.OffsetY + (int)source.size.Height < 0)
                maxY = 0;
            else if ((int)source.size.OffsetY + (int)source.size.Height > (int)trg.size.Height)
                maxY = (int)trg.size.Height;
            else
                maxY = (int)source.size.OffsetY + (int)source.size.Height;



            int trgDataLength = trg.data.Length;

            double Cb2 = source.color.b;
            double Cg2 = source.color.g;
            double Cr2 = source.color.r;

            Parallel.For(strY, maxY, y => 
            {
                if (trg.size.OffsetY <= y && y < trg.size.OffsetY + trg.size.Height)
                    for (int x = strX; x < maxX; x++)
                    {
                        if (trg.size.OffsetX <= y && y < trg.size.OffsetX + trg.size.Width)
                        {
                            int index = (x + y * trgSizeW) * 4;
                            if (index >= trgDataLength) break;

                            double A1 = trg.data[index + 3] / 255.0;
                            double A2 = source.color.a / 255.0;
                            double _A2 = 1 - A2;

                            double A = A2 + _A2 * A1;
                            double B = (Cb2 * A2 + _A2 * A1 * trg.data[index]) / A;
                            double G = (Cg2 * A2 + _A2 * A1 * trg.data[index + 1]) / A;
                            double R = (Cr2 * A2 + _A2 * A1 * trg.data[index + 2]) / A;

                            trg.data[index] = (byte)B;
                            trg.data[index + 1] = (byte)G;
                            trg.data[index + 2] = (byte)R;
                            trg.data[index + 3] = (byte)(A * 255.0);
                        }
                    }
            });
        }
    }
}
