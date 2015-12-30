using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace IBFramework.IBProject
{
    public abstract class IBProjectElement
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public ObservableCollection<IBProjectElement> Children { get; set; } = new ObservableCollection<IBProjectElement>();
    }
}
