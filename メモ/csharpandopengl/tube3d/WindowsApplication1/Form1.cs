using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {

        private tube3d.classes.classtube3DDLL m_dll;

		//---------------------------------------------
		//コンストラクタ
		//
		//---------------------------------------------
        public Form1()
        {
            InitializeComponent();
        }

		//---------------------------------------------
		//フォームロードイベント
		//
		//---------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            m_dll = new tube3d.classes.classtube3DDLL();            //アンマネージDLLのラッパクラスのインスタンスを生成

            m_dll.WindowHandle = panel1.Handle;                     //描画を行う，コントロールのウィンドウハンドルをセット(今回はパネルを使用)
                    
        }

		//---------------------------------------------
		//フォームクローズイベント
		//
		//---------------------------------------------
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_dll != null)                                  //アンマネージDLLのラッパクラスのインスタンスを破棄
            {
                m_dll.Dispose();
                m_dll = null;
            }
        }

		//---------------------------------------------
		//描画開始ボタン
		//
		//---------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;                              //描画用タイマを起動
        }

		//---------------------------------------------
		//DLLシステム側をオープン（オープンで各種初期化を行う）
		//
		//---------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            m_dll.Open();
        }

		//---------------------------------------------
		//DLLシステム側をクローズ（クローズで各種破棄処理を行う）
		//
		//---------------------------------------------
        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;             //描画タイマを停止

            System.Threading.Thread.Sleep(300); //少しだけ待って

            m_dll.Close();                      //DLLを閉じる
        }

		//---------------------------------------------
		//描画用タイマ
		//
		//---------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            m_dll.DrawData();

        }

		//---------------------------------------------
		//マウスムーブイベント(描画面空間の回転を行う)
		//
		//---------------------------------------------
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_dll.SetMousePosMove(e.X, e.Y);
            }
        }

		//---------------------------------------------
		//マウスアップイベント（とりあえず，未使用）
		//
		//---------------------------------------------
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            m_dll.SetMousePosUp(e.X, e.Y);
        }

		//---------------------------------------------
		//マウスダウンイベント（描画面空間の回転のための初期座標をセット）
		//
		//---------------------------------------------
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            m_dll.SetMousePosDown(e.X, e.Y);

        }
    }
}