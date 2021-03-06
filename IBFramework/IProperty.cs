﻿using System;
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
        /// PropertiesPanel
        /// </summary>
        Control GetPP();

        string PropertyHeaderName { get; set; }

        bool IsLocked { get; set; }
    }
}
