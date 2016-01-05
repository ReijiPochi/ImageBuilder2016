using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCLFunctions.Wrappers;

namespace OpenCLFunctions
{
    /// <summary>
    /// 一つ以上のデバイスをまとめるオブジェクト
    /// </summary>
    public class CLContext
    {
        public CLContext(IntPtr[] devices)
        {
            int error;
            InternalPointer = CLfunc.clCreateContext(null, devices.Length, devices, null, IntPtr.Zero, out error);
        }

        ~CLContext()
        {
            CLfunc.clReleaseContext(InternalPointer);
        }

        public IntPtr InternalPointer { get; private set; }
    }
}
