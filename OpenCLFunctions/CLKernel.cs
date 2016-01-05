using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using OpenCLFunctions.Wrappers;

namespace OpenCLFunctions
{
    public class CLKernel
    {
        public CLKernel(CLProgram program, string functionName)
        {
            int errorCode;
            InternalPointer = CLfunc.clCreateKernel(
                program.InternalPointer,
                functionName,
                out errorCode
                );
        }

        ~CLKernel()
        {
            CLfunc.clReleaseKernel(InternalPointer);
        }


        public IntPtr InternalPointer { get; private set; }


        public void SetArgument(int argumentIndex, CLBuffer buffer)
        {
            IntPtr bufferPointer = buffer.InternalPointer;
            CLfunc.clSetKernelArg(InternalPointer, argumentIndex, Marshal.SizeOf(typeof(IntPtr)), ref bufferPointer);
        }

        public void SetArgument<T>(int argumentIndex, T value) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);

            CLfunc.clSetKernelArg(InternalPointer, argumentIndex, Marshal.SizeOf(typeof(T)), handle.AddrOfPinnedObject());

            handle.Free();
        }
    }
}
