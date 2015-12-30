using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework.Image.Pixel
{
    public class PixcelImage : IBImage
    {
        public PixelData[,] currentData;

        public override PixelData GetRenderdPixelData(int x, int y)
        {
            return currentData[x, y];
        }

        public override void RenderTo(BGRA32FormattedImage trg)
        {
            throw new NotImplementedException();
        }
    }
}
