using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

using IBFramework.Image;
using IBFramework.Project;
using IBFramework.Project.IBProjectElements;
using System.Windows.Controls;

namespace IBFramework.Timeline.TimelineElements
{
    public class Cell : TimelineElement, IProperty
    {
        public Cell(CellSource source, int w, int h) : base(w, h)
        {
            Type = IBProjectElementTypes.Cell;
            PropertyHeaderName = "Cell";
            Source = source;
            PropertyChanged += Cell_PropertyChanged;
        }

        private void Cell_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
                PropertyHeaderName = "Cell \"" + Name + "\"";
        }


        private CellSource _Source;
        public CellSource Source
        {
            get { return _Source; }
            set
            {
                if (_Source == value)
                    return;
                _Source = value;
                value.UseCount++;
                RaisePropertyChanged("Source");
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
            return new CellPP() { DataContext = this };
        }
    }
}
