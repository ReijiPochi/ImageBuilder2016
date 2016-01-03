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

using System.Windows.Markup;

using Livet;
using Livet.Commands;

namespace IBGUI
{
    public class PanelActivatedEventArgs : RoutedEventArgs
    {
        public PanelActivatedEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {
        }
    }

    public class IBTabItem : TabItem
    {
        static IBTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBTabItem), new FrameworkPropertyMetadata(typeof(IBTabItem)));
        }

        public IBTabItem()
        {
            AllIBTabItem.Add(this);
        }

        protected static List<IBTabItem> AllIBTabItem = new List<IBTabItem>();

        /// <summary>
        /// 他のパネルにドロップできるかどうか
        /// </summary>
        public bool AllowDropToAnother { get; set; } = true;

        public bool IsActivePanel
        {
            get { return (bool)GetValue(IsActivePanelProperty); }
            set { SetValue(IsActivePanelProperty, value); }
        }
        public static readonly DependencyProperty IsActivePanelProperty =
            DependencyProperty.Register("IsActivePanel", typeof(bool), typeof(IBTabItem), new PropertyMetadata(false));


        public delegate void PanelActivatedEventHandler(object sender, PanelActivatedEventArgs e);
        public static RoutedEvent PanelActivatedEvent = EventManager.RegisterRoutedEvent(
            "PanelActivated", RoutingStrategy.Bubble, typeof(PanelActivatedEventHandler), typeof(IBTabItem));
        public event PanelActivatedEventHandler PanelActivated
        {
            add { AddHandler(PanelActivatedEvent, value); }
            remove { RemoveHandler(PanelActivatedEvent, value); }
        }



        /// <summary>
        /// ドラッグ、ドロップ用のマスク
        /// </summary>
        protected Rectangle MaskRect;

        protected bool isMouseDown = false;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            MaskRect = GetTemplateChild("IBMaskRect") as Rectangle;
            MaskRect.MouseDown += MaskRect_MouseDown;
            MaskRect.MouseUp += MaskRect_MouseUp;
            MaskRect.PreviewDragEnter += MaskRect_PreviewDragEnter;
            MaskRect.Drop += MaskRect_Drop;
            MaskRect.DragLeave += MaskRect_DragLeave;
            MaskRect.MouseLeave += MaskRect_MouseLeave;

            PreviewMouseDown += IBTabItem_PreviewMouseDown;
        }

        private void IBTabItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PanelActivate();
        }

        protected virtual void MaskRect_PreviewDragEnter(object sender, DragEventArgs e)
        {
            EndDrag();

            IBPageTabItem pti = e.Data.GetData(typeof(IBPageTabItem)) as IBPageTabItem;
            if (pti != null)
                IsSelected = true;

            IBTabItem ti = e.Data.GetData(typeof(IBTabItem)) as IBTabItem;
            if (ti == null) return;

            if (ti == this) return;

            IBTabControl parentTC = Parent as IBTabControl;
            if (parentTC == null)
                throw new IBLayoutException("IBTabItem の親が IBTabControl でありません");

            IBTabControl parentTC2 = ti.Parent as IBTabControl;
            if (parentTC2 == null)
                throw new IBLayoutException("IBTabItem の親が IBTabControl でありません");

            if (parentTC != parentTC2) return;

            parentTC.ReplaceItems(this, ti);
            ti.IsSelected = true;
            parentTC.EndDrag();
            parentTC.currentDropPos = Position.none;
        }

        protected virtual void MaskRect_Drop(object sender, DragEventArgs e)
        {
            EndDrag();

            IBTabControl parentTC = Parent as IBTabControl;
            if (parentTC == null)
                throw new IBLayoutException("IBTabItem の親が IBTabControl でありません");

            parentTC.EndDrag();
        }

        private void MaskRect_DragLeave(object sender, DragEventArgs e)
        {
            EndDrag();
        }

        private void MaskRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            PanelActivate();
        }

        protected virtual void MaskRect_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                try
                {
                    DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
                    IBPanel.ResetLayout();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(
                                    "レイアウトの変更中に例外がスローされました。\n\n"
                                    + ex.Message + "\n\n場所 : " + ex.Source + "\nターゲット : " + ex.TargetSite,
                                    "レイアウトの変更に失敗しました",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }

            isMouseDown = false;
        }

        private void MaskRect_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        /// <summary>
        /// このIBTabItemを親IBTabControlから消去します。
        /// </summary>
        public void RemoveFromParent()
        {
            IBTabControl parent = Parent as IBTabControl;

            if (parent == null)
                return;

            parent.Items.Remove(this);
        }

        protected void EndDrag()
        {
            MaskRect.Fill = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            MaskRect.Opacity = 1.0;
        }

        public virtual void PanelActivate()
        {
            foreach(IBTabItem i in AllIBTabItem)
            {
                i.IsActivePanel = false;
            }

            Window owner = Window.GetWindow(this);
            if (owner != null && owner.IsActive)
            {
                IsActivePanel = true;
                FrameworkElement child = Content as FrameworkElement;
                if (child != null)
                    child.Focus();
            }

            RaiseEvent(new PanelActivatedEventArgs(PanelActivatedEvent));
        }

        public static void ClearAllIBTabItemList()
        {
            AllIBTabItem.Clear();
        }

        public IBTabItem CloneOrCopy()
        {
            string data = XamlWriter.Save(this);
            IBTabItem i = XamlReader.Parse(data) as IBTabItem;

            return i;
        }
    }





    public class IBTabItemVM : ViewModel
    {
        #region CloseCommand
        private ListenerCommand<object> _CloseCommand;

        public ListenerCommand<object> CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new ListenerCommand<object>(DoCloseCommand, CanCloseCommand);
                }
                return _CloseCommand;
            }
        }

        public bool CanCloseCommand()
        {
            return true;
        }

        public void DoCloseCommand(object parameter)
        {
            IBMenuItem from = parameter as IBMenuItem;
            if (from == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");
            ContextMenu cm = from.Parent as ContextMenu;
            if (cm == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");
            IBTabItem trg = cm.PlacementTarget as IBTabItem;
            if (trg == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");

            IBTabControl tc = trg.Parent as IBTabControl;
            if (tc == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");
            IBPanel panel = tc.Parent as IBPanel;
            if (panel == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");


            trg.RemoveFromParent();

            if(tc.Items.Count == 0)
            {
                panel.RemoveIBPanel();
            }
        }
        #endregion

        #region FloatCommand
        private ListenerCommand<object> _FloatCommand;

        public ListenerCommand<object> FloatCommand
        {
            get
            {
                if (_FloatCommand == null)
                {
                    _FloatCommand = new ListenerCommand<object>(Float, CanFloat);
                }
                return _FloatCommand;
            }
        }

        public bool CanFloat()
        {
            return true;
        }

        public void Float(object parameter)
        {
            IBMenuItem from = parameter as IBMenuItem;
            if (from == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");
            ContextMenu cm = from.Parent as ContextMenu;
            if (cm == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");
            IBTabItem trg = cm.PlacementTarget as IBTabItem;
            if (trg == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");

            IBTabControl tc = trg.Parent as IBTabControl;
            if (tc == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");
            IBPanel panel = tc.Parent as IBPanel;
            if (panel == null)
                throw new IBDisableCommandException("コマンドターゲットが不正、または取得できません");

            IBWindow ibw = new IBWindow();
            ibw.SetIBTabItem(trg);
            ibw.Show();

            if(tc.Items.Count == 0)
            {
                panel.RemoveIBPanel();
            }
        }
        #endregion

    }
}
