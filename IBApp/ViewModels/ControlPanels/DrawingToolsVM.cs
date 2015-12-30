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
    public class DrawingToolsVM : ViewModel
    {
        #region PencilON変更通知プロパティ
        private bool _PencilON;

        public bool PencilON
        {
            get
            { return _PencilON; }
            set
            { 
                _PencilON = true;
                _PenON = false;
                _EraserON = false;
                RaisePropertyChanged("PencilON");
                RaisePropertyChanged("PenON");
                RaisePropertyChanged("EraserON");
            }
        }
        #endregion

        #region PenON変更通知プロパティ
        private bool _PenON;

        public bool PenON
        {
            get
            { return _PenON; }
            set
            { 
                _PenON = true;
                _PencilON = false;
                _EraserON = false;
                RaisePropertyChanged("PencilON");
                RaisePropertyChanged("PenON");
                RaisePropertyChanged("EraserON");
            }
        }
        #endregion

        #region EraserON変更通知プロパティ
        private bool _EraserON;

        public bool EraserON
        {
            get
            { return _EraserON; }
            set
            { 
                _EraserON = true;
                _PencilON = false;
                _PenON = false;
                RaisePropertyChanged("PencilON");
                RaisePropertyChanged("PenON");
                RaisePropertyChanged("EraserON");

                //IBAppModel.Current.SelectedPropertyItem = Eraser;
            }
        }
        #endregion


    }
}
