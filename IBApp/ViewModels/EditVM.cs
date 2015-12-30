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
        public void Initialize()
        {
        }


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
