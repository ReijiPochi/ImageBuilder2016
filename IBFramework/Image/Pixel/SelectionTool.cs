using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using IBFramework.Project;
using IBFramework.Timeline;
using IBFramework.Project.IBProjectElements;

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

        public override void Set(IBCanvas canvas, IBProjectElement trg, IBCoord coord)
        {
            base.Set(canvas, trg, coord);

            ActiveBrush = this;

            actionSummary = "Selection Tool / " + trg.Name;

            if (trgImage as CellSource == null || trgLayer == null) return;

            if (SelectedArea != null)
            {
                ((CellSource)trgImage).Layers.Remove(SelectedArea);
                SelectedArea.imageData.data = null;
            }

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
        }

        public override void Draw(IBCoord coord)
        {
            base.Draw(coord);

            double dist = IBCoord.GetDistance(histCoord[0], coord);
            if (dist < 0.1) return;

            Draw(SelectedArea);
        }

        public override void End()
        {
            //base.End();

            EndDrawing(SelectedArea);
            Fill(SelectedArea);
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

                DrawCircle(trg, x, y, 0.5);

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

                DrawCircle(trg, x, y, 0.5);

                t += 0.5 * interval;
            }

            last_t = length * (t - 1.0);


            EntryTexUpdate(trg.imageData);
        }

        private void Fill(IBImage trg)
        {
            
        }

        private void DrawCircle(IBImage trg, double x, double y, double r)
        {
            if (r < 0.001) return;

            int imageW = (int)trg.imageData.actualSize.Width;
            int imageH = (int)trg.imageData.actualSize.Height;
            byte[] data = trg.imageData.data;
            int stride = imageW * 4;

            int xs = (int)Math.Floor(x - r);
            int xe = (int)Math.Floor(x + r);
            int ys = (int)Math.Floor(y - r);
            int ye = (int)Math.Floor(y + r);

            if (xs < 0) xs = 0;
            if (ys < 0) ys = 0;
            if (xe >= imageW) xe = imageW - 1;
            if (ye >= imageH) ye = imageH - 1;

            RecordDrawArea(xs, ys, xe, ye);

            double r2 = r * r;
            double sample = 4.0;

            for (int yi = ys; yi <= ye; yi++)
            {
                int offset = yi * stride;
                int xp = xs * 4;

                for (int xi = xs; xi <= xe; xi++)
                {
                    int c = 0;

                    for (int _yi = 0; _yi < sample; _yi++)
                    {
                        double yy = yi - y + _yi / sample;

                        for (int _xi = 0; _xi < sample; _xi++)
                        {
                            double xx = xi - x + _xi / sample;

                            if (xx * xx + yy * yy < r2)
                                c++;
                        }
                    }

                    if (c != 0)
                    {
                        data[offset + xp + 0] = 255;
                        data[offset + xp + 1] = 100;
                        data[offset + xp + 2] = 0;
                        int a = (255 * c) >> 4;
                        data[offset + xp + 3] = a > data[offset + xp + 3] ? (byte)a : data[offset + xp + 3];
                    }

                    xp += 4;
                }
            }
        }
    }
}
