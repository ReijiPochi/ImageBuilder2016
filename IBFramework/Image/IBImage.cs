using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBFramework.Image.Blend;

namespace IBFramework.Image
{
    public abstract class IBImage
    {
        public abstract PixelData GetRenderdPixelData(int x, int y);

        public abstract void RenderTo(BGRA32FormattedImage trg);

        public IBRectangle size = new IBRectangle();
        public BlendMode blendMode = new Normal();
    }
}
