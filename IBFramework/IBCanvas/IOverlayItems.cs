using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework.IBCanvas
{
    public interface IOverlayItems
    {
        double Zoom { get; set; }
        double CamOffsetX { get; set; }
        double CamOffsetY { get; set; }


        double OverlayWidth { get; set; }
        double OverlayHeight { get; set; }
        double OverlayOffsetX { get; set; }
        double OverlayOffsetY { get; set; }
    }
}
