using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using IBFramework;
using System.Windows.Controls;
using System.ComponentModel;

namespace IBFramework.Project
{
    public class IBProject : INotifyPropertyChanged, IProperty
    {
        public ObservableCollection<IBProjectElement> IBProjectElements { get; set; } = new ObservableCollection<IBProjectElement>();

        private int IDCount { get; set; } = 0;

        public int GenNewID()
        {
            IDCount++;
            return IDCount;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _IBProjectName;
        public string IBProjectName
        {
            get { return _IBProjectName; }
            set
            {
                if (_IBProjectName == value)
                    return;
                _IBProjectName = value;
                RaisePropertyChanged("IBProjectName");
                PropertyHeaderName = "Project \"" + value + "\"";
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

        private bool _IsLocked;
        public bool IsLocked
        {
            get { return _IsLocked; }
            set
            {
                if (_IsLocked == value)
                    return;
                _IsLocked = value;
                RaisePropertyChanged("IsLocked");
            }
        }

        public Control GetPP()
        {
            return new IBProjectPP() { DataContext = this };
        }
    }
}
