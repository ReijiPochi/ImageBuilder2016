using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework.Image
{
    public class SingleColorImage : IBImage
    {
        public SingleColorImage()
        {

        }

        public SingleColorImage(byte b, byte g, byte r, byte a)
        {
            color.b = b;
            color.g = g;
            color.r = r;
            color.a = a;
        }

        public override PixelData GetRenderdPixelData(int x, int y)
        {
            return color;
        }

        public override void RenderTo(BGRA32FormattedImage trg)
        {
            blendMode.Blend(this, trg);
        }

        public PixelData color = new PixelData();
    }
}
