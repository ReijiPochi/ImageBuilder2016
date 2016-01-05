using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

using OpenCLFunctions.Utilities;

using OpenCLFunctions;

namespace IBFramework.OpenCL
{
    public class CLUtilities
    {
        public static int MaxImage2DHeight { get; private set; }
        public static int MaxImage2DWidth { get; private set; }

        /// <summary>
        /// OpenCL Cプログラムを読み込んで初期化します。既に初期化されている場合は何もしません
        /// </summary>
        public static void Init()
        {
            if (CL.Program == null)
            {
                CL.Initialize(Application.Current.FindResource("TestProgram") as string);

                int maxW, maxH;
                CL.GetDeviceImage2DMaxSize(out maxW, out maxH);
                MaxImage2DWidth = maxW;
                MaxImage2DHeight = maxH;

                CLImage2D image = CL.GenImage2D(
                    MemoryFlags.CopyHostMemory,
                    new CLImageFormat(ChannelOrder.Bgra, ChannelType.UnsignedInt8),
                    8,
                    8,
                    new byte[8 * 8 * 4]);
            }
        }
    }
}
