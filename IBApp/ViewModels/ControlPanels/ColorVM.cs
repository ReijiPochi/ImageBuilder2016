using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using IBApp.Models;
using IBApp.Views.ControlPanels;

namespace IBApp.ViewModels.ControlPanels
{
    public class ColorVM : ViewModel
    {
        public ColorVM()
        {
            if (IBProjectModel.Current == null) return;
            IBProjectModel.Current.PropertyChanged += IBProjectModel_PropertyChanged;
            SetMode();
        }

        private void IBProjectModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "SelectedBrush")
            {
                SetMode();
            }
            else if(e.PropertyName == "SelectedDrawingColor")
            {
                SelectedDrawingColor = new SolidColorBrush(Color.FromArgb(
                    IBProjectModel.Current.SelectedDrawingColor.a,
                    IBProjectModel.Current.SelectedDrawingColor.r,
                    IBProjectModel.Current.SelectedDrawingColor.g,
                    IBProjectModel.Current.SelectedDrawingColor.b));
            }
        }

        private void SetMode()
        {
            if (IBProjectModel.Current.SelectedBrush == null) return;

            switch (IBProjectModel.Current.SelectedBrush.GetType().Name)
            {
                case "Pen":
                    Mode = ColorCPMode.Pen;
                    break;

                case "Eraser":
                    Mode = ColorCPMode.NULL;
                    break;

                default:
                    break;
            }
        }


        #region Mode変更通知プロパティ
        private ColorCPMode _Mode;

        public ColorCPMode Mode
        {
            get
            { return _Mode; }
            set
            { 
                if (_Mode == value)
                    return;
                _Mode = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SelectedDrawingColor変更通知プロパティ
        private SolidColorBrush _SelectedDrawingColor;

        public SolidColorBrush SelectedDrawingColor
        {
            get
            { return _SelectedDrawingColor; }
            set
            { 
                if (_SelectedDrawingColor == value)
                    return;
                _SelectedDrawingColor = value;
                RaisePropertyChanged();
            }
        }
        #endregion



        #region SetColorCommand
        private ListenerCommand<SolidColorBrush> _SetColorCommand;

        public ListenerCommand<SolidColorBrush> SetColorCommand
        {
            get
            {
                if (_SetColorCommand == null)
                {
                    _SetColorCommand = new ListenerCommand<SolidColorBrush>(SetColor);
                }
                return _SetColorCommand;
            }
        }

        public void SetColor(SolidColorBrush parameter)
        {
            IBProjectModel.Current.SelectedDrawingColor
                = new IBFramework.Image.PixelData()
                {
                    b = parameter.Color.B,
                    g = parameter.Color.G,
                    r = parameter.Color.R,
                    a = parameter.Color.A
                };
        }
        #endregion

    }
}
