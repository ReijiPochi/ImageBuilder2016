using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using Livet;

using IBFramework;
using IBFramework.Project;
using IBFramework.Project.IBProjectElements;
using IBFramework.Image;
using IBFramework.Timeline.TimelineElements;
using IBFramework.RedoUndo;

namespace IBApp.Models
{
    public class IBProjectModel : NotificationObject
    {
        public IBProjectModel()
        {
            IBProject.Current = new IBProject() { IBProjectName = "Untitled" };
            SelectedPropertyItem = IBProject.Current;
        }

        /// <summary>
        /// App.xaml.cs のスタートアップでとりあえずインスタントを設定
        /// </summary>
        public static IBProjectModel Current { get; set; }



        #region IBProject_Name変更通知プロパティ
        public string IBProject_Name
        {
            get
            { return IBProject.Current.IBProjectName; }
            set
            { 
                if (IBProject.Current.IBProjectName == value)
                    return;
                IBProject.Current.IBProjectName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IBProject_Elements変更通知プロパティ
        public ObservableCollection<IBProjectElement> IBProject_Elements
        {
            get
            { return IBProject.Current.IBProjectElements; }
            set
            { 
                if (IBProject.Current.IBProjectElements == value)
                    return;
                IBProject.Current.IBProjectElements = value;
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

                if (_ActiveTargetElement != null)
                    _ActiveTargetElement.PropertyChanged -= _ActiveTargetElement_PropertyChanged;

                _ActiveTargetElement = value;

                if (_ActiveTargetElement != null)
                    _ActiveTargetElement.PropertyChanged += _ActiveTargetElement_PropertyChanged;

                RaisePropertyChanged();

                if (value as IProperty != null)
                    SelectedPropertyItem = (IProperty)value;
            }
        }

        private void _ActiveTargetElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DELETE")
            {
                ActiveTargetElement = null;
                ActiveShowingItem = null;
                SelectedPropertyItem = null;
            }
        }
        #endregion

        #region ActiveCanvasItems変更通知プロパティ
        private ObservableCollection<IBProjectElement> _ActiveCanvasItems;
        /// <summary>
        /// 現在、または最後にアクティブになったIBCanvasのアイテムリスト
        /// </summary>
        public ObservableCollection<IBProjectElement> ActiveCanvasItems
        {
            get
            { return _ActiveCanvasItems; }
            set
            { 
                if (_ActiveCanvasItems == value)
                    return;
                _ActiveCanvasItems = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ActiveShowingItem変更通知プロパティ
        private IBProjectElement _ActiveShowingItem;
        /// <summary>
        /// キャンバスに表示されているエレメント。登録すると、可能ならSelectedPropertyItemにも追加される
        /// </summary>
        public IBProjectElement ActiveShowingItem
        {
            get
            { return _ActiveShowingItem; }
            set
            { 
                if (_ActiveShowingItem == value)
                    return;

                if(_ActiveShowingItem != null)
                    _ActiveShowingItem.PropertyChanged -= _ActiveShowingItem_PropertyChanged;

                _ActiveShowingItem = value;

                if (_ActiveShowingItem != null)
                    _ActiveShowingItem.PropertyChanged += _ActiveShowingItem_PropertyChanged;

                RaisePropertyChanged();

                if (value as IProperty != null)
                    SelectedPropertyItem = (IProperty)value;
            }
        }

        private void _ActiveShowingItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DELETE")
            {
                ActiveTargetElement = null;
                ActiveShowingItem = null;
                SelectedPropertyItem = null;
            }
        }
        #endregion


        /// <summary>
        /// IBProjectModel.ActiveTargetElementの子に新規フォルダを追加
        /// </summary>
        /// <param name="Parent">nullの場合、現在開かれているプロジェクトにフォルダを追加</param>
        public void AddNewFolder()
        {
            Folder newFolder = new Folder(IBProject.Current)
            {
                Name = "Folder",
                IsSelected = true
            };

            if (ActiveTargetElement != null)
            {
                IBProjectElement parent = ActiveTargetElement;
                if (ActiveTargetElement.Type != IBProjectElementTypes.Folder) return;

                parent.Children.Add(newFolder);
                newFolder.Parent = parent;
                RedoUndoModel.Current.Record(new RUAddNewElement(parent, newFolder));
            }
            else
            {
                IBProject.Current.IBProjectElements.Add(newFolder);
                newFolder.Parent = null;
                RedoUndoModel.Current.Record(new RUAddNewElement(null, newFolder));
            }

            RaisePropertyChanged("IBProject_Elements");
        }

        /// <summary>
        /// IBProjectModel.ActiveTargetElementの子に新規セルソースを追加
        /// </summary>
        /// <param name="Parent">nullの場合、現在開かれているプロジェクトにセルソースを追加</param>
        public void AddNewCellSource()
        {
            CellSource newCellSource = new CellSource(IBProject.Current)
            {
                Name = "Cell",
                IsSelected = true,
            };

            SingleColorImage i = new SingleColorImage(255, 0, 0, 100);
            i.Rect.Width = 192;
            i.Rect.Height = 108;
            i.Rect.OffsetX = 100;
            i.Rect.OffsetY = 150;
            i.LayerName = "layer2";
            newCellSource.Layers.Add(i);
            SingleColorImage i2 = new SingleColorImage(0, 0, 255, 100);
            i2.Rect.Width = 192;
            i2.Rect.Height = 108;
            i2.Rect.OffsetX = 100;
            i2.Rect.OffsetY = 100;
            i2.LayerName = "layer1";
            newCellSource.Layers.Add(i2);
            SingleColorImage bg = new SingleColorImage(200, 255, 255, 255);
            bg.Rect.Width = 1920;
            bg.Rect.Height = 1080;
            bg.Rect.OffsetX = 0;
            bg.Rect.OffsetY = 0;
            bg.LayerName = "BackGround";
            newCellSource.Layers.Add(bg);

            if (ActiveTargetElement != null)
            {
                if (ActiveTargetElement.Type == IBProjectElementTypes.Cell)
                {
                    SetName(newCellSource);

                    if (ActiveTargetElement.Parent != null)
                    {
                        IBProjectElement parent = ActiveTargetElement.Parent;
                        if (parent.Type != IBProjectElementTypes.Folder) return;

                        parent.Children.Add(newCellSource);
                        newCellSource.Parent = parent;
                        RedoUndoModel.Current.Record(new RUAddNewElement(parent, newCellSource));
                    }
                    else
                    {
                        IBProject.Current.IBProjectElements.Add(newCellSource);
                        newCellSource.Parent = null;
                        RedoUndoModel.Current.Record(new RUAddNewElement(null, newCellSource));
                    }
                }
                else
                {
                    IBProjectElement parent = ActiveTargetElement;

                    parent.Children.Add(newCellSource);
                    newCellSource.Parent = parent;
                    RedoUndoModel.Current.Record(new RUAddNewElement(parent, newCellSource));
                }
            }
            else
            {
                IBProject.Current.IBProjectElements.Add(newCellSource);
                newCellSource.Parent = null;
                RedoUndoModel.Current.Record(new RUAddNewElement(null, newCellSource));
            }

            RaisePropertyChanged("IBProject_Elements");
        }

        private class RUAddNewElement : RedoUndoAction
        {
            public RUAddNewElement(IBProjectElement parent, IBProjectElement newElement)
            {
                _parent = parent;
                _newElement = newElement;
            }
            private IBProjectElement _parent;
            private IBProjectElement _newElement;
            public override void Redo()
            {
                base.Redo();

                if (_parent != null)
                    _parent.Children.Add(_newElement);
                else if (IBProject.Current != null)
                    IBProject.Current.IBProjectElements.Add(_newElement);
            }

            public override void Undo()
            {
                base.Undo();

                _newElement.Remove();
            }
        }

        /// <summary>
        /// 名前をActiveTargetElementの名前からつけます
        /// </summary>
        /// <param name="newCellSource"></param>
        private void SetName(CellSource newCellSource)
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
            if (Num.Length != 0)
            {
                Num = Reverse(Num);
                tempName = Reverse(tempName);
                tempNum = int.Parse(Num) + 1;
                newCellSource.Name = tempName + tempNum.ToString("d" + Num.Length.ToString());
            }
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
