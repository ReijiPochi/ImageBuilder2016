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
            SetColor();
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
                SetColor();
            }
        }

        private void SetMode()
        {
            if (IBProjectModel.Current.SelectedBrush == null) return;

            switch (IBProjectModel.Current.SelectedBrush.GetType().Name)
            {
                case "Pen":
                case "Pencil":
                    Mode = ColorCPMode.Pen;
                    break;

                default:
                    Mode = ColorCPMode.NULL;
                    break;
            }
        }

        private void SetColor()
        {
            if (IBProjectModel.Current.SelectedDrawingColor == null) return;

            SelectedDrawingColor = new SolidColorBrush(Color.FromArgb(
                IBProjectModel.Current.SelectedDrawingColor.a,
                IBProjectModel.Current.SelectedDrawingColor.r,
                IBProjectModel.Current.SelectedDrawingColor.g,
                IBProjectModel.Current.SelectedDrawingColor.b));

            _SelectedDrawingColor_B = SelectedDrawingColor.Color.B;
            _SelectedDrawingColor_G = SelectedDrawingColor.Color.G;
            _SelectedDrawingColor_R = SelectedDrawingColor.Color.R;

            RaisePropertyChanged("SelectedDrawingColor_R");
            RaisePropertyChanged("SelectedDrawingColor_G");
            RaisePropertyChanged("SelectedDrawingColor_B");
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

        #region SelectedDrawingColor_R変更通知プロパティ
        private int _SelectedDrawingColor_R;

        public int SelectedDrawingColor_R
        {
            get
            { return _SelectedDrawingColor_R; }
            set
            { 
                if (_SelectedDrawingColor_R == value)
                    return;
                _SelectedDrawingColor_R = value;
                RaisePropertyChanged();

                SetColor(new SolidColorBrush(Color.FromArgb(
                    255,
                    (byte)SelectedDrawingColor_R,
                    (byte)SelectedDrawingColor_G,
                    (byte)SelectedDrawingColor_B)));
            }
        }
        #endregion

        #region SelectedDrawingColor_G変更通知プロパティ
        private int _SelectedDrawingColor_G;

        public int SelectedDrawingColor_G
        {
            get
            { return _SelectedDrawingColor_G; }
            set
            { 
                if (_SelectedDrawingColor_G == value)
                    return;
                _SelectedDrawingColor_G = value;
                RaisePropertyChanged();

                SetColor(new SolidColorBrush(Color.FromArgb(
                    255,
                    (byte)SelectedDrawingColor_R,
                    (byte)SelectedDrawingColor_G,
                    (byte)SelectedDrawingColor_B)));
            }
        }
        #endregion

        #region SelectedDrawingColor_B変更通知プロパティ
        private int _SelectedDrawingColor_B;

        public int SelectedDrawingColor_B
        {
            get
            { return _SelectedDrawingColor_B; }
            set
            { 
                if (_SelectedDrawingColor_B == value)
                    return;
                _SelectedDrawingColor_B = value;
                RaisePropertyChanged();

                SetColor(new SolidColorBrush(Color.FromArgb(
                    255,
                    (byte)SelectedDrawingColor_R,
                    (byte)SelectedDrawingColor_G,
                    (byte)SelectedDrawingColor_B)));
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



        #region SetDrawingColorCommand
        private ListenerCommand<SolidColorBrush> _SetDrawingColorCommand;

        public ListenerCommand<SolidColorBrush> SetDrawingColorCommand
        {
            get
            {
                if (_SetDrawingColorCommand == null)
                {
                    _SetDrawingColorCommand = new ListenerCommand<SolidColorBrush>(SetColor);
                }
                return _SetDrawingColorCommand;
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
