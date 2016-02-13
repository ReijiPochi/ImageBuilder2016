using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;

namespace IBFramework
{
    public class IBCursor
    {
        public static Cursor BitmapImageToCursor(BitmapSource bmpImg, int hotSpotX, int hotSpotY)
        {
            byte[] pngData;
            using (MemoryStream pngStream = new MemoryStream())
            {
                PngBitmapEncoder enc = new PngBitmapEncoder();
                BitmapFrame frame = BitmapFrame.Create(bmpImg);
                enc.Frames.Add(frame);
                enc.Save(pngStream);
                pngData = pngStream.ToArray();
            }

            using (MemoryStream curStream = new MemoryStream())
            {
                curStream.Write(BitConverter.GetBytes((Int16)0), 0, 2);
                curStream.Write(BitConverter.GetBytes((Int16)2), 0, 2);
                curStream.Write(BitConverter.GetBytes((Int16)1), 0, 2);

                curStream.WriteByte((byte)bmpImg.Width);
                curStream.WriteByte((byte)bmpImg.Height);
                curStream.WriteByte(0);
                curStream.WriteByte(0);
                curStream.Write(BitConverter.GetBytes((Int16)hotSpotX), 0, 2);
                curStream.Write(BitConverter.GetBytes((Int16)hotSpotY), 0, 2);
                curStream.Write(BitConverter.GetBytes(pngData.GetLength(0)), 0, 4);
                curStream.Write(BitConverter.GetBytes((Int32)22), 0, 4);

                curStream.Write(pngData, 0, pngData.GetLength(0));

                curStream.Seek(0, SeekOrigin.Begin);
                return new Cursor(curStream);
            }
        }

        public static Cursor GenCircleCursor(double r)
        {
            if (r < 2.0) r = 2.0;

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)r * 2 + 2, (int)r * 2 + 2, 96, 96, PixelFormats.Pbgra32);

            Grid visual = new Grid();
            visual.Width = bmp.PixelWidth;
            visual.Height = bmp.PixelHeight;
            System.Windows.Size renderSize = new System.Windows.Size(visual.Width, visual.Height);

            Border b1 = new Border();
            b1.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 255, 255, 255));
            b1.BorderThickness = new Thickness(1.0);
            b1.CornerRadius = new CornerRadius(r);
            b1.Width = r * 2.0;
            b1.Height = r * 2.0;
            b1.HorizontalAlignment = HorizontalAlignment.Center;
            b1.VerticalAlignment = VerticalAlignment.Center;

            Border b2 = new Border();
            b2.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 0, 0, 0));
            b2.BorderThickness = new Thickness(1.0);
            b2.CornerRadius = new CornerRadius(r - 1.0);
            b2.Width = r * 2.0 - 2.0;
            b2.Height = r * 2.0 - 2.0;
            b2.HorizontalAlignment = HorizontalAlignment.Center;
            b2.VerticalAlignment = VerticalAlignment.Center;


            visual.Children.Add(b1);
            visual.Children.Add(b2);
            visual.Measure(renderSize);
            visual.Arrange(new Rect(renderSize));

            bmp.Render(visual);


            return BitmapImageToCursor(bmp, (int)r + 1, (int)r + 1);
        }
    }
}
