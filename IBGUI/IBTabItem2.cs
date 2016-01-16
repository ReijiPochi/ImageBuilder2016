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
    public class IBTabItem2 : TabItem
    {
        static IBTabItem2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBTabItem2), new FrameworkPropertyMetadata(typeof(IBTabItem2)));
        }
    }
}
