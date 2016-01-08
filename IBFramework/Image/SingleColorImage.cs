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
            imageData = new BGRA32FormattedImage(1, 1, Color);
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
    }
}
