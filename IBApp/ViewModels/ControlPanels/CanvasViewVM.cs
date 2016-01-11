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

using System.Collections.ObjectModel;

using IBApp.Models;
using IBFramework.Image;
using IBFramework.Image.Blend;
using IBFramework.Timeline;
using IBFramework.Timeline.TimelineElements;
using IBFramework.Project;

namespace IBApp.ViewModels.ControlPanels
{
    public class CanvasViewVM : ViewModel
    {
        public CanvasViewVM()
        {
            if (IBProjectModel.Current == null) return;

            IBProjectModel.Current.ActiveCanvasItems = Items;
            ActiveBrush = IBProjectModel.Current.SelectedBrush;
            IBProjectModel.Current.PropertyChanged += IBProjectModel_PropertyChanged;
        }

        private void IBProjectModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedBrush")
                ActiveBrush = IBProjectModel.Current.SelectedBrush;
        }

        #region Itemsプロパティ
        private ObservableCollection<IBProjectElement> _Items = new ObservableCollection<IBProjectElement>();

        public ObservableCollection<IBProjectElement> Items
        {
            get
            { return _Items; }
            set
            { 
                if (_Items == value)
                    return;
                _Items = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ShowingItem変更通知プロパティ
        private IBProjectElement _ShowingItem;

        public IBProjectElement ShowingItem
        {
            get
            { return _ShowingItem; }
            set
            { 
                if (_ShowingItem == value)
                    return;
                _ShowingItem = value;

                IBProjectModel.Current.ActiveShowingItem = value;

                RaisePropertyChanged();
            }
        }
        #endregion

        #region ActiveBrush変更通知プロパティ
        private IBBrush _ActiveBrush;

        public IBBrush ActiveBrush
        {
            get
            { return _ActiveBrush; }
            set
            { 
                if (_ActiveBrush == value)
                    return;
                _ActiveBrush = value;
                RaisePropertyChanged();
            }
        }
        #endregion




        #region FocusCommand
        private ViewModelCommand _FocusCommand;

        public ViewModelCommand FocusCommand
        {
            get
            {
                if (_FocusCommand == null)
                {
                    _FocusCommand = new ViewModelCommand(Focus);
                }
                return _FocusCommand;
            }
        }

        public void Focus()
        {
            IBProjectModel.Current.ActiveCanvasItems = Items; 
        }
        #endregion


    }
}
