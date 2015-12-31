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

            CurrentIBProjectName = IBProjectModel.Current.IBProject_Name;
            IBProjectModel.Current.PropertyChanged += IBProjectModelCurrent_PropertyChanged;
        }

        private void IBProjectModelCurrent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IBProject_Name":
                    CurrentIBProjectName = IBProjectModel.Current.IBProject_Name;
                    break;

                case "IBProject_Elements":
                    CurrentIBProjectElements = IBProjectModel.Current.IBProject_Elements;
                    RaisePropertyChanged("CurrentIBProjectElements");
                    break;

                default:
                    break;
            }
        }


        #region CurrentIBProjectName変更通知プロパティ
        private string _CurrentIBProjectName;

        public string CurrentIBProjectName
        {
            get
            { return _CurrentIBProjectName; }
            set
            { 
                if (_CurrentIBProjectName == value)
                    return;
                _CurrentIBProjectName = value;

                IBProjectModel.Current.IBProject_Name = value;

                RaisePropertyChanged();
            }
        }
        #endregion

        #region CurrentIBProjectElements変更通知プロパティ
        private ObservableCollection<IBProjectElement> _CurrentIBProjectElements;

        public ObservableCollection<IBProjectElement> CurrentIBProjectElements
        {
            get
            { return _CurrentIBProjectElements; }
            set
            {
                if (_CurrentIBProjectElements == value)
                    return;
                _CurrentIBProjectElements = value;
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
                RaisePropertyChanged();
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
            return true;
        }

        public void AddNewFolder()
        {
            IBProjectModel.Current.AddNewFolder(SelectedIBProjectElement);
        }
        #endregion

        #region AddNewCellCommand
        private ViewModelCommand _AddNewCellCommand;

        public ViewModelCommand AddNewCellCommand
        {
            get
            {
                if (_AddNewCellCommand == null)
                {
                    _AddNewCellCommand = new ViewModelCommand(AddNewCell, CanAddNewCell);
                }
                return _AddNewCellCommand;
            }
        }

        public bool CanAddNewCell()
        {
            return true;
        }

        public void AddNewCell()
        {
            IBProjectModel.Current.AddNewCell(SelectedIBProjectElement);
        }
        #endregion


    }
}
