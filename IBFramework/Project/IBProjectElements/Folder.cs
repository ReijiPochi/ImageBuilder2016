using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IBFramework.Project.IBProjectElements
{
    public class Folder : IBProjectElement, IProperty
    {
        public Folder() : base()
        {
            Type = IBProjectElementTypes.Folder;
            PropertyHeaderName = "Folder";
            PropertyChanged += Folder_PropertyChanged;
        }

        private void Folder_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
                PropertyHeaderName = "Folder \"" + Name + "\"";
        }

        public string PropertyHeaderName { get; set; }

        public Control GetPP()
        {
            return new FolderPP() { DataContext = this };
        }
    }
}
