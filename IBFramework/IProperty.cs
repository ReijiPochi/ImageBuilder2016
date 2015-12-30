using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework
{
    public interface IProperty
    {
        string ElementName { get; set; }

        bool IsLocked { get; set; }
    }
}
