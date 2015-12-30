using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using IBFramework.IBProject;

namespace IBFramework.Timeline
{
    public abstract class  TimelineElement : IBProjectElement
    {
        public ObservableCollection<TimelineElement> Elements { get; set; } = new ObservableCollection<TimelineElement>();
    }
}
