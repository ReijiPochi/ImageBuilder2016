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
    /// WTInfoで取得するデータの種別
    /// </summary>
    public enum EWTICategoryIndex
    {
        WTI_INTERFACE = 1,
        WTI_STATUS = 2,
        WTI_DEFCONTEXT = 3,
        WTI_DEFSYSCTX = 4,
        WTI_DEVICES = 100,
        WTI_CURSORS = 200,
        WTI_EXTENSIONS = 300,
        WTI_DDCTXS = 400,
        WTI_DSCTXS = 500
    }

    /// <summary>
    /// WTI_DEVICESのインデックス
    /// </summary>
    public enum EWTIDevicesIndex
    {
        DVC_NAME = 1,
        DVC_HARDWARE = 2,
        DVC_NCSRTYPES = 3,
        DVC_FIRSTCSR = 4,
        DVC_PKTRATE = 5,
        DVC_PKTDATA = 6,
        DVC_PKTMODE = 7,
        DVC_CSRDATA = 8,
        DVC_XMARGIN = 9,
        DVC_YMARGIN = 10,
        DVC_ZMARGIN = 11,
        DVC_X = 12,
        DVC_Y = 13,
        DVC_Z = 14,
        DVC_NPRESSURE = 15,
        DVC_TPRESSURE = 16,
        DVC_ORIENTATION = 17,
        DVC_ROTATION = 18,
        DVC_PNPID = 19
    }

    /// <summary>
    /// パケットから取得するデータを定義するビット
    /// </summary>
    public enum EWintabPacketBit
    {
        PK_CONTEXT = 0x0001,   /* reporting context */
        PK_STATUS = 0x0002,   /* status bits */
        PK_TIME = 0x0004,   /* time stamp */
        PK_CHANGED = 0x0008,   /* change bit vector */
        PK_SERIAL_NUMBER = 0x0010,   /* packet serial number */
        PK_CURSOR = 0x0020,   /* reporting cursor */
        PK_BUTTONS = 0x0040,   /* button information */
        PK_X = 0x0080,   /* x axis */
        PK_Y = 0x0100,   /* y axis */
        PK_Z = 0x0200,   /* z axis */
        PK_NORMAL_PRESSURE = 0x0400,	/* normal or tip pressure */
        PK_TANGENT_PRESSURE = 0x0800,	/* tangential or barrel pressure */
        PK_ORIENTATION = 0x1000,   /* orientation info: tilts */
        PK_ROTATION = 0x2000,   /* rotation info */
        PK_PKTBITS_ALL = 0x3FFF    // The Full Monty - all the bits
    }

    /// <summary>
    /// コンテキストオプション
    /// </summary>
    public enum ECTXOptionValues
    {
        CXO_NONE = 0x0000,
        CXO_SYSTEM = 0x0001,
        CXO_PEN = 0x0002,
        CXO_MESSAGES = 0x0004,
        CXO_CSRMESSAGES = 0x0008,
        CXO_MGNINSIDE = 0x4000,
        CXO_MARGIN = 0x8000,
    }

    /// <summary>
    /// タブレット座標
    /// DVC_X, DVC_Y, DVC_Zのみ指定するよう定義
    /// </summary>
    public enum EAxisDimension
    {
        AXIS_X = EWTIDevicesIndex.DVC_X,
        AXIS_Y = EWTIDevicesIndex.DVC_Y,
        AXIS_Z = EWTIDevicesIndex.DVC_Z
    }

    /// <summary>
    /// Wintab用メッセージ
    /// </summary>
    public enum EWintabEventMessage
    {
        WT_PACKET = 0x7FF0,
        WT_CTXOPEN = 0x7FF1,
        WT_CTXCLOSE = 0x7FF2,
        WT_CTXUPDATE = 0x7FF3,
        WT_CTXOVERLAP = 0x7FF4,
        WT_PROXIMITY = 0x7FF5,
        WT_INFOCHANGE = 0x7FF6,
        WT_CSRCHANGE = 0x7FF7,
        WT_PACKETEXT = 0x7FF8
    }


    /// <summary>
    /// AXISデータ構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WintabAxis
    {
        public Int32 axMin;
        public Int32 axMax;
        public UInt32 axUnits;
        public UInt32 axResolution;
    }

    /// <summary>
    /// コンテキストデータ構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WintabLogContext
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string lcName;
        public UInt32 lcOptions;
        public UInt32 lcStatus;
        public UInt32 lcLocks;
        public UInt32 lcMsgBase;
        public UInt32 lcDevice;
        public UInt32 lcPktRate;
        public UInt32 lcPktData;
        public UInt32 lcPktMode;
        public UInt32 lcMoveMask;
        public UInt32 lcBtnDnMask;
        public UInt32 lcBtnUpMask;
        public Int32 lcInOrgX;
        public Int32 lcInOrgY;
        public Int32 lcInOrgZ;
        public Int32 lcInExtX;
        public Int32 lcInExtY;
        public Int32 lcInExtZ;
        public Int32 lcOutOrgX;
        public Int32 lcOutOrgY;
        public Int32 lcOutOrgZ;
        public Int32 lcOutExtX;
        public Int32 lcOutExtY;
        public Int32 lcOutExtZ;
        public UInt32 lcSensX;
        public UInt32 lcSensY;
        public UInt32 lcSensZ;
        public bool lcSysMode;
        public Int32 lcSysOrgX;
        public Int32 lcSysOrgY;
        public Int32 lcSysExtX;
        public Int32 lcSysExtY;
        public UInt32 lcSysSensX;
        public UInt32 lcSysSensY;
    }

    /// <summary>
    /// 筆圧の構造体
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct WintabPressure
    {
        [FieldOffset(0)]
        public Int32 pkRelativePressure;

        [FieldOffset(0)]
        public UInt32 pkAbsolutePressure;
    }

    /// <summary>
    /// 向き情報の構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WintabOrientation
    {
        public Int32 orAzimuth;
        public Int32 orAltitude;
        public Int32 orTwist;
    }

    /// <summary>
    /// 回転情報の構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WintabRotation
    {
        public Int32 rotPitch;
        public Int32 rotRoll;
        public Int32 rotYaw;
    }

    /// <summary>
    /// パケット構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WintabPacket
    {
        public UInt32 pkContext;                    // PK_CONTEXT
        public UInt32 pkStatus;                     // PK_STATUS
        public UInt32 pkTime;                       // PK_TIME
        public UInt32 pkChanged;                    // PK_CHANGED
        public UInt32 pkSerialNumber;               // PK_SERIAL_NUMBER
        public UInt32 pkCursor;                     // PK_CURSOR
        public UInt32 pkButtons;                    // PK_BUTTONS
        public Int32 pkX;                           // PK_X
        public Int32 pkY;                           // PK_Y
        public Int32 pkZ;                           // PK_Z
        public WintabPressure pkNormalPressure;     // PK_NORMAL_PRESSURE
        public WintabPressure pkTangentPressure;    // PK_TANGENT_PRESSURE
        public WintabOrientation pkOrientation;     // ORIENTATION
        public WintabRotation pkRotation;           // ROTATION
    }
}
