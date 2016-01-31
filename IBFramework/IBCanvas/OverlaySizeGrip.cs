using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IBFramework.IBCanvas
{
    public class OverlaySizeGrip : Thumb, IOverlayItems
    {
        static OverlaySizeGrip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlaySizeGrip), new FrameworkPropertyMetadata(typeof(OverlaySizeGrip)));
        }

        public OverlaySizeGrip(double w, double h, double offsetX, double offsetY, int r)
        {
            OverlayWidth = w;
            OverlayHeight = h;
            OverlayOffsetX = offsetX;
            OverlayOffsetY = offsetY;
            R = new CornerRadius(r);
            SnapsToDevicePixels = true;
        }

        public CornerRadius R { get; set; }

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
                SetValue(Canvas.LeftProperty, (OverlayOffsetX - OverlayWidth / 2.0) * Zoom + _CamOffsetX);
                SetValue(Canvas.TopProperty, _CamOffsetY + (OverlayOffsetY - OverlayHeight / 2.0) * Zoom);
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
                SetValue(Canvas.LeftProperty, (OverlayOffsetX) * Zoom + _CamOffsetX - OverlayWidth / 2.0);
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
                SetValue(Canvas.TopProperty, _CamOffsetY + (OverlayOffsetY) * Zoom - OverlayHeight / 2.0);
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
                Height = _OverlayHeight;
                SetValue(Canvas.TopProperty, _CamOffsetY + (OverlayOffsetY) * Zoom - OverlayHeight / 2.0);
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
                Width = _OverlayWidth;
                SetValue(Canvas.LeftProperty, (OverlayOffsetX) * Zoom + _CamOffsetX - OverlayWidth / 2.0);
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
                SetValue(Canvas.LeftProperty, (OverlayOffsetX) * Zoom + _CamOffsetX - OverlayWidth / 2.0);
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
                SetValue(Canvas.TopProperty, _CamOffsetY + (OverlayOffsetY) * Zoom - OverlayHeight / 2.0);
            }
        }
    }
}
