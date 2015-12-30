using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBFramework.Image.Pixel;

namespace IBFramework.Image.Blend
{
    public enum BlendModes
    {
        Normal,
        Add
    }

    public abstract class BlendMode
    {
        public abstract void Blend(SingleColorImage source, BGRA32FormattedImage trg);
        public abstract void Blend(PixcelImage source, BGRA32FormattedImage trg);
    }
}
