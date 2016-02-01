using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using System.Drawing;
using System.Windows.Controls;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace IBFramework.IBCanvas
{
    public partial class IBCanvasControl
    {
        public static IBCoord GetImageCoord(IBCanvasControl source, System.Windows.Point mousePos, double zoom)
        {
            if (source.TargetLayer == null) return new IBCoord();

            PresentationSource s = PresentationSource.FromVisual(source);
            double dpiX = s.CompositionTarget.TransformToDevice.M11;
            double dpiY = s.CompositionTarget.TransformToDevice.M22;

            double resultX = (source.camX - (source.glControl.Width / 2 - mousePos.X * dpiX)) / zoom;
            double resultY = (source.TargetLayer.imageData.actualSize.Height * zoom - source.camY - (source.glControl.Height / 2 - mousePos.Y * dpiY)) / zoom;

            resultY += source.TargetLayer.Rect.OffsetY * 2.0;

            return new IBCoord(resultX, resultY);
        }

        public void DrawOuterCenterMark()
        {
            double imageW = ShowingElement.Width * ZoomPerCent * 0.01, imageH = ShowingElement.Height * ZoomPerCent * 0.01;
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(0, imageH / 2, 30);
                GL.Vertex3(-15, imageH / 2, 30);

                GL.Vertex3(imageW, imageH / 2, 30);
                GL.Vertex3(imageW + 15, imageH / 2, 30);

                GL.Vertex3(imageW / 2, 0, 30);
                GL.Vertex3(imageW / 2, -15, 30);

                GL.Vertex3(imageW / 2, imageH, 30);
                GL.Vertex3(imageW / 2, imageH + 15, 30);
            }
            GL.End();
        }

        public void DrawCornerMark()
        {
            double imageW = ShowingElement.Width * ZoomPerCent * 0.01, imageH = ShowingElement.Height * ZoomPerCent * 0.01;
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(0, -1, 30);
                GL.Vertex3(-10, -1, 30);
                GL.Vertex3(-1, 0, 30);
                GL.Vertex3(-1, -10, 30);

                GL.Vertex3(-1, imageH, 30);
                GL.Vertex3(-1 - 10, imageH, 30);
                GL.Vertex3(-1, imageH, 30);
                GL.Vertex3(-1, imageH + 10, 30);

                GL.Vertex3(imageW, -1, 30);
                GL.Vertex3(imageW + 10, -1, 30);
                GL.Vertex3(imageW, -1, 30);
                GL.Vertex3(imageW, -1 - 10, 30);

                GL.Vertex3(imageW, imageH, 30);
                GL.Vertex3(imageW + 10, imageH, 30);
                GL.Vertex3(imageW, imageH, 30);
                GL.Vertex3(imageW, imageH + 10, 30);
            }
            GL.End();
        }

        public void DrawImageFrame()
        {
            double imageW = ShowingElement.Width * ZoomPerCent * 0.01, imageH = ShowingElement.Height * ZoomPerCent * 0.01;
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(-1, -1, 30);
                GL.Vertex3(-1, imageH, 30);

                GL.Vertex3(-1, imageH, 30);
                GL.Vertex3(imageW, imageH, 30);

                GL.Vertex3(imageW, imageH, 30);
                GL.Vertex3(imageW, -1, 30);

                GL.Vertex3(imageW, -1, 30);
                GL.Vertex3(-1, -1, 30);
            }
            GL.End();
        }

        public void DrawCinemaScopeFrame()
        {
            double imageW = ShowingElement.Width * ZoomPerCent * 0.01, imageH = ShowingElement.Width * ZoomPerCent * 0.01 / 2.35;
            double LowHori = (ShowingElement.Height * ZoomPerCent * 0.01 - imageH) / 2, HighHori = ShowingElement.Height * ZoomPerCent * 0.01 - LowHori;
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(-1, LowHori, 29);
                GL.Vertex3(-1, HighHori, 29);

                GL.Vertex3(-1, HighHori, 29);
                GL.Vertex3(imageW, HighHori, 29);

                GL.Vertex3(imageW, HighHori, 29);
                GL.Vertex3(imageW, LowHori, 29);

                GL.Vertex3(imageW, LowHori, 29);
                GL.Vertex3(-1, LowHori, 29);
            }
            GL.End();
        }

        public void AddOverlayItem(FrameworkElement item)
        {
            if (item.Parent != null)
            {
                ((Canvas)item.Parent).Children.Remove(item);
            }
            
            overlayCanvas.Children.Add(item);
            RefreshOverlay();
        }

        public void RemoveOverlayItem(FrameworkElement item)
        {
            if (item.Parent != null)
            {
                ((Canvas)item.Parent).Children.Remove(item);
            }

            overlayCanvas.Children.Remove(item);
            RefreshOverlay();
        }

        public void RefreshOverlay()
        {
            if (overlayCanvas == null) return;
            double zoom = ZoomPerCent / 100.0;

            foreach (UIElement c in overlayCanvas.Children)
            {
                IOverlayItems item = c as IOverlayItems;
                if(item != null)
                {
                    item.Zoom = zoom;
                    item.CamOffsetX = glControl.Width / 2 - camX;
                    item.CamOffsetY = camY + glControl.Height / 2;
                }
            }
        }
    }
}
