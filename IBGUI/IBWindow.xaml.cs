using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IBGUI
{
    /// <summary>
    /// Interaction logic for IBWindow.xaml
    /// </summary>
    public partial class IBWindow : Window
    {
        public IBWindow()
        {
            InitializeComponent();
            AllIBWindows.Add(this);
            Resources = Application.Current.Resources;
        }

        private static List<IBWindow> AllIBWindows = new List<IBWindow>();

        public void SetIBTabItem(IBTabItem item)
        {
            item.RemoveFromParent();
            MainTabControl.Items.Add(item);
            item.IsSelected = true;
        }

        public static void AllWindowTopmostOn()
        {
            foreach(IBWindow ibw in AllIBWindows)
            {
                ibw.Topmost = true;
            }
        }

        public static void AllWindowTopmostOff()
        {
            foreach (IBWindow ibw in AllIBWindows)
            {
                ibw.Topmost = false;
            }
        }

        public static void AllWindowClose()
        {
            while(AllIBWindows.Count > 0)
            {
                AllIBWindows[0].Close();
            }
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AllIBWindows.Remove(this);
        }
    }
}
