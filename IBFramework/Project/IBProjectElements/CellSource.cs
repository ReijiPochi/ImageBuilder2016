using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;

using IBFramework.Image;

namespace IBFramework.Project.IBProjectElements
{
    public class CellSource : IBProjectElement, IProperty
    {
        public CellSource(IBProject Master) : base(Master)
        {
            Type = IBProjectElementTypes.Cell;
            PropertyHeaderName = "CellSource";
            PropertyChanged += CellSource_PropertyChanged;
        }

        private void CellSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
                PropertyHeaderName = "CellSource \"" + Name + "\"";
        }

        public ObservableCollection<IBImage> _Layers = new ObservableCollection<IBImage>();
        public ObservableCollection<IBImage> Layers
        {
            get { return _Layers; }
            set { Layers = value; }
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
            return new CellSourcePP() { DataContext = this };
        }
    }
}
