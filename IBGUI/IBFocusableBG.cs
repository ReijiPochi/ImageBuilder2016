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
    public class IBFocusableBG : Control
    {
        static IBFocusableBG()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBFocusableBG), new FrameworkPropertyMetadata(typeof(IBFocusableBG)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Border border = GetTemplateChild("border") as Border;
            border.MouseDown += Border_MouseDown;

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Focus();
        }
    }
}
