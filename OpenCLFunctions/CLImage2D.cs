using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using OpenCLFunctions.Wrappers;

namespace OpenCLFunctions
{
    public class CLImage2D
    {
        public CLImage2D(CLContext context, MemoryFlags flags, CLImageFormat format, int W, int H, byte[] data)
        {
            int errorCode;
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            InternalPointer = CLfunc.clCreateImage2D(context.InternalPointer, flags, ref format, W, H, 0, handle.AddrOfPinnedObject(), out errorCode);

            handle.Free();
            if(errorCode != 0)
            {
                throw new Exception("OpenCL 2D Image オブジェクトの生成に失敗しまた。\nエラーコード：" + errorCode.ToString() + "\n");
            }
        }

        ~CLImage2D()
        {
            CLfunc.clReleaseMemObject(InternalPointer);
        }

        public IntPtr InternalPointer { get; private set; }
    }
}
