using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace IBFramework.Image.Pixel
{
    public class PixcelImage : IBImage
    {
        public PixcelImage()
        {
            LayerType = ImageTypes.Pixel;
        }

        public PixcelImage(Bitmap bitmap)
        {
            LayerType = ImageTypes.Pixel;

            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            imageData = new BGRA32FormattedImage(data.Width, data.Height);
            Rect = new IBRectangle(data.Width, data.Height);
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                for (int i = 0; i < data.Stride * data.Height; i++)
                {
                    imageData.data[i] = ptr[i];
                }
            }
            imageData.TextureUpdate();
        }

        public PixcelImage(int w, int h, int offsetX, int offsetY)
        {
            imageData = new BGRA32FormattedImage(w, h);
            Rect = new IBRectangle(w, h, offsetX, offsetY);
        }

        public override void CopyTo(IBImage i)
        {
            i = new PixcelImage();
            base.CopyTo(i);
        }
    }
}
