using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using OpenCLFunctions.Wrappers;

namespace OpenCLFunctions
{
    public class CLCommandQueue
    {
        public CLCommandQueue(CLContext context, IntPtr device)
        {
            int error;
            InternalPointer = CLfunc.clCreateCommandQueue(context.InternalPointer, device, 0, out error);
        }

        ~CLCommandQueue()
        {
            CLfunc.clReleaseCommandQueue(InternalPointer);
        }

        public IntPtr InternalPointer { get; private set; }

        /// <summary>
        /// バッファからデータを読み出します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="systemBuffer"></param>
        public void ReadBuffer<T>(CLBuffer buffer, T[] systemBuffer) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(systemBuffer, GCHandleType.Pinned);

            CLfunc.clEnqueueReadBuffer(
                InternalPointer,
                buffer.InternalPointer,
                true,
                0,
                Math.Min(buffer.SizeInBytes, Marshal.SizeOf(typeof(T)) * systemBuffer.Length),
                handle.AddrOfPinnedObject(),
                0,
                IntPtr.Zero,
                IntPtr.Zero);

            handle.Free();
        }

        /// <summary>
        /// カーネルを実行します
        /// </summary>
        /// <param name="kernel"></param>
        public void EnqueueTask(CLKernel kernel)
        {
            CLfunc.clEnqueueTask(InternalPointer, kernel.InternalPointer, 0, null, IntPtr.Zero);
        }

        /// <summary>
        /// カーネルを並列実行します
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="globalWorkSize"></param>
        /// <param name="localWorkSize"></param>
        public void EnqueueRange(CLKernel kernel, MultiDimension globalWorkSize, MultiDimension localWorkSize)
        {
            MultiDimension offset = new MultiDimension();
            CLfunc.clEnqueueNDRangeKernel(
                InternalPointer,
                kernel.InternalPointer,
                globalWorkSize.Dimension,
                ref offset,
                ref globalWorkSize,
                ref localWorkSize,
                0,
                null,
                IntPtr.Zero);
        }

        public void EnqueueFillColor(CLImage2D image, CLColor fillColor)
        {
            int errorCode;
            Coordinate2D origin = new Coordinate2D(0);
            GCHandle handle = GCHandle.Alloc(fillColor, GCHandleType.Pinned);

            errorCode = CLfunc.clEnqueueFillImage(InternalPointer, image.InternalPointer, handle.AddrOfPinnedObject(),
                ref origin, ref image._Size, 0, null, IntPtr.Zero);

            handle.Free();
            if (errorCode != 0)
            {
                throw new Exception("OpenCL 2D Image オブジェクトの塗りつぶしに失敗しまた。\nエラーコード：" + errorCode.ToString() + "\n");
            }
        }

        public void EnqueueWriteImageData(CLImage2D image, byte[] data)
        {
            int errorCode;
            Coordinate2D origin = new Coordinate2D(0);
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            errorCode = CLfunc.clEnqueueWriteImage(InternalPointer, image.InternalPointer, true,
                ref origin, ref image._Size, image._Size.X * 4, 0, handle.AddrOfPinnedObject(), 0, null, IntPtr.Zero);

            handle.Free();
            if (errorCode != 0)
            {
                throw new Exception("OpenCL 2D Image オブジェクトへのデータ書き込みに失敗しまた。\nエラーコード：" + errorCode.ToString() + "\n");
            }
        }

        public void EnqueueReadImageData(CLImage2D image, byte[] data)
        {
            int errorCode;
            Coordinate2D origin = new Coordinate2D(0);
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            errorCode = CLfunc.clEnqueueReadImage(InternalPointer, image.InternalPointer, true,
                ref origin, ref image._Size, image._Size.X * 4, 0, handle.AddrOfPinnedObject(), 0, null, IntPtr.Zero);

            handle.Free();
            if (errorCode != 0)
            {
                throw new Exception("OpenCL 2D Image オブジェクトからのデータ読み出しに失敗しまた。\nエラーコード：" + errorCode.ToString() + "\n");
            }
        }
    }
}
