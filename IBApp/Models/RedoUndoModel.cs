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

        private ObservableCollection<RedoUndoAction> History = new ObservableCollection<RedoUndoAction>();

        private int index = 0;

        public void Record(RedoUndoAction ru)
        {
            if (CanRedoOneStep)
            {
                int count = History.Count - index;
                for(int i = 0; i < count; i++)
                {
                    History.RemoveAt(index);
                }
                CanRedoOneStep = false;
            }
            History.Add(ru);
            index++;

            CanUndoOneStep = true;
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


        public void RedoOneStep()
        {
            History[index].Redo();
            index++;

            if (index >= History.Count)
                CanRedoOneStep = false;

            if(index <= History.Count)
            {
                CanUndoOneStep = true;
            }
            else
            {
                CanUndoOneStep = false;
            }
        }

        public void UndoOneStep()
        {
            index--;
            History[index].Undo();

            if (index <= 0)
                CanUndoOneStep = false;

            if(index < History.Count)
            {
                CanRedoOneStep = true;
            }
            else
            {
                CanRedoOneStep = false;
            }
        }
    }
}
