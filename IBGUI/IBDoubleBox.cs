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
    public class IBDoubleBox : IBTextBox
    {
        static IBDoubleBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBDoubleBox), new FrameworkPropertyMetadata(typeof(IBDoubleBox)));
        }

        public IBDoubleBox()
        {
            LostFocus += IBDoubleBox_LostFocus;
            MouseWheel += IBDoubleBox_MouseWheel;
        }

        private void IBDoubleBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(Text, out value))
            {
                if (value > Maximum) value = Maximum;
                else if (value < 0.0) value = 0.0;

                string temp = value.ToString(Format);
                DoubleValue = double.Parse(temp);
            }

            Text = DoubleValue.ToString(Format);
        }

        private void IBDoubleBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!IsFocused) return;

            if (e.Delta > 0)
            {
                if (DoubleValue + Delta < Maximum) DoubleValue += Delta;
            }
            else
            {
                if (DoubleValue - Delta > 0) DoubleValue -= Delta;
            }
        }



        public double DoubleValue
        {
            get { return (double)GetValue(DoubleValueProperty); }
            set { SetValue(DoubleValueProperty, value); }
        }
        public static readonly DependencyProperty DoubleValueProperty =
            DependencyProperty.Register("DoubleValue", typeof(double), typeof(IBDoubleBox), new PropertyMetadata(0.0, new PropertyChangedCallback(OnDoubleValueChanged)));

        private static void OnDoubleValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IBDoubleBox trg = (IBDoubleBox)d;

            if ((double)e.NewValue > trg.Maximum) trg.DoubleValue = trg.Maximum;
            else if((double)e.NewValue < 0.0) trg.DoubleValue = 0.0;
            trg.Text = trg.DoubleValue.ToString(trg.Format);
        }


        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(IBDoubleBox), new PropertyMetadata(810.0));


        public double Delta
        {
            get { return (double)GetValue(DeltaProperty); }
            set { SetValue(DeltaProperty, value); }
        }
        public static readonly DependencyProperty DeltaProperty =
            DependencyProperty.Register("Delta", typeof(double), typeof(IBDoubleBox), new PropertyMetadata(1.0));


        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(string), typeof(IBDoubleBox), new PropertyMetadata("f1"));




    }
}
