using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using IBFramework.Image.Pixel;

namespace IBApp.Models
{
    public class IBBrushModel : NotificationObject
    {
        private static Pen PenBrush = new Pen();
        private static Eraser EraserBrush = new Eraser();

        public static void SetToProjectPen()
        {
            IBProjectModel.Current.SelectedBrush = PenBrush;
        }

        public static void SetToProjectEraser()
        {
            IBProjectModel.Current.SelectedBrush = EraserBrush;
        }
    }
}
