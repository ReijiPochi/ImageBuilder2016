using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using IBFramework.Project;
using IBFramework.Project.IBProjectElements;
using IBFramework.Timeline.TimelineElements;

namespace IBFramework
{
    public class ElementToBrushConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((IBProjectElementTypes)value)
            {
                case IBProjectElementTypes.Folder:
                    return Application.Current.FindResource("FolderBrush") as SolidColorBrush;

                case IBProjectElementTypes.Cell:
                case IBProjectElementTypes.CellSource:
                    return Application.Current.FindResource("CellBrush") as SolidColorBrush;

                default:
                    break;
            }

            return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return typeof(Folder);
        }
    }
}
