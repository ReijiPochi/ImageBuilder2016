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

                    case "SelectionTool":
                        SelectionToolON = true;
                        break;

                    case "Deformer":
                        DeformerON = true;
                        break;

                    case "Pencil":
                        PencilON = true;
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
                _DeformerON = false;
                StateChange();

                IBBrushModel.SetToProjectPencil();
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
                _DeformerON = false;
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
                _DeformerON = false;
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
                _DeformerON = false;
                StateChange();

                IBBrushModel.SetToProjectSelectionTool();
            }
        }
        #endregion

        #region DeformerON変更通知プロパティ
        private bool _DeformerON;

        public bool DeformerON
        {
            get
            { return _DeformerON; }
            set
            {
                _EraserON = false;
                _PencilON = false;
                _PenON = false;
                _SelectionToolON = false;
                _DeformerON = true;
                StateChange();

                IBBrushModel.SetToProjectDeformer();
            }
        }
        #endregion


        private void StateChange()
        {
            RaisePropertyChanged("PencilON");
            RaisePropertyChanged("PenON");
            RaisePropertyChanged("EraserON");
            RaisePropertyChanged("SelectionToolON");
            RaisePropertyChanged("DeformerON");
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

        #region OnSelectionToolCommand
        private ViewModelCommand _OnSelectionToolCommand;

        public ViewModelCommand OnSelectionToolCommand
        {
            get
            {
                if (_OnSelectionToolCommand == null)
                {
                    _OnSelectionToolCommand = new ViewModelCommand(OnSelectionTool);
                }
                return _OnSelectionToolCommand;
            }
        }

        public void OnSelectionTool()
        {
            SelectionToolON = true;
        }
        #endregion

        #region OnDeformerCommand
        private ViewModelCommand _OnDeformerCommand;

        public ViewModelCommand OnDeformerCommand
        {
            get
            {
                if (_OnDeformerCommand == null)
                {
                    _OnDeformerCommand = new ViewModelCommand(OnDeformer);
                }
                return _OnDeformerCommand;
            }
        }

        public void OnDeformer()
        {
            DeformerON = true;
        }
        #endregion

        #region OnPencilCommand
        private ViewModelCommand _OnPencilCommand;

        public ViewModelCommand OnPencilCommand
        {
            get
            {
                if (_OnPencilCommand == null)
                {
                    _OnPencilCommand = new ViewModelCommand(OnPencil);
                }
                return _OnPencilCommand;
            }
        }

        public void OnPencil()
        {
            PencilON = true;
        }
        #endregion

    }
}
