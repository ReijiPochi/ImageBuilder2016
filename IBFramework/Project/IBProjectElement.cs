using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace IBFramework.Project
{
    public abstract class IBProjectElement
    {
        public IBProjectElement(IBProject Master)
        {
            if(Master != null)
            {
                ID = Master.GenNewID();
            }
        }

        public int ID { get; private set; }

        public string Name { get; set; }

        /// <summary>
        /// nullの場合、親は現在のプロジェクト
        /// </summary>
        public IBProjectElement Parent = null;

        public ObservableCollection<IBProjectElement> Children { get; set; } = new ObservableCollection<IBProjectElement>();
    }
}
