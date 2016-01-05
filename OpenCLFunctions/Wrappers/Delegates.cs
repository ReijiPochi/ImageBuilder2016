using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLFunctions.Wrappers
{
    /// <summary>
    /// エラー情報を受け取るコールバック
    /// </summary>
    /// <param name="errorInfo">エラー文字列へのポインタ</param>
    /// <param name="privateInfoSize">デバッグ時に役に立つ情報</param>
    /// <param name="cb">デバッグ時に役に立つ情報</param>
    /// <param name="userData">ユーザー定義の情報（clCreateContext()の引数userDataの中身と同じ）</param>
    public delegate void NotifyCallback(string errorInfo, IntPtr privateInfoSize, int cb, IntPtr userData);

    /// <summary>
    /// プログラムのビルドが終了したときのコールバック
    /// </summary>
    /// <param name="program">ビルドしたプログラム</param>
    /// <param name="userData">ユーザー定義の情報（clBuildProgram()の引数userDataの中身と同じ）</param>
    public delegate void NotifyProgramBuilt(IntPtr program, IntPtr userData);
}
