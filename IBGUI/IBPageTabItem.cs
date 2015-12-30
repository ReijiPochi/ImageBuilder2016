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

namespace IBGUI
{
    public class IBPageTabItem : IBTabItem
    {
        static IBPageTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBPageTabItem), new FrameworkPropertyMetadata(typeof(IBPageTabItem)));
        }

        private bool isDummyItem { get; set; } = false;

        private IBButton closeBtn;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            closeBtn = GetTemplateChild("CloseBtn") as IBButton;
            if (closeBtn != null)
            {
                closeBtn.Click += CloseBtn_Click;
            }

            MouseDown += IBPageTabItem_MouseDown;
        }

        private void IBPageTabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        protected override void MaskRect_PreviewDragEnter(object sender, DragEventArgs e)
        {
            //base.MaskRect_PreviewDragEnter(sender, e);

            EndDrag();

            IBPageTabItem ti = e.Data.GetData(typeof(IBPageTabItem)) as IBPageTabItem;
            if (ti == null) return;

            if (ti == this) return;

            IBTabControl parentTC = Parent as IBTabControl;
            if (parentTC == null)
                throw new IBLayoutException("IBPageTabItem の親が IBTabControl でありません");

            IBTabControl parentTC2 = ti.Parent as IBTabControl;
            if (parentTC2 == null)
                throw new IBLayoutException("IBPageTabItem の親が IBTabControl でありません");

            if (parentTC == parentTC2)
            {
                parentTC.ReplaceItems(this, ti);
            }
            else
            {
                parentTC2.Items.Remove(ti);
                parentTC.Items.Insert(parentTC.Items.IndexOf(this), ti);

                if (isDummyItem) parentTC.Items.Remove(this);

                if (parentTC2.Items.Count == 0)
                    parentTC2.Items.Add(new IBPageTabItem() { isDummyItem = true, Header = "****" });
            }

            ti.IsSelected = true;
            parentTC.EndDrag();
        }

        protected override void MaskRect_MouseLeave(object sender, MouseEventArgs e)
        {
            //base.MaskRect_MouseLeave(sender, e);

            if (isMouseDown)
            {
                if(!isDummyItem) DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
                IBPanel.ResetLayout();
            }

            isMouseDown = false;
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);

            if (isDummyItem)
                IsSelected = false;
        }

        public override void PanelActivate()
        {
            //base.PanelActivate();
        }


        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isDummyItem) return;

            IBTabControl parent = Parent as IBTabControl;
            if (parent == null) return;

            parent.Items.Remove(this);

            if (parent.Items.Count == 0)
                parent.Items.Add(new IBPageTabItem() { isDummyItem = true, Header = "****" });
        }


        //protected override void MaskRect_Drop(object sender, DragEventArgs e)
        //{
        //    //base.MaskRect_Drop(sender, e);
        //}
    }
}
