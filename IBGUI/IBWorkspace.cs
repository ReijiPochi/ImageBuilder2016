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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;

namespace IBGUI
{
    public class IBWorkspace : ContentControl
    {
        static IBWorkspace()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBWorkspace), new FrameworkPropertyMetadata(typeof(IBWorkspace)));
        }

        public IBWorkspace()
        {
            AllIBWorkspace.Add(this);
            Unloaded += IBWorkspace_Unloaded;
            AddHandler(IBTabItem.PanelActivatedEvent, new IBTabItem.PanelActivatedEventHandler(ChildPanelActivated));
            Loaded += IBWorkspace_Loaded;
        }

        private void IBWorkspace_Loaded(object sender, RoutedEventArgs e)
        {
            Window owner = Window.GetWindow(this);

            if (owner == null) return;

            owner.Activated += IBWorkspace_Activated;
            owner.Deactivated += IBWorkspace_Deactivated;
        }

        private void IBWorkspace_Activated(object sender, EventArgs e)
        {
            if (LastActivePanel != null)
            {
                LastActivePanel.PanelActivate();
            }
            else
            {
                
            }
        }

        private void IBWorkspace_Deactivated(object sender, EventArgs e)
        {
            if (LastActivePanel != null)
            {
                LastActivePanel.IsActivePanel = false;
            }
        }

        private IBTabItem LastActivePanel;

        private void IBWorkspace_Unloaded(object sender, RoutedEventArgs e)
        {
            AllIBWorkspace.Remove(this);
        }

        private void ChildPanelActivated(object sender, PanelActivatedEventArgs e)
        {
            LastActivePanel = e.OriginalSource as IBTabItem;
        }

        [Description("メインウインドウのコンテントであれば true に設定してください。レイアウトの復元に関係します"), Category("IBGUI"), DefaultValue(false)]
        public bool IsMainWindowContent { get; set; }

        public static List<IBWorkspace> AllIBWorkspace = new List<IBWorkspace>();

        public static void SetToMainwindowContent(IBWorkspace setItem)
        {
            foreach(IBWorkspace ws in AllIBWorkspace)
            {
                if (ws.IsMainWindowContent)
                {
                    ws.Content = setItem.Content;
                }
            }
        }
    }
}
