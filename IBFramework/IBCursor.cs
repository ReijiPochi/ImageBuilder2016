using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Imaging;

namespace IBFramework
{
    public class IBCursor
    {
        public static Cursor BitmapImageToCursor(BitmapImage bmpImg, int hotSpotX, int hotSpotY)
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
    }
}
