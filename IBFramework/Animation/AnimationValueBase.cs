using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace IBFramework.Animation
{
    public abstract class AnimationValueBase<T>
    {
        public IBTime CurrentTime { get; set; }

        public ObservableCollection<KeyFrame<T>> KeyFrames { get; private set; } = new ObservableCollection<KeyFrame<T>>();

        public T CurrentValue
        {
            get { return GetCurrentValue(); }
        }

        protected T GetCurrentValue()
        {
            if (CurrentTime == null) return default(T);

            if (KeyFrames.Count == 0)
                return default(T);

            if (KeyFrames.Count == 1)
                return KeyFrames[0].Value;


            KeyFrame<T> f1 = null, f2 = null;

            foreach(KeyFrame<T> key in KeyFrames)
            {
                if(key.Time.Frame <= CurrentTime.Frame)
                {
                    if (f1 == null)
                        f1 = key;
                    else if (f1.Time.Frame < key.Time.Frame)
                        f1 = key;
                }
                else
                {
                    if (f2 == null)
                        f2 = key;
                    else if (f2.Time.Frame > key.Time.Frame)
                        f2 = key;
                }
            }

            if (f1 == null && f2 != null)
                return f2.Value;

            if (f2 == null && f1 != null)
                return f1.Value;

            if (f1 != null && f2 != null)
                return CalcInternalDivision(f1, f2);

            return default(T);
        }

        protected abstract T CalcInternalDivision(KeyFrame<T> key1, KeyFrame<T> key2);
    }
}
