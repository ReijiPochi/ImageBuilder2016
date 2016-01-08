using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

using OpenCLFunctions;
using OpenCLFunctions.Utilities;
using IBFramework.Image;

namespace IBFramework.OpenCL
{
    /// <summary>
    /// 先にIBFramework.OpenCL.CLUtilities.Init()をコールしといてください
    /// </summary>
    public class CLImageProcessing
    {
        private static CLKernel test;
        private static CLKernel fillRect;

        public static void Test()
        {
            if (test == null)
                test = CL.GetKernel("myKernelFunction");

            const int BUFFERSIZE = 10;
            CLBuffer buf = CL.GenBuffer(new float[BUFFERSIZE]);

            test.SetArgument(0, buf);

            CL.EnqueueRange(test, new MultiDimension(BUFFERSIZE), new MultiDimension(1));

            float[] readBack = new float[BUFFERSIZE];
            CL.ReadBuffer(buf, readBack);

            foreach (var v in readBack)
                Console.WriteLine(v);
        }

        /// <summary>
        /// CLImage2D を指定された色で、全て塗りつぶします
        /// </summary>
        /// <param name="clImage"></param>
        /// <param name="color"></param>
        public static void ClearColor(CLImage2D clImage, CLColor color)
        {
            CL.FillImage2D(clImage, color);
        }

        public static void FillRectangle(CLBuffer trg, IBRectangle trgSize, PixelData color, IBRectangle rect)
        {
            if (fillRect == null)
                fillRect = CL.GetKernel("fillRect");

            int[] _trgSize = new int[] { (int)trgSize.Width, (int)trgSize.Height };
            int[] _offset = new int[] { (int)rect.OffsetX, (int)rect.OffsetY };
            int[] _size = new int[] { (int)rect.Width, (int)rect.Height };
            float[] _color = new float[] { color.b / 255.0f, color.g / 255.0f, color.r / 255.0f, color.a / 255.0f };
            CLBuffer __trgSize = CL.GenBuffer(_trgSize);
            CLBuffer __offset = CL.GenBuffer(_offset);
            CLBuffer __size = CL.GenBuffer(_size);
            CLBuffer __color = CL.GenBuffer(_color);

            fillRect.SetArgument(0, trg.InternalPointer);
            fillRect.SetArgument(1, __trgSize);
            fillRect.SetArgument(2, __color);
            fillRect.SetArgument(3, __offset);
            fillRect.SetArgument(4, __size);

            CL.EnqueueRange(fillRect, new MultiDimension(1080), new MultiDimension(1));
        }
    }
}
