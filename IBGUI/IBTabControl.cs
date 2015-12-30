using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Collections;

namespace IBGUI
{
    public enum Position
    {
        none,
        center,
        left,
        right,
        top,
        bottom,
        newWindow
    }

    public class ItemsChangedEventArgs : EventArgs
    {
    }

    public class IBTabControl : TabControl
    {
        static IBTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBTabControl), new FrameworkPropertyMetadata(typeof(IBTabControl)));
        }

        /// <summary>
        /// IBTabItemのドロップを受け付けるためのマスク
        /// </summary>
        private Rectangle MaskRect;

        /// <summary>
        /// IBTabItemがどこにドロップされようとしているかを表示するためのモノ
        /// </summary>
        private Rectangle PosRect;

        /// <summary>
        /// IBTabItemが今どこにドロップされようとしているか
        /// </summary>
        public Position currentDropPos = Position.none;

        /// <summary>
        /// カーソルの位置
        /// </summary>
        public Point cursorPos;


        #region ItemsChangedイベント
        public delegate void ItemsChangedEventHandler(object sender, ItemsChangedEventArgs e);
        public event ItemsChangedEventHandler ItemsChanged;
        public virtual void OnItemsChanged(ItemsChangedEventArgs e)
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, e);
            }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DragEnter += IBTabControl_DragEnter;
            SizeChanged += IBTabControl_SizeChanged;
            
            MaskRect = GetTemplateChild("IBMaskRect") as Rectangle;
            MaskRect.DragOver += MaskRect_DragOver;
            MaskRect.DragLeave += MaskRect_DragLeave;
            MaskRect.Drop += MaskRect_Drop;

            PosRect = GetTemplateChild("IBPosRect") as Rectangle;
        }

        private void IBTabControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ItemsSizeSet();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            ItemsSizeSet();
            OnItemsChanged(new ItemsChangedEventArgs());
        }

        private void ItemsSizeSet()
        {
            if (Items.Count == 1)
            {
                IBTabItem i = Items[0] as IBTabItem;
                if (i != null)
                {
                    i.Width = ActualWidth;
                }
            }
            else
            {
                foreach (object o in Items)
                {
                    IBTabItem i = o as IBTabItem;
                    if (i != null)
                    {
                        i.Width = double.NaN;
                    }
                }
            }
        }

        private void IBTabControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(IBTabItem)) == null || !AllowDrop) return;

            MaskRect.Visibility = Visibility.Visible;
            PosRect.Visibility = Visibility.Visible;
        }

        private void MaskRect_DragOver(object sender, DragEventArgs e)
        {
            cursorPos = e.GetPosition(this);
            currentDropPos = GetPosition(cursorPos);
            SetMaskRectangle(currentDropPos);
        }

        private void MaskRect_Drop(object sender, DragEventArgs e)
        {
            EndDrag();

            if (e.Data.GetData(typeof(IBTabItem)) == null) return;

            IBTabItem ti = e.Data.GetData(typeof(IBTabItem)) as IBTabItem;
            if (ti == null)
                throw new IBLayoutException("ドロップされたパネルを取得できませんでした");

            if (!ti.AllowDropToAnother) return;

            IBPanel parentPanel = Parent as IBPanel;
            if (parentPanel == null)
                throw new IBLayoutException("IBTabControl の親が IBPanel でありません");

            IBTabControl parent_ti = ti.Parent as IBTabControl;
            if (parent_ti == null)
                throw new IBLayoutException("IBTabItem の親が IBTabControl でありません");

            IBPanel parent_parent_ti = parent_ti.Parent as IBPanel;
            if (parent_parent_ti == null)
                throw new IBLayoutException("IBTabControl の親が IBPanel でありません");

            if(currentDropPos == Position.newWindow)
            {
                IBWindow ibw = new IBWindow();
                ibw.SetIBTabItem(ti);
                ibw.Show();
            }
            else
            {
                if (parentPanel == parent_parent_ti && parent_ti.Items.Count == 1) return;

                if (currentDropPos == Position.center)
                {
                    ti.RemoveFromParent();
                    Items.Add(ti);
                    ti.IsSelected = true;
                }
                else
                {
                    IBTabControl tc2 = new IBTabControl();

                    ti.RemoveFromParent();
                    tc2.Items.Add(ti);
                    parentPanel.AddIBTabControl(tc2, currentDropPos);
                }
            }

            if(parent_ti.Items.Count == 0)
            {
                parent_parent_ti.RemoveIBPanel();
            }
        }

        private void MaskRect_DragLeave(object sender, DragEventArgs e)
        {
            EndDrag();
        }

        public void ReplaceItems(IBTabItem ti1, IBTabItem ti2)
        {
            int index_ti1 = Items.IndexOf(ti1);
            int index_ti2 = Items.IndexOf(ti2);

            Items.Remove(ti1);
            Items.Remove(ti2);

            if(index_ti1 > index_ti2)
            {
                Items.Insert(index_ti1 - 1, ti2);
                Items.Insert(index_ti2, ti1);
            }
            else
            {
                Items.Insert(index_ti1, ti2);
                Items.Insert(index_ti2, ti1);
            }
        }

        public void EndDrag()
        {
            if (MaskRect == null || PosRect == null) return;

            MaskRect.Visibility = Visibility.Collapsed;
            PosRect.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// IBTabItemが今どこにドロップされようとしているか、カーソルの位置から判定します
        /// </summary>
        /// <param name="cursorPos"></param>
        /// <returns></returns>
        private Position GetPosition(Point cursorPos)
        {
            Position result = Position.center;

            if (cursorPos.X > this.ActualWidth * 0.25 && cursorPos.X < this.ActualWidth * 0.75)
            {
                if (cursorPos.Y < this.ActualHeight * 0.25)
                    return Position.top;
                else if (cursorPos.Y > this.ActualHeight * 0.75)
                    return Position.bottom;
                else
                    return Position.center;
            }
            else if (cursorPos.Y > this.ActualHeight * 0.25 && cursorPos.Y < this.ActualHeight * 0.75)
            {
                if (cursorPos.X < this.ActualWidth * 0.25)
                    return Position.left;
                else if (cursorPos.X > this.ActualWidth * 0.75)
                    return Position.right;
            }
            else
            {
                return Position.newWindow;
            }

            return result;
        }

        /// <summary>
        /// IBTabItemが今どこにドロップされようとしているかを表示します
        /// </summary>
        /// <param name="currentPos"></param>
        private void SetMaskRectangle(Position currentPos)
        {
            switch (currentPos)
            {
                case Position.none:
                    PosRect.Width = 0;
                    PosRect.Height = 0;
                    break;

                case Position.left:
                    PosRect.Margin = new Thickness(0);
                    PosRect.HorizontalAlignment = HorizontalAlignment.Left;
                    PosRect.VerticalAlignment = VerticalAlignment.Stretch;
                    PosRect.Width = this.ActualWidth * 0.25;
                    PosRect.Height = double.NaN;
                    break;

                case Position.right:
                    PosRect.Margin = new Thickness(0);
                    PosRect.HorizontalAlignment = HorizontalAlignment.Right;
                    PosRect.VerticalAlignment = VerticalAlignment.Stretch;
                    PosRect.Width = this.ActualWidth * 0.25;
                    PosRect.Height = double.NaN;
                    break;

                case Position.top:
                    PosRect.Margin = new Thickness(0);
                    PosRect.HorizontalAlignment = HorizontalAlignment.Stretch;
                    PosRect.VerticalAlignment = VerticalAlignment.Top;
                    PosRect.Width = double.NaN;
                    PosRect.Height = this.ActualHeight * 0.25;
                    break;

                case Position.bottom:
                    PosRect.Margin = new Thickness(0);
                    PosRect.HorizontalAlignment = HorizontalAlignment.Stretch;
                    PosRect.VerticalAlignment = VerticalAlignment.Bottom;
                    PosRect.Width = double.NaN;
                    PosRect.Height = this.ActualHeight * 0.25;
                    break;

                case Position.center:
                    PosRect.Margin = new Thickness(0);
                    PosRect.HorizontalAlignment = HorizontalAlignment.Stretch;
                    PosRect.VerticalAlignment = VerticalAlignment.Stretch;
                    PosRect.Width = double.NaN;
                    PosRect.Height = double.NaN;
                    break;

                case Position.newWindow:
                    PosRect.HorizontalAlignment = HorizontalAlignment.Left;
                    PosRect.VerticalAlignment = VerticalAlignment.Top;
                    PosRect.Margin = new Thickness(cursorPos.X, cursorPos.Y, 0, 0);
                    PosRect.Width = 100;
                    PosRect.Height = 100;
                    break;

                default:
                    break;
            }
        }

        public void RemoveFromParent()
        {
            IBPanel tc1parent = Parent as IBPanel;
            if (tc1parent == null)
                return;

            tc1parent.Children.Remove(this);
        }
    }
}
