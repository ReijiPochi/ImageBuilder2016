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
using IBFramework.IBProject;

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



        #region AddCellCommand
        private ViewModelCommand _AddCellCommand;

        public ViewModelCommand AddCellCommand
        {
            get
            {
                if (_AddCellCommand == null)
                {
                    _AddCellCommand = new ViewModelCommand(AddCell, CanAddCell);
                }
                return _AddCellCommand;
            }
        }

        public bool CanAddCell()
        {
            return true;
        }

        public void AddCell()
        {

        }
        #endregion

    }
}
