using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCLFunctions;

namespace IBFramework.Image.Pixel
{
    public class PixcelImage : IBImage
    {
        public PixcelImage()
        {
            LayerType = ImageTypes.Pixel;
        }

        public PixelData[,] currentData;

        public override void Render(CLBuffer trg, IBRectangle trgSize, CLBuffer buffer1)
        {
            throw new NotImplementedException();
        }
    }
}
