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
using IBFramework.RedoUndo;

namespace IBApp.ViewModels.ControlPanels
{
    public class AppCommandVM : ViewModel
    {
        public AppCommandVM()
        {
            if (RedoUndoManager.Current != null)
            {
                RedoUndoManager.Current.PropertyChanged += Current_PropertyChanged;
            }
        }

        ~AppCommandVM()
        {
            if (RedoUndoManager.Current != null)
            {
                RedoUndoManager.Current.PropertyChanged -= Current_PropertyChanged;
            }
        }

        private void Current_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }

        #region UndoCommand
        private ViewModelCommand _UndoCommand;

        public ViewModelCommand UndoCommand
        {
            get
            {
                if (_UndoCommand == null)
                {
                    _UndoCommand = new ViewModelCommand(Undo, CanUndo);
                }
                return _UndoCommand;
            }
        }

        public bool CanUndo()
        {
            if (RedoUndoManager.Current == null) return false;

            return RedoUndoManager.Current.CanUndoOneStep;
        }

        public void Undo()
        {
            if (RedoUndoManager.Current == null) return;

            RedoUndoManager.Current.UndoOneStep();
        }
        #endregion

        #region RedoCommand
        private ViewModelCommand _RedoCommand;

        public ViewModelCommand RedoCommand
        {
            get
            {
                if (_RedoCommand == null)
                {
                    _RedoCommand = new ViewModelCommand(Redo, CanRedo);
                }
                return _RedoCommand;
            }
        }

        public bool CanRedo()
        {
            if (RedoUndoManager.Current == null) return false;

            return RedoUndoManager.Current.CanRedoOneStep;
        }

        public void Redo()
        {
            if (RedoUndoManager.Current == null) return;

            RedoUndoManager.Current.RedoOneStep();
        }
        #endregion

    }
}
