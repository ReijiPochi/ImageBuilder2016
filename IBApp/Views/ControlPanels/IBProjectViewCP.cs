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

using IBGUI;
using IBFramework.Project;
using IBFramework.Project.IBProjectElements;
using IBFramework.RedoUndo;
using System.Windows.Interactivity;

namespace IBApp.Views.ControlPanels
{
    public class IBProjectViewCP : IBControlPanelBase
    {
        static IBProjectViewCP()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBProjectViewCP), new FrameworkPropertyMetadata(typeof(IBProjectViewCP)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            tv = GetTemplateChild("TreeView") as IBTreeView;
            tv.SelectedItemChanged += Tv_SelectedItemChanged;
            tv.MouseLeftButtonDown += Tv_MouseLeftButtonDown;
            tv.Drop += Tv_Drop;

            Unloaded += IBProjectViewCP_Unloaded;
        }

        private void IBProjectViewCP_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }

        IBTreeView tv;
        static IBProjectElement grobalSelectedElement;

        public IBProjectElement SelectedElement
        {
            get { return (IBProjectElement)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }
        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof(IBProjectElement), typeof(IBProjectViewCP), new PropertyMetadata(null,new PropertyChangedCallback(OnSelectedElementChanged)));

        private static void OnSelectedElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                grobalSelectedElement = e.NewValue as IBProjectElement;
        }

        private void Tv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tv.SelectedItem == null) return;

            ((IBProjectElement)tv.SelectedItem).IsSelected = false;
        }

        private void Tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedElement = e.NewValue as IBProjectElement;
        }

        private void Tv_Drop(object sender, DragEventArgs e)
        {
            string[] formats = e.Data.GetFormats();
            if (formats.Length == 0) return;

            IBProjectElement from = e.Data.GetData(formats[0]) as IBProjectElement;
            if (from == null) return;

            IBProjectElement preParent = from.Parent;
            int preIndex;

            preIndex = preParent.Children.IndexOf(from);

            from.Parent.RemoveChild(from);

            IBProject.Current.ElementsRoot.AddChild(from);

            RedoUndoManager.Current.Record(new RUMoveProjectElement(from, preParent, preIndex));
        }

        /// <summary>
        /// TemplatedParentからTreeViewItemを探し、返します
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TreeViewItem FindTreeViewItem(FrameworkElement source)
        {
            TreeViewItem result = source as TreeViewItem;
            if (result != null)
                return result;
            else
            {
                FrameworkElement parent = source.TemplatedParent as FrameworkElement;
                if (parent == null) return null;

                result = FindTreeViewItem(parent);
            }

            return result;
        }

        public static IBProjectElement GetSelectedElement()
        {
            return grobalSelectedElement;
        }
    }

    public class RUMoveProjectElement : RedoUndoAction
    {
        public RUMoveProjectElement(IBProjectElement _trg, IBProjectElement _preParent, int _preIndex)
        {
            trg = _trg;
            preParent = _preParent;
            preIndex = _preIndex;
            newParent = trg.Parent;

            newIndex = newParent.Children.IndexOf(trg);
        }

        private IBProjectElement trg;

        private int preIndex;
        private IBProjectElement preParent;

        private int newIndex;
        private IBProjectElement newParent;

        public override void Redo()
        {
            base.Redo();

            preParent.RemoveChild(trg);

            newParent.Children.Insert(newIndex, trg);
            trg.Parent = newParent;
        }

        public override void Undo()
        {
            base.Undo();

            newParent.RemoveChild(trg);

            preParent.Children.Insert(preIndex, trg);
            trg.Parent = preParent;
        }
    }

    public class IBProjectViewItemsBehavier : Behavior<StackPanel>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            trg = IBProjectViewCP.FindTreeViewItem(AssociatedObject as FrameworkElement);

            if (trg != null)
            {
                trg.MouseRightButtonDown += AssociatedObject_MouseRightButtonDown;
                trg.AllowDrop = true;
                trg.MouseLeave += Trg_MouseLeave;
                trg.DragLeave += Trg_DragLeave;
                trg.DragEnter += Trg_DragEnter;
                trg.DragOver += Trg_DragOver;
                trg.Drop += Trg_Drop;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (trg != null)
            {
                trg.MouseRightButtonDown -= AssociatedObject_MouseRightButtonDown;
                trg.MouseLeave -= Trg_MouseLeave;
                trg.DragLeave -= Trg_DragLeave;
                trg.DragEnter -= Trg_DragEnter;
                trg.DragOver -= Trg_DragOver;
                trg.Drop -= Trg_Drop;
            }
        }

        TreeViewItem trg;
        const double HEIGHT = 22;
        IBProjectElement trgElement;
        IBProjectElement from_temp;

        private void AssociatedObject_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (trg != null)
                trg.IsSelected = true;

            e.Handled = true;
        }

        private void Trg_MouseLeave(object sender, MouseEventArgs e)
        {
            if(e.LeftButton== MouseButtonState.Pressed)
            {
                IBProjectElement selectedItem = IBProjectViewCP.GetSelectedElement();
                if (selectedItem == null) return;
                DragDrop.DoDragDrop(trg, selectedItem, DragDropEffects.Move);
            }

            EndDrag();
        }

        private void Trg_Drop(object sender, DragEventArgs e)
        {
            EndDrag();

            e.Handled = true;

            string[] formats = e.Data.GetFormats();
            if (formats.Length == 0) return;

            IBProjectElement from = e.Data.GetData(formats[0]) as IBProjectElement;
            if (from == null) return;

            if (trgElement == null || from == trgElement) return;

            int trgIndex, fromIndex;
            IBProjectElement preParent = from.Parent;

            fromIndex = from.Parent.Children.IndexOf(from);
            from.Parent.RemoveChild(from);

            if (trgElement.Type == IBProjectElementTypes.Folder)
            {
                IBProjectElement parent = trgElement;
                trgElement.AddChild(from);

                from.IsSelected = true;
            }
            else
            {
                trgIndex = trgElement.Parent.Children.IndexOf(trgElement);

                IBProjectElement parent = trgElement.Parent;
                from.Parent = parent;

                if (trgIndex >= fromIndex)
                    trgElement.Parent.Children.Insert(trgElement.Parent.Children.IndexOf(trgElement) + 1, from);
                else
                    trgElement.Parent.Children.Insert(trgElement.Parent.Children.IndexOf(trgElement), from);

                from.IsSelected = true;
            }

            RedoUndoManager.Current.Record(new RUMoveProjectElement(from, preParent, fromIndex));
        }

        private void Trg_DragLeave(object sender, DragEventArgs e)
        {
            EndDrag();
        }

        private void Trg_DragEnter(object sender, DragEventArgs e)
        {
            string[] formats = e.Data.GetFormats();
            if (formats.Length == 0) return;

            from_temp = e.Data.GetData(formats[0]) as IBProjectElement;
            if (from_temp != null)
            {
                trg.IsSelected = true;
                trgElement = IBProjectViewCP.GetSelectedElement();
                if (trgElement == null) return;

                trg.BorderBrush = Application.Current.FindResource("IBFocusBorderBrush") as SolidColorBrush;
            }
        }

        private void Trg_DragOver(object sender, DragEventArgs e)
        {
            if (trg != null && from_temp != null)
            {
                if(trgElement.Type == IBProjectElementTypes.Folder)
                {
                    trg.BorderBrush = Application.Current.FindResource("IBFocusBorderBrush") as SolidColorBrush;

                    if (e.GetPosition(trg).Y < HEIGHT / 4)
                    {
                        trg.BorderThickness = new Thickness(0, 3, 0, 0);
                    }
                    else if (e.GetPosition(trg).Y > HEIGHT * 3 / 4 && e.GetPosition(trg).Y < HEIGHT)
                    {
                        trg.BorderThickness = new Thickness(0, 0, 0, 3);
                    }
                    else
                    {
                        trg.BorderBrush = Application.Current.FindResource("IBSelectedBrush") as SolidColorBrush;
                        trg.BorderThickness = new Thickness(2);
                    }
                }
                else
                {
                    if (e.GetPosition(trg).Y < HEIGHT / 2)
                    {
                        trg.BorderThickness = new Thickness(0, 3, 0, 0);
                    }
                    else
                    {
                        trg.BorderThickness = new Thickness(0, 0, 0, 3);
                    }
                }
            }
        }

        private void EndDrag()
        {
            trg.BorderThickness = new Thickness(0);
            trg.BorderBrush = null;
            from_temp = null;
        }

    }
}
