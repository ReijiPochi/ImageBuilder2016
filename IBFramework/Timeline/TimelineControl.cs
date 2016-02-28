using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;

using IBGUI;

namespace IBFramework.Timeline
{
    public class TimelineControl : Control
    {
        static TimelineControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineControl), new FrameworkPropertyMetadata(typeof(TimelineControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Tabs = GetTemplateChild("Tabs") as IBTabControl;
            Tabs.ItemsChanged += Tabs_ItemsChanged;
            Tabs.SelectionChanged += Tabs_SelectionChanged;
            Tabs.Items.Add(new SubTabItem() { isDummyItem = true, Header = "*** NoItems ***" });
        }

        private IBTabControl Tabs;

        [Description("開かれているエレメント"), Category("IBFramework")]
        public ObservableCollection<TimelineElement> OpenedElements
        {
            get { return (ObservableCollection<TimelineElement>)GetValue(OpenedElementsProperty); }
            set { SetValue(OpenedElementsProperty, value); }
        }
        public static readonly DependencyProperty OpenedElementsProperty =
            DependencyProperty.Register("OpenedElements", typeof(ObservableCollection<TimelineElement>), typeof(TimelineControl), new PropertyMetadata(new ObservableCollection<TimelineElement>()));


        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        #region Tabs と OpenedElements のバインド
        private bool ResetTabs_LOCK = false;
        /// <summary>
        /// OpenedElementsの変更をTabs.Itemsに反映
        /// </summary>
        private void ResetTabs()
        {
            if (ResetTabs_LOCK) return;

            Tabs_ItemsChanged_LOCK = true;

            Tabs.Items.Clear();

            // OpenedElementsからItemsに追加
            for (int count = 0; count < OpenedElements.Count; count++)
            {
                TimelineElement trgC = OpenedElements[count];

                SubTabItem s = new SubTabItem();
                s.Element = trgC;
                s.Header = trgC.Name;
                s.LockWidth = true;
                Tabs.Items.Add(s);
            }

            if (Tabs.Items.Count != 0)
            {
                ((SubTabItem)Tabs.Items[Tabs.Items.Count - 1]).IsSelected = true;
            }

            Tabs_ItemsChanged_LOCK = false;
        }

        private bool Tabs_ItemsChanged_LOCK = false;
        /// <summary>
        /// Tabs.Itemsの変更をOpenedElementsに反映
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tabs_ItemsChanged(object sender, IBGUI.ItemsChangedEventArgs e)
        {
            if (Tabs_ItemsChanged_LOCK) return;

            ResetTabs_LOCK = true;

            OpenedElements.Clear();

            // ItemsからOpenedElementsに追加
            foreach (SubTabItem s in Tabs.Items)
            {
                if (s.isDummyItem)
                {
                    ResetTabs_LOCK = false;
                    return;
                }

                TimelineElement c = s.Element as TimelineElement;
                if (c == null) break;

                OpenedElements.Add(c);

            }

            ResetTabs_LOCK = false;
        }
        #endregion
    }
}
