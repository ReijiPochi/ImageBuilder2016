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
    public class IBIntBox : IBTextBox
    {
        static IBIntBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBIntBox), new FrameworkPropertyMetadata(typeof(IBIntBox)));
        }

        public IBIntBox()
        {
            TextChanged += IBIntBox_TextChanged;
            MouseWheel += IBIntBox_MouseWheel;
        }

        private void IBIntBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!IsFocused) return;

            if (e.Delta > 0)
            {
                if(IntValue < Maximum)IntValue++;
            }
            else
            {
                if (IntValue > 0) IntValue--;
            }
        }

        private void IBIntBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value;
            if(int.TryParse(Text, out value))
            {
                if (value > Maximum) value = Maximum;
                IntValue = value;
            }
        }

        public int IntValue
        {
            get { return (int)GetValue(IntValueProperty); }
            set { SetValue(IntValueProperty, value); }
        }
        public static readonly DependencyProperty IntValueProperty =
            DependencyProperty.Register("IntValue", typeof(int), typeof(IBIntBox), new PropertyMetadata(0,new PropertyChangedCallback(OnIntValueChanged)));

        private static void OnIntValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IBIntBox trg = (IBIntBox)d;
            if ((int)e.NewValue > trg.Maximum) return;
            trg.Text = ((int)e.NewValue).ToString();
        }


        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(IBIntBox), new PropertyMetadata(114514));


    }
}
