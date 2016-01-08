using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.Windows.Markup;
using System.IO;

using IBGUI;
using IBFramework.OpenCL;

namespace IBApp.Views
{
    /* 
	 * ViewModelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedWeakEventListenerや
     * CollectionChangedWeakEventListenerを使うと便利です。独自イベントの場合はLivetWeakEventListenerが使用できます。
     * クローズ時などに、LivetCompositeDisposableに格納した各種イベントリスナをDisposeする事でイベントハンドラの開放が容易に行えます。
     *
     * WeakEventListenerなので明示的に開放せずともメモリリークは起こしませんが、できる限り明示的に開放するようにしましょう。
     */

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadLayouts("default.iblayout");
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("OpenCL Initialized : OK\nImage2D Max Width : " + CLUtilities.MaxImage2DWidth + "\nImage2D Max Height : " + CLUtilities.MaxImage2DHeight,
            //    "Alt + PrtSc でスクショとってください何でもしますから");
        }

        private void window_Activated(object sender, EventArgs e)
        {
            IBWindow.AllWindowTopmostOn();
        }

        private void window_Deactivated(object sender, EventArgs e)
        {
            IBWindow.AllWindowTopmostOff();
        }

        private void SaveLayoutAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = ".iblayout",
                Filter = "Image Builder 2016 LayoutFile|*.iblayout|すべてのファイル(*.*)|*.*",
                Title = "ワークスペースのレイアウトを名前をつけて保存",
            };

            bool? result = false;
            result = dialog.ShowDialog();

            if ((bool)result)
            {
                string layoutData = "Image Builder 2016 LayoutFile\n";
                foreach (IBWorkspace ws in IBWorkspace.AllIBWorkspace)
                {
                    if (ws.IsMainWindowContent)
                    {
                        layoutData += "// MainWindow" + "\n";
                        layoutData += "{" + "\n";
                        layoutData += "// IBWorkspace" + "\n";
                        layoutData += XamlWriter.Save(ws) + "\n";
                        layoutData += "}" + "\n";
                    }
                    else
                    {
                        IBWindow w = ws.Parent as IBWindow;
                        if (w != null)
                        {
                            layoutData += "// Window" + "\n";
                            layoutData += "{" + "\n";
                            layoutData += "// Top" + "\n";
                            layoutData += w.Top.ToString() + "\n";
                            layoutData += "// Left" + "\n";
                            layoutData += w.Left.ToString() + "\n";
                            layoutData += "// Height" + "\n";
                            layoutData += w.ActualHeight.ToString() + "\n";
                            layoutData += "// Width" + "\n";
                            layoutData += w.ActualWidth.ToString() + "\n";
                            layoutData += "// IBWorkspace" + "\n";
                            layoutData += XamlWriter.Save(ws) + "\n";
                            layoutData += "}" + "\n";
                        }
                    }
                }

                File.WriteAllText(dialog.FileName, layoutData);
            }
        }

        private void LoadLayout_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Image Builder 2016 LayoutFile|*.iblayout|すべてのファイル(*.*)|*.*",
                Title = "ワークスペースのレイアウトを読み込み",
            };

            bool? result = false;
            result = dialog.ShowDialog();

            if ((bool)result)
            {
                LoadLayouts(dialog.FileName);
            }
        }

        private static void LoadLayouts(string @FileName)
        {
            IBWindow.AllWindowClose();
            IBTabItem.ClearAllIBTabItemList();

            using (StreamReader sr = new StreamReader(FileName))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    switch (line)
                    {
                        case "// MainWindow":
                            LoadMainWindow(sr);
                            break;

                        case "// Window":
                            LoadWindow(sr);
                            break;

                        default:
                            break;
                    }
                }
            }

            IBPanel.ResetLayout();
        }

        /// <summary>
        /// .iblayoutファイルの一部からメインウインドウを読み込み、復元します。
        /// </summary>
        /// <param name="sr"></param>
        private static void LoadMainWindow(StreamReader sr)
        {
            string data = sr.ReadLine();

            if (data != "{")
                return;

            do
            {
                data = sr.ReadLine();
                switch (data)
                {
                    case "// IBWorkspace":
                        data = sr.ReadLine();
                        IBWorkspace temp = XamlReader.Parse(data) as IBWorkspace;
                        if (temp.IsMainWindowContent)
                            IBWorkspace.SetToMainwindowContent(temp);
                        IBWorkspace.AllIBWorkspace.Remove(temp);
                        break;

                    default:
                        break;
                }
            }
            while (data != "}");

            return;
        }

        /// <summary>
        /// .iblayoutファイルの一部からウインドウを一つ読み込み、復元します。
        /// </summary>
        /// <param name="sr"></param>
        private static void LoadWindow(StreamReader sr)
        {
            string data = sr.ReadLine();

            if (data != "{")
                return;

            IBWindow ibw = new IBWindow();

            do
            {
                data = sr.ReadLine();
                switch (data)
                {
                    case "// Top":
                        ibw.Top = double.Parse(sr.ReadLine());
                        break;

                    case "// Left":
                        ibw.Left = double.Parse(sr.ReadLine());
                        break;

                    case "// Height":
                        ibw.Height = double.Parse(sr.ReadLine());
                        break;

                    case "// Width":
                        ibw.Width = double.Parse(sr.ReadLine());
                        break;

                    case "// IBWorkspace":
                        data = sr.ReadLine();
                        IBWorkspace temp = XamlReader.Parse(data) as IBWorkspace;
                        ibw.Content = temp;
                        break;

                    default:
                        break;
                }
            }
            while (data != "}");

            ibw.Show();

            return;
        }
    }
}
