using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using IBFramework.IBCanvas;
using IBFramework.Project;
using IBFramework.Project.IBProjectElements;
using IBFramework.RedoUndo;
using System.Windows.Media;
using System.Windows.Input;

namespace IBFramework.Image.Pixel
{
    public class Deformer : IBBrush
    {
        public override Control GetBP()
        {
            return null;
        }

        public Deformer()
        {
            move.PreviewMouseDown += MouseDown;
            move.DragDelta += Move_DragDelta;
            move.PreviewMouseUp += Move_MouseUp;
        }

        private bool isActive;
        RUDeformImage action;

        CellSource trgCell;
        private OverlayBorder selectingAreaRect = new OverlayBorder(0, 0, 0, 0);
        private OverlayThumb move = new OverlayThumb(0, 0, 0, 0);
        private OverlaySizeGrip topLeft = new OverlaySizeGrip(15, 15, 0, 0, 7);
        private OverlaySizeGrip topRight = new OverlaySizeGrip(15, 15, 0, 0, 7);
        private OverlaySizeGrip bottomLeft = new OverlaySizeGrip(15, 15, 0, 0, 7);
        private OverlaySizeGrip bottomRight = new OverlaySizeGrip(15, 15, 0, 0, 7);
        private OverlaySizeGrip left = new OverlaySizeGrip(13, 13, 0, 0, 0);
        private OverlaySizeGrip right = new OverlaySizeGrip(13, 13, 0, 0, 0);
        private OverlaySizeGrip top = new OverlaySizeGrip(13, 13, 0, 0, 0);
        private OverlaySizeGrip bottom = new OverlaySizeGrip(13, 13, 0, 0, 0);

        public override void Activate(IBCanvasControl canvas, IBProjectElement trg)
        {
            base.Activate(canvas, trg);

            trgCell = trgImage as CellSource;
            if (trgCell == null || currentCanvas == null || !trgCell.IsPixcelSelecting || trgCell.PixcelSelectedArea == null || !trgLayer.imageData.CanDraw)
            {
                Deacive();
                return;
            }

            action = new RUDeformImage(trgCell, trgLayer as PixcelImage);
            action.SetBefore(trgLayer as PixcelImage);

            trgCell.PixcelSelectedArea.imageData.EndDrawingMode();

            int xs = (int)trgCell.PixcelSelectedArea.imageData.drawingAreaSize.OffsetX;
            int ys = (int)trgCell.PixcelSelectedArea.imageData.drawingAreaSize.OffsetY;
            int w = (int)trgCell.PixcelSelectedArea.imageData.drawingAreaSize.Width;
            int h = (int)trgCell.PixcelSelectedArea.imageData.drawingAreaSize.Height;

            trgCell.TempLayer = new PixcelImage(w, h, xs + (int)trgLayer.Rect.OffsetX, ((int)trgLayer.Rect.Height - ys - h) + (int)trgLayer.Rect.OffsetY);
            trgCell.TempLayer.imageData.SetDrawingMode();

            for(int y = 0; y < h; y++)
            {
                int offset = y * w * 4;
                int sourceOffset = (ys + y) * (int)trgLayer.imageData.actualSize.Width * 4 + xs * 4;

                for(int x = 0; x < w * 4 - 3; x++)
                {
                    if(trgCell.PixcelSelectedArea.imageData.data[offset + x + 3] == 255)
                    {
                        trgCell.TempLayer.imageData.data[offset + x] = trgLayer.imageData.data[sourceOffset + x];
                        trgLayer.imageData.data[sourceOffset + x] = 0;
                        x++;
                        trgCell.TempLayer.imageData.data[offset + x] = trgLayer.imageData.data[sourceOffset + x];
                        trgLayer.imageData.data[sourceOffset + x] = 0;
                        x++;
                        trgCell.TempLayer.imageData.data[offset + x] = trgLayer.imageData.data[sourceOffset + x];
                        trgLayer.imageData.data[sourceOffset + x] = 0;
                        x++;
                        trgCell.TempLayer.imageData.data[offset + x] = trgLayer.imageData.data[sourceOffset + x];
                        trgLayer.imageData.data[sourceOffset + x] = 0;
                    }
                    else
                    {
                        x += 3;
                    }
                }
            }

            isActive = true;
            trgCell.TempLayer.imageData.TextureUpdate();
            trgLayer.imageData.TextureUpdate();

            selectingAreaRect.OverlayWidth = trgCell.PixcelSelectedArea.imageData.drawingAreaSize.Width;
            selectingAreaRect.OverlayHeight = trgCell.PixcelSelectedArea.imageData.drawingAreaSize.Height;
            selectingAreaRect.OverlayOffsetX = trgCell.PixcelSelectedArea.Rect.OffsetX + trgCell.PixcelSelectedArea.imageData.drawingAreaSize.OffsetX;
            selectingAreaRect.OverlayOffsetY = trgCell.PixcelSelectedArea.imageData.drawingAreaSize.OffsetY - trgCell.PixcelSelectedArea.Rect.OffsetY - trgCell.PixcelSelectedArea.Rect.Height;
            selectingAreaRect.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 255, 0, 0));
            selectingAreaRect.BorderThickness = new System.Windows.Thickness(1);
            currentCanvas.AddOverlayItem(selectingAreaRect);

            move.OverlayWidth = selectingAreaRect.OverlayWidth;
            move.OverlayHeight = selectingAreaRect.OverlayHeight;
            move.OverlayOffsetX = selectingAreaRect.OverlayOffsetX;
            move.OverlayOffsetY = selectingAreaRect.OverlayOffsetY;
            move.Cursor = Cursors.SizeAll;
            currentCanvas.AddOverlayItem(move);

            topLeft.OverlayOffsetX = selectingAreaRect.OverlayOffsetX;
            topLeft.OverlayOffsetY = selectingAreaRect.OverlayOffsetY;
            topLeft.Cursor = Cursors.SizeNWSE;
            currentCanvas.AddOverlayItem(topLeft);

            topRight.OverlayOffsetX = selectingAreaRect.OverlayOffsetX + selectingAreaRect.OverlayWidth;
            topRight.OverlayOffsetY = selectingAreaRect.OverlayOffsetY;
            topRight.Cursor = Cursors.SizeNESW;
            currentCanvas.AddOverlayItem(topRight);

            bottomLeft.OverlayOffsetX = selectingAreaRect.OverlayOffsetX;
            bottomLeft.OverlayOffsetY = selectingAreaRect.OverlayOffsetY + selectingAreaRect.OverlayHeight;
            bottomLeft.Cursor = Cursors.SizeNESW;
            currentCanvas.AddOverlayItem(bottomLeft);

            bottomRight.OverlayOffsetX = selectingAreaRect.OverlayOffsetX + selectingAreaRect.OverlayWidth;
            bottomRight.OverlayOffsetY = selectingAreaRect.OverlayOffsetY + selectingAreaRect.OverlayHeight;
            bottomRight.Cursor = Cursors.SizeNWSE;
            currentCanvas.AddOverlayItem(bottomRight);

            left.OverlayOffsetX = selectingAreaRect.OverlayOffsetX;
            left.OverlayOffsetY= selectingAreaRect.OverlayOffsetY + selectingAreaRect.OverlayHeight / 2.0;
            left.Cursor = Cursors.SizeWE;
            currentCanvas.AddOverlayItem(left);

            right.OverlayOffsetX = selectingAreaRect.OverlayOffsetX + selectingAreaRect.OverlayWidth;
            right.OverlayOffsetY = selectingAreaRect.OverlayOffsetY + selectingAreaRect.OverlayHeight / 2.0;
            right.Cursor = Cursors.SizeWE;
            currentCanvas.AddOverlayItem(right);

            top.OverlayOffsetX = selectingAreaRect.OverlayOffsetX + selectingAreaRect.OverlayWidth / 2.0;
            top.OverlayOffsetY = selectingAreaRect.OverlayOffsetY;
            top.Cursor = Cursors.SizeNS;
            currentCanvas.AddOverlayItem(top);

            bottom.OverlayOffsetX = selectingAreaRect.OverlayOffsetX + selectingAreaRect.OverlayWidth / 2.0;
            bottom.OverlayOffsetY = selectingAreaRect.OverlayOffsetY + selectingAreaRect.OverlayHeight;
            bottom.Cursor = Cursors.SizeNS;
            currentCanvas.AddOverlayItem(bottom);
        }

        public override bool Set(IBCanvasControl canvas, IBProjectElement trg, IBCoord coord)
        {
            if (base.Set(canvas, trg, coord)) return false;

            return true;
        }

        public override void Draw(IBCoord coord)
        {
            //base.Draw(coord);
        }

        public override void End()
        {
            //base.End();
        }

        public override void Deacive()
        {
            base.Deacive();

            if (currentCanvas == null) return;

            currentCanvas.RemoveOverlayItem(selectingAreaRect);
            currentCanvas.RemoveOverlayItem(move);
            currentCanvas.RemoveOverlayItem(topLeft);
            currentCanvas.RemoveOverlayItem(topRight);
            currentCanvas.RemoveOverlayItem(bottomLeft);
            currentCanvas.RemoveOverlayItem(bottomRight);
            currentCanvas.RemoveOverlayItem(left);
            currentCanvas.RemoveOverlayItem(right);
            currentCanvas.RemoveOverlayItem(top);
            currentCanvas.RemoveOverlayItem(bottom);

            if (!isActive)
            {
                isActive = false;
                return;
            }

            if (trgCell != null && trgCell.TempLayer != null)
            {
                int xs = (int)trgCell.TempLayer.Rect.OffsetX - (int)trgLayer.Rect.OffsetX;
                int ys = (int)trgLayer.imageData.actualSize.Height + (int)trgLayer.Rect.OffsetY - ((int)trgCell.TempLayer.Rect.OffsetY + (int)trgCell.TempLayer.Rect.Height);
                int w = (int)trgCell.TempLayer.imageData.actualSize.Width;
                int h = (int)trgCell.TempLayer.imageData.actualSize.Height;

                for (int y = 0; y < h; y++)
                {
                    int offset = y * w * 4;
                    int sourceOffset = (ys + y) * (int)trgLayer.imageData.actualSize.Width * 4 + xs * 4;

                    for (int x = 0; x < w * 4 - 3; x++)
                    {
                        if (sourceOffset + x < 0 || sourceOffset + x + 3 >= trgLayer.imageData.data.Length) break;

                        if (trgCell.TempLayer.imageData.data[offset + x + 3] != 0)
                        {
                            trgLayer.imageData.data[sourceOffset + x] = trgCell.TempLayer.imageData.data[offset + x];
                            x++;
                            trgLayer.imageData.data[sourceOffset + x] = trgCell.TempLayer.imageData.data[offset + x];
                            x++;
                            trgLayer.imageData.data[sourceOffset + x] = trgCell.TempLayer.imageData.data[offset + x];
                            int r = trgCell.TempLayer.imageData.data[offset + x];
                            x++;
                            trgLayer.imageData.data[sourceOffset + x] = trgLayer.imageData.data[sourceOffset + x] >= trgCell.TempLayer.imageData.data[offset + x] ?
                                                                            trgLayer.imageData.data[sourceOffset + x] : trgCell.TempLayer.imageData.data[offset + x];
                        }
                        else
                        {
                            x += 3;
                        }
                    }
                }

                trgLayer.imageData.TextureUpdate();
                action.SetAfter(trgLayer as PixcelImage);
                RedoUndoManager.Current.Record(action);

                trgCell.TempLayer.imageData.data = null;
                trgCell.TempLayer = null;

                trgCell.PixcelSelectedArea.imageData.data = null;
                trgCell.PixcelSelectedArea = null;
                trgCell.IsPixcelSelecting = false;

                IBCanvasControl.RefreshAll();
            }
        }



        private void Move_MouseUp(object sender, MouseButtonEventArgs e)
        {
            penUp = true;
            drawing = false;
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            penUp = false;
            drawing = true;
        }


        private void Move_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double deltaX = e.HorizontalChange / (currentCanvas.ZoomPerCent / 100.0);
            double deltaY = e.VerticalChange / (currentCanvas.ZoomPerCent / 100.0);

            trgCell.TempLayer.Rect.OffsetX += deltaX;
            trgCell.TempLayer.Rect.OffsetY -= deltaY;

            trgCell.PixcelSelectedArea.Rect.OffsetX += deltaX;
            trgCell.PixcelSelectedArea.Rect.OffsetY -= deltaY;

            AddOffset(move, deltaX, deltaY);
            AddOffset(selectingAreaRect, deltaX, deltaY);
            AddOffset(topLeft, deltaX, deltaY);
            AddOffset(topRight, deltaX, deltaY);
            AddOffset(bottomLeft, deltaX, deltaY);
            AddOffset(bottomRight, deltaX, deltaY);
            AddOffset(left, deltaX, deltaY);
            AddOffset(right, deltaX, deltaY);
            AddOffset(top, deltaX, deltaY);
            AddOffset(bottom, deltaX, deltaY);

            IBCanvasControl.RefreshAll();
        }

        private void AddOffset(IOverlayItems trg, double dx, double dy)
        {
            trg.OverlayOffsetX += dx;
            trg.OverlayOffsetY += dy;
        }

        private class RUDeformImage : RedoUndoAction
        {
            public RUDeformImage(CellSource _trgCell, PixcelImage trg)
            {
                if (trg == null) return;

                trgCell = _trgCell;
                trgImage = trg;
            }

            private CellSource trgCell;
            private IBImage trgImage;

            private PixcelImage before = new PixcelImage();
            private PixcelImage after = new PixcelImage();

            public void SetBefore(PixcelImage trg)
            {
                if (trg == null) return;

                before.imageData = new BGRA32FormattedImage((int)trg.imageData.actualSize.Width, (int)trg.imageData.actualSize.Height);
                before.imageData.SetDrawingMode();
                for (int i = 0; i < trgImage.imageData.data.Length; i++)
                {
                    before.imageData.data[i] = trg.imageData.data[i];
                }
            }

            public void SetAfter(PixcelImage trg)
            {
                if (trg == null) return;

                after.imageData = new BGRA32FormattedImage((int)trg.imageData.actualSize.Width, (int)trg.imageData.actualSize.Height);
                after.imageData.SetDrawingMode();
                for (int i = 0; i < trgImage.imageData.data.Length; i++)
                {
                    after.imageData.data[i] = trg.imageData.data[i];
                }
            }

            public override void Redo()
            {
                base.Redo();

                bool wasCanDraw = trgImage.imageData.CanDraw;

                if (!wasCanDraw)
                    trgImage.imageData.SetDrawingMode();

                for (int i = 0; i < trgImage.imageData.data.Length; i++)
                {
                    trgImage.imageData.data[i] = after.imageData.data[i];
                }

                trgImage.imageData.TextureUpdate();
                IBCanvasControl.RefreshAll();

                if (!wasCanDraw)
                    trgImage.imageData.EndDrawingMode();
            }

            public override void Undo()
            {
                base.Undo();

                bool wasCanDraw = trgImage.imageData.CanDraw;

                if (!wasCanDraw)
                    trgImage.imageData.SetDrawingMode();

                for (int i = 0; i < trgImage.imageData.data.Length; i++)
                {
                    trgImage.imageData.data[i] = before.imageData.data[i];
                }

                trgImage.imageData.TextureUpdate();
                IBCanvasControl.RefreshAll();

                if (!wasCanDraw)
                    trgImage.imageData.EndDrawingMode();
            }
        }
    }
}
