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
            _Color = new PixelData();
        }

        public SingleColorImage(byte b, byte g, byte r, byte a)
        {
            LayerType = ImageTypes.SingleColor;
            PixelData color = new PixelData()
            {
                b = b,
                g = g,
                r = r,
                a = a
            };

            Color = color;
            imageData = new BGRA32FormattedImage(1, 1);
            imageData.ClearData(Color);
            imageData.TextureUpdate();
        }

        private PixelData _Color;
        public PixelData Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                RaisePropertyChanged("Color");
            }
        }

        public override void CopyTo(IBImage i)
        {
            i = new SingleColorImage();
            base.CopyTo(i);
            ((SingleColorImage)i).Color = Color;
        }
    }
}
