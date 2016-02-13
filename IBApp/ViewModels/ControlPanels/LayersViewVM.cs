using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using IBApp.Models;
using IBFramework.Project.IBProjectElements;
using IBFramework.Image;

namespace IBApp.ViewModels.ControlPanels
{
    public class LayersViewVM : ViewModel
    {
        public LayersViewVM()
        {
            if (IBProjectModel.Current == null) return;

            CurrentCell = IBProjectModel.Current.ActiveShowingItem as CellSource;
            IBProjectModel.Current.PropertyChanged += IBProjectModel_PropertyChanged;
        }

        private void IBProjectModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "ActiveShowingItem")
            {
                if (IBProjectModel.Current.ActiveShowingItem == null)
                    CurrentCell = null;

                if (IBProjectModel.Current.ActiveShowingItem as CellSource != null)
                    CurrentCell = IBProjectModel.Current.ActiveShowingItem as CellSource;
            }
        }

        #region CurrentCell変更通知プロパティ
        private CellSource _CurrentCell;

        public CellSource CurrentCell
        {
            get
            { return _CurrentCell; }
            set
            { 
                if (_CurrentCell == value)
                    return;
                _CurrentCell = value;
                RaisePropertyChanged();
                AddSingleColorImageCommand.RaiseCanExecuteChanged();
                AddLineDrawingImageCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region SetTargetLayerCommand
        private ViewModelCommand _SetTargetLayerCommand;

        public ViewModelCommand SetTargetLayerCommand
        {
            get
            {
                if (_SetTargetLayerCommand == null)
                {
                    _SetTargetLayerCommand = new ViewModelCommand(SetTargetLayer);
                }
                return _SetTargetLayerCommand;
            }
        }

        public void SetTargetLayer()
        {
            if (CurrentCell == null) return;

            foreach(IBImage i in CurrentCell.Layers)
            {
                if (i.IsSelectedLayer)
                {
                    IBProjectModel.Current.ActiveTargetLayer = i;
                }
            }
        }
        #endregion

        #region AddSingleColorImageCommand
        private ViewModelCommand _AddSingleColorImageCommand;

        public ViewModelCommand AddSingleColorImageCommand
        {
            get
            {
                if (_AddSingleColorImageCommand == null)
                {
                    _AddSingleColorImageCommand = new ViewModelCommand(AddSingleColorImage, CanAddSingleColorImage);
                }
                return _AddSingleColorImageCommand;
            }
        }

        public bool CanAddSingleColorImage()
        {
            if (CurrentCell != null) return true;
            else return false;
        }

        public void AddSingleColorImage()
        {
            CurrentCell.AddNewLayer("SingleColor", true, ImageTypes.SingleColor);
        }
        #endregion

        #region AddLineDrawingImageCommand
        private ViewModelCommand _AddLineDrawingImageCommand;

        public ViewModelCommand AddLineDrawingImageCommand
        {
            get
            {
                if (_AddLineDrawingImageCommand == null)
                {
                    _AddLineDrawingImageCommand = new ViewModelCommand(AddLineDrawingImage, CanAddLineDrawingImage);
                }
                return _AddLineDrawingImageCommand;
            }
        }

        public bool CanAddLineDrawingImage()
        {
            if (CurrentCell != null) return true;
            else return false;
        }

        public void AddLineDrawingImage()
        {
            CurrentCell.AddNewLayer("Layer", true, ImageTypes.LineDrawing);
        }
        #endregion

    }
}
