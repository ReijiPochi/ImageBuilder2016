﻿using System;
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
using System.ComponentModel;

namespace IBGUI
{
    public class IBButton : Button
    {
        static IBButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBButton), new FrameworkPropertyMetadata(typeof(IBButton)));
        }


        [Category("IBGUI")]
        public bool MonoColorIcon
        {
            get { return (bool)GetValue(MonoColorIconProperty); }
            set { SetValue(MonoColorIconProperty, value); }
        }
        public static readonly DependencyProperty MonoColorIconProperty =
            DependencyProperty.Register("MonoColorIcon", typeof(bool), typeof(IBButton), new PropertyMetadata(true));

    }
}
