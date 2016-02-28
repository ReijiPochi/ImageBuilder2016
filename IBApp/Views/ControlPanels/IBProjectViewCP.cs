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
using System.Globalization;

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
            IBProject.Current.ElementsRoot.ClearAllSelections();

            if (tv.SelectedItem != null)
                ((IBProjectElement)tv.SelectedItem).IsSelected = false;
        }

        private void Tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedElement = e.NewValue as IBProjectElement;
        }

        private void Tv_Drop(object sender, DragEventArgs e)
        {
            if (IBProjectElement.MultiSelectingList.Count == 0)
            {
                string[] formats = e.Data.GetFormats();
                if (formats.Length == 0) return;

                IBProjectElement from = e.Data.GetData(formats[0]) as IBProjectElement;
                if (from == null) return;

                RedoUndoManager.Current.Record(DoMove(from));
            }
            else
            {
                RUMoveMultiProjectElements hists = new RUMoveMultiProjectElements();

                foreach (IBProjectElement from in IBProjectElement.MultiSelectingList)
                {
                    hists.RecordToThis(DoMove(from));
                }

                RedoUndoManager.Current.Record(hists);
            }
        }

        private static RUMoveProjectElement DoMove(IBProjectElement from)
        {
            IBProjectElement preParent = from.Parent;
            int preIndex;

            preIndex = preParent.Children.IndexOf(from);

            from.Parent.RemoveChild(from);
            IBProject.Current.ElementsRoot.AddChild(from);

            return new RUMoveProjectElement(from, preParent, preIndex);
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


        public static bool GetAttachmentIsMultiSelecting(DependencyObject obj)
        {
            return (bool)obj.GetValue(AttachmentIsMultiSelectingProperty);
        }

        public static void SetAttachmentIsMultiSelecting(DependencyObject obj, bool value)
        {
            obj.SetValue(AttachmentIsMultiSelectingProperty, value);
        }
        public static readonly DependencyProperty AttachmentIsMultiSelectingProperty =
            DependencyProperty.RegisterAttached("AttachmentIsMultiSelecting", typeof(bool), typeof(IBProjectViewCP), new PropertyMetadata(false));
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

    public class RUMoveMultiProjectElements : RedoUndoAction
    {
        public RUMoveMultiProjectElements()
        {

        }
        
        public void RecordToThis(RUMoveProjectElement hist)
        {
            currentHistory.Add(hist);
        }

        private List<RUMoveProjectElement> currentHistory = new List<RUMoveProjectElement>();

        public override void Redo()
        {
            base.Redo();

            foreach (RUMoveProjectElement hist in currentHistory)
            {
                hist.Redo();
            }
        }

        public override void Undo()
        {
            base.Undo();

            foreach(RUMoveProjectElement hist in currentHistory.Reverse<RUMoveProjectElement>())
            {
                hist.Undo();
            }
        }
    }

    public class IBProjectViewItemsBehavier : Behavior<StackPanel>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            trg = IBProjectViewCP.FindTreeViewItem(AssociatedObject as FrameworkElement);
            trgElement = AssociatedObject.DataContext as IBProjectElement;

            if (trg != null)
            {
                AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
                AssociatedObject.PreviewMouseUp += AssociatedObject_PreviewMouseUp;
                trg.MouseRightButtonDown += AssociatedObject_MouseRightButtonDown;
                trg.AllowDrop = true;
                AssociatedObject.MouseEnter += AssociatedObject_MouseEnter;
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
                AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
                AssociatedObject.PreviewMouseUp -= AssociatedObject_PreviewMouseUp;
                trg.MouseRightButtonDown -= AssociatedObject_MouseRightButtonDown;
                AssociatedObject.MouseEnter -= AssociatedObject_MouseEnter;
                trg.MouseLeave -= Trg_MouseLeave;
                trg.DragLeave -= Trg_DragLeave;
                trg.DragEnter -= Trg_DragEnter;
                trg.DragOver -= Trg_DragOver;
                trg.Drop -= Trg_Drop;
            }
        }

        private enum DropTo
        {
            None,
            Child,
            Top,
            Bottom
        }

        bool isMousePressed;
        TreeViewItem trg;
        const double HEIGHT = 22;
        IBProjectElement trgElement;
        IBProjectElement from_temp;
        DropTo fromDropTo;

        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMousePressed = true;

            if (trgElement != null && !trgElement.IsMultiSelecting)
            {
                IBProject.Current.ElementsRoot.ClearAllSelections();
            }
        }

        private void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            isMousePressed = false;
        }

        private void AssociatedObject_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (trg != null)
            {
                IBProject.Current.ElementsRoot.ClearAllSelections();
                trg.IsSelected = true;
            }

            e.Handled = true;
        }

        private void AssociatedObject_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!isMousePressed && e.LeftButton == MouseButtonState.Pressed)
            {
                if (trgElement != null)
                    trgElement.IsMultiSelecting = true;
            }
        }

        private void Trg_MouseLeave(object sender, MouseEventArgs e)
        {
            if(isMousePressed)
            {
                if (trgElement == null) return;
                DragDrop.DoDragDrop(trg, trgElement, DragDropEffects.Move);
            }

            isMousePressed = false;
            EndDrag();
        }

        private void Trg_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (fromDropTo == DropTo.None)
            {
                EndDrag();
                return;
            }

            if (IBProjectElement.MultiSelectingList.Count == 0)
            {
                string[] formats = e.Data.GetFormats();
                if (formats.Length == 0) return;

                IBProjectElement from = e.Data.GetData(formats[0]) as IBProjectElement;
                if (from == null) return;

                if (trgElement == null || from == trgElement) return;

                RUMoveProjectElement hist = DoMove(from);
                RedoUndoManager.Current.Record(hist);
            }
            else
            {
                RUMoveMultiProjectElements hists = new RUMoveMultiProjectElements();

                if(fromDropTo == DropTo.Bottom)
                {
                    foreach (IBProjectElement from in IBProjectElement.MultiSelectingList.Reverse())
                    {
                        hists.RecordToThis(DoMove(from));
                    }
                }
                else
                {
                    foreach (IBProjectElement from in IBProjectElement.MultiSelectingList)
                    {
                        hists.RecordToThis(DoMove(from));
                    }
                }

                RedoUndoManager.Current.Record(hists);
            }

            EndDrag();
        }

        private RUMoveProjectElement DoMove(IBProjectElement from)
        {
            int trgIndex, fromIndex;
            IBProjectElement preParent = from.Parent;

            fromIndex = from.Parent.Children.IndexOf(from);
            from.Parent.RemoveChild(from);

            switch (fromDropTo)
            {
                case DropTo.Child:
                    trgElement.AddChild(from);
                    break;

                case DropTo.Top:
                    trgIndex = trgElement.Parent.Children.IndexOf(trgElement);
                    from.Parent = trgElement.Parent;
                    trgElement.Parent.Children.Insert(trgElement.Parent.Children.IndexOf(trgElement), from);
                    break;

                case DropTo.Bottom:
                    trgIndex = trgElement.Parent.Children.IndexOf(trgElement);
                    from.Parent = trgElement.Parent;
                    trgElement.Parent.Children.Insert(trgElement.Parent.Children.IndexOf(trgElement) + 1, from);
                    break;

                default:
                    break;
            }

            from.IsSelected = true;

            return new RUMoveProjectElement(from, preParent, fromIndex);
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
                    if (e.GetPosition(trg).Y < HEIGHT / 4)
                    {
                        trg.BorderThickness = new Thickness(0, 3, 0, 0);
                        fromDropTo = DropTo.Top;
                    }
                    else if (e.GetPosition(trg).Y > HEIGHT * 3 / 4 && e.GetPosition(trg).Y < HEIGHT)
                    {
                        trg.BorderThickness = new Thickness(0, 0, 0, 3);
                        fromDropTo = DropTo.Bottom;
                    }
                    else
                    {
                        trg.BorderThickness = new Thickness(2);
                        fromDropTo = DropTo.Child;
                    }
                }
                else
                {
                    if (e.GetPosition(trg).Y < HEIGHT / 2)
                    {
                        trg.BorderThickness = new Thickness(0, 3, 0, 0);
                        fromDropTo = DropTo.Top;
                    }
                    else
                    {
                        trg.BorderThickness = new Thickness(0, 0, 0, 3);
                        fromDropTo = DropTo.Bottom;
                    }
                }
            }
        }

        private void EndDrag()
        {
            trg.BorderThickness = new Thickness(0);
            trg.BorderBrush = null;
            from_temp = null;
            fromDropTo = DropTo.None;
        }

    }

    public class TypeToIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((IBProjectElementTypes)value)
            {
                case IBProjectElementTypes.Folder:
                    return "/IBFramework;component/ImageResources/Folder.png";

                case IBProjectElementTypes.CellSource:
                    return "/IBFramework;component/ImageResources/CellSource.png";

                default:
                    return "/IBFramework;component/ImageResources/Folder.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
