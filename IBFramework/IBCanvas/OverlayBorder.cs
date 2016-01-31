using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IBFramework.IBCanvas
{
    public class OverlayBorder : Border, IOverlayItems
    {
        public OverlayBorder(double w, double h, double offsetX, double offsetY)
        {
            OverlayWidth = w;
            OverlayHeight = h;
            OverlayOffsetX = offsetX;
            OverlayOffsetY = offsetY;
            SnapsToDevicePixels = true;
        }

        private double _Zoom;
        public double Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                _Zoom = value;
                Width = OverlayWidth * _Zoom;
                Height = OverlayHeight * _Zoom;
            }
        }

        private double _CamOffsetX;
        public double CamOffsetX
        {
            get
            {
                return _CamOffsetX;
            }
            set
            {
                _CamOffsetX = value;
                SetValue(Canvas.LeftProperty, (OverlayOffsetX) * Zoom + _CamOffsetX);
            }
        }

        private double _CamOffsetY;
        public double CamOffsetY
        {
            get
            {
                return _CamOffsetY;
            }
            set
            {
                _CamOffsetY = value;
                SetValue(Canvas.TopProperty, _CamOffsetY + (OverlayOffsetY) * Zoom);
            }
        }


        private double _OverlayHeight;
        public double OverlayHeight
        {
            get
            {
                return _OverlayHeight;
            }
            set
            {
                _OverlayHeight = value;
                Height = _OverlayHeight * Zoom;
            }
        }

        private double _OverlayWidth;
        public double OverlayWidth
        {
            get
            {
                return _OverlayWidth;
            }
            set
            {
                _OverlayWidth = value;
                Width = _OverlayWidth * Zoom;
            }
        }

        private double _OverlayOffsetX;
        public double OverlayOffsetX
        {
            get
            {
                return _OverlayOffsetX;
            }
            set
            {
                _OverlayOffsetX = value;
                SetValue(Canvas.LeftProperty, (OverlayOffsetX) * Zoom + _CamOffsetX);
            }
        }

        private double _OverlayOffsetY;
        public double OverlayOffsetY
        {
            get
            {
                return _OverlayOffsetY;
            }
            set
            {
                _OverlayOffsetY = value;
                SetValue(Canvas.TopProperty, _CamOffsetY + (OverlayOffsetY) * Zoom);
            }
        }
    }
}
