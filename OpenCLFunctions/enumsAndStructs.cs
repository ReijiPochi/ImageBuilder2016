using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLFunctions
{
    /// <summary>
    /// デバイスの種類
    /// </summary>
    public enum DeviceType : long
    {
        /// <summary>
        /// デフォルトのデバイス
        /// </summary>
        Default = (1 << 0),

        /// <summary>
        /// CPU
        /// </summary>
        Cpu = (1 << 1),

        /// <summary>
        /// GPU
        /// </summary>
        Gpu = (1 << 2),

        /// <summary>
        /// IBM CELL など
        /// </summary>
        Accelerator = (1 << 3),

        /// <summary>
        /// 使用可能な全てのOpenCLデバイス
        /// </summary>
        All = 0xFFFFFFFF
    }

    /// <summary>
    /// バッファの性質
    /// </summary>
    public enum MemoryFlags : long
    {
        /// <summary>
        /// GPUによってR/Wされる
        /// </summary>
        ReadWrite = (1 << 0),

        /// <summary>
        /// GPUによってWのみされる
        /// </summary>
        WriteOnly = (1 << 1),

        /// <summary>
        /// GPUによってRのみされる
        /// </summary>
        ReadOnly = (1 << 2),

        /// <summary>
        /// clCreateBuffer()のhostPtrがnullでないときに限り有効
        /// </summary>
        UseHostMemory = (1 << 3),

        /// <summary>
        /// CPUからアクセス可能なメモリ
        /// </summary>
        HostAccessible = (1 << 4),

        /// <summary>
        /// clCreateBuffer()のhostPtrがnullでないときに限り有効
        /// （hostPtrに書かれたデータをコピーしてメモリがGPUに確保される）
        /// </summary>
        CopyHostMemory = (1 << 5)
    }

    /// <summary>
    /// 取得したいコンパイルエラーの情報
    /// </summary>
    public enum ProgramBuildInfoString
    {
        /// <summary>
        /// clBuildProgram()のoptions引数に渡されたオプション
        /// </summary>
        Options = 0x1182,

        /// <summary>
        /// ビルドログ
        /// </summary>
        Log = 0x1183
    }

    public enum DeviceInfo : uint
    {
        DeviceType = 0x1000,
        DeviceVendorID = 0x1001,
        DeviceMaxComputeUnits = 0x1002,
        DeviceMaxWorkItemDimensions = 0x1003,
        DeviceMaxWorkGroupSize = 0x1004,
        DeviceMaxWorkItemSizes = 0x1005,
        DevicePreferredVectorWidthChar = 0x1006,
        DevicePreferredVectorWidthShort = 0x1007,
        DevicePreferredVectorWidthInt = 0x1008,
        DevicePreferredVectorWidthLong = 0x1009,
        DevicePreferredVectorWidthFloat = 0x100A,
        DevicePreferredVectorWidthDouble = 0x100B,
        DeviceMaxClockFrequency = 0x100C,
        DeviceAddressBits = 0x100D,
        DeviceMaxReadImageArgs = 0x100E,
        DeviceMaxWriteImageArgs = 0x100F,
        DeviceMaxMemAllocSize = 0x1010,
        DeviceImage2DMaxWidth = 0x1011,
        DeviceImage2DMaxHeight = 0x1012,
        DeviceImage3DMaxWidth = 0x1013,
        DeviceImage3DMaxHeight = 0x1014,
        DeviceImage3DMaxDepth = 0x1015,
        DeviceImageSupport = 0x1016,
        DeviceMaxParameterSize = 0x1017,
        DeviceMaxSamplers = 0x1018,
        DeviceMemBaseAddrAlign = 0x1019,
        DeviceMinDataTypeAlignSize = 0x101A,
        DeviceSingleFPConfig = 0x101B,
        DeviceGlobalMemCacheType = 0x101C,
        DeviceGlobalMemCachelineSize = 0x101D,
        DeviceGlobalMemCacheSize = 0x101E,
        DeviceGlobalMemSize = 0x101F,
        DeviceMaxConstantBufferSize = 0x1020,
        DeviceMaxConstantArgs = 0x1021,
        DeviceLocalMemType = 0x1022,
        DeviceLocalMemSize = 0x1023,
        DeviceErrorCorrectionSupport = 0x1024,
        DeviceProfilingTimerResolution = 0x1025,
        DeviceEndianLittle = 0x1026,
        DeviceAvailable = 0x1027,
        DeviceCompilerAvailable = 0x1028,
        DeviceExecutionCapabilities = 0x1029,
        DeviceQueueProperties = 0x102A,
        DeviceName = 0x102B,
        DeviceVendor = 0x102C,
        DriverVersion = 0x102D,
        DeviceProfile = 0x102E,
        DeviceVersion = 0x102F,
        DeviceExtensions = 0x1030,
    }

    public enum ChannelOrder : uint
    {
        R = 0x10B0,
        A = 0x10B1,
        RG = 0x10B2,
        RA = 0x10B3,
        Rgb = 0x10B4,
        Rgba = 0x10B5,
        Bgra = 0x10B6,
        Argb = 0x10B7,
    }

    public enum ChannelType : uint
    {
        SNormInt8 = 0x10D0,
        SNormInt16 = 0x10D1,
        UNormInt8 = 0x10D2,
        UNormInt16 = 0x10D3,
        UNormShort565 = 0x10D4,
        UNormShort555 = 0x10D5,
        UNormInt101010 = 0x10D6,
        SignedInt8 = 0x10D7,
        SignedInt16 = 0x10D8,
        SignedInt32 = 0x10D9,
        UnsignedInt8 = 0x10DA,
        UnsignedInt16 = 0x10DB,
        UnsignedInt32 = 0x10DC,
        HalfFloat = 0x10DD,
        Float = 0x10DE,
    }

    public struct CLImageFormat
    {
        public ChannelOrder ImageChannelOrder;
        public ChannelType ImageChannelDataType;

        public CLImageFormat(ChannelOrder order, ChannelType type)
        {
            ImageChannelOrder = order;
            ImageChannelDataType = type;
        }
    }

    public struct MultiDimension
    {
        public int X;
        public int Y;
        public int Z;
        public int Dimension;

        public MultiDimension(int x)
        {
            X = x;
            Y = 0;
            Z = 0;
            Dimension = 1;
        }

        public MultiDimension(int x, int y)
        {
            X = x;
            Y = y;
            Z = 0;
            Dimension = 2;
        }

        public MultiDimension(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            Dimension = 3;
        }
    }

    public struct Coordinate2D
    {
        public int X;
        public int Y;
        public int Z;

        public Coordinate2D(int value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Coordinate2D(int x, int y)
        {
            X = x;
            Y = y;
            Z = 1;
        }
    }

    public struct CLColor
    {
        public int R;
        public int G;
        public int B;
        public int A;

        public CLColor(int r, int g, int b, int a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
