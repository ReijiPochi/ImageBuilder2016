using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using IBFramework.Project;

namespace IBFramework.Timeline
{
    public abstract class  TimelineElement : IBProjectElement
    {
        public TimelineElement(IBProject Master) : base(Master)
        {
        }

        public ObservableCollection<TimelineElement> Elements { get; set; } = new ObservableCollection<TimelineElement>();
    }
}
