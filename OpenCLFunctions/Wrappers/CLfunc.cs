using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace OpenCLFunctions.Wrappers
{
    public class CLfunc
    {
        /// <summary>
        /// インストールされたプラットフォームを取得します
        /// </summary>
        /// <param name="entryCount">platforms配列の長さ</param>
        /// <param name="platforms">取得したプラットフォームを格納する配列（nullなら無視）</param>
        /// <param name="platformCount">インストールされたプラットフォームの数（nullなら無視）</param>
        /// <returns>0：関数が成功</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clGetPlatformIDs(
            int entryCount,
            IntPtr[] platforms,
            out int platformCount);

        /// <summary>
        /// プラットフォームからデバイスを取得します
        /// </summary>
        /// <param name="platform">取得するデバイスのプラットフォーム</param>
        /// <param name="deviceType">取得するデバイスの種類</param>
        /// <param name="entryCount">devices配列の長さ（devicesがnullの時のみ、0でもおｋ）</param>
        /// <param name="devices">取得したデバイスを格納する配列（nullなら無視）</param>
        /// <param name="deviceCount"></param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clGetDeviceIDs(
            IntPtr platform,
            DeviceType deviceType,
            int entryCount,
            IntPtr[] devices,
            out int deviceCount);

        /// <summary>
        /// Int型のデバイスの情報を取得します
        /// </summary>
        /// <param name="device"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValueSize"></param>
        /// <param name="paramValue">情報（nullなら無視）</param>
        /// <param name="paramValueSizeReturn">実際の情報のサイズ（nullなら無視）</param>
        /// <returns></returns>
        [DllImport("OpenCL.dll")]
        public static extern int clGetDeviceInfo(
            IntPtr device,
            DeviceInfo paramName,
            int paramValueSize,
            int[] paramValue,
            out int paramValueSizeReturn);

        /// <summary>
        /// デバイスからコンテキストを生成します
        /// </summary>
        /// <param name="properties">コンテキストを生成するときの情報（nullでもおｋ）</param>
        /// <param name="deviceCount">devices配列の中に入っているデバイスの数</param>
        /// <param name="devices">コンテキストを生成するのに使うデバイス</param>
        /// <param name="pfnNotify">エラー情報を受け取るコールバック</param>
        /// <param name="userData">コールバックの引数として渡されるデータ（nullでもおｋ）</param>
        /// <param name="errorCode">エラーコード（nullでもおｋ）</param>
        /// <returns>生成したコンテキスト</returns>
        [DllImport("OpenCL.dll")]
        public static extern IntPtr clCreateContext(
            IntPtr[] properties,
            int deviceCount,
            IntPtr[] devices,
            NotifyCallback pfnNotify,
            IntPtr userData,
            out int errorCode);

        /// <summary>
        /// コンテキストを破棄します
        /// </summary>
        /// <param name="context">破棄するコンテキスト</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clReleaseContext(
            IntPtr context);

        /// <summary>
        /// コンテキストとデバイスから、コマンドキューを生成します
        /// </summary>
        /// <param name="context">コマンドキューに結び付けられるコンテキスト</param>
        /// <param name="device">コマンドキューに結び付けられるデバイス</param>
        /// <param name="properties">生成するコマンドキューの属性のリスト</param>
        /// <param name="errorCodeReturn">エラーコード（nullでもおｋ）</param>
        /// <returns>生成されたコマンドキュー</returns>
        [DllImport("OpenCL.dll")]
        public static extern IntPtr clCreateCommandQueue(
            IntPtr context,
            IntPtr device,
            long properties,
            out int errorCodeReturn);

        /// <summary>
        /// コマンドキューを破棄します
        /// </summary>
        /// <param name="commandQueue">破棄するコマンドキュー</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clReleaseCommandQueue(
            IntPtr commandQueue);

        /// <summary>
        /// デバイス（GPU）側にメモリ領域を確保し、バッファを生成します
        /// </summary>
        /// <param name="context">バッファに関連付けるコンテキスト</param>
        /// <param name="allocationAndUsage">生成するバッファの性質</param>
        /// <param name="sizeInBytes"></param>
        /// <param name="hostPtr"></param>
        /// <param name="errorCodeReturn"></param>
        /// <returns>生成したバッファ</returns>
        [DllImport("OpenCL.dll")]
        public static extern IntPtr clCreateBuffer(
            IntPtr context,
            MemoryFlags allocationAndUsage,
            int sizeInBytes,
            IntPtr hostPtr,
            out int errorCodeReturn);

        /// <summary>
        /// 2D Imageオブジェクトを生成します
        /// </summary>
        /// <param name="context"></param>
        /// <param name="allocationAndUsage"></param>
        /// <param name="imageFormat"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="imageRowPitch"></param>
        /// <param name="hostPtr"></param>
        /// <param name="errorCodeReturn"></param>
        /// <returns></returns>
        [DllImport("OpenCL.dll")]
        public static extern IntPtr clCreateImage2D(
            IntPtr context,
            MemoryFlags allocationAndUsage,
            ref CLImageFormat imageFormat,
            int imageWidth,
            int imageHeight,
            int imageRowPitch,
            IntPtr hostPtr,
            out int errorCodeReturn);

        /// <summary>
        /// バッファを破棄します
        /// </summary>
        /// <param name="memoryObject">破棄するメモリオブジェクト</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clReleaseMemObject(
            IntPtr memoryObject);

        /// <summary>
        /// コマンドキューに「バッファからの読み出し命令」を追加します
        /// </summary>
        /// <param name="commandQueue">読み出しを行うコマンドキュー</param>
        /// <param name="buffer">読み出すバッファ</param>
        /// <param name="isBlocking">falseなら、この関数は非同期に実行される</param>
        /// <param name="offset">バッファから読み出しを始めるオフセット（バイト単位）</param>
        /// <param name="sizeInBytes">読み込むサイズ（バイト単位）</param>
        /// <param name="result">読み出し先のメモリ</param>
        /// <param name="numberOfEventsInWaitList">この命令を実行する前に実行しないといけない命令の数</param>
        /// <param name="eventWaitList">この命令を実行する前に実行しないといけない命令（nullでもおｋ）</param>
        /// <param name="eventObjectOut">この命令のイベント</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clEnqueueReadBuffer(
            IntPtr commandQueue,
            IntPtr buffer,
            bool isBlocking,
            int offset,
            int sizeInBytes,
            IntPtr result,
            int numberOfEventsInWaitList,
            IntPtr eventWaitList,
            IntPtr eventObjectOut);

        /// <summary>
        /// OpenCL C言語のソースコードから、プログラムオブジェクトを作成します
        /// </summary>
        /// <param name="context">生成したプログラムを使うコンテキスト</param>
        /// <param name="count">programSources配列の長さ</param>
        /// <param name="programSources">プログラムのソースコード（複数可）</param>
        /// <param name="sourceLengths">ソースコードの文字列の長さ（nullなら、「null」で終わる文字列だと解釈される）</param>
        /// <param name="errorCode">エラーコード（nullでもおｋ）</param>
        /// <returns>生成されたプログラムオブジェクト</returns>
        [DllImport("OpenCL.dll")]
        public static extern IntPtr clCreateProgramWithSource(
            IntPtr context,
            int count,
            string[] programSources,
            int[] sourceLengths,
            out int errorCode);

        /// <summary>
        /// プログラムをコンパイルします
        /// </summary>
        /// <param name="program">ビルドするプログラム</param>
        /// <param name="deviceCount">deviceListに入っているデバイスの数</param>
        /// <param name="deviceList">プログラムと関連付けるデバイス（nullの場合、既に関連付けられている全てのデバイスに対してビルド）</param>
        /// <param name="buildOptions">ビルドオプション</param>
        /// <param name="notify">非同期にこの関数を呼び出す場合のコールバック</param>
        /// <param name="userData">コールバックの引数として渡されるデータ（nullでもおｋ）</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clBuildProgram(
            IntPtr program,
            int deviceCount,
            IntPtr[] deviceList,
            string buildOptions,
            NotifyProgramBuilt notify,
            IntPtr userData);

        /// <summary>
        /// プログラムオブジェクトを破棄します
        /// </summary>
        /// <param name="program">破棄するプログラム</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clReleaseProgram(
            IntPtr program);

        /// <summary>
        /// カーネルを生成します
        /// </summary>
        /// <param name="program">カーネルを持つプログラム</param>
        /// <param name="functionName">カーネル関数の名前</param>
        /// <param name="errorCode">エラーコード（nullでもおｋ）</param>
        /// <returns>生成されたカーネル</returns>
        [DllImport("OpenCL.dll")]
        public static extern IntPtr clCreateKernel(
            IntPtr program,
            string functionName,
            out int errorCode);

        /// <summary>
        /// カーネルを破棄します
        /// </summary>
        /// <param name="kernel">破棄するカーネル</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clReleaseKernel(
            IntPtr kernel);

        /// <summary>
        /// カーネルに引数をセットします
        /// </summary>
        /// <param name="kernel">引数をセットするカーネル</param>
        /// <param name="argumentIndex">引数の位置</param>
        /// <param name="size">valueに渡すメモリのサイズ</param>
        /// <param name="value">セットする引数</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clSetKernelArg(
            IntPtr kernel,
            int argumentIndex,
            int size,
            ref IntPtr value);

        /// <summary>
        /// カーネルに引数をセットします
        /// </summary>
        /// <param name="kernel">引数をセットするカーネル</param>
        /// <param name="argumentIndex">引数の位置</param>
        /// <param name="size">valueに渡すメモリのサイズ</param>
        /// <param name="value">セットするint型の定数</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clSetKernelArg(
            IntPtr kernel,
            int argumentIndex,
            int size,
            IntPtr value);

        /// <summary>
        /// コマンドキューに「カーネルの実行命令」を追加します
        /// </summary>
        /// <param name="commandQueue">カーネルを実行するコマンドキュー</param>
        /// <param name="kernel">実行するカーネル</param>
        /// <param name="countOfEventsInWaitList">この命令を実行する前に実行しないといけない命令の数</param>
        /// <param name="eventWaitList">この命令を実行する前に実行しないといけない命令（nullでもおｋ）</param>
        /// <param name="eventObject">この実行が終わったことを教えるイベントオブジェクト</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clEnqueueTask(
            IntPtr commandQueue,
            IntPtr kernel,
            int countOfEventsInWaitList,
            IntPtr[] eventWaitList,
            IntPtr eventObject);

        /// <summary>
        /// コマンドキューに「カーネルの並列実行命令」を追加します
        /// </summary>
        /// <param name="commandQueue">カーネルを実行するコマンドキュー</param>
        /// <param name="kernel">実行するカーネル</param>
        /// <param name="workDimension">実行するスレッド群の次元数</param>
        /// <param name="globalWorkOffset">グローバルIDのオフセット（nullでもおｋ）</param>
        /// <param name="globalWorkSize">全スレッドの量</param>
        /// <param name="localWorkSize">1グループのスレッドの量</param>
        /// <param name="countOfEventsInWaitList">この命令を実行する前に実行しないといけない命令の数</param>
        /// <param name="eventList">この実行が終わったことを教えるイベントオブジェクト</param>
        /// <param name="eventObject">この実行が終わったことを教えるイベントオブジェクト（nullでもおｋ）</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clEnqueueNDRangeKernel(
            IntPtr commandQueue,
            IntPtr kernel,
            int workDimension,
            ref MultiDimension globalWorkOffset,
            ref MultiDimension globalWorkSize,
            ref MultiDimension localWorkSize,
            int countOfEventsInWaitList,
            IntPtr[] eventList,
            IntPtr eventObject);

        /// <summary>
        /// コマンドキューに「画像塗りつぶし命令」を追加します
        /// </summary>
        /// <param name="commandQueue"></param>
        /// <param name="image"></param>
        /// <param name="fillColor"></param>
        /// <param name="origin"></param>
        /// <param name="region"></param>
        /// <param name="countOfEventsInWaitList"></param>
        /// <param name="eventList"></param>
        /// <param name="eventObject"></param>
        /// <returns></returns>
        [DllImport("OpenCL.dll")]
        public static extern int clEnqueueFillImage(
            IntPtr commandQueue,
            IntPtr image,
            IntPtr fillColor,
            ref Coordinate2D origin,
            ref Coordinate2D region,
            int countOfEventsInWaitList,
            IntPtr[] eventList,
            IntPtr eventObject);

        /// <summary>
        /// コマンドキューに「画像データ書き込み命令」を追加します
        /// </summary>
        /// <param name="commandQueue"></param>
        /// <param name="image"></param>
        /// <param name="blockingWrite"></param>
        /// <param name="origin"></param>
        /// <param name="region"></param>
        /// <param name="inputRowPitch"></param>
        /// <param name="inputSlicePitch"></param>
        /// <param name="ptr"></param>
        /// <param name="countOfEventsInWaitList"></param>
        /// <param name="eventList"></param>
        /// <param name="eventObject"></param>
        /// <returns></returns>
        [DllImport("OpenCL.dll")]
        public static extern int clEnqueueWriteImage(
            IntPtr commandQueue,
            IntPtr image,
            bool blockingWrite,
            ref Coordinate2D origin,
            ref Coordinate2D region,
            int inputRowPitch,
            int inputSlicePitch,
            IntPtr ptr,
            int countOfEventsInWaitList,
            IntPtr[] eventList,
            IntPtr eventObject);

        /// <summary>
        /// コマンドキューに「画像データ読み出し命令」を追加します
        /// </summary>
        /// <param name="commandQueue"></param>
        /// <param name="image"></param>
        /// <param name="blockingWrite"></param>
        /// <param name="origin"></param>
        /// <param name="region"></param>
        /// <param name="rowPitch"></param>
        /// <param name="slicePitch"></param>
        /// <param name="ptr"></param>
        /// <param name="countOfEventsInWaitList"></param>
        /// <param name="eventList"></param>
        /// <param name="eventObject"></param>
        /// <returns></returns>
        [DllImport("OpenCL.dll")]
        public static extern int clEnqueueReadImage(
            IntPtr commandQueue,
            IntPtr image,
            bool blockingWrite,
            ref Coordinate2D origin,
            ref Coordinate2D region,
            int rowPitch,
            int slicePitch,
            IntPtr ptr,
            int countOfEventsInWaitList,
            IntPtr[] eventList,
            IntPtr eventObject);

        /// <summary>
        /// コンパイルエラーを取得します
        /// </summary>
        /// <param name="program">ビルドしたプログラム</param>
        /// <param name="device">ビルド情報を得るデバイス</param>
        /// <param name="paramName">取得したい情報の種類</param>
        /// <param name="paramValueSize">ビルド情報を格納するメモリのサイズ</param>
        /// <param name="paramValue">ビルド情報を格納するメモリ</param>
        /// <param name="paramValueSizeReturn">取得したいビルド情報のサイズを取得（nullでもおｋ）</param>
        /// <returns>エラーの場合、エラーコードが返される</returns>
        [DllImport("OpenCL.dll")]
        public static extern int clGetProgramBuildInfo(
            IntPtr program,
            IntPtr device,
            ProgramBuildInfoString paramName,
            int paramValueSize,
            StringBuilder paramValue,
            out int paramValueSizeReturn);
    }
}
