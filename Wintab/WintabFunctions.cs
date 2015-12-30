using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Wintab
{
/* 参考元：http://blog.wdnet.jp/
* 　　　：http://blog.wdnet.jp/tech/archives/156
* 　　　：http://blog.wdnet.jp/tech/archives/162
* 　　　：http://blog.wdnet.jp/tech/archives/168
*/

    /// <summary>
    /// Wintabの関数を定義するクラス
    /// </summary>
    class WintabFunctions
    {

        /// <summary>
        /// タブレット情報の取得
        /// </summary>
        /// <param name="wCategory"></param>
        /// <param name="nIndex"></param>
        /// <param name="lpOutput"></param>
        /// <returns></returns>
        [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
        public static extern UInt32 WTInfoA(UInt32 wCategory, UInt32 nIndex, IntPtr lpOutput);

        /// <summary>
        /// タブレットのオープン
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="logContext"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr WTOpenA(IntPtr hWnd, ref WintabLogContext logContext, bool enable);

        /// <summary>
        /// タブレットのクローズ
        /// </summary>
        /// <param name="hctx"></param>
        /// <returns></returns>
        [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
        public static extern bool WTClose(IntPtr hctx);

        /// <summary>
        /// パケットの取得
        /// </summary>
        /// <param name="hctx"></param>
        /// <param name="pktSerialNum"></param>
        /// <param name="pktBuf"></param>
        /// <returns></returns>
        [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
        public static extern bool WTPacket(IntPtr hctx, UInt32 pktSerialNum, IntPtr pktBuf);
    }
}
