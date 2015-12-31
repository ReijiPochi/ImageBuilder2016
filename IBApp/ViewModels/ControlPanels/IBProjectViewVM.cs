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


        #region AddNewFolderCommand
        private ListenerCommand<IBProjectElement> _AddNewFolderCommand;

        public ListenerCommand<IBProjectElement> AddNewFolderCommand
        {
            get
            {
                if (_AddNewFolderCommand == null)
                {
                    _AddNewFolderCommand = new ListenerCommand<IBProjectElement>(AddNewFolder, CanAddNewFolder);
                }
                return _AddNewFolderCommand;
            }
        }

        public bool CanAddNewFolder()
        {
            return true;
        }

        public void AddNewFolder(IBProjectElement parameter)
        {
            IBProjectModel.Current.AddNewFolder(parameter);
        }
        #endregion

        #region AddNewCellCommand
        private ListenerCommand<IBProjectElement> _AddNewCellCommand;

        public ListenerCommand<IBProjectElement> AddNewCellCommand
        {
            get
            {
                if (_AddNewCellCommand == null)
                {
                    _AddNewCellCommand = new ListenerCommand<IBProjectElement>(AddNewCell, CanAddNewCell);
                }
                return _AddNewCellCommand;
            }
        }

        public bool CanAddNewCell()
        {
            return true;
        }

        public void AddNewCell(IBProjectElement parameter)
        {
            IBProjectModel.Current.AddNewCell(parameter);
        }
        #endregion

    }
}
