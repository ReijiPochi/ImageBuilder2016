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
    public class IBWindowTop : Control
    {
        static IBWindowTop()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBWindowTop), new FrameworkPropertyMetadata(typeof(IBWindowTop)));
        }

        public IBWindowTop()
        {
            AllIBWindowTop.Add(this);
        }

        private static List<IBWindowTop> AllIBWindowTop = new List<IBWindowTop>();

        private Window owner;
        public bool ownerMaximized { get; private set; } = false;
        private bool doubleClicked = false;
        private double ownerPreHeight = 100;
        private double ownerPreWidth = 100;
        private double ownerPrePosL = 100;
        private double ownerPrePosT = 100;

        private IBButton closeBtn;
        private IBButton maxBtn;
        private IBButton minBtn;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            MouseLeftButtonDown += IBWindowTop_MouseLeftButtonDown;
            MouseDoubleClick += IBWindowTop_MouseDoubleClick;
            MouseLeave += IBWindowTop_MouseLeave;

            closeBtn = GetTemplateChild("CloseBtn") as IBButton;
            closeBtn.Click += CloseBtn_Click;
            maxBtn = GetTemplateChild("MaxBtn") as IBButton;
            maxBtn.Click += MaxBtn_Click;
            minBtn = GetTemplateChild("MinBtn") as IBButton;
            minBtn.Click += MinBtn_Click;

            owner = Window.GetWindow(this);
            if (owner != null)
            {
                owner.StateChanged += Owner_StateChanged;
                if (owner.WindowState == WindowState.Maximized) ownerMaximized = true;
            }
        }

        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            owner.WindowState = WindowState.Minimized;
        }

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ownerMaximized)
                WindowRestore();
            else
                owner.WindowState = WindowState.Maximized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            owner.Close();
        }

        private void IBWindowTop_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ownerMaximized)
            {
                WindowRestore();
            }
            else
            {
                doubleClicked = true;
                owner.WindowState = WindowState.Maximized;
            }
        }

        private void Owner_StateChanged(object sender, EventArgs e)
        {
            if(owner.WindowState == WindowState.Maximized)
            {
                WindowMaximize();
            }
        }

        private void IBWindowTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!ownerMaximized)
                owner.DragMove();
        }

        private void IBWindowTop_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ownerMaximized && e.LeftButton == MouseButtonState.Pressed && !doubleClicked)
            {
                ownerMaximized = false;
                owner.Height = ownerPreHeight;
                owner.Width = ownerPreWidth;
                owner.BorderThickness = new Thickness(1);
                Point p = PointToScreen(e.GetPosition(this));
                owner.Left = p.X - ownerPreWidth / 2;
                owner.Top = p.Y - 15;
                owner.DragMove();
            }

            doubleClicked = false;
        }

        private void WindowMaximize()
        {
            ownerMaximized = true;
            owner.WindowState = WindowState.Normal;
            owner.BorderThickness = new Thickness(0);
            ownerPreHeight = owner.ActualHeight;
            ownerPreWidth = owner.ActualWidth;
            ownerPrePosL = owner.Left;
            ownerPrePosT = owner.Top;

            Point p = PointToScreen(Mouse.GetPosition(this));
            System.Drawing.Point point = new System.Drawing.Point { X = (int)p.X, Y = (int)p.Y };
            System.Drawing.Rectangle workspace = System.Windows.Forms.Screen.GetWorkingArea(point);
            System.Windows.Forms.Screen scr = System.Windows.Forms.Screen.FromPoint(point);
            if (scr.Primary && workspace.Height == scr.Bounds.Height)
                workspace.Height -= 1;  // タスクバーが隠れてしまうのを防ぐため

            owner.Left = workspace.Left;
            owner.Top = workspace.Top - 7;
            owner.Height = workspace.Height + 7;
            owner.Width = workspace.Width;
        }

        private void WindowRestore()
        {
            ownerMaximized = false;

            owner.Height = ownerPreHeight;
            owner.Width = ownerPreWidth;
            owner.Left = ownerPrePosL;
            owner.Top = ownerPrePosT;
            owner.BorderThickness = new Thickness(1);
        }

        public static IBWindowTop GetIBWindowTop(Window w)
        {
            foreach(IBWindowTop wt in AllIBWindowTop)
            {
                Window trg = Window.GetWindow(wt);
                if (trg == w)
                {
                    return wt;
                }
            }

            return null;
        }
    }
}
