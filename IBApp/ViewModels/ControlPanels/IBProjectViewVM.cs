using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using IBApp.Models;
using IBFramework.Project;
using IBFramework.Timeline.TimelineElements;

namespace IBApp.ViewModels.ControlPanels
{
    public class IBProjectViewVM : ViewModel
    {
        public IBProjectViewVM()
        {
            if (IBProjectModel.Current == null) return;
            CurrentIBProject = IBProject.Current;
            IBProjectModel.Current.PropertyChanged += IBProjectModelCurrent_PropertyChanged;
        }

        private void IBProjectModelCurrent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ActiveTargetElement":
                    SelectedIBProjectElement = IBProjectModel.Current.ActiveTargetElement;
                    break;

                default:
                    break;
            }
        }


        #region CurrentIBProject変更通知プロパティ
        private IBProject _CurrentIBProject;

        public IBProject CurrentIBProject
        {
            get
            { return _CurrentIBProject; }
            set
            { 
                if (_CurrentIBProject == value)
                    return;
                _CurrentIBProject = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SelectedIBProjectElement変更通知プロパティ
        private IBProjectElement _SelectedIBProjectElement;

        public IBProjectElement SelectedIBProjectElement
        {
            get
            { return _SelectedIBProjectElement; }
            set
            { 
                if (_SelectedIBProjectElement == value)
                    return;
                _SelectedIBProjectElement = value;

                IBProjectModel.Current.ActiveTargetElement = value;

                RaisePropertyChanged();

                AddNewFolderCommand.RaiseCanExecuteChanged();
                AddNewCellSourceCommand.RaiseCanExecuteChanged();
                ShowOnCanvasCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region AddNewFolderCommand
        private ViewModelCommand _AddNewFolderCommand;

        public ViewModelCommand AddNewFolderCommand
        {
            get
            {
                if (_AddNewFolderCommand == null)
                {
                    _AddNewFolderCommand = new ViewModelCommand(AddNewFolder, CanAddNewFolder);
                }
                return _AddNewFolderCommand;
            }
        }

        public bool CanAddNewFolder()
        {
            if (SelectedIBProjectElement == null) return true;

            if (SelectedIBProjectElement.Type == IBProjectElementTypes.Folder
                || SelectedIBProjectElement.Type == IBProjectElementTypes.Null)
                return true;
            else
                return false;
        }

        public void AddNewFolder()
        {
            IBProjectModel.Current.AddNewFolder();
        }
        #endregion

        #region AddNewCellSourceCommand
        private ViewModelCommand _AddNewCellSourceCommand;

        public ViewModelCommand AddNewCellSourceCommand
        {
            get
            {
                if (_AddNewCellSourceCommand == null)
                {
                    _AddNewCellSourceCommand = new ViewModelCommand(AddNewCellSource, CanAddNewCell);
                }
                return _AddNewCellSourceCommand;
            }
        }

        public bool CanAddNewCell()
        {
            if (SelectedIBProjectElement == null) return true;

            if (SelectedIBProjectElement.Type == IBProjectElementTypes.Folder
                || SelectedIBProjectElement.Type == IBProjectElementTypes.Null
                || SelectedIBProjectElement.Type == IBProjectElementTypes.Cell)
                return true;
            else
                return false;
        }

        public void AddNewCellSource()
        {
            IBProjectModel.Current.AddNewCellSource();
        }
        #endregion

        #region ShowPropertiesCommand
        private ViewModelCommand _ShowPropertiesCommand;

        public ViewModelCommand ShowPropertiesCommand
        {
            get
            {
                if (_ShowPropertiesCommand == null)
                {
                    _ShowPropertiesCommand = new ViewModelCommand(ShowProperties);
                }
                return _ShowPropertiesCommand;
            }
        }

        public void ShowProperties()
        {
            IBProjectModel.Current.SelectedPropertyItem = CurrentIBProject;
        }
        #endregion

        #region ShowOnCanvasCommand
        private ViewModelCommand _ShowOnCanvasCommand;

        public ViewModelCommand ShowOnCanvasCommand
        {
            get
            {
                if (_ShowOnCanvasCommand == null)
                {
                    _ShowOnCanvasCommand = new ViewModelCommand(ShowOnCanvas, CanShowOnCanvas);
                }
                return _ShowOnCanvasCommand;
            }
        }

        public bool CanShowOnCanvas()
        {
            if (SelectedIBProjectElement == null) return false;

            if (SelectedIBProjectElement.Type != IBProjectElementTypes.Folder)
                return true;

            return false;
        }

        public void ShowOnCanvas()
        {
            IBProjectModel.Current.ActiveCanvasItems.Add(SelectedIBProjectElement);
        }
        #endregion


    }
}
