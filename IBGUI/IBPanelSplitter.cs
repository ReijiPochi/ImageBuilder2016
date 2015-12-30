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

using System.ComponentModel;

namespace IBGUI
{
    public class IBPanelSplitter : GridSplitter
    {
        static IBPanelSplitter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBPanelSplitter), new FrameworkPropertyMetadata(typeof(IBPanelSplitter)));
        }

       
        [Description(""), Category("IBGUI"), DefaultValue(false)]
        public bool IsDropping
        {
            get { return (bool)GetValue(IsDroppingProperty); }
            set { SetValue(IsDroppingProperty, value); }
        }
        public static readonly DependencyProperty IsDroppingProperty =
            DependencyProperty.Register("IsDropping", typeof(bool), typeof(IBPanelSplitter), new PropertyMetadata(false));



        private Border border;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            border = GetTemplateChild("Border") as Border;
            if (border == null)
                throw new IBInitializingException("IBPanelSplitter の要素 Border を取得できませんでした");

            border.DragOver += Border_DragOver;
            border.DragLeave += Border_DragLeave;
            border.Drop += Border_Drop;
        }

        private void Border_DragOver(object sender, DragEventArgs e)
        {
            IBTabItem ti = e.Data.GetData(typeof(IBTabItem)) as IBTabItem;
            if (ti == null) return;

            //Background = FindResource("IBFocusBrush2_T") as SolidColorBrush;
            IsDropping = true;
        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            //Background = FindResource("IBBackGroundBrush") as SolidColorBrush;
            IsDropping = false;
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            //Background = FindResource("IBBackGroundBrush") as SolidColorBrush;
            IsDropping = false;

            if (e.Data.GetData(typeof(IBTabItem)) == null) return;

            IBTabItem ti = e.Data.GetData(typeof(IBTabItem)) as IBTabItem;
            if (ti == null)
                throw new IBLayoutException("ドロップされたパネルを取得できませんでした");

            if (!ti.AllowDropToAnother) return;

            IBPanel parentPanel = Parent as IBPanel;
            if (parentPanel == null)
                throw new IBLayoutException("IBPanelSplitter の親が IBPanel でありません");

            IBTabControl parent_ti = ti.Parent as IBTabControl;
            if (parent_ti == null)
                throw new IBLayoutException("IBTabItem の親が IBTabControl でありません");

            IBPanel parent_parent_ti = parent_ti.Parent as IBPanel;
            if (parent_parent_ti == null)
                throw new IBLayoutException("IBTabControl の親が IBPanel でありません");

            if (parentPanel == parent_parent_ti && parent_ti.Items.Count == 1) return;


            IBTabControl tc2 = new IBTabControl();
            IBPanel p = new IBPanel();
            IBPanel p1 = new IBPanel();

            ti.RemoveFromParent();
            tc2.Items.Add(ti);

            if(HorizontalAlignment== HorizontalAlignment.Left)
            {
                p1 = parentPanel.GetChildIBPanelAt(Position.right);
                p1.RemoveFromParent();
                p.Children.Add(p1);
                p.P2 = p1;
                p.CurrentPos = p1.CurrentPos;
                p.AddIBTabControl(tc2, Position.left);
                parentPanel.Children.Add(p);
                parentPanel.P2 = p;
            }
            else if(HorizontalAlignment == HorizontalAlignment.Right)
            {
                p1 = parentPanel.GetChildIBPanelAt(Position.left);
                p1.RemoveFromParent();
                p.Children.Add(p1);
                p.P1 = p1;
                p.CurrentPos = p1.CurrentPos;
                p.AddIBTabControl(tc2, Position.right);
                parentPanel.Children.Add(p);
                parentPanel.P1 = p;
            }
            else if(VerticalAlignment == VerticalAlignment.Top)
            {
                p1 = parentPanel.GetChildIBPanelAt(Position.bottom);
                p1.RemoveFromParent();
                p.Children.Add(p1);
                p.P2 = p1;
                p.CurrentPos = p1.CurrentPos;
                p.AddIBTabControl(tc2, Position.top);
                parentPanel.Children.Add(p);
                parentPanel.P2 = p;
            }
            else if(VerticalAlignment == VerticalAlignment.Bottom)
            {
                p1 = parentPanel.GetChildIBPanelAt(Position.top);
                p1.RemoveFromParent();
                p.Children.Add(p1);
                p.P1 = p1;
                p.CurrentPos = p1.CurrentPos;
                p.AddIBTabControl(tc2, Position.bottom);
                parentPanel.Children.Add(p);
                parentPanel.P1 = p;
            }

            if (parent_ti.Items.Count == 0)
            {
                parent_parent_ti.RemoveIBPanel();
            }
        }

    }
}
