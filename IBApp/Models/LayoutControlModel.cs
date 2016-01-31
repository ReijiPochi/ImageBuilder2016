using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Livet;

using Microsoft.Win32;
using System.Windows.Markup;
using System.IO;
using System.Xml.Linq;
using System.Windows.Controls;
using IBGUI;

namespace IBApp.Models
{
    public class LayoutControlModel : NotificationObject
    {
        public static void ShowControlPanel(object panelOwner)
        {
            IBTabItem trg = panelOwner as IBTabItem;
            if (trg == null) return;

            IBWindow ibw = new IBWindow();
            ibw.InputBindings.AddRange(Application.Current.MainWindow.InputBindings);
            ibw.SetIBTabItem(trg.CloneOrCopy());
            ibw.Show();
        }
    }
}
