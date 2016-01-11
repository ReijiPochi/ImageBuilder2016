using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBFramework.RedoUndo;
using System.Collections.ObjectModel;
using Livet;

namespace IBApp.Models
{
    public class RedoUndoModel : NotificationObject
    {
        public static RedoUndoModel Current { get; set; }

        public RedoUndoModel()
        {
            RedoUndoManager.Current = new RedoUndoManager();
            RedoUndoManager.Current.PropertyChanged += CurrentRedoUndo_PropertyChanged;
        }

        private void CurrentRedoUndo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CanRedoOneStep":
                    CanRedoOneStep = RedoUndoManager.Current.CanRedoOneStep;
                    break;

                case "CanUndoOneStep":
                    CanUndoOneStep = RedoUndoManager.Current.CanUndoOneStep;
                    break;

                default:
                    break;
            }
        }



        #region CanRedoOneStep変更通知プロパティ
        private bool _CanRedoOneStep;

        public bool CanRedoOneStep
        {
            get
            { return _CanRedoOneStep; }
            set
            { 
                if (_CanRedoOneStep == value)
                    return;
                _CanRedoOneStep = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region CanUndoOneStep変更通知プロパティ
        private bool _CanUndoOneStep;

        public bool CanUndoOneStep
        {
            get
            { return _CanUndoOneStep; }
            set
            { 
                if (_CanUndoOneStep == value)
                    return;
                _CanUndoOneStep = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
