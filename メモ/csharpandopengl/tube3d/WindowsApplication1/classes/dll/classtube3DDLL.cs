using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace tube3d.classes
{
    public class classtube3DDLL : IDisposable
    {
		#region DLL-API定義
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_Create();						//インスタンス生成
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_Destroy();						//インスタンス破棄
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_Open();						//Open
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_Close();						//Close
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_DrawData();
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_SetMousePosUp(int x, int y);
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_SetMousePosDown(int x, int y);
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_SetMousePosMove(int x, int y);
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_set_WindowHandle(IntPtr h);
		#endregion 

		private bool				m_Disposed = false;

        public IntPtr WindowHandle { set { TUBE3D_set_WindowHandle(value);}}

		//---------------------------------------------
		//コンストラクタ
		//
		//---------------------------------------------
		public classtube3DDLL()
		{
            int re = TUBE3D_Create();
		}

		//---------------------------------------------
		//デストラクタ，リソースの解放
		//
		//---------------------------------------------
		~classtube3DDLL()	{Dispose(false);} //マネージ系リソースの解放無しで呼び出す
		public void Dispose(){Dispose(true);} //マネージ系リソースの解放有りで呼び出す

		//---------------------------------------------
		//リソースの解放(本体)
		//
		//---------------------------------------------
		private void Dispose(bool disposing)
		{
			#region
			if(!m_Disposed)   //既に解放済みかチェック
			{
				//まだのとき
				if(disposing)        //マネージ系のリソース解放
				{
					//ここいらにマネージリソースの解放処理を
				}

				//アンマネージ系のリソースの解放
				//ここいらにアンマネージリソースの解放処理を

                int re = TUBE3D_Destroy();
			}
			m_Disposed = true;         //解放済みフラグを立てる
			#endregion
		}

		//---------------------------------------------
		//オープン
		//
		//---------------------------------------------
        public int Open()
        {
            int re = TUBE3D_Open();
            return re;
        }

		//---------------------------------------------
		//クローズ
		//
		//---------------------------------------------
        public int Close()
        {
            int re = TUBE3D_Close();
            return re;
        }


		//---------------------------------------------
		//描画主処理
		//
		//---------------------------------------------
        public int DrawData()
        {
            int re = TUBE3D_DrawData();
            return re;
        }

		//---------------------------------------------
		//マウス座標セットアップ時
		//
		//---------------------------------------------
        public int SetMousePosUp(int x, int y)
        {
            int re = TUBE3D_SetMousePosUp(x, y);
            return re;
        }

		//---------------------------------------------
		//マウス座標セットアップ時
		//
		//---------------------------------------------
        public int SetMousePosDown(int x, int y)
        {
            int re = TUBE3D_SetMousePosDown(x, y);
            return re;
        }

		//---------------------------------------------
		//マウス座標セットアップ時
		//
		//---------------------------------------------
        public int SetMousePosMove(int x, int y)
        {
            int re = TUBE3D_SetMousePosMove(x, y);
            return re;
        }
    }
}
