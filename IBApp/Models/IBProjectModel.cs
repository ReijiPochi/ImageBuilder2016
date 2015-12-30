using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using Livet;

using IBFramework;
using IBFramework.IBProject;
using IBFramework.Timeline.TimelineElements;

namespace IBApp.Models
{
    public class IBProjectModel : NotificationObject
    {
        public IBProjectModel()
        {
            _OpenedIBProject = new IBProject();
        }

        /// <summary>
        /// App.xaml.cs のスタートアップでとりあえずインスタントを設定
        /// </summary>
        public static IBProjectModel Current { get; set; }

        public IBProject _OpenedIBProject;



        #region IBProject_Name変更通知プロパティ
        public string IBProject_Name
        {
            get
            { return _OpenedIBProject.IBProjectName; }
            set
            { 
                if (_OpenedIBProject.IBProjectName == value)
                    return;
                _OpenedIBProject.IBProjectName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IBProject_Elements変更通知プロパティ
        public ObservableCollection<IBProjectElement> IBProject_Elements
        {
            get
            { return _OpenedIBProject.IBProjectElements; }
            set
            { 
                if (_OpenedIBProject.IBProjectElements == value)
                    return;
                _OpenedIBProject.IBProjectElements = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedPropertyItem変更通知プロパティ
        private IProperty _SelectedPropertyItem;

        public IProperty SelectedPropertyItem
        {
            get
            { return _SelectedPropertyItem; }
            set
            {
                if (_SelectedPropertyItem == value)
                    return;
                _SelectedPropertyItem = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
