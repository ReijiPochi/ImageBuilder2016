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
                if(IBProjectModel.Current.ActiveShowingItem as CellSource != null)
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
            }
        }
        #endregion

    }
}
