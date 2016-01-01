using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IBFramework
{
    public interface IProperty
    {
        /// <summary>
        /// ヘッダーに表示される名前
        /// </summary>
        string PropertyHeaderName { get; set; }

        /// <summary>
        /// プロパティの変更を禁止するかどうか
        /// </summary>
        bool IsLocked { get; set; }

        /// <summary>
        /// PropertiesPanel
        /// </summary>
        Control GetPP();
    }
}
