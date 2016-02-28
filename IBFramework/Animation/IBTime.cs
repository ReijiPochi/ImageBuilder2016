using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IBFramework.Animation
{
    public class IBTime
    {
        public int Frame { get; private set; }

        public void SetTime(int frame)
        {
            Frame = frame;
        }
    }
}
