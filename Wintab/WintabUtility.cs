using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

using System.Windows.Interop;
using System.Windows;

namespace Wintab
{
    public class WintabUtility
    {
        private static DispatcherTimer watchDogTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 1000) };

        /// <summary>
        /// ペンタブレットを初期化します
        /// </summary>
        public static void Initialize()
        {
            if (WintabManager.IsWintabAvailable() == false || Application.Current.MainWindow == null)
                return;

            try
            {
                WintabLogContext context = WintabManager.GetDefaultSystemContext(ECTXOptionValues.CXO_SYSTEM);
                context.lcName = "Wintab sample";

                // ウインドウハンドルの取得
                WindowInteropHelper helper = new WindowInteropHelper(Application.Current.MainWindow);
                IntPtr hWnd = helper.Handle;

                // Wintabハンドル
                IntPtr m_hCtx = IntPtr.Zero;

                // タブレットの受信開始
                m_hCtx = WintabManager.Open(hWnd, context);
                if (m_hCtx != IntPtr.Zero)
                {
                    // ウインドウプロシージャをフックする
                    HwndSource source = HwndSource.FromHwnd(helper.Handle);
                    source.AddHook(new HwndSourceHook(WndProc));
                }

                maxPressure = WintabManager.GetDeviceNPressure().axMax;
                maxZ = WintabManager.GetTabletAxis(EAxisDimension.AXIS_Z).axMax;
                Enable = WintabManager.IsWintabAvailable();

                watchDogTimer.Tick += WatchDogTimer_Tick;
                watchDogTimer.Start();
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void WatchDogTimer_Tick(object sender, EventArgs e)
        {
            if (packet.pkZ < maxZ) return;
            usingPen = false;
        }

        private static WintabPacket packet = new WintabPacket();
        private static double maxPressure;
        private static int maxZ;
        private static bool ButtonPressed = false;
        private static bool Enable = false;
        private static bool usingPen;

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)EWintabEventMessage.WT_PACKET)
            {
                packet = WintabManager.GetPacket(lParam, (uint)wParam.ToInt32());
                usingPen = true;

                uint button = packet.pkButtons & 0x00000001;
                if(button == 1)
                {
                    uint highWord = (packet.pkButtons & 0xFFFF0000) >> 16;

                    if ((highWord&2) != 0)
                    {
                        ButtonPressed = true;
                    }
                    else if ((highWord & 1) != 0)
                    {
                        ButtonPressed = false;
                    }
                }
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// ペンが操作されているかどうか
        /// </summary>
        public static bool PenUsing
        {
            get
            {
                if (!Enable) return false;

                return usingPen;
            }
        }

        /// <summary>
        /// 現在のペンの位置
        /// </summary>
        public static Point Position
        {
            get
            {
                if (!Enable) return new Point(0, 0);

                return new Point(packet.pkX, packet.pkY);
            }
        }

        /// <summary>
        /// 現在のペンの筆圧
        /// </summary>
        public static double Pressure
        {
            get
            {
                if (!Enable || maxPressure <= 0) return 0;

                return packet.pkNormalPressure.pkAbsolutePressure / maxPressure;
            }
        }

        /// <summary>
        /// ペンのボタンが押され、さらにペンが降ろされているか
        /// </summary>
        public static bool PenButtonPressed
        {
            get
            {
                if (!Enable) return false;

                if (ButtonPressed && packet.pkZ == 0) return true;
                else return false;
            }
        }
    }
}
