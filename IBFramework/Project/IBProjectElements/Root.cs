using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework.Project.IBProjectElements
{
    public class Root : IBProjectElement
    {
        public Root() : base()
        {
            Type = IBProjectElementTypes.Root;
        }

        protected override void OnInitializing()
        {
            ID = -1;
            //base.OnInitializing();
        }
    }
}
