using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using IBFramework.IBProject;

namespace IBFramework.IBProject
{
    public class IBProject
    {
        public string IBProjectName { get; set; } = "Untitled";

        public ObservableCollection<IBProjectElement> IBProjectElements { get; set; }
    }
}
