using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Livet;

namespace IBApp
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DispatcherHelper.UIDispatcher = Dispatcher;

#if !DEBUG
            if(MessageBox.Show("このアプリケーションは開発途中です。" +
                "使用することによって起きたいかなる損害について、開発者ReijiPochiは一切責任を負いません。\n\n" +
                "Yesをクリックすると、自己責任でアプリケーションを使用することに同意し、直ちにアプリケーションを起動します。\n" +
                "Noをクリックすると、アプリケーションを終了します。",
                "免責",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                Current.Shutdown();
            }
#endif

            Models.IBAppModel.Current = new Models.IBAppModel();
            Models.IBProjectModel.Current = new Models.IBProjectModel();
            Models.RedoUndoModel.Current = new Models.RedoUndoModel();

#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif
        }

        //集約エラーハンドラ
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            if(ex != null)
            {
                MessageBox.Show(
                    "解決できない例外がスローされました。(´・ω・｀)\nプロジェクトのバックアップが、プロジェクトファイルと同じディレクトリに保存される予定です。\n\n"
                    + ex.Message + "\n\n場所 : " + ex.Source + "\nターゲット : " + ex.TargetSite,
                    "ご迷惑をお掛けして申し訳ありません",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            Environment.Exit(1);
        }
    }
}
