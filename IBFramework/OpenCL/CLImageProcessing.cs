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
        /// CLImage2D を指定された色で塗りつぶします
        /// </summary>
        /// <param name="clImage"></param>
        /// <param name="color"></param>
        public static void ClearColor(CLImage2D clImage,CLColor color)
        {
            CL.FillImage2D(clImage, color);
        }
    }
}
