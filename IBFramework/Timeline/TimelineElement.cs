using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using IBFramework.Project;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace IBFramework.Timeline
{
    public abstract class  TimelineElement : IBProjectElement
    {
        public TimelineElement(int w, int h) : base()
        {
            Width = w;
            Height = h;
        }

        public ObservableCollection<TimelineElement> Elements { get; set; } = new ObservableCollection<TimelineElement>();
    }
}
