﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Input;

using IBFramework.IBCanvas;
using IBFramework.Project;
using IBFramework.Project.IBProjectElements;
using IBFramework.RedoUndo;
using IBFramework.OpenGL;
using Wintab;

namespace IBFramework.Image
{
    public abstract class IBBrush
    {
        static IBBrush()
        {
            Clock.Tick += Clock_Tick;
            Clock.Start();
            ResetDrawArea();
        }

        public IBBrush()
        {
            for (int i = 0; i < histCoord.Length; i++)
            {
                histCoord[i] = new IBCoord();
            }
        }

        public static PixelData Color { get; set; } = new PixelData();

        protected static IBProjectElement trgImage;
        protected static IBImage trgLayer;
        protected static IBBrush ActiveBrush;
        protected static IBCoord[] histCoord = new IBCoord[2];
        protected static IBCoord curCoord = new IBCoord();
        protected double[] histPressure = new double[2];
        protected double curPressure = 0;

        protected static int count = 0;
        protected static bool drawing;
        protected static bool penUp = true;
        protected static byte[] beforeData = new byte[5000 * 4 * 3000];
        protected static int beforeDataStride;

        protected static int drawAreaXS;
        protected static int drawAreaYS;
        protected static int drawAreaXE;
        protected static int drawAreaYE;
        protected static string actionSummary = "";

        private static DispatcherTimer Clock = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 10) };
        private static BGRA32FormattedImage waitingImage;
        protected static IBCanvasControl currentCanvas;

        public static bool SelectersLayerMode { get; protected set; }
        private static PixelData color1 = new PixelData() { r = 255, g = 20, b = 200};
        private static PixelData color2 = new PixelData() { r = 100, g = 210, b = 255};


        public abstract Control GetBP();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private double _Size = 5.0;
        public double Size
        {
            get
            { return _Size; }
            set
            {
                if (_Size == value)
                    return;
                _Size = value;
                RaisePropertyChanged("Size");
            }
        }

        private bool _AllowCorrection = true;
        public bool AllowCorrection
        {
            get
            { return _AllowCorrection; }
            set
            {
                if (_AllowCorrection == value)
                    return;
                _AllowCorrection = value;
                RaisePropertyChanged("AllowCorrection");
            }
        }

        private static bool colorState = false;
        private static double colorBalance = 0.0;
        private static void Clock_Tick(object sender, EventArgs e)
        {
            if (drawing && penUp)
            {
                if (count > 5)
                {
                    ActiveBrush.End();
                    
                    ResetDrawArea();
                    count = 0;
                }
                count++;
            }

            if (!drawing && SelectersLayerMode)
            {
                if(colorState)
                {
                    colorBalance += 0.02;
                    if (colorBalance >= 1.0) colorState = false;
                }
                else
                {
                    colorBalance -= 0.02;
                    if (colorBalance <= 0.0) colorState = true;
                }

                DynamicRender.OverrayColor[0] = (float)(color1.r * colorBalance + color2.r * (1.0 - colorBalance)) / 255.0f;
                DynamicRender.OverrayColor[1] = (float)(color1.g * colorBalance + color2.g * (1.0 - colorBalance)) / 255.0f;
                DynamicRender.OverrayColor[2] = (float)(color1.b * colorBalance + color2.b * (1.0 - colorBalance)) / 255.0f;
                DynamicRender.OverrayColor[3] = 0.6f;

                if (currentCanvas != null)
                    currentCanvas.glControl.Refresh();
            }

            if (waitingImage != null)
            {
                waitingImage.TextureUpdate();
                waitingImage = null;

                if (currentCanvas != null)
                    currentCanvas.glControl.Refresh();
            }
        }

        public virtual void Activate(IBCanvasControl canvas, IBProjectElement trg)
        {
            if (canvas != null) currentCanvas = canvas;
            ActiveBrush = this;

            if (trg == null) return;
            trgImage = trg;
            trgLayer = GetSelectedLayer();
        }

        public virtual void Refresh()
        {

        }

        public virtual bool Set(IBCanvasControl canvas, IBProjectElement trg, IBCoord coord)
        {
            currentCanvas = canvas;
            trgImage = trg;
            trgLayer = GetSelectedLayer();
            if (trgImage == null || trgLayer == null || !trgLayer.imageData.CanDraw) return false;

            if (!drawing)
            {
                beforeDataStride = (int)trgLayer.imageData.actualSize.Width * 4;
                RecordBeforeData();
            }

            penUp = false;
            drawing = true;
            count = 0;

            for (int i = 0; i < histPressure.Length; i++)
            {
                histPressure[i] = currentCanvas.StylusPressure;
                if(WintabUtility.Pressure!=0) histPressure[i] = WintabUtility.Pressure;
            }

            foreach(IBCoord c in histCoord)
            {
                c.x = coord.x;
                c.y = coord.y;
            }

            return true;
        }

        public virtual void Draw(IBCoord coord)
        {
            if (!drawing) return;

            count = 0;

            double x = coord.x, y = coord.y;

            if (AllowCorrection)
            {
                for (int i = 0; i < 2; i++)
                {
                    x += histCoord[i].x * (0.75 - i * 0.5);
                    y += histCoord[i].y * (0.75 - i * 0.5);
                }

                curCoord.x = x / 2.0;
                curCoord.y = y / 2.0;
            }
            else
            {
                curCoord.x = x;
                curCoord.y = y;
            }

            for (int i = histCoord.Length - 1; i > 0; i--)
            {
                histCoord[i].x = histCoord[i - 1].x;
                histCoord[i].y = histCoord[i - 1].y;
            }

            for (int i = histPressure.Length - 1; i > 0; i--)
            {
                histPressure[i] = histPressure[i - 1];
            }

            histCoord[0].x = curCoord.x;
            histCoord[0].y = curCoord.y;
            histPressure[0] = currentCanvas.StylusPressure;
            if (WintabUtility.Pressure != 0) histPressure[0] = WintabUtility.Pressure;
            curPressure = histPressure[0];
        }

        public virtual void End()
        {
            if (drawing)
            {
                drawing = false;

                if (drawAreaYS == 3000 || drawAreaXS == 5000) return;
                RUDraw action = new RUDraw(beforeData, trgLayer);
                action.Summary = actionSummary;
                RedoUndoManager.Current.Record(action);
            }
        }

        public virtual void Deacive()
        {

        }

        public void EndRequest()
        {
            penUp = true;
        }

        protected IBImage GetSelectedLayer()
        {
            if (trgImage == null) return null;

            switch (trgImage.Type)
            {
                case IBProjectElementTypes.CellSource:
                    foreach (IBImage i in ((CellSource)trgImage).Layers)
                    {
                        if (i.IsSelectedLayer) return i;
                    }
                    break;

                default:
                    break;
            }

            return null;
        }

        protected void EntryTexUpdate(BGRA32FormattedImage image)
        {
            waitingImage = image;
        }

        protected static void ResetDrawArea()
        {
            drawAreaXS = 5000;
            drawAreaXE = 0;
            drawAreaYS = 3000;
            drawAreaYE = 0;
        }

        protected static void RecordDrawArea(int xs, int ys,int xe, int ye)
        {
            if (xs < drawAreaXS) drawAreaXS = xs;
            if (ys < drawAreaYS) drawAreaYS = ys;
            if (xe >= drawAreaXE) drawAreaXE = xe + 1;
            if (ye >= drawAreaYE) drawAreaYE = ye + 1;
        }

        protected virtual void RecordBeforeData()
        {
            byte[] original = trgLayer.imageData.data;

            Parallel.For(0, (int)trgLayer.imageData.actualSize.Height, y =>
            {
                int offset = y * beforeDataStride;
                for(int x = 0; x < beforeDataStride; x++)
                {
                    int index = offset + x;
                    beforeData[index] = original[index];
                }
            });
        }

        public class RUDraw : RedoUndoAction, IDisposable
        {
            public RUDraw(byte[] _beforData, IBImage layer)
            {
                trg = layer;
                byte[] _newData = layer.imageData.data;
                stride = (drawAreaXE - drawAreaXS) * 4;
                height = drawAreaYE - drawAreaYS;
                offsetX = drawAreaXS;
                offsetY = drawAreaYS;

                BeforeData = new byte[height * stride];
                NewData = new byte[height * stride];
                int layerStride = (int)layer.imageData.actualSize.Width * 4;
                int _offsetx = offsetX * 4;

                for (int y = 0; y < height; y++)
                {
                    int offset = (drawAreaYS + y) * layerStride;
                    for (int x= 0; x < stride; x++)
                    {
                        BeforeData[y * stride + x] = _beforData[offset + _offsetx + x];
                        NewData[y * stride + x] = _newData[offset + _offsetx + x];
                    }
                }
            }

            byte[] BeforeData;
            byte[] NewData;
            int stride;
            int height;
            int offsetX, offsetY;
            IBImage trg;

            public override void Redo()
            {
                base.Redo();

                bool wasCanDraw = trg.imageData.CanDraw;

                if (!wasCanDraw)
                    trg.imageData.SetDrawingMode();

                byte[] trgData = trg.imageData.data;
                int layerStride = (int)trg.imageData.actualSize.Width * 4;
                int _offsetx = offsetX * 4;
                for (int y = 0; y < height; y++)
                {
                    int offset = (offsetY + y) * layerStride;
                    for (int x = 0; x < stride; x++)
                    {
                        trgData[offset + _offsetx + x] = NewData[y * stride + x];
                    }
                }

                if (!wasCanDraw)
                    trg.imageData.EndDrawingMode();

                trg.imageData.TextureUpdate();
                IBCanvasControl.RefreshAll();
            }

            public override void Undo()
            {
                base.Undo();

                bool wasCanDraw = trg.imageData.CanDraw;

                if (!wasCanDraw)
                    trg.imageData.SetDrawingMode();

                byte[] trgData = trg.imageData.data;
                int layerStride = (int)trg.imageData.actualSize.Width * 4;
                int _offsetx = offsetX * 4;
                for (int y = 0; y < height; y++)
                {
                    int offset = (offsetY + y) * layerStride;
                    for (int x = 0; x < stride; x++)
                    {
                        trgData[offset + _offsetx + x] = BeforeData[y * stride + x];
                    }
                }

                if (!wasCanDraw)
                    trg.imageData.EndDrawingMode();

                trg.imageData.TextureUpdate();
                IBCanvasControl.RefreshAll();
            }

            public void Dispose()
            {
                BeforeData = null;
                NewData = null;
            }
        }
    }
}
