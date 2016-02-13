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
using System.Windows.Interactivity;

using IBGUI;
using Livet;
using IBFramework.Image;
using IBFramework.Project.IBProjectElements;
using IBFramework.IBCanvas;
using IBFramework.RedoUndo;

namespace IBApp.Views.ControlPanels
{
    public class LayersViewCP : IBControlPanelBase
    {
        static LayersViewCP()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayersViewCP), new FrameworkPropertyMetadata(typeof(LayersViewCP)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            view = GetTemplateChild("View") as ListView;
            view.SelectionChanged += View_SelectionChanged;
        }

        ListView view;

        private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SetTargetLayerCommand != null)
            {
                if (SetTargetLayerCommand.CanExecute(null))
                {
                    SetTargetLayerCommand.Execute(null);
                }
            }
        }



        public ICommand SetTargetLayerCommand
        {
            get { return (ICommand)GetValue(SetTargetLayerCommandProperty); }
            set { SetValue(SetTargetLayerCommandProperty, value); }
        }
        public static readonly DependencyProperty SetTargetLayerCommandProperty =
            DependencyProperty.Register("SetTargetLayerCommand", typeof(ICommand), typeof(LayersViewCP), new PropertyMetadata(null));
    }

    public class LayerBehavior : Behavior<Border>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
            AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                IBImage trg = AssociatedObject.DataContext as IBImage;
                if (trg == null || trg.owner == null) return;

                CellSource trgOwner = trg.owner as CellSource;
                if (trgOwner == null) return;

                LayerDragData data = new LayerDragData()
                {
                    from = trg,
                    fromOwner = trgOwner,
                    fromIndex = trgOwner.Layers.IndexOf(trg)
                };

                DragDrop.DoDragDrop(AssociatedObject, data, DragDropEffects.Move);
            }
        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            LayerDragData data = e.Data.GetData(typeof(LayerDragData)) as LayerDragData;

            if (data != null)
            {
                IBImage trg = AssociatedObject.DataContext as IBImage;
                if (trg == null || trg == data.from || trg.owner == null) return;

                CellSource trgOwner = trg.owner as CellSource;
                if (trgOwner == null || trgOwner != data.fromOwner) return;

                int trgIndex = trgOwner.Layers.IndexOf(trg);

                trgOwner.Layers.Remove(data.from);
                trgOwner.Layers.Insert(trgIndex, data.from);

                RedoUndoManager.Current.Record(new RUSortLayer(data.from, data.fromIndex, data.fromOwner, trg, trgIndex));

                IBCanvasControl.RefreshAll();
            }
        }
    }

    class LayerDragData
    {
        public IBImage from;
        public int fromIndex;
        public CellSource fromOwner;
    }

    class RUSortLayer : RedoUndoAction
    {
        public RUSortLayer(IBImage from, int fromIndex, CellSource owner, IBImage to, int toIndex)
        {
            From = from;
            FromIndex = fromIndex;

            Owner = owner;

            To = to;
            ToIndex = toIndex;
        }

        IBImage From, To;
        int FromIndex, ToIndex;
        CellSource Owner;

        public override void Undo()
        {
            base.Undo();

            Owner.Layers.Remove(To);
            Owner.Layers.Insert(ToIndex, To);

            IBCanvasControl.RefreshAll();
        }

        public override void Redo()
        {
            base.Redo();

            Owner.Layers.Remove(From);
            Owner.Layers.Insert(ToIndex, From);

            IBCanvasControl.RefreshAll();
        }
    }
}
