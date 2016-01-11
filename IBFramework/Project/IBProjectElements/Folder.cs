using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework.Project.IBProjectElements
{
    public class Folder : IBProjectElement
    {
        public Folder() : base()
        {
            Type = IBProjectElementTypes.Folder;
        }
    }
}
