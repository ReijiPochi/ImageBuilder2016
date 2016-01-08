using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBFramework.Image.Pixel;
using OpenCLFunctions;

namespace IBFramework.Image.Blend
{
    public enum BlendModes
    {
        Normal,
        Add
    }

    public abstract class BlendMode
    {
        public abstract void Blend(SingleColorImage source, CLBuffer trg, IBRectangle trgSize, CLBuffer buffer);
        public abstract void Blend(PixcelImage source, CLBuffer trg, IBRectangle trgSize, CLBuffer buffer);
    }
}
