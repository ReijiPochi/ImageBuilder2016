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
using IBFramework;

namespace IBApp.ViewModels.ControlPanels
{
    public class PropertiesVM : ViewModel
    {
        public PropertiesVM()
        {
            CurrentPropertyItem = IBProjectModel.Current.SelectedPropertyItem;
            IBProjectModel.Current.PropertyChanged += IBAppModelCurrent_PropertyChanged;
        }

        private void IBAppModelCurrent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPropertyItem")
                CurrentPropertyItem = IBProjectModel.Current.SelectedPropertyItem;
        }


        #region Pin変更通知プロパティ
        private bool _Pin;

        public bool Pin
        {
            get
            { return _Pin; }
            set
            { 
                if (_Pin == value || _CurrentPropertyItem == null)
                    return;
                _Pin = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region CurrentPropertyItem変更通知プロパティ
        private IProperty _CurrentPropertyItem;

        public IProperty CurrentPropertyItem
        {
            get
            { return _CurrentPropertyItem; }
            set
            { 
                if (Pin || _CurrentPropertyItem == value)
                    return;
                _CurrentPropertyItem = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
