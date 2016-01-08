using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBFramework.Image.Blend;
using System.ComponentModel;
using OpenCLFunctions;

namespace IBFramework.Image
{
    public abstract class IBImage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _LayerName;
        public string LayerName
        {
            get
            { return _LayerName; }
            set
            {
                if (_LayerName == value)
                    return;
                _LayerName = value;
                RaisePropertyChanged("LayerName");
            }
        }

        private ImageTypes _LayerType;
        public ImageTypes LayerType
        {
            get
            { return _LayerType; }
            set
            {
                if (_LayerType == value)
                    return;
                _LayerType = value;
                RaisePropertyChanged("LayerType");
            }
        }

        private bool _IsSelectedLayer;
        public bool IsSelectedLayer
        {
            get
            { return _IsSelectedLayer; }
            set
            {
                if (_IsSelectedLayer == value)
                    return;
                _IsSelectedLayer = value;
                RaisePropertyChanged("IsSelectedLayer");
            }
        }


        private IBRectangle _Rect = new IBRectangle();
        public IBRectangle Rect
        {
            get
            { return _Rect; }
            set
            {
                if (_Rect == value)
                    return;
                _Rect = value;
                RaisePropertyChanged("Rect");
            }
        }

        private BlendMode _BlendMode = new Normal();
        public BlendMode BlendMode
        {
            get
            { return _BlendMode; }
            set
            {
                if (_BlendMode == value)
                    return;
                _BlendMode = value;
                RaisePropertyChanged("blendMode");
            }
        }

        /// <summary>
        /// trgにこの画像をレンダリングします。buffer1は、データ転送に使います
        /// </summary>
        /// <param name="trg"></param>
        /// <param name="buffer1"></param>
        public abstract void Render(CLBuffer trg, IBRectangle trgSize, CLBuffer buffer1);
    }
}
