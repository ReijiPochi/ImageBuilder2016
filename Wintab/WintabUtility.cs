using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Interop;
using System.Windows;

namespace Wintab
{
    public class Wintab
    {
        /// <summary>
        /// ペンタブレットを初期化します
        /// </summary>
        public static void Initialize()
        {
            if (WintabManager.IsWintabAvailable() == false || Application.Current.MainWindow == null)
                return;

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
        }


        private static WintabPacket packet = new WintabPacket();

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)EWintabEventMessage.WT_PACKET)
            {
                packet = WintabManager.GetPacket(lParam, (uint)wParam.ToInt32());
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// ペンの位置
        /// </summary>
        public static Point Position
        {
            get
            {
                return new Point(packet.pkX, packet.pkY);
            }
        }


    }
}
