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
    public class IBToggleButton : Button
    {
        static IBToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBToggleButton), new FrameworkPropertyMetadata(typeof(IBToggleButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Click += IBToggleButton_Click;
        }

        private void IBToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonON = !ButtonON;
        }

        [Description(""), Category("IBGUI"), DefaultValue(false)]
        public bool ButtonON
        {
            get { return (bool)GetValue(ButtonONProperty); }
            set { SetValue(ButtonONProperty, value); }
        }
        public static readonly DependencyProperty ButtonONProperty =
            DependencyProperty.Register("ButtonON", typeof(bool), typeof(IBToggleButton), new PropertyMetadata(false));


    }
}
