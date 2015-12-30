using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

using System.Windows;
using IBGUI;


namespace IBApp.Models
{
    /// <summary>
    /// IBAppの共通的なロジック
    /// </summary>
    public class IBAppModel : NotificationObject
    {
        /// <summary>
        /// App.xaml.cs のスタートアップでインスタントを設定
        /// </summary>
        public static IBAppModel Current { get; set; }

        /// <summary>
        /// アプリケーションを終了します
        /// </summary>
        public static void AppExit()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// アプリケーションの言語を設定します
        /// </summary>
        /// <param name="cultureCode">"jp-JP", "in-MU" 等</param>
        public static void SetLanguage(string cultureCode)
        {
            ResourceDictionary pack = new ResourceDictionary();
            pack.Source = new Uri(@"Languages/LanguagePack." + cultureCode + @".xaml", UriKind.Relative);
            App.Current.Resources.MergedDictionaries.Add(pack);

            IBGUIUtility.SetLanguage(cultureCode);
        }

        /// <summary>
        /// アプリケーションのGUIカラーテーマを設定します
        /// </summary>
        /// <param name="theme">"dark-pink", "light-pink" 等</param>
        public static void SetColorTheme(string theme)
        {
            IBGUIUtility.SetColorTheme(theme);
        }
    }
}
