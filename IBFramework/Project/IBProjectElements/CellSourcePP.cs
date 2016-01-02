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

namespace IBFramework.Project.IBProjectElements
{
    public class CellSourcePP : Control
    {
        static CellSourcePP()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CellSourcePP), new FrameworkPropertyMetadata(typeof(CellSourcePP)));
        }
    }
}
