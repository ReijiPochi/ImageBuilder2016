using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace IBGUI
{
    public class IBGUIUtility
    {
        /// <summary>
        /// IBGUIの言語カルチャを設定します
        /// </summary>
        /// <param name="cultureCode"></param>
        public static void SetLanguage(string cultureCode)
        {
            ResourceDictionary pack = new ResourceDictionary();
            pack.Source = new Uri(@"/IBGUI;component/StringResources." + cultureCode + @".xaml", UriKind.Relative);

            ResourceDictionary resource = new ResourceDictionary();
            resource.Source = new Uri(@"/IBGUI;component/ContextMenus.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(pack);
        }

        /// <summary>
        /// IBGUIのカラーテーマを設定します
        /// </summary>
        /// <param name="theme"></param>
        public static void SetColorTheme(string theme)
        {
            ResourceDictionary themeRes = new ResourceDictionary();
            themeRes.Source = new Uri(@"/IBGUI;component/IBColors." + theme + @".xaml", UriKind.Relative);

            ResourceDictionary resource = new ResourceDictionary();
            resource.Source = new Uri(@"/IBGUI;component/ResourceDictionaries.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(themeRes);
        }
    }
}
