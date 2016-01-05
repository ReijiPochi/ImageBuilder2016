using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCLFunctions.Wrappers;

namespace OpenCLFunctions
{
    public class CLProgram
    {
        public CLProgram(CLContext context, params string[] sources)
        {
            int errorCode;

            InternalPointer = CLfunc.clCreateProgramWithSource(context.InternalPointer, sources.Length, sources, null, out errorCode);
        }

        ~CLProgram()
        {
            CLfunc.clReleaseProgram(InternalPointer);
        }


        public IntPtr InternalPointer { get; private set; }

        public void Build(params IntPtr[] devices)
        {
            int error = CLfunc.clBuildProgram(InternalPointer, devices.Length, devices, null, null, IntPtr.Zero);

            if (error != 0)
            {
                int paramValueSize = 0;
                CLfunc.clGetProgramBuildInfo(InternalPointer, devices.First(), ProgramBuildInfoString.Log, 0, null, out paramValueSize);
                StringBuilder text = new StringBuilder(paramValueSize);
                CLfunc.clGetProgramBuildInfo(InternalPointer, devices.First(), ProgramBuildInfoString.Log, paramValueSize, text, out paramValueSize);
                throw new Exception(text.ToString());
            }

        }
    }
}
