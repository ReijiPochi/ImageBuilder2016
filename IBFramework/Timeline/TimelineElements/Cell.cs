using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

using IBFramework.Image;
using IBFramework.Project;

namespace IBFramework.Timeline.TimelineElements
{
    public class Cell : TimelineElement
    {
        public Cell(IBProject Master) : base(Master)
        {
        }

        public ObservableCollection<IBImage> _Layers = new ObservableCollection<IBImage>();
        public ObservableCollection<IBImage> Layers
        {
            get { return _Layers; }
            set { Layers = value; }
        }
    }
}
