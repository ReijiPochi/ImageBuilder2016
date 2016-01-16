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

using IBGUI;

namespace IBApp.Views.ControlPanels
{
    public class CanvasViewCP : IBControlPanelBase
    {
        static CanvasViewCP()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasViewCP), new FrameworkPropertyMetadata(typeof(CanvasViewCP)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            g = GetTemplateChild("root") as Grid;

            GotFocus += CanvasViewCP_GotFocus;
        }

        Grid g;

        private void CanvasViewCP_GotFocus(object sender, RoutedEventArgs e)
        {
            if (g != null)
                g.Focus();
        }
    }
}
