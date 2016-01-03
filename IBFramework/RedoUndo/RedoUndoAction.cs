using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework.RedoUndo
{
    public abstract class RedoUndoAction : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
