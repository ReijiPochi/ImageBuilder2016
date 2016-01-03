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

namespace IBApp.ViewModels
{
    public class EditVM : ViewModel
    {
        public EditVM()
        {
            RedoUndoModel.Current.PropertyChanged += RedoUndoModel_PropertyChanged;
        }

        private void RedoUndoModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CanRedoOneStep":
                    RedoCommand.RaiseCanExecuteChanged();
                    break;

                case "CanUndoOneStep":
                    UndoCommand.RaiseCanExecuteChanged();
                    break;

                default:
                    break;
            }
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
            return RedoUndoModel.Current.CanUndoOneStep;
        }

        public void Undo()
        {
            RedoUndoModel.Current.UndoOneStep();
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
            return RedoUndoModel.Current.CanRedoOneStep;
        }

        public void Redo()
        {
            RedoUndoModel.Current.RedoOneStep();
        }
        #endregion



        #region SetLanguageCommand
        private ListenerCommand<string> _SetLanguageCommand;

        public ListenerCommand<string> SetLanguageCommand
        {
            get
            {
                if (_SetLanguageCommand == null)
                {
                    _SetLanguageCommand = new ListenerCommand<string>(SetLanguage);
                }
                return _SetLanguageCommand;
            }
        }

        public void SetLanguage(string parameter)
        {
            IBAppModel.SetLanguage(parameter);
        }
        #endregion

        #region SetColorThemeCommand
        private ListenerCommand<string> _SetColorThemeCommand;

        public ListenerCommand<string> SetColorThemeCommand
        {
            get
            {
                if (_SetColorThemeCommand == null)
                {
                    _SetColorThemeCommand = new ListenerCommand<string>(SetColorTheme, CanSetColorTheme);
                }
                return _SetColorThemeCommand;
            }
        }

        public bool CanSetColorTheme()
        {
            return true;
        }

        public void SetColorTheme(string parameter)
        {
            IBAppModel.SetColorTheme(parameter);
        }
        #endregion

    }
}
