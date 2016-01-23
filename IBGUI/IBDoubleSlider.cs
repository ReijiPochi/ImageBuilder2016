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
    public class IBDoubleSlider : Control
    {
        static IBDoubleSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBDoubleSlider), new FrameworkPropertyMetadata(typeof(IBDoubleSlider)));
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
            double result = (p.X * Maximum) / ValueArea.ActualWidth;

            string temp = result.ToString("f" + FloatDigit.ToString("d"));
            TempValue = double.Parse(temp);
        }

        private void CalcValue(double delta)
        {
            double result = TempValue + delta * Maximum / ValueArea.ActualWidth;

            string temp = result.ToString("f" + FloatDigit.ToString("d"));
            TempValue = double.Parse(temp);
        }

        private void IBSetValue()
        {
            if (Value < 0) Value = 0;
            else if (Value > Maximum) Value = Maximum;

            if (Meter == null || ValueArea == null) return;
            Meter.Margin = new Thickness(ValueArea.ActualWidth * Value / Maximum - 2.0, 0, 0, 0);
        }

        private void IBSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            IBSetValue();
        }

        private double _TempValue;
        private double TempValue
        {
            get { return _TempValue; }
            set
            {
                _TempValue = value;

                if (Math.Abs(Value - value) < 0.000001) return;

                double result = value;
                if (result < 0) result = 0;
                else if (result > Maximum) result = Maximum;
                Value = result;
            }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(IBDoubleSlider), new PropertyMetadata(0.0, new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IBDoubleSlider)d).IBSetValue();
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(IBDoubleSlider), new PropertyMetadata(810.0));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(IBDoubleSlider), new PropertyMetadata(0.0));

        public int FloatDigit
        {
            get { return (int)GetValue(FloatDigitProperty); }
            set { SetValue(FloatDigitProperty, value); }
        }
        public static readonly DependencyProperty FloatDigitProperty =
            DependencyProperty.Register("FloatDigit", typeof(int), typeof(IBDoubleSlider), new PropertyMetadata(1));
    }
}
