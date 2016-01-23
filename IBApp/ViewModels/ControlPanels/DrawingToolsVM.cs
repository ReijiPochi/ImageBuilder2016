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
        public DrawingToolsVM()
        {
            if (IBProjectModel.Current == null) return;
            IBProjectModel.Current.PropertyChanged += IBProjectModel_PropertyChanged;
        }

        private void IBProjectModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedBrush")
            {
                switch (IBProjectModel.Current.SelectedBrush.GetType().Name)
                {
                    case "Pen":
                        PenON = true;
                        break;

                    case "Eraser":
                        EraserON = true;
                        break;

                    default:
                        break;
                }
            }
        }

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
                _SelectionToolON = false;
                StateChange();
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
                _SelectionToolON = false;
                StateChange();

                IBBrushModel.SetToProjectPen();
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
                _SelectionToolON = false;
                StateChange();

                IBBrushModel.SetToProjectEraser();
            }
        }
        #endregion

        #region SelectionToolON変更通知プロパティ
        private bool _SelectionToolON;

        public bool SelectionToolON
        {
            get
            { return _SelectionToolON; }
            set
            {
                _EraserON = false;
                _PencilON = false;
                _PenON = false;
                _SelectionToolON = true;
                StateChange();

                IBBrushModel.SetToProjectSelectionTool();
            }
        }
        #endregion


        private void StateChange()
        {
            RaisePropertyChanged("PencilON");
            RaisePropertyChanged("PenON");
            RaisePropertyChanged("EraserON");
            RaisePropertyChanged("SelectionToolON");
        }


        #region OnPenCommand
        private ViewModelCommand _OnPenCommand;

        public ViewModelCommand OnPenCommand
        {
            get
            {
                if (_OnPenCommand == null)
                {
                    _OnPenCommand = new ViewModelCommand(OnPen);
                }
                return _OnPenCommand;
            }
        }

        public void OnPen()
        {
            PenON = true;
        }
        #endregion

        #region OnEraserCommand
        private ViewModelCommand _OnEraserCommand;

        public ViewModelCommand OnEraserCommand
        {
            get
            {
                if (_OnEraserCommand == null)
                {
                    _OnEraserCommand = new ViewModelCommand(OnEraser);
                }
                return _OnEraserCommand;
            }
        }

        public void OnEraser()
        {
            EraserON = true;
        }
        #endregion

    }
}
