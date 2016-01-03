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
            LayerType = ImageTypes.SingleColor;
        }

        public SingleColorImage(byte b, byte g, byte r, byte a)
        {
            LayerType = ImageTypes.SingleColor;
            color.b = b;
            color.g = g;
            color.r = r;
            color.a = a;
        }

        public override void RenderTo(BGRA32FormattedImage trg)
        {
            BlendMode.Blend(this, trg);
        }

        public PixelData color = new PixelData();
    }
}
