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
using IBFramework.Image;

namespace IBApp.Views.ControlPanels
{
    public enum ColorCPMode
    {
        NULL,
        Pen,
        Fill
    }

    public class ColorCP : IBControlPanelBase
    {
        static ColorCP()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorCP), new FrameworkPropertyMetadata(typeof(ColorCP)));
        }

        public ColorCPMode Mode
        {
            get { return (ColorCPMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(ColorCPMode), typeof(ColorCP), new PropertyMetadata(ColorCPMode.Pen));

    
    }
}
