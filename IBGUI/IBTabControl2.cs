using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IBGUI
{
    public class IBTabControl2 : TabControl
    {
        static IBTabControl2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBTabControl2), new FrameworkPropertyMetadata(typeof(IBTabControl2)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ug = GetTemplateChild("HeaderPanel") as UniformGrid;
            ug.SizeChanged += Ug_SizeChanged;
        }

        private void Ug_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Area = new Rect(0, 0, ug.ActualWidth, ug.ActualHeight);
        }

        UniformGrid ug;

        public Rect Area
        {
            get { return (Rect)GetValue(AreaProperty); }
            set { SetValue(AreaProperty, value); }
        }
        public static readonly DependencyProperty AreaProperty =
            DependencyProperty.Register("Area", typeof(Rect), typeof(IBTabControl2), new PropertyMetadata(new Rect(0,0,10,10)));


    }
}
