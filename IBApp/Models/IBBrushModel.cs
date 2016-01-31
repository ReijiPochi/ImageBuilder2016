using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using IBFramework.Image;
using IBFramework.Image.Pixel;

namespace IBApp.Models
{
    public class IBBrushModel : NotificationObject
    {
        private static Pen PenBrush = new Pen();
        private static Pencil PencilBrush = new Pencil();
        private static Eraser EraserBrush = new Eraser();
        private static SelectionTool SelectionToolBrush = new SelectionTool();
        private static Deformer DeformerBrush = new Deformer();

        private static void Set(IBBrush brush)
        {
            if (IBProjectModel.Current.SelectedBrush != null)
            {
                if (IBProjectModel.Current.SelectedBrush == brush) return;

                IBProjectModel.Current.SelectedBrush.Deacive();
            }

            IBProjectModel.Current.SelectedBrush = brush;
            brush.Activate(null, null);
        }

        public static void SetToProjectPen()
        {
            Set(PenBrush);
        }

        public static void SetToProjectPencil()
        {
            Set(PencilBrush);
        }

        public static void SetToProjectEraser()
        {
            Set(EraserBrush);
        }

        public static void SetToProjectSelectionTool()
        {
            Set(SelectionToolBrush);
        }

        public static void SetToProjectDeformer()
        {
            Set(DeformerBrush);
        }
    }
}
