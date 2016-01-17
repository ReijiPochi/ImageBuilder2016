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
            if (preParent != null)
                preIndex = preParent.Children.IndexOf(from);
            else
                preIndex = IBProject.Current.IBProjectElements.IndexOf(from);

            if (from.Parent != null)
            {
                from.Parent.Children.Remove(from);
            }
            else
            {
                IBProject.Current.IBProjectElements.Remove(from);
            }

            IBProject.Current.IBProjectElements.Add(from);
            from.Parent = null;

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
            if(newParent != null)
            {
                newIndex = newParent.Children.IndexOf(trg);
            }
            else
            {
                newIndex = IBProject.Current.IBProjectElements.IndexOf(trg);
            }
        }

        private IBProjectElement trg;

        private int preIndex;
        private IBProjectElement preParent;

        private int newIndex;
        private IBProjectElement newParent;

        public override void Redo()
        {
            base.Redo();

            if (preParent != null)
            {
                preParent.Children.Remove(trg);
            }
            else
            {
                IBProject.Current.IBProjectElements.Remove(trg);
            }

            if (newParent != null)
            {
                newParent.Children.Insert(newIndex, trg);
                trg.Parent = newParent;
            }
            else
            {
                IBProject.Current.IBProjectElements.Insert(newIndex, trg);
                trg.Parent = null;
            }
        }

        public override void Undo()
        {
            base.Undo();

            if (newParent != null)
            {
                newParent.Children.Remove(trg);
            }
            else
            {
                IBProject.Current.IBProjectElements.Remove(trg);
            }

            if (preParent != null)
            {
                preParent.Children.Insert(preIndex, trg);
                trg.Parent = preParent;
            }
            else
            {
                IBProject.Current.IBProjectElements.Insert(preIndex, trg);
                trg.Parent = null;
            }
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
        IBProjectElement trgElement;

        private void AssociatedObject_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (trg != null)
                trg.IsSelected = true;
        }

        private void Trg_MouseLeave(object sender, MouseEventArgs e)
        {
            if(e.LeftButton== MouseButtonState.Pressed)
            {
                IBProjectElement selectedItem = IBProjectViewCP.GetSelectedElement();
                if (selectedItem == null) return;
                DragDrop.DoDragDrop(trg, selectedItem, DragDropEffects.Move);
            }

            trg.BorderThickness = new Thickness(0);
            trg.BorderBrush = null;
        }

        private void Trg_Drop(object sender, DragEventArgs e)
        {
            trg.BorderThickness = new Thickness(0);
            trg.BorderBrush = null;

            e.Handled = true;

            string[] formats = e.Data.GetFormats();
            if (formats.Length == 0) return;

            IBProjectElement from = e.Data.GetData(formats[0]) as IBProjectElement;
            if (from == null) return;

            if (from == trgElement) return;

            int trgIndex, fromIndex;
            IBProjectElement preParent = from.Parent;

            if (from.Parent != null)
            {
                fromIndex = from.Parent.Children.IndexOf(from);
                from.Parent.Children.Remove(from);
            }
            else
            {
                fromIndex = IBProject.Current.IBProjectElements.IndexOf(from);
                IBProject.Current.IBProjectElements.Remove(from);
            }

            if (trgElement.Type == IBProjectElementTypes.Folder)
            {
                IBProjectElement parent = trgElement;
                from.Parent = parent;
                trgElement.Children.Add(from);

                from.IsSelected = true;
            }
            else
            {
                if (trgElement.Parent != null)
                {
                    trgIndex = trgElement.Parent.Children.IndexOf(trgElement);

                    IBProjectElement parent = trgElement.Parent;
                    from.Parent = parent;

                    if (trgIndex >= fromIndex)
                        trgElement.Parent.Children.Insert(trgElement.Parent.Children.IndexOf(trgElement) + 1, from);
                    else
                        trgElement.Parent.Children.Insert(trgElement.Parent.Children.IndexOf(trgElement), from);
                }
                else
                {
                    trgIndex = IBProject.Current.IBProjectElements.IndexOf(trgElement);

                    from.Parent = null;

                    if (trgIndex >= fromIndex)
                        IBProject.Current.IBProjectElements.Insert(IBProject.Current.IBProjectElements.IndexOf(trgElement) + 1, from);
                    else
                        IBProject.Current.IBProjectElements.Insert(IBProject.Current.IBProjectElements.IndexOf(trgElement), from);
                }

                from.IsSelected = true;
            }

            RedoUndoManager.Current.Record(new RUMoveProjectElement(from, preParent, fromIndex));
        }

        private void Trg_DragLeave(object sender, DragEventArgs e)
        {
            trg.BorderThickness = new Thickness(0);
            trg.BorderBrush = null;
        }

        private void Trg_DragEnter(object sender, DragEventArgs e)
        {
            string[] formats = e.Data.GetFormats();
            if (formats.Length == 0) return;

            IBProjectElement from = e.Data.GetData(formats[0]) as IBProjectElement;

            if (from != null)
            {
                trg.IsSelected = true;
                trgElement = IBProjectViewCP.GetSelectedElement();
                if (trgElement == null) return;

                if(trgElement.Type == IBProjectElementTypes.Folder)
                {
                    trg.BorderThickness = new Thickness(2);
                    trg.BorderBrush = Application.Current.FindResource("IBFocusBorderBrush") as SolidColorBrush;
                }
                else
                {
                    trg.BorderThickness = new Thickness(1);
                    trg.BorderBrush = Application.Current.FindResource("IBFocusBrush2_T") as SolidColorBrush;
                }
            }
        }

        private void Trg_DragOver(object sender, DragEventArgs e)
        {
            
        }

    }
}
