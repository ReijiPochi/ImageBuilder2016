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

namespace IBFramework.Project
{
    public class FlagButton : Control
    {
        static FlagButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlagButton), new FrameworkPropertyMetadata(typeof(FlagButton)));
        }


        [Category("IBFramework")]
        public IBProjectElementFlags Flag
        {
            get { return (IBProjectElementFlags)GetValue(FlagProperty); }
            set { SetValue(FlagProperty, value); }
        }
        public static readonly DependencyProperty FlagProperty =
            DependencyProperty.Register("Flag", typeof(IBProjectElementFlags), typeof(FlagButton), new PropertyMetadata(IBProjectElementFlags.Non));
    }


}
