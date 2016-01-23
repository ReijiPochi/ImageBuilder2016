using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using IBApp.Models;
using IBFramework.Image;

namespace IBApp.ViewModels.ControlPanels
{
    public class BrushVM : ViewModel
    {
        public BrushVM()
        {
            if (IBProjectModel.Current == null) return;

            IBProjectModel.Current.PropertyChanged += Current_PropertyChanged;
        }

        private void Current_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedBrush")
            {
                CurrentBrush = IBProjectModel.Current.SelectedBrush;
                RaisePropertyChanged("CurrentBrushName");
                RaisePropertyChanged("CurrentBP");
            }
        }


        #region CurrentBrush変更通知プロパティ
        private IBBrush _CurrentBrush;

        public IBBrush CurrentBrush
        {
            get
            { return _CurrentBrush; }
            set
            { 
                if (_CurrentBrush == value)
                    return;
                _CurrentBrush = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region CurrentBrushNameプロパティ
        public string CurrentBrushName
        {
            get
            {
                if (CurrentBrush == null)
                    return null;
                return CurrentBrush.GetType().Name;
            }
            set { }
        }
        #endregion

        #region CurrentBPプロパティ
        public Control CurrentBP
        {
            get
            {
                if (CurrentBrush == null)
                    return null;
                return CurrentBrush.GetBP();
            }
            set { }
        }
        #endregion
    }
}
