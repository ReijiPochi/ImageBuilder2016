using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace OpenCLFunctions
{
    public class CL
    {
        [DllImport("OpenCL.dll")]
        public static extern int clGetPlatformIDs(int entryCount, IntPtr[] platforms, out int platformCount);
    }
}
