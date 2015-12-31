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

using IBGUI;
using IBFramework.Project;

namespace IBApp.Views.ControlPanels
{
    public class IBProjectViewCP : Control
    {
        static IBProjectViewCP()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBProjectViewCP), new FrameworkPropertyMetadata(typeof(IBProjectViewCP)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            tv = GetTemplateChild("TreeView") as IBTreeView;
            tv.SelectedItemChanged += Tv_SelectedItemChanged;
            tv.MouseLeftButtonDown += Tv_MouseLeftButtonDown;

            Unloaded += IBProjectViewCP_Unloaded;
        }

        private void IBProjectViewCP_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }

        IBTreeView tv;

        public IBProjectElement SelectedElement
        {
            get { return (IBProjectElement)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }
        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof(IBProjectElement), typeof(IBProjectViewCP), new PropertyMetadata(null));


        private void Tv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tv.SelectedItem == null) return;

            ((IBProjectElement)tv.SelectedItem).IsSelected = false;
        }

        private void Tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedElement = e.NewValue as IBProjectElement;
        }
    }
}
