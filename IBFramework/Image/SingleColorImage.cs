using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IBFramework.Image
{
    public class SingleColorImage : IBImage, IProperty
    {
        public SingleColorImage()
        {
            LayerType = ImageTypes.SingleColor;
            _Color = new PixelData();
            PropertyChanged += SingleColorImage_PropertyChanged;
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
            PropertyChanged += SingleColorImage_PropertyChanged;
        }

        private void SingleColorImage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "LayerName")
            {
                PropertyHeaderName = "Layer \"" + LayerName + "\"";
            }
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

        private string _PropertyHeaderName;
        public string PropertyHeaderName
        {
            get { return _PropertyHeaderName; }
            set
            {
                if (_PropertyHeaderName == value)
                    return;
                _PropertyHeaderName = value;
                RaisePropertyChanged("PropertyHeaderName");
            }
        }

        private bool _IsLocked;
        public bool IsLocked
        {
            get
            { return _IsLocked; }
            set
            {
                if (_IsLocked == value)
                    return;
                _IsLocked = value;
                RaisePropertyChanged("IsLocked");
            }
        }

        public Control GetPP()
        {
            return new SingleColorImagePP() { DataContext = this };
        }
    }
}
