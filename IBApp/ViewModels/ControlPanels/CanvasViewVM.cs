using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using System.Collections.ObjectModel;

using IBApp.Models;
using IBFramework.Image;
using IBFramework.Image.Blend;
using IBFramework.Timeline;
using IBFramework.Timeline.TimelineElements;

namespace IBApp.ViewModels.ControlPanels
{
    public class CanvasViewVM : ViewModel
    {
        public CanvasViewVM()
        {
            //SingleColorImage background = new SingleColorImage(100, 100, 100, 255);
            //background.size.Height = 1080;
            //background.size.Width = 1920;

            //SingleColorImage test = new SingleColorImage(10, 10, 48, 255);
            //test.size.Height = 1000;
            //test.size.Width = 200;
            //test.size.OffsetX = 200;
            //test.size.OffsetY = 200;
            //test.blendMode = new Add();

            //SingleColorImage test2 = new SingleColorImage(100, 200, 19, 127);
            //test2.size.Height = 99;
            //test2.size.Width = 99;
            //test2.size.OffsetX = 200;
            //test2.size.OffsetY = 200;
            //test2.blendMode = new Normal();

            //Cell c = new Cell();
            //c.Layers.Add(background);
            //c.Layers.Add(test);
            //c.Name = "cell1";

            //Cell c2 = new Cell();
            //c2.Layers.Add(test2);
            //c2.Name = "cell2";

            //Cell c3 = new Cell();
            //c3.Layers.Add(background);
            //c3.Layers.Add(test2);
            //c3.Name = "cell3";

            //_Items.Add(c);
            //_Items.Add(c2);
            //_Items.Add(c3);
        }

        #region Itemsプロパティ
        private ObservableCollection<TimelineElement> _Items = new ObservableCollection<TimelineElement>();

        public ObservableCollection<TimelineElement> Items
        {
            get
            { return _Items; }
            set
            { 
                if (_Items == value)
                    return;
                _Items = value;
                //RaisePropertyChanged();
            }
        }
        #endregion

    }
}
