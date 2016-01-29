using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintab
{
    /* 参考元：http://blog.wdnet.jp/
    * 　　　：http://blog.wdnet.jp/tech/archives/156
    * 　　　：http://blog.wdnet.jp/tech/archives/162
    * 　　　：http://blog.wdnet.jp/tech/archives/168
    */

    public class WintabManager
    {
        private const int MAX_STRING_SIZE = 256;

        /// <summary>
        ///  Wintabの動作状態を取得
        /// </summary>
        /// <returns> true: Wintabが動作している // false: Wintabが動作していない</returns>
        public static bool IsWintabAvailable()
        {
            try
            {
                uint result = WintabFunctions.WTInfoA(0, 0, IntPtr.Zero);

                if (result > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("WintabManager IsWintabAvailable : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 端末名の取得
        /// </summary>
        /// <returns></returns>
        public static String GetDeviceName()
        {
            string devName = null;
            IntPtr buf = WintabMemoryUtility.AllocUnmanagedBuffer(MAX_STRING_SIZE);

            try
            {
                uint sz = WintabFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)EWTIDevicesIndex.DVC_NAME,
                    buf);

                if (sz == 0)
                {
                    throw new Exception("GetDeviceName : Name is empty");
                }

                devName = WintabMemoryUtility.MarshalUnmanagedString(buf, (int)sz - 1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetDeviceName : " + ex.Message);
                throw ex;
            }
            finally
            {
                //メモリの開放
                WintabMemoryUtility.FreeUnmanagedBuffer(buf);
            }

            return devName;
        }

        /// <summary>
        /// 筆圧の最大最小値取得
        /// </summary>
        /// <returns></returns>
        public static WintabAxis GetDeviceNPressure()
        {
            WintabAxis axis = new WintabAxis();
            IntPtr pAxis = WintabMemoryUtility.AllocUnmanagedBuffer(axis);

            try
            {
                uint sz = WintabFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)EWTIDevicesIndex.DVC_NPRESSURE,
                    pAxis);

                if (sz != 0)
                    axis = WintabMemoryUtility.MarshalUnmanagedBuffer<WintabAxis>(pAxis, sz);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetNPressure : " + ex.Message);
                throw ex;
            }
            finally
            {
                //メモリの開放
                WintabMemoryUtility.FreeUnmanagedBuffer(pAxis);
            }

            return axis;
        }

        /// <summary>
        /// Azimuth,Atitude,Twistの範囲取得
        /// </summary>
        /// <returns></returns>
        public static WintabAxis[] GetDeviceOrientation()
        {
            WintabAxis[] axis = new WintabAxis[3];
            IntPtr pAxis = WintabMemoryUtility.AllocUnmanagedBuffer(typeof(WintabAxis), 3);

            try
            {
                uint sz = WintabFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)EWTIDevicesIndex.DVC_ORIENTATION,
                    pAxis);

                if (sz != 0)
                    axis = WintabMemoryUtility.MarshalUnmanagedBuffer<WintabAxis>(pAxis, sz, 3);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetNPressure : " + ex.Message);
                throw ex;
            }
            finally
            {
                //メモリの開放
                WintabMemoryUtility.FreeUnmanagedBuffer(pAxis);
            }

            return axis;
        }

        /// <summary>
        /// デフォルトのコンテキスト情報を取得する
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static WintabLogContext GetDefaultSystemContext(ECTXOptionValues options)
        {
            // パケットのサイズは、指定したビットによって可変となる。
            // 全てのビットを指定し、パケットのサイズを固定にする。
            uint pktData = (uint)EWintabPacketBit.PK_PKTBITS_ALL;
            uint pktMode = (uint)EWintabPacketBit.PK_BUTTONS;

            // デフォルトのコンテキストの取得
            WintabLogContext context = new WintabLogContext();

            // メモリの確保
            IntPtr pContext = WintabMemoryUtility.AllocUnmanagedBuffer(context);

            try
            {
                uint sz = WintabFunctions.WTInfoA((uint)EWTICategoryIndex.WTI_DEFSYSCTX, 0, pContext);
                context = WintabMemoryUtility.MarshalUnmanagedBuffer<WintabLogContext>(pContext, sz);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetDefaultSystemContext : " + ex.Message);
                throw ex;
            }
            finally
            {
                // メモリの開放を忘れないこと
                WintabMemoryUtility.FreeUnmanagedBuffer(pContext);
            }

            // パケットメッセージを受信するために指定
            context.lcOptions |= (uint)ECTXOptionValues.CXO_MESSAGES;
            // 指定されたオプション
            context.lcOptions |= (uint)options;

            // コンテキストのビット設定
            context.lcPktData = pktData;
            context.lcPktMode = pktMode;
            context.lcMoveMask = pktData;
            context.lcBtnUpMask = context.lcBtnDnMask;

            return context;
        }

        /// <summary>
        /// タブレットのXYZ座標の範囲取得
        /// </summary>
        /// <param name="dimention"></param>
        /// <returns></returns>
        public static WintabAxis GetTabletAxis(EAxisDimension dimention)
        {
            WintabAxis axis = new WintabAxis();
            IntPtr pAxis = WintabMemoryUtility.AllocUnmanagedBuffer(axis);

            try
            {
                uint sz = WintabFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)dimention,
                    pAxis);

                if (sz != 0)
                    axis = WintabMemoryUtility.MarshalUnmanagedBuffer<WintabAxis>(pAxis, sz);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetTabletAxis : " + ex.Message);
                throw ex;
            }
            finally
            {
                WintabMemoryUtility.FreeUnmanagedBuffer(pAxis);
            }

            return axis;
        }

        /// <summary>
        /// タブレットのオープン
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IntPtr Open(IntPtr hWnd, WintabLogContext context)
        {
            IntPtr hCtx = IntPtr.Zero;

            try
            {
                hCtx = WintabFunctions.WTOpenA(hWnd, ref context, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Open : " + ex.Message);
            }

            return hCtx;
        }

        /// <summary>
        /// タブレットのクローズ
        /// </summary>
        /// <param name="hCtx"></param>
        public static void Close(IntPtr hCtx)
        {
            if (hCtx != IntPtr.Zero)
            {
                WintabFunctions.WTClose(hCtx);
                hCtx = IntPtr.Zero;
            }
        }

        /// <summary>
        /// パケットの取得
        /// </summary>
        /// <param name="hCtx"></param>
        /// <param name="pktID"></param>
        /// <returns></returns>
        public static WintabPacket GetPacket(IntPtr hCtx, UInt32 pktID)
        {
            WintabPacket packet = new WintabPacket();
            IntPtr pPacket = WintabMemoryUtility.AllocUnmanagedBuffer(packet);

            try
            {
                bool result = WintabFunctions.WTPacket(hCtx, pktID, pPacket);
                packet = WintabMemoryUtility.MarshalUnmanagedBuffer<WintabPacket>(
                    pPacket,
                    (uint)WintabMemoryUtility.SizeOf(typeof(WintabPacket)));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetPacket : " + ex.Message);
                throw ex;
            }
            finally
            {
                WintabMemoryUtility.FreeUnmanagedBuffer(pPacket);
            }

            return packet;
        }
    }
}
