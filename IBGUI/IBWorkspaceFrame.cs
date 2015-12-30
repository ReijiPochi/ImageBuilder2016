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

using System.ComponentModel;

namespace IBGUI
{
    public class IBWorkspaceFrame : Thumb
    {
        static IBWorkspaceFrame()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBWorkspaceFrame), new FrameworkPropertyMetadata(typeof(IBWorkspaceFrame)));
        }

        [Description("このプロパティはこのIBWorkspaceFrameのみに適用されます"), Category("IBGUI"), DefaultValue(true)]
        public bool ThisDragAllow { get; set; } = true;

        [Description(""), Category("IBGUI"), DefaultValue(false)]
        public bool IsDropping
        {
            get { return (bool)GetValue(IsDroppingProperty); }
            set { SetValue(IsDroppingProperty, value); }
        }
        public static readonly DependencyProperty IsDroppingProperty =
            DependencyProperty.Register("IsDropping", typeof(bool), typeof(IBWorkspaceFrame), new PropertyMetadata(false));

        private Window owner;
        private IBWindowTop ownerTop;
        private Border border;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            border = GetTemplateChild("Border") as Border;
            if (border == null)
                throw new IBInitializingException("IBWorkspaceFrame の要素 Border を取得できませんでした");

            border.MouseEnter += Border_MouseEnter;
            border.DragOver += Border_DragOver;
            border.DragLeave += Border_DragLeave;
            border.Drop += Border_Drop;

            DragDelta += IBWorkspaceFrame_DragDelta;

            owner = Window.GetWindow(this);
            ownerTop = IBWindowTop.GetIBWindowTop(owner);
            if (ownerTop == null)
                ownerTop = new IBWindowTop();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!ownerTop.ownerMaximized && ThisDragAllow)
            {
                if (HorizontalAlignment == HorizontalAlignment.Left)
                {
                    if (VerticalAlignment == VerticalAlignment.Top)
                        Cursor = Cursors.SizeNWSE;
                    else if (VerticalAlignment == VerticalAlignment.Bottom)
                        Cursor = Cursors.SizeNESW;
                    else
                        Cursor = Cursors.SizeWE;
                }
                else if(HorizontalAlignment == HorizontalAlignment.Right)
                {
                    if (VerticalAlignment == VerticalAlignment.Top)
                        Cursor = Cursors.SizeNESW;
                    else if (VerticalAlignment == VerticalAlignment.Bottom)
                        Cursor = Cursors.SizeNWSE;
                    else
                        Cursor = Cursors.SizeWE;
                }
                else if(VerticalAlignment == VerticalAlignment.Top)
                {
                    Cursor = Cursors.SizeNS;
                }
                else if(VerticalAlignment == VerticalAlignment.Bottom)
                {
                    Cursor = Cursors.SizeNS;
                }
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void IBWorkspaceFrame_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ownerTop.ownerMaximized || !ThisDragAllow) return;


            if (HorizontalAlignment == HorizontalAlignment.Left)
            {
                if (owner.ActualWidth - e.HorizontalChange > 100)
                {
                    owner.Width -= e.HorizontalChange;
                    owner.Left += e.HorizontalChange;
                }
            }

            if (HorizontalAlignment == HorizontalAlignment.Right)
            {
                if (owner.ActualWidth + e.HorizontalChange > 100)
                    owner.Width += e.HorizontalChange;
            }

            if (VerticalAlignment == VerticalAlignment.Top)
            {
                if (owner.ActualHeight - e.VerticalChange > 100)
                {
                    owner.Height -= e.VerticalChange;
                    owner.Top += e.VerticalChange;
                }
            }

            if (VerticalAlignment == VerticalAlignment.Bottom)
            {
                if (owner.ActualHeight + e.VerticalChange > 100)
                    owner.Height += e.VerticalChange;
            }
        }

        private void Border_DragOver(object sender, DragEventArgs e)
        {
            IBTabItem ti = e.Data.GetData(typeof(IBTabItem)) as IBTabItem;
            if (ti == null) return;

            IsDropping = true;
        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            IsDropping = false;
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            IsDropping = false;

            if (e.Data.GetData(typeof(IBTabItem)) == null) return;

            IBTabItem ti = e.Data.GetData(typeof(IBTabItem)) as IBTabItem;
            if (ti == null)
                throw new IBLayoutException("ドロップされたパネルを取得できません");

            if (!ti.AllowDropToAnother) return;

            IBPanel parentPanel = Parent as IBPanel;
            if (parentPanel == null)
                throw new IBLayoutException("IBPanelSplitter の親が IBPanel でありません");

            IBWorkspace Workspace = parentPanel.GetVisualParent() as IBWorkspace;
            if (Workspace == null)
                throw new IBLayoutException("IBWorkspace を取得できません");

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
            
            p1 = Workspace.Content as IBPanel;
            p1.RemoveFromParent();
            p.Children.Add(p1);

            ti.RemoveFromParent();
            tc2.Items.Add(ti);

            if (HorizontalAlignment == HorizontalAlignment.Left)
            {
                p.P2 = p1;
                p.CurrentPos = p1.CurrentPos;
                p.AddIBTabControl(tc2, Position.left);
                Workspace.Content = p;
            }
            else if (HorizontalAlignment == HorizontalAlignment.Right)
            {
                p.P1 = p1;
                p.CurrentPos = p1.CurrentPos;
                p.AddIBTabControl(tc2, Position.right);
                Workspace.Content = p;
            }
            else if (VerticalAlignment == VerticalAlignment.Top)
            {
                p.P2 = p1;
                p.CurrentPos = p1.CurrentPos;
                p.AddIBTabControl(tc2, Position.top);
                Workspace.Content = p;
            }
            else if (VerticalAlignment == VerticalAlignment.Bottom)
            {
                p.P1 = p1;
                p.CurrentPos = p1.CurrentPos;
                p.AddIBTabControl(tc2, Position.bottom);
                Workspace.Content = p;
            }

            if (parent_ti.Items.Count == 0)
            {
                parent_parent_ti.RemoveIBPanel();
            }
        }
    }
}
