using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using System.ComponentModel;
using System.Windows;

namespace IBGUI
{
    public class IBControlPanelBase : Control
    {
        public double DefaultHeight { get; set; } = 200;

        public double DefaultWidth { get; set; } = 200;


        public bool IsActivePanel
        {
            get { return (bool)GetValue(IsActivePanelProperty); }
            set { SetValue(IsActivePanelProperty, value); }
        }
        public static readonly DependencyProperty IsActivePanelProperty =
            DependencyProperty.Register("IsActivePanel", typeof(bool), typeof(IBControlPanelBase), new PropertyMetadata(false));
    }
}
