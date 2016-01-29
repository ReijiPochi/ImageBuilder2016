using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using IBFramework.Project;
using IBFramework.Timeline;
using IBFramework.Project.IBProjectElements;
using IBFramework.OpenGL;

namespace IBFramework.Image.Pixel
{
    public class SelectionTool : IBBrush
    {
        public override Control GetBP()
        {
            return null;
        }

        private PixcelImage SelectedArea;
        private IBCoord start = new IBCoord();
        public bool IsSelecting { get; private set; }

        public override bool Set(IBCanvas canvas, IBProjectElement trg, IBCoord coord)
        {
            if (!base.Set(canvas, trg, coord)) return false;

            ActiveBrush = this;

            actionSummary = "Selection Tool / " + trg.Name;
            Render.OverrayColor[0] = 0;
            Render.OverrayColor[1] = 0;
            Render.OverrayColor[2] = 0;

            if (SelectedArea != null)
            {
                if (IsSelecting)
                {
                    IsSelecting = false;
                    SelectersLayerMode = false;
                    EntryTexUpdate(SelectedArea.imageData);
                }

                for (int a = ((CellSource)trgImage).Layers.Count - 1; a >= 0; a--)
                {
                    IBImage i = ((CellSource)trgImage).Layers[a];
                    if (!i.IsNotSelectersLayer)
                    {
                        ((CellSource)trgImage).Layers.Remove(i);
                        i.imageData.data = null;
                    }
                }
            }

            if (trgImage as CellSource == null || trgLayer == null) return false;

            SelectedArea = new PixcelImage(
                (int)trgLayer.imageData.actualSize.Width,
                (int)trgLayer.imageData.actualSize.Height,
                (int)trgLayer.Rect.OffsetX,
                (int)trgLayer.Rect.OffsetY);
            SelectedArea.IsNotSelectersLayer = false;

            ((CellSource)trgImage).Layers.Insert(0, SelectedArea);
            SelectedArea.imageData.SetDrawingMode();

            start.x = histCoord[0].x;
            start.y = histCoord[0].y;

            return true;
        }

        public override void Draw(IBCoord coord)
        {
            base.Draw(coord);

            IsSelecting = true;
            SelectersLayerMode = true;

            double dist = IBCoord.GetDistance(histCoord[0], coord);
            if (dist < 0.1) return;

            Draw(SelectedArea);
        }

        public override void End()
        {
            //base.End();

            if (IsSelecting)
            {
                EndDrawing(SelectedArea);
                Thread fill = new Thread(new ThreadStart(Fill), 500000000);
                fill.Start();
                fill.Join();
                Reverse();
            }
        }


        private double last_t = 0;
        private void Draw(IBImage trg)
        {
            if (trg == null) return;

            double _x = curCoord.x - trg.Rect.OffsetX, _y = curCoord.y - trg.Rect.OffsetY;

            double preX = histCoord[1].x - trg.Rect.OffsetX, preY = histCoord[1].y - trg.Rect.OffsetY;
            double dx = curCoord.x - histCoord[1].x, dy = curCoord.y - histCoord[1].y;
            double length = Math.Sqrt(dx * dx + dy * dy);
            if (length > 200) return;
            double interval = 0.1 / length;
            double t = last_t / length;

            while (t < 1.0)
            {
                double x = preX + dx * t;
                double y = preY + dy * t;

                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x >= trg.imageData.actualSize.Width) x = trg.imageData.actualSize.Width - 1;
                if (y >= trg.imageData.actualSize.Height) y = trg.imageData.actualSize.Height - 1;

                DrawPoint(trg, x, y);

                t += 0.5 * interval;
            }

            last_t = length * (t - 1.0);


            EntryTexUpdate(trg.imageData);
        }

        private void EndDrawing(IBImage trg)
        {
            double _x = curCoord.x - trg.Rect.OffsetX, _y = curCoord.y - trg.Rect.OffsetY;

            double preX = start.x - trg.Rect.OffsetX, preY = start.y - trg.Rect.OffsetY;
            double dx = curCoord.x - start.x, dy = curCoord.y - start.y;
            double length = Math.Sqrt(dx * dx + dy * dy);
            double interval = 0.1 / length;
            double t = last_t / length;

            while (t < 1.0)
            {
                double x = preX + dx * t;
                double y = preY + dy * t;

                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x >= trg.imageData.actualSize.Width) x = trg.imageData.actualSize.Width - 1;
                if (y >= trg.imageData.actualSize.Height) y = trg.imageData.actualSize.Height - 1;

                DrawPoint(trg, x, y);

                t += interval;
            }

            last_t = length * (t - 1.0);


            EntryTexUpdate(trg.imageData);
        }

        private void Fill()
        {
            stride = (drawAreaXE - drawAreaXS) * 4;
            height = drawAreaYE - drawAreaYS;
            width = drawAreaXE - drawAreaXS;

            SelectedArea.imageData.drawingAreaSize.OffsetX = drawAreaXS;
            SelectedArea.imageData.drawingAreaSize.OffsetY = drawAreaYS;
            SelectedArea.imageData.drawingAreaSize.Width = width;
            SelectedArea.imageData.drawingAreaSize.Height = height;

            _fill(- 1, - 1);
        }

        private int stride, height, width;

        private void _fill(int x, int y)
        {
            int index = (drawAreaYS + y) * (int)SelectedArea.imageData.actualSize.Width * 4 + (drawAreaXS + x) * 4;
            int stride = (int)SelectedArea.imageData.actualSize.Width;
            int length = SelectedArea.imageData.data.Length;

            if (index >= 0 && index < length)
            {
                SelectedArea.imageData.data[index] = 255;
                SelectedArea.imageData.data[index + 3] = 255;
            }

            if (x >= -1 && y >= 0 && x <= width && y <= height)
            {
                int i = ((drawAreaYS + y - 1) * stride + (drawAreaXS + x)) * 4;
                if (i >= 0 && i < length && SelectedArea.imageData.data[i] != 255 && SelectedArea.imageData.data[i + 2] != 255) _fill(x, y - 1);
            }
            if (x >= 0 && y >= -1 && x <= width && y <= height)
            {
                int i = ((drawAreaYS + y) * stride + (drawAreaXS + x - 1)) * 4;
                if (i >= 0 && i < length && SelectedArea.imageData.data[i] != 255 && SelectedArea.imageData.data[i + 2] != 255) _fill(x - 1, y);
            }
            if (x >= -1 && y >= -1 && x <= width && y <= height - 1)
            {
                int i = ((drawAreaYS + y + 1) * stride + (drawAreaXS + x)) * 4;
                if (i >= 0 && i < length && SelectedArea.imageData.data[i] != 255 && SelectedArea.imageData.data[i + 2] != 255) _fill(x, y + 1);
            }
            if (x >= -1 && y >= -1 && x <= width - 1 && y <= height)
            {
                int i = ((drawAreaYS + y) * stride + (drawAreaXS + x + 1)) * 4;
                if (i >= 0 && i < length && SelectedArea.imageData.data[i] != 255 && SelectedArea.imageData.data[i + 2] != 255) _fill(x + 1, y);
            }
        }

        private void Reverse()
        {
            stride = (drawAreaXE - drawAreaXS) * 4;
            int height = drawAreaYE - drawAreaYS;
            int width = drawAreaXE - drawAreaXS;

            int layerStride = (int)SelectedArea.imageData.actualSize.Width * 4;
            int _offsetx = drawAreaXS * 4;

            for (int y = drawAreaYS - 1; y <= drawAreaYE; y++)
            {
                int offset = y * layerStride;
                for (int x = drawAreaXS - 1; x <= drawAreaXE; x++)
                {
                    int index = offset + x * 4;
                    if (index < 0 || index >= SelectedArea.imageData.data.Length) break;

                    if (SelectedArea.imageData.data[index] == 255)
                    {
                        SelectedArea.imageData.data[index] = 0;
                        SelectedArea.imageData.data[index + 1] = 0;
                        SelectedArea.imageData.data[index + 2] = 0;
                        SelectedArea.imageData.data[index + 3] = 0;
                    }
                    else
                    {
                        SelectedArea.imageData.data[index] = 0;
                        SelectedArea.imageData.data[index + 1] = 0;
                        SelectedArea.imageData.data[index + 2] = 0;
                        SelectedArea.imageData.data[index + 3] = 255;
                    }
                }
            }
        }

        private void DrawPoint(IBImage trg, double x, double y)
        {
            int imageW = (int)trg.imageData.actualSize.Width;
            int imageH = (int)trg.imageData.actualSize.Height;
            byte[] data = trg.imageData.data;
            int stride = imageW * 4;

            RecordDrawArea((int)x, (int)y, (int)x, (int)y);

            int offset = (int)y * stride;
            int xp = (int)x * 4;

            data[offset + xp + 2] = 255;
            data[offset + xp + 3] = 255;
        }
    }
}
