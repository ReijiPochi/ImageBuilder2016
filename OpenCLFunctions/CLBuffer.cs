using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using OpenCLFunctions.Wrappers;

namespace OpenCLFunctions
{
    public class CLBuffer
    {
        private CLBuffer()
        {

        }

        ~CLBuffer()
        {
            CLfunc.clReleaseMemObject(InternalPointer);
        }

        public IntPtr InternalPointer { get; private set; }
        public int SizeInBytes { get; private set; }

        /// <summary>
        /// バッファにデータを書き込みます
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="initialData"></param>
        /// <returns></returns>
        public static CLBuffer FromCopiedHostMemory<T>(CLContext context, T[] initialData) where T : struct
        {
            CLBuffer result = new CLBuffer();
            result.SizeInBytes = Marshal.SizeOf(typeof(T)) * initialData.Length;

            int errorCode;
            GCHandle handle = GCHandle.Alloc(initialData, GCHandleType.Pinned);

            result.InternalPointer = CLfunc.clCreateBuffer(
                context.InternalPointer,
                MemoryFlags.CopyHostMemory,
                result.SizeInBytes,
                handle.AddrOfPinnedObject(),
                out errorCode);

            handle.Free();
            return result;
        }
    }
}
