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
    public class IBIntSlider : Control
    {
        static IBIntSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBIntSlider), new FrameworkPropertyMetadata(typeof(IBIntSlider)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ValueArea = GetTemplateChild("ValueArea") as Thumb;
            ValueArea.PreviewMouseDown += ValueArea_PreviewMouseDown;
            ValueArea.DragDelta += ValueArea_DragDelta;
            ValueArea.DragCompleted += ValueArea_DragCompleted;

            Meter = GetTemplateChild("Meter") as Thumb;
            Meter.DragDelta += Meter_DragDelta;

            SizeChanged += IBSlider_SizeChanged;
        }

        Thumb ValueArea;
        Thumb Meter;

        private void ValueArea_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CalcValue(e.GetPosition(ValueArea));
        }

        double pre;
        private void ValueArea_DragDelta(object sender, DragDeltaEventArgs e)
        {
            CalcValue(e.HorizontalChange - pre);
            pre = e.HorizontalChange;
        }

        private void ValueArea_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            pre = 0.0;
        }

        private void Meter_DragDelta(object sender, DragDeltaEventArgs e)
        {
            CalcValue(e.HorizontalChange);
        }

        private void CalcValue(Point p)
        {
            double result = (p.X / ValueArea.ActualWidth) * Maximum;

            if (result < 0) result = 0;
            else if (result > Maximum) result = Maximum;

            DoubleValue = result;
        }

        private void CalcValue(double delta)
        {
            double result = DoubleValue + (delta / ValueArea.ActualWidth) * Maximum;

            if (result < 0) result = 0;
            else if (result > Maximum) result = Maximum;

            DoubleValue = result;
        }

        private void IBSetValue()
        {
            if (Value < 0) Value = 0;
            else if (Value > Maximum) Value = Maximum;

            if (Meter == null || ValueArea == null) return;
            Meter.Margin = new Thickness(ValueArea.ActualWidth * ((double)Value / (double)Maximum) - 2.0, 0, 0, 0);
        }

        private void IBSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            IBSetValue();
        }

        public double DoubleValue
        {
            get { return (double)GetValue(DoubleValueProperty); }
            set
            {
                SetValue(DoubleValueProperty, value);

                if (Value == (int)value) return;
                Value = (int)value;
            }
        }
        public static readonly DependencyProperty DoubleValueProperty =
            DependencyProperty.Register("DoubleValue", typeof(double), typeof(IBIntSlider), new PropertyMetadata(0.0));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(IBIntSlider), new PropertyMetadata(0, new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IBIntSlider)d).IBSetValue();
        }

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(IBIntSlider), new PropertyMetadata(114514));


    }
}
