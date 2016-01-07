using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using OpenCLFunctions.Wrappers;

namespace OpenCLFunctions
{
    /// <summary>
    /// BGRA, 32bit の画像オブジェクト
    /// </summary>
    public class CLImage2D
    {
        public CLImage2D(CLContext context, MemoryFlags flags, int W, int H, byte[] data)
        {
            int errorCode;
            CLImageFormat format = new CLImageFormat(ChannelOrder.Bgra, ChannelType.UnsignedInt8);
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            InternalPointer = CLfunc.clCreateImage2D(context.InternalPointer, flags, ref format, W, H, 0, handle.AddrOfPinnedObject(), out errorCode);

            handle.Free();
            Size = new Coordinate2D(W, H);
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

        public Coordinate2D _Size;
        public Coordinate2D Size
        {
            get { return _Size; }
            private set { _Size = value; }
        }
    }
}
