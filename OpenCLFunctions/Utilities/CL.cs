using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenCLFunctions.Wrappers;

namespace OpenCLFunctions.Utilities
{
    public partial class CL
    {
        public static IntPtr[] Platforms { get; private set; }
        public static IntPtr[] Devices { get; private set; }
        public static CLContext Context { get; private set; }
        public static CLCommandQueue CommandQueue { get; private set; }
        public static CLProgram Program { get; private set; }

        /// <summary>
        /// プログラムのコンパイルまでを行います
        /// </summary>
        /// <param name="programSource"></param>
        public static void Initialize(params string[] programSource)
        {
            IntPtr[] device = getDevices(getPlatforms()[0], DeviceType.Default);
            Context = new CLContext(device);

            CommandQueue = new CLCommandQueue(Context, device[0]);

            Program = new CLProgram(Context, programSource);
            Program.Build(Devices);
        }

        /// <summary>
        /// 現在のデバイスで扱える2次元画像の大きさを取得します
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="result"></param>
        public static void GetDeviceImage2DMaxSize(out int W, out int H)
        {
            int valueSize;

            CLfunc.clGetDeviceInfo(Devices[0], DeviceInfo.DeviceImage2DMaxWidth, 0, null, out valueSize);
            int[] resultWidth = new int[1];
            CLfunc.clGetDeviceInfo(Devices[0], DeviceInfo.DeviceImage2DMaxWidth, valueSize, resultWidth, out valueSize);
            W = resultWidth[0];

            CLfunc.clGetDeviceInfo(Devices[0], DeviceInfo.DeviceImage2DMaxHeight, 0, null, out valueSize);
            int[] resultHeight = new int[1];
            CLfunc.clGetDeviceInfo(Devices[0], DeviceInfo.DeviceImage2DMaxHeight, valueSize, resultHeight, out valueSize);
            H = resultHeight[0];
        }

        /// <summary>
        /// 現在のプログラムからカーネルを生成します
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static CLKernel GetKernel(string functionName)
        {
            if (Program == null) throw new Exception("OpenCLのプログラムソースが読み込まれていません");

            return new CLKernel(Program, functionName);
        }

        /// <summary>
        /// 現在のコンテキストからバッファを作成します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CLBuffer GenBuffer<T>(T[] data)where T : struct
        {
            return CLBuffer.FromCopiedHostMemory(Context, data);
        }

        /// <summary>
        /// 2D Imageオブジェクトを生成します
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="format"></param>
        /// <param name="W"></param>
        /// <param name="H"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CLImage2D GenImage2D(MemoryFlags flags, int W, int H, byte[] data)
        {
            return new CLImage2D(Context, flags, W, H, data);
        }

        /// <summary>
        /// 2D Imageオブジェクトを塗りつぶします
        /// </summary>
        /// <param name="image"></param>
        /// <param name="fillColor"></param>
        /// <param name="origin"></param>
        /// <param name="region"></param>
        public static void FillImage2D(CLImage2D image, CLColor fillColor)
        {
            CommandQueue.EnqueueFillColor(image, fillColor);
        }

        /// <summary>
        /// 2D Imageオブジェクトにデータを書き込みます
        /// </summary>
        /// <param name="image"></param>
        /// <param name="data"></param>
        public static void WriteImage2DData(CLImage2D image, byte[] data)
        {
            CommandQueue.EnqueueWriteImageData(image, data);
        }

        /// <summary>
        /// 2D Imageオブジェクトからデータを読み出します
        /// </summary>
        /// <param name="image"></param>
        /// <param name="data"></param>
        public static void ReadImage2DData(CLImage2D image, byte[] data)
        {
            CommandQueue.EnqueueReadImageData(image, data);
        }

        /// <summary>
        /// 現在のコマンドキューを使ってカーネルを並列実行します
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="globalWorkSize"></param>
        /// <param name="localWorkSize"></param>
        public static void EnqueueRange(CLKernel kernel, MultiDimension globalWorkSize, MultiDimension localWorkSize)
        {
            CommandQueue.EnqueueRange(kernel, globalWorkSize, localWorkSize);
        }

        /// <summary>
        /// 現在のコマンドキューを使ってカーネルを実行します
        /// </summary>
        /// <param name="kernel"></param>
        public static void EnqueueTask(CLKernel kernel)
        {
            CommandQueue.EnqueueTask(kernel);
        }

        /// <summary>
        /// 現在のコマンドキューを使ってバッファを読み出します
        /// </summary>
        public static void ReadBuffer<T>(CLBuffer buffer, T[] systemBuffer) where T : struct
        {
            CommandQueue.ReadBuffer(buffer, systemBuffer);
        }

        private static IntPtr[] getDevices(IntPtr platform, DeviceType deviceType)
        {
            // デバイスの数を取得
            int deviceCount;
            CLfunc.clGetDeviceIDs(platform, deviceType, 0, null, out deviceCount);

            // デバイスを取得
            IntPtr[] result = new IntPtr[deviceCount];
            CLfunc.clGetDeviceIDs(platform, deviceType, deviceCount, result, out deviceCount);

            Devices = result;
            return result;
        }

        private static IntPtr[] getPlatforms()
        {
            // プラットフォームの数を取得
            int platformCount;
            CLfunc.clGetPlatformIDs(0, null, out platformCount);

            // プラットフォームを取得
            IntPtr[] result = new IntPtr[platformCount];
            CLfunc.clGetPlatformIDs(platformCount, result, out platformCount);

            Platforms = result;
            return result;
        }
    }
}
