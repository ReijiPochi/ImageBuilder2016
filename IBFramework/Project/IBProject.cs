using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using IBFramework.Project;

namespace IBFramework.Project
{
    public class IBProject
    {
        public string IBProjectName { get; set; } = "Untitled";

        public ObservableCollection<IBProjectElement> IBProjectElements { get; set; } = new ObservableCollection<IBProjectElement>();

        private int IDCount { get; set; } = 0;

        public int GenNewID()
        {
            IDCount++;
            return IDCount;
        }
    }
}
