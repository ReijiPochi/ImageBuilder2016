using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBFramework.Image.Pixel;

namespace IBFramework.Image.Blend
{
    class Multiply : BlendMode
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

            int maxX = (int)source.size.OffsetX + (int)source.size.Width;
            int maxY = (int)source.size.OffsetY + (int)source.size.Height;

            int trgDataLength = trg.data.Length;

            for (int y = (int)source.size.OffsetY; y < maxY; y++)
            {
                for (int x = (int)source.size.OffsetX; x < maxX; x++)
                {
                    int index = (x + y * trgSizeW) * 4;
                    if (index >= trgDataLength) break;

                    double A1 = trg.data[index + 3] / 255.0;
                    double A2 = source.color.a / 255.0;

                    double A = A2 + (1 - A2) * A1;
                    double R = ((source.color.r * trg.data[index + 2]) * A2 + (1 - A2) * A1 * trg.data[index + 2] * source.color.r) / 255.0 / A;
                    double G = ((source.color.g * trg.data[index + 2]) * A2 + (1 - A2) * A1 * trg.data[index + 1] * source.color.r) / 255.0 / A;
                    double B = ((source.color.b * trg.data[index + 2]) * A2 + (1 - A2) * A1 * trg.data[index] * source.color.r) / 255.0 / A;

                    trg.data[index] = (byte)(B > 255 ? 255 : B);
                    trg.data[index + 1] = (byte)(G > 255 ? 255 : G);
                    trg.data[index + 2] = (byte)(R > 255 ? 255 : R);
                    trg.data[index + 3] = (byte)(A * 255.0);
                }
            }
        }
    }
}
