using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using Livet;

using IBFramework;
using IBFramework.Project;
using IBFramework.Project.IBProjectElements;
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

        /// <summary>
        /// 現在開かれているプロジェクト
        /// </summary>
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
        /// <summary>
        /// PropertiesCPに表示される
        /// </summary>
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

        #region ActiveTargetElement変更通知プロパティ
        private IBProjectElement _ActiveTargetElement;
        /// <summary>
        /// 編集等の対象になっているエレメント。登録すると、可能ならSelectedPropertyItemにも追加される
        /// </summary>
        public IBProjectElement ActiveTargetElement
        {
            get
            { return _ActiveTargetElement; }
            set
            { 
                if (_ActiveTargetElement == value)
                    return;
                _ActiveTargetElement = value;
                RaisePropertyChanged();

                if (value as IProperty != null)
                    SelectedPropertyItem = (IProperty)value;
            }
        }
        #endregion


        /// <summary>
        /// IBProjectModel.ActiveTargetElementの子に新規フォルダを追加
        /// </summary>
        /// <param name="Parent">nullの場合、現在開かれているプロジェクトにフォルダを追加</param>
        public void AddNewFolder()
        {
            Folder newFolder = new Folder(_OpenedIBProject)
            {
                Name = "Folder",
                IsSelected = true
            };

            if (ActiveTargetElement != null)
            {
                ActiveTargetElement.Children.Add(newFolder);
                newFolder.Parent = ActiveTargetElement;
            }
            else
            {
                _OpenedIBProject.IBProjectElements.Add(newFolder);
                newFolder.Parent = null;
            }

            RaisePropertyChanged("IBProject_Elements");
        }

        /// <summary>
        /// IBProjectModel.ActiveTargetElementの子に新規セルソースを追加
        /// </summary>
        /// <param name="Parent">nullの場合、現在開かれているプロジェクトにセルソースを追加</param>
        public void AddNewCellSource()
        {
            CellSource newCellSource = new CellSource(_OpenedIBProject)
            {
                Name = "Cell",
                IsSelected = true
            };

            if(ActiveTargetElement != null)
            {
                if (ActiveTargetElement.Type == IBProjectElementTypes.Cell)
                {
                    string Num = "", tempName = "";
                    int tempNum;
                    bool f = false;
                    for (int i = ActiveTargetElement.Name.Length - 1; i >= 0; i--)
                    {
                        if (!f && int.TryParse(ActiveTargetElement.Name[i].ToString(), out tempNum))
                        {
                            Num += tempNum.ToString();
                        }
                        else
                        {
                            f = true;
                            tempName += ActiveTargetElement.Name[i].ToString();
                        }
                    }
                    if(Num.Length != 0)
                    {
                        Num = Reverse(Num);
                        tempName = Reverse(tempName);
                        tempNum = int.Parse(Num) + 1;
                        newCellSource.Name = tempName + tempNum.ToString("d" + Num.Length.ToString());
                    }

                    if(ActiveTargetElement.Parent != null)
                    {
                        IBProjectElement parent = ActiveTargetElement.Parent;
                        parent.Children.Add(newCellSource);
                        newCellSource.Parent = parent;
                    }
                    else
                    {
                        _OpenedIBProject.IBProjectElements.Add(newCellSource);
                        newCellSource.Parent = null;
                    }
                }
                else
                {
                    ActiveTargetElement.Children.Add(newCellSource);
                    newCellSource.Parent = ActiveTargetElement;
                }
            }
            else
            {
                _OpenedIBProject.IBProjectElements.Add(newCellSource);
                newCellSource.Parent = null;
            }

            RaisePropertyChanged("IBProject_Elements");
        }

        private string Reverse(string s)
        {
            string result = "";
            for(int c = s.Length - 1; c >= 0; c--)
            {
                result += s[c];
            }
            return result;
        }
    }
}
