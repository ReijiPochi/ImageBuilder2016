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

using IBGUI;

namespace IBApp.Views.ControlPanels
{
    public class DrawingToolsCP : IBControlPanelBase
    {
        static DrawingToolsCP()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawingToolsCP), new FrameworkPropertyMetadata(typeof(DrawingToolsCP)));
        }

        public DrawingToolsCP()
        {
            DefaultHeight = 50;
        }
    }
}
