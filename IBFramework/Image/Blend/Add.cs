using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBFramework.Image.Pixel;

namespace IBFramework.Image.Blend
{
    //public class Add : BlendMode
    //{
    //    public override void Blend(PixcelImage source, BGRA32FormattedImage trg)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Blend(SingleColorImage source, BGRA32FormattedImage trg)
    //    {
    //        int trgSizeW = (int)trg.size.Width;
    //        int trgSizeH = (int)trg.size.Height;

    //        if ((int)source.Size.OffsetX > trgSizeW || (int)source.Size.OffsetY > trgSizeH)
    //            return;

    //        int strX = 0, strY = 0;

    //        if ((int)source.Size.OffsetX < 0)
    //            strX = 0;
    //        else if ((int)source.Size.OffsetX > (int)trg.size.OffsetX + (int)trg.size.Width)
    //            strX = (int)trg.size.OffsetX + (int)trg.size.Width;
    //        else
    //            strX = (int)source.Size.OffsetX;

    //        if ((int)source.Size.OffsetY < 0)
    //            strY = 0;
    //        else if ((int)source.Size.OffsetY > (int)trg.size.OffsetY + (int)trg.size.Height)
    //            strY = (int)trg.size.OffsetY + (int)trg.size.Height;
    //        else
    //            strY = (int)source.Size.OffsetY;


    //        int maxX = 0, maxY = 0;

    //        if ((int)source.Size.OffsetX + (int)source.Size.Width < (int)trg.size.OffsetX)
    //            maxX = (int)trg.size.OffsetX;
    //        else if ((int)source.Size.OffsetX + (int)source.Size.Width > (int)trg.size.OffsetX + (int)trg.size.Width)
    //            maxX = (int)trg.size.OffsetX + (int)trg.size.Width;
    //        else
    //            maxX = (int)source.Size.OffsetX + (int)source.Size.Width;

    //        if ((int)source.Size.OffsetY + (int)source.Size.Height < (int)trg.size.OffsetY)
    //            maxY = (int)trg.size.OffsetY;
    //        else if ((int)source.Size.OffsetY + (int)source.Size.Height > (int)trg.size.OffsetY + (int)trg.size.Height)
    //            maxY = (int)trg.size.OffsetY + (int)trg.size.Height;
    //        else
    //            maxY = (int)source.Size.OffsetY + (int)source.Size.Height;

    //        int trgDataLength = trg.data.Length;

    //        for (int y = strY; y < maxY; y++)
    //        {
    //            for (int x = strX; x < maxX; x++)
    //            {
    //                int index = (x + y * trgSizeW) * 4;
    //                if (index >= trgDataLength) break;

    //                double A1 = trg.data[index + 3] / 255.0;
    //                double A2 = source.color.a / 255.0;

    //                double A = A2 + (1 - A2) * A1;
    //                double R = ((source.color.r + trg.data[index + 2]) * A2 + (1 - A2) * A1 * trg.data[index + 2]) / A;
    //                double G = ((source.color.g + trg.data[index + 1]) * A2 + (1 - A2) * A1 * trg.data[index + 1]) / A;
    //                double B = ((source.color.b + trg.data[index]) * A2 + (1 - A2) * A1 * trg.data[index]) / A;

    //                trg.data[index] = (byte)(B > 255 ? 255 : B);
    //                trg.data[index + 1] = (byte)(G > 255 ? 255 : G);
    //                trg.data[index + 2] = (byte)(R > 255 ? 255 : R);
    //                trg.data[index + 3] = (byte)(A * 255.0);
    //            }
    //        }
    //    }
    //}
}
