using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

using OpenCLFunctions;
using OpenCLFunctions.Utilities;

namespace IBFramework.OpenCL
{
    public class CLImageProcessing
    {
        private static CLKernel test;

        public static void Test()
        {
            CLUtilities.Init();
            if (test == null)
                test = CL.GetKernel("myKernelFunction");

            const int BUFFERSIZE = 3;
            CLBuffer buf = CL.GenBuffer(new float[BUFFERSIZE]);

            test.SetArgument(0, buf);

            CL.EnqueueRange(test, new MultiDimension(BUFFERSIZE), new MultiDimension(1));

            float[] readBack = new float[BUFFERSIZE];
            CL.ReadBuffer(buf, readBack);

            foreach (var v in readBack)
                Console.WriteLine(v);
        }
    }
}
