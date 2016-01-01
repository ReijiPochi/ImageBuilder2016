using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IBFramework.Project
{
    public enum IBProjectElementTypes
    {
        Null,
        File,
        Cell
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
        public IBProjectElement(IBProject Master)
        {
            if(Master != null)
            {
                ID = Master.GenNewID();
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

        private IBProjectElement _Parent;
        /// <summary>
        /// nullの場合、親は現在のプロジェクト
        /// </summary>
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
    }
}
