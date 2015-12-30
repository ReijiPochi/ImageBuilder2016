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
using System.Windows;

namespace IBApp.ViewModels
{
    public class TestVM : ViewModel
    {

        #region PenTabletPos変更通知プロパティ
        private Point _PenTabletPos;

        public Point PenTabletPos
        {
            get
            { return _PenTabletPos; }
            set
            { 
                if (_PenTabletPos == value)
                    return;
                _PenTabletPos = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region GetPenTabletValuesCommand
        private ViewModelCommand _GetPenTabletValuesCommand;

        public ViewModelCommand GetPenTabletValuesCommand
        {
            get
            {
                if (_GetPenTabletValuesCommand == null)
                {
                    _GetPenTabletValuesCommand = new ViewModelCommand(GetPenTabletValues);
                }
                return _GetPenTabletValuesCommand;
            }
        }

        public void GetPenTabletValues()
        {
            PenTabletPos = Wintab.Wintab.Position;
        }
        #endregion


    }
}
