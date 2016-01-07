using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.IO;
using System.Reflection;

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
                string source;
                Assembly a = Assembly.GetExecutingAssembly();
                using (StreamReader sr = new StreamReader(a.GetManifestResourceStream("IBFramework.OpenCL.Sources.cl")))
                {
                    source = sr.ReadToEnd();
                }

                CL.Initialize(source);

                int maxW, maxH;
                CL.GetDeviceImage2DMaxSize(out maxW, out maxH);
                MaxImage2DWidth = maxW;
                MaxImage2DHeight = maxH;

                //CLImage2D image = CL.GenImage2D(
                //    MemoryFlags.ReadWrite,
                //    1920,
                //    1080,
                //    null);

                //CL.WriteImage2DData(image, new byte[1920 * 1080 * 4]);

                //CL.FillImage2D(image, new CLColor(114, 51, 4, 255));

                //byte[] ret = new byte[8 * 8 * 4];
                //CL.ReadImage2DData(image, ret);
            }
        }
    }
}
