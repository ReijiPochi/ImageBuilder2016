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
    /// アンマネージドメモリを管理するクラス
    /// </summary>
    class WintabMemoryUtility
    {
        /// <summary>
        /// アンマネージドメモリの確保
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IntPtr AllocUnmanagedBuffer(Object obj)
        {
            IntPtr buf = IntPtr.Zero;

            try
            {
                int sz = Marshal.SizeOf(obj);
                buf = Marshal.AllocHGlobal(sz);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AllocUnmanagedBuffer : " + ex.Message);
            }

            return buf;
        }

        public static IntPtr AllocUnmanagedBuffer(Type t, int count)
        {
            IntPtr buf = IntPtr.Zero;

            try
            {
                int sz = Marshal.SizeOf(t) * count;
                buf = Marshal.AllocHGlobal(sz);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AllocUnmanagedBuffer : " + ex.Message);
            }

            return buf;
        }

        public static IntPtr AllocUnmanagedBuffer(int sz)
        {
            IntPtr buf = IntPtr.Zero;

            try
            {
                buf = Marshal.AllocHGlobal(sz);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AllocUnmanagedBuffer : " + ex.Message);
            }

            return buf;
        }

        /// <summary>
        /// アンマネージドメモリの開放
        /// </summary>
        /// <param name="p"></param>
        public static void FreeUnmanagedBuffer(IntPtr p)
        {
            if (p != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(p);
                p = IntPtr.Zero;
            }
        }

        /// <summary>
        /// アンマネージドメモリを指定のオブジェクトに変換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buf"></param>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static T MarshalUnmanagedBuffer<T>(IntPtr buf, uint sz)
        {
            if (buf == IntPtr.Zero)
            {
                throw new Exception("MarshalUnmanagedBuffer : メモリが空です");
            }
            if (sz != Marshal.SizeOf(typeof(T)))
            {
                throw new Exception("MarshalUnmanagedBuffer : 構造体のサイズが違います");
            }

            return (T)Marshal.PtrToStructure(buf, typeof(T));
        }

        public static string MarshalUnmanagedString(IntPtr buf, int sz)
        {
            string result = null;

            if (buf == IntPtr.Zero)
            {
                throw new Exception("MarshalUnmanagedString : Buffer is null");
            }

            if (sz <= 0)
            {
                throw new Exception("MarshalUnmanagedString : Size is zero");
            }

            try
            {
                Byte[] byteArray = new Byte[sz];

                Marshal.Copy(buf, byteArray, 0, sz);

                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                result = encoding.GetString(byteArray);
            }
            catch (Exception ex)
            {
                throw new Exception("MarshalUnmanagedString : " + ex.Message);
            }

            return result;
        }

        public static T[] MarshalUnmanagedBuffer<T>(IntPtr buf, uint sz, uint count)
        {
            T[] result = new T[count];

            if (buf == IntPtr.Zero)
            {
                throw new Exception("MarshalUnmanagedBuffer : Buffer is null");
            }
            if (sz <= 0)
            {
                throw new Exception("MarshalUnmanagedBuffer : Size is zero");
            }
            if (count <= 0)
            {
                throw new Exception("MarshalUnmanagedBuffer : Count is zero");
            }

            try
            {
                IntPtr nowPtr = buf;
                for (int i = 0; i < count; i++)
                {
                    result[i] = (T)Marshal.PtrToStructure(nowPtr, typeof(T));
                    nowPtr = (IntPtr)((int)nowPtr + Marshal.SizeOf(typeof(T)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MarshalUnmanagedBuffer : " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 対象のTypeのサイズ取得
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int SizeOf(Type t)
        {
            return Marshal.SizeOf(t);
        }
    }
}
