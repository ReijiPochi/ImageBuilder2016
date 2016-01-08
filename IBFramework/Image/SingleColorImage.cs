using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCLFunctions;

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

        public override void Render(CLBuffer trg, IBRectangle trgSize, CLBuffer buffer)
        {
            BlendMode.Blend(this, trg, trgSize, buffer);
        }

        public PixelData color = new PixelData();
    }
}
