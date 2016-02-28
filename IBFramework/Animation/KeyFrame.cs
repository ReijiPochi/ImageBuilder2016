using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework.Animation
{
    public class KeyFrame<T>
    {
        public IBTime Time { get; set; }
        public T Value { get; set; }
    }
}
