using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using System.ComponentModel;

namespace IBApp.Views.ControlPanels
{
    public class IBControlPanel : Control
    {
        public double DefaultHeight { get; set; } = 200;

        public double DefaultWidth { get; set; } = 200;

        public virtual IBControlPanel CloneOrCopy()
        {
            return new IBControlPanel();
        }
    }
}
