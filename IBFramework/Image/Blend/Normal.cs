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

            if ((int)source.Size.OffsetX > trgSizeW || (int)source.Size.OffsetY > trgSizeH)
                return;



            int strX = 0, strY = 0;

            if ((int)source.Size.OffsetX < 0)
                strX = 0;
            else if ((int)source.Size.OffsetX > (int)trg.size.Width)
                strX = (int)trg.size.Width;
            else
                strX = (int)source.Size.OffsetX;

            if ((int)source.Size.OffsetY < 0)
                strY = 0;
            else if ((int)source.Size.OffsetY > (int)trg.size.Height)
                strY = (int)trg.size.Height;
            else
                strY = (int)source.Size.OffsetY;


            int maxX = 0, maxY = 0;

            if ((int)source.Size.OffsetX + (int)source.Size.Width < 0)
                maxX = 0;
            else if ((int)source.Size.OffsetX + (int)source.Size.Width > (int)trg.size.Width)
                maxX = (int)trg.size.Width;
            else
                maxX = (int)source.Size.OffsetX + (int)source.Size.Width;

            if ((int)source.Size.OffsetY + (int)source.Size.Height < 0)
                maxY = 0;
            else if ((int)source.Size.OffsetY + (int)source.Size.Height > (int)trg.size.Height)
                maxY = (int)trg.size.Height;
            else
                maxY = (int)source.Size.OffsetY + (int)source.Size.Height;



            int trgDataLength = trg.data.Length;

            double Cb2 = source.color.b;
            double Cg2 = source.color.g;
            double Cr2 = source.color.r;

            Parallel.For(strY, maxY, y => 
            {
                if (trg.size.OffsetY <= y && y < trg.size.OffsetY + trg.size.Height)
                    for (int x = strX; x < maxX; x++)
                    {
                        double A1;
                        double A2;
                        double _A2;

                        double A;
                        double B;
                        double G;
                        double R;

                        if (trg.size.OffsetX <= y && y < trg.size.OffsetX + trg.size.Width)
                        {
                            int index = (x + y * trgSizeW) * 4;
                            if (index >= trgDataLength) break;

                            A1 = trg.data[index + 3] / 255.0;
                            A2 = source.color.a / 255.0;
                            _A2 = 1 - A2;

                            A = A2 + _A2 * A1;
                            B = (Cb2 * A2 + _A2 * A1 * trg.data[index]) / A;
                            G = (Cg2 * A2 + _A2 * A1 * trg.data[index + 1]) / A;
                            R = (Cr2 * A2 + _A2 * A1 * trg.data[index + 2]) / A;

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
