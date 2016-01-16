using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace IBFramework.RedoUndo
{
    public class RedoUndoManager : INotifyPropertyChanged
    {
        public static RedoUndoManager Current { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<RedoUndoAction> History = new ObservableCollection<RedoUndoAction>();

        private int index = 0;

        public void Record(RedoUndoAction ru)
        {
            if (CanRedoOneStep)
            {
                int count = History.Count - index;
                for (int i = 0; i < count; i++)
                {
                    if (History[index] as IDisposable != null)
                        ((IDisposable)History[index]).Dispose();

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
                RaisePropertyChanged("CanRedoOneStep");
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
                RaisePropertyChanged("CanUndoOneStep");
            }
        }
        #endregion



        public void RedoOneStep()
        {
            History[index].Redo();
            index++;

            if (index >= History.Count)
                CanRedoOneStep = false;

            if (index <= History.Count)
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

            if (index < History.Count)
            {
                CanRedoOneStep = true;
            }
            else
            {
                CanRedoOneStep = false;
            }
        }
    }

    public abstract class RedoUndoAction : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



        private string _Summary;
        public string Summary
        {
            get
            { return _Summary; }
            set
            {
                if (_Summary == value)
                    return;
                _Summary = value;
                RaisePropertyChanged("Summary");
            }
        }

        private bool _CanRedo;
        public bool CanRedo
        {
            get
            { return _CanRedo; }
            private set
            {
                if (_CanRedo == value)
                    return;
                _CanRedo = value;
                RaisePropertyChanged("CanRedo");
            }
        }

        public virtual void Redo()
        {
            if (!CanRedo) return;
            CanRedo = false;
        }


        private bool _CanUndo;
        public bool CanUndo
        {
            get
            { return _CanUndo; }
            private set
            {
                if (_CanUndo == value)
                    return;
                _CanUndo = value;
                RaisePropertyChanged("CanUndo");
            }
        }

        public virtual void Undo()
        {
            if (!CanUndo) return;
            CanUndo = false;
        }
    }
}
