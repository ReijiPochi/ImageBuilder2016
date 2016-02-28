using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBFramework.Animation
{
    public class IBDouble : AnimationValueBase<double>
    {
        protected override double CalcInternalDivision(KeyFrame<double> key1, KeyFrame<double> key2)
        {
            return (key2.Time.Frame * key1.Value + key1.Time.Frame * key2.Value) / (key1.Time.Frame + key2.Time.Frame);
        }
    }
}
