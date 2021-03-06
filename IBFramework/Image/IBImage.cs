﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBFramework.Image.Blend;
using System.ComponentModel;
using IBFramework.IBCanvas;
using IBFramework.Project;

namespace IBFramework.Image
{
    public class IBImage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public IBProjectElement owner;
        public BGRA32FormattedImage imageData;

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

        private bool _IsVisible = true;
        public bool IsVisible
        {
            get
            { return _IsVisible; }
            set
            {
                if (_IsVisible == value)
                    return;
                _IsVisible = value;
                RaisePropertyChanged("IsVisible");
                IBCanvasControl.RefreshAll();
            }
        }

        private bool _IsNotSelectersLayer = true;
        public bool IsNotSelectersLayer
        {
            get
            { return _IsNotSelectersLayer; }
            set
            {
                if (_IsNotSelectersLayer == value)
                    return;
                _IsNotSelectersLayer = value;
                RaisePropertyChanged("IsNotSelectersLayer");
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

        public virtual void CopyTo(IBImage i)
        {
            i.imageData = new BGRA32FormattedImage((int)imageData.actualSize.Width, (int)imageData.actualSize.Height);
            i.imageData.SetDrawingMode();
            for(int c = 0; c < i.imageData.data.Length; c++)
            {
                i.imageData.data[c] = imageData.data[c];
            }
            i.IsSelectedLayer = IsSelectedLayer;
            i.LayerName = LayerName;
            i.LayerType = LayerType;
            i.PropertyChanged = PropertyChanged;
            i.Rect = new IBRectangle(Rect.Width, Rect.Height, Rect.OffsetX, Rect.OffsetY);
            i.imageData.EndDrawingMode();
        }
    }
}
