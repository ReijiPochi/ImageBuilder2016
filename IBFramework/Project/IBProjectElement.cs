using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using IBFramework.IBCanvas;

namespace IBFramework.Project
{
    public enum IBProjectElementTypes
    {
        Null,
        Root,
        Folder,
        Cell,
        CellSource
    }

    public enum IBProjectElementFlags
    {
        Non,
        Drawing,
        Finished,
        Checked,
        Advice
    }

    public abstract class IBProjectElement : INotifyPropertyChanged
    {
        public IBProjectElement()
        {
            OnInitializing();
        }

        protected virtual void OnInitializing()
        {
            if (IBProject.Current != null)
            {
                ID = IBProject.Current.GenNewID();
            }
            else
            {
                throw new Exception("IBProject.Current が null です");
            }
        }

        public IBProjectElementTypes Type { get; protected set; }

        public ObservableCollection<IBProjectElement> Children { get; set; } = new ObservableCollection<IBProjectElement>();


        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _ID;
        public int ID
        {
            get
            { return _ID; }
            set
            { 
                if (_ID == value)
                    return;
                _ID = value;
                RaisePropertyChanged("ID");
            }
        }

        /// <summary>
        /// プロパティの変更を禁止するかどうか
        /// </summary>
        private bool _IsLocked;
        public bool IsLocked
        {
            get
            { return _IsLocked; }
            set
            {
                if (_IsLocked == value)
                    return;
                _IsLocked = value;
                RaisePropertyChanged("IsLocked");
            }
        }

        private string _Name;
        public string Name
        {
            get
            { return _Name; }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string _PropertyHeaderName;
        public string PropertyHeaderName
        {
            get { return _PropertyHeaderName; }
            set
            {
                if (_PropertyHeaderName == value)
                    return;
                _PropertyHeaderName = value;
                RaisePropertyChanged("PropertyHeaderName");
            }
        }

        private int _Width;
        public int Width
        {
            get
            { return _Width; }
            set
            {
                if (_Width == value)
                    return;
                _Width = value;
                RaisePropertyChanged("Width");
                IBCanvasControl.RefreshAll();
            }
        }

        private int _Height;
        public int Height
        {
            get
            { return _Height; }
            set
            {
                if (_Height == value)
                    return;
                _Height = value;
                RaisePropertyChanged("Height");
                IBCanvasControl.RefreshAll();
            }
        }

        private bool _DELETE;
        /// <summary>
        /// trueになると、削除されるようにして
        /// </summary>
        public bool DELETE
        {
            get
            { return _DELETE; }
            private set
            {
                if (_DELETE == value)
                    return;
                _DELETE = value;
                RaisePropertyChanged("DELETE");
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            { return _IsSelected; }
            set
            {
                if (_IsSelected == value)
                    return;
                _IsSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        private bool _IsShowing;
        public bool IsShowing
        {
            get
            { return _IsShowing; }
            set
            {
                if (_IsShowing == value)
                    return;
                _IsShowing = value;
                RaisePropertyChanged("IsShowing");
            }
        }

        private IBProjectElement _Parent;
        public IBProjectElement Parent
        {
            get
            { return _Parent; }
            set
            {
                if (_Parent == value)
                    return;
                _Parent = value;
                RaisePropertyChanged("Parent");
            }
        }

        private int _UseCount;
        public int UseCount
        {
            get
            { return _UseCount; }
            set
            {
                if (_UseCount == value)
                    return;
                _UseCount = value;
                RaisePropertyChanged("UseCount");
            }
        }

        private string _Comment;
        public string Comment
        {
            get
            { return _Comment; }
            set
            {
                if (_Comment == value)
                    return;
                _Comment = value;
                RaisePropertyChanged("Comment");
            }
        }

        private IBProjectElementFlags _StateFlag;
        public IBProjectElementFlags StateFlag
        {
            get
            { return _StateFlag; }
            set
            {
                if (_StateFlag == value)
                    return;
                _StateFlag = value;
                RaisePropertyChanged("StateFlag");
            }
        }

        public void RemoveChild(IBProjectElement child)
        {
            Children.Remove(child);
            child.Parent = null;
        }

        public void AddChild(IBProjectElement child)
        {
            if (child.Parent != null) throw new Exception("IBProjectElement.Parent プロパティが null でありません");

            Children.Add(child);
            child.Parent = this;

            DELETE = false;
        }

        public virtual void Remove()
        {
            Parent.RemoveChild(this);

            DELETE = true;
        }
    }
}
