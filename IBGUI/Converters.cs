using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using System.Windows.Data;

namespace IBGUI
{
    public class Bool_BrushConverter : IValueConverter
    {
        public object Convert(object value, Type tragetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return Application.Current.FindResource("IBSelectedBrush") as SolidColorBrush;

            return Application.Current.FindResource("IBWindowBorderBrush") as SolidColorBrush;
        }

        public object ConvertBack(object v, Type targetType, object param, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }

    public class Bool_VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type tragetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return Visibility.Visible;

            return Visibility.Hidden;
        }

        public object ConvertBack(object v, Type targetType, object param, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
