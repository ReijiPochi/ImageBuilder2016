using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using System.Drawing;

namespace IBFramework.Timeline
{
    public class IBCanvas_utilities
    {
        public static IBCoord GetImageCoord(IBCanvas source, Point mousePos, double zoom)
        {
            if (source.ShowingElement == null) return new IBCoord();

            double resultX = (source.camX - (source.glControl.Width / 2 - mousePos.X)) / zoom;
            double resultY = (source.ShowingElement.Height * zoom - source.camY - (source.glControl.Height / 2 - mousePos.Y)) / zoom;

            return new IBCoord(resultX, resultY);
        }
    }
}
