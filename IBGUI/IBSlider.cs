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
    public class IBSlider : Control
    {
        static IBSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBSlider), new FrameworkPropertyMetadata(typeof(IBSlider)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ValueArea = GetTemplateChild("ValueArea") as Rectangle;
            ValueArea.MouseDown += ValueArea_MouseDown;
            ValueArea.MouseEnter += ValueArea_MouseEnter;
            ValueArea.MouseMove += ValueArea_MouseMove;
            ValueArea.MouseUp += ValueArea_MouseUp;

            Meter = GetTemplateChild("Meter") as Rectangle;
            Meter.MouseMove += Meter_MouseMove;
            Meter.MouseUp += Meter_MouseUp;

            MouseLeave += IBSlider_MouseLeave;
            SizeChanged += IBSlider_SizeChanged;
        }

        Rectangle ValueArea;
        Rectangle Meter;
        private bool isMouseDown;

        private void ValueArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            CalcValue(e);
        }

        private void ValueArea_MouseEnter(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                isMouseDown = true;
            }
        }

        private void ValueArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                CalcValue(e);
            }
        }

        private void Meter_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                CalcValue(e);
            }
        }

        private void CalcValue(MouseEventArgs e)
        {
            int result = (int)((e.GetPosition(ValueArea).X / ValueArea.ActualWidth) * Maximum);

            if (result < 0) result = 0;
            else if (result > Maximum) result = Maximum;

            Value = result;
        }

        private void IBSetValue()
        {
            if (Value < 0) Value = 0;
            else if (Value > Maximum) Value = Maximum;

            if (Meter == null || ValueArea == null) return;
            Meter.Margin = new Thickness(ValueArea.ActualWidth * ((double)Value / (double)Maximum) - 2.0, 0, 0, 0);
        }

        private void ValueArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        private void Meter_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        private void IBSlider_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void IBSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            IBSetValue();
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(IBSlider), new PropertyMetadata(0,new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IBSlider)d).IBSetValue();
        }

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(IBSlider), new PropertyMetadata(114514));


    }
}
