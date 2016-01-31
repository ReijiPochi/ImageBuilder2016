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

        public Control GetPP()
        {
            return new CellSourcePP() { DataContext = this };
        }

        public void AddNewLayer()
        {
            PixcelImage pi = new PixcelImage(Width + 300, Height + 300, -150, -150);
            pi.imageData.ClearData(new PixelData() { r = 255, g = 255, b = 255, a = 0 });
            pi.LayerName = "Layer";
            pi.LayerType = ImageTypes.LineDrawing;
            pi.IsSelectedLayer = true;
            pi.imageData.TextureUpdate();

            Layers.Insert(0, pi);
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
    }
}
