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

namespace IBGUI
{
    public class IBTextBox : TextBox
    {
        static IBTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBTextBox), new FrameworkPropertyMetadata(typeof(IBTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            KeyDown += IBTextBox_KeyDown;
        }

        private void IBTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
        }
    }
}
