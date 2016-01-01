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

namespace IBFramework.Timeline.TimelineElements
{
    public class CellPP : Control
    {
        static CellPP()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CellPP), new FrameworkPropertyMetadata(typeof(CellPP)));
        }
    }
}
