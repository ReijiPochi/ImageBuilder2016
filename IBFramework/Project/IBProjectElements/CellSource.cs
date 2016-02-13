using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;

using IBFramework.Image;
using IBFramework.Image.Pixel;
using IBFramework.RedoUndo;

namespace IBFramework.Project.IBProjectElements
{
    public class CellSource : IBProjectElement, IProperty
    {
        public CellSource() : base()
        {
            Type = IBProjectElementTypes.CellSource;
            PropertyHeaderName = "CellSource";
            PropertyChanged += CellSource_PropertyChanged;
        }

        private void CellSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
                PropertyHeaderName = "CellSource \"" + Name + "\"";
        }

        public ObservableCollection<IBImage> _Layers = new ObservableCollection<IBImage>();
        public ObservableCollection<IBImage> Layers
        {
            get { return _Layers; }
            set
            {
                if (_Layers == value)
                    return;
                _Layers = value;
                RaisePropertyChanged("Layers");
            }
        }

        public PixcelImage PixcelSelectedArea;
        public PixcelImage TempLayer;
        public bool IsPixcelSelecting { get; set; }

        public string PropertyHeaderName { get; set; }

        public Control GetPP()
        {
            return new CellSourcePP() { DataContext = this };
        }

        public void AddNewLayer(string name, bool RURecord = true, ImageTypes type = ImageTypes.LineDrawing)
        {
            IBImage l = null;
            switch (type)
            {
                case ImageTypes.Coloring:
                case ImageTypes.LineDrawing:
                    l = new PixcelImage(Width + 300, Height + 300, -150, -150);
                    l.imageData.ClearData(new PixelData() { r = 255, g = 255, b = 255, a = 0 });
                    break;

                case ImageTypes.SingleColor:
                    l = new SingleColorImage(255, 255, 255, 255);
                    l.Rect = new IBRectangle(1920 + 300, 1080 + 300, -150, -150);
                    break;

                case ImageTypes.Pixel:
                    l = new PixcelImage();
                    break;

                default:
                    break;
            }

            if (l == null) return;

            l.LayerName = name;
            l.LayerType = type;
            l.IsSelectedLayer = true;
            l.owner = this;
            l.imageData.TextureUpdate();

            foreach (IBImage i in Layers)
            {
                if(i.IsSelectedLayer)
                {
                    i.IsSelectedLayer = false;
                    int index = Layers.IndexOf(i);
                    Layers.Insert(index, l);

                    if (RURecord) RedoUndoManager.Current.Record(new RUAddNewLayer(this, l));

                    return;
                }
            }

            if (RURecord) RedoUndoManager.Current.Record(new RUAddNewLayer(this, l));

            Layers.Insert(0, l);
        }

        public void SetDrawingModeLayers()
        {
            foreach(IBImage l in Layers)
            {
                if (l.LayerType != ImageTypes.SingleColor && l.IsNotSelectersLayer)
                    l.imageData.SetDrawingMode();
            }
        }

        public void EndDrawingModeLayers()
        {
            foreach (IBImage l in Layers)
            {
                if (l.LayerType != ImageTypes.SingleColor && l.IsNotSelectersLayer)
                    l.imageData.EndDrawingMode();
            }
        }

        class RUAddNewLayer : RedoUndoAction
        {
            public RUAddNewLayer(CellSource owner, IBImage newLayer)
            {
                Owner = owner;
                trgLayer = newLayer;
                index = Owner.Layers.IndexOf(trgLayer);
            }

            CellSource Owner;
            IBImage trgLayer;
            int index;

            public override void Undo()
            {
                base.Undo();

                Owner.Layers.Remove(trgLayer);
                trgLayer.owner = null;
            }

            public override void Redo()
            {
                base.Redo();

                Owner.Layers.Insert(index, trgLayer);
                trgLayer.owner = Owner;
            }
        }
    }
}
