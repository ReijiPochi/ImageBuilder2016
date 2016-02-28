using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using IBFramework.Project;
using IBFramework.Animation;

namespace IBFramework.Timeline
{
    public abstract class  TimelineElement : IBProjectElement
    {
        public TimelineElement(int w, int h) : base()
        {
            Width = w;
            Height = h;
            Elements.Owner = this;
            PosX.CurrentTime = Clock.CurrentTime;
            PosY.CurrentTime = Clock.CurrentTime;
        }

        public class TimelineElementCollection : ObservableCollection<TimelineElement>
        {
            public TimelineElement Owner;

            protected override void SetItem(int index, TimelineElement item)
            {
                if (Owner == null)
                    throw new Exception("TimelineElementCollectionのOwnerが設定されていません");
                if (item.TimelineParent != null)
                    throw new Exception("TimelineElementの親が既に設定されています");

                base.SetItem(index, item);

                item.TimelineParent = Owner;
            }

            protected override void RemoveItem(int index)
            {
                this[index].Parent = null;

                base.RemoveItem(index);
            }
        }

        public TimelineElementCollection Elements { get; private set; } = new TimelineElementCollection();

        private TimelineElement _TimelineParent;
        public TimelineElement TimelineParent
        {
            get
            { return _TimelineParent; }
            private set
            {
                if (_TimelineParent == value)
                    return;
                _TimelineParent = value;
                RaisePropertyChanged("TimelineParent");
            }
        }

        public IBAnimationClock Clock { get; private set; } = new IBAnimationClock();

        public IBDouble PosX { get; private set; } = new IBDouble();
        public IBDouble PosY { get; private set; } = new IBDouble();
    }
}
