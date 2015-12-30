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
using IBApp.Views.ControlPanels;

namespace IBApp.ViewModels
{
    public class PanelVM : ViewModel
    {


        #region ShowControlPanelCommand
        private ListenerCommand<object> _ShowControlPanelCommand;

        public ListenerCommand<object> ShowControlPanelCommand
        {
            get
            {
                if (_ShowControlPanelCommand == null)
                {
                    _ShowControlPanelCommand = new ListenerCommand<object>(ShowControlPanel);
                }
                return _ShowControlPanelCommand;
            }
        }

        public void ShowControlPanel(object ibTabItem)
        {
            LayoutControlModel.ShowControlPanel(ibTabItem);
        }
        #endregion


    }
}
