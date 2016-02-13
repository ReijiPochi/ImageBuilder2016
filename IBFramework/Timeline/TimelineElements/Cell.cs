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

        public string PropertyHeaderName { get; set; }

        public Control GetPP()
        {
            return new CellPP() { DataContext = this };
        }
    }
}
