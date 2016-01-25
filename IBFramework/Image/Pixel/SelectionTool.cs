﻿using System;
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

        public override void Set(IBCanvas canvas, IBProjectElement trg, IBCoord coord)
        {
            base.Set(canvas, trg, coord);

            ActiveBrush = this;

            actionSummary = "Selection Tool / " + trg.Name;

            if (trgImage as CellSource == null || trgLayer == null) return;

            if (SelectedArea != null)
                ((CellSource)trgImage).Layers.Remove(SelectedArea);

            SelectedArea = new PixcelImage(
                (int)trgLayer.imageData.actualSize.Width,
                (int)trgLayer.imageData.actualSize.Height,
                (int)trgLayer.Rect.OffsetX,
                (int)trgLayer.Rect.OffsetY);
            SelectedArea.IsNotSelectersLayer = false;

            //SelectedArea.Rect.Width = trgLayer.imageData.actualSize.Width;
            //SelectedArea.Rect.Height = trgLayer.imageData.actualSize.Height;
            //SelectedArea.Rect.OffsetX = trgLayer.Rect.OffsetX;
            //SelectedArea.Rect.OffsetY = trgLayer.Rect.OffsetY;


            ((CellSource)trgImage).Layers.Insert(0, SelectedArea);
            SelectedArea.imageData.SetDrawingMode();
        }

        public override void Draw(IBCoord coord)
        {
            base.Draw(coord);

            double dist = IBCoord.GetDistance(histCoord[0], coord);
            if (dist < 0.1) return;

            DrawToLineDrawingImage(SelectedArea, 1, new PixelData() { r = 255, g = 50, b = 150, a = 100});
        }

        public override void End()
        {
            //base.End();
            //((CellSource)trgImage).Layers.Remove(SelectedArea);
        }














        private double last_t = 0;
        private void DrawToLineDrawingImage(IBImage trg, double r, PixelData color)
        {
            double _x = curCoord.x - trg.Rect.OffsetX, _y = curCoord.y - trg.Rect.OffsetY;
            if (_x < 0 || _y < 0 || _x >= trg.imageData.actualSize.Width || _y >= trg.imageData.actualSize.Height) return;

            double preX = histCoord[1].x - trg.Rect.OffsetX, preY = histCoord[1].y - trg.Rect.OffsetY, prePre = histPressure[1];
            double dx = curCoord.x - histCoord[1].x, dy = curCoord.y - histCoord[1].y, dp = curPressure - prePre;
            double length = Math.Sqrt(dx * dx + dy * dy);
            if (length > 200) return;
            double interval = 0.1 / length;
            double t = last_t / length;

            while (t < 1.0)
            {
                double x = preX + dx * t;
                double y = preY + dy * t;

                double p = prePre + dp * t;
                double _r = r;
                if (p != 0.0) _r *= PenTouch(p);

                DrawCircle(trg, x, y, _r, color);

                t += r * interval;
            }

            last_t = length * (t - 1.0);


            EntryTexUpdate(trg.imageData);
        }

        private double PenTouch(double inValue)
        {
            const double PI2 = Math.PI / 2.5;

            return 1.0 - Math.Sin((1 - inValue * inValue) * PI2);
        }

        private void DrawCircle(IBImage trg, double x, double y, double r, PixelData color)
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
                        data[offset + xp] = color.b;
                        data[offset + xp + 1] = color.g;
                        data[offset + xp + 2] = color.r;

                        int a = (color.a * c) >> 4;
                        data[offset + xp + 3] = a > data[offset + xp + 3] ? (byte)a : data[offset + xp + 3];
                    }

                    xp += 4;
                }
            }
        }
    }
}
