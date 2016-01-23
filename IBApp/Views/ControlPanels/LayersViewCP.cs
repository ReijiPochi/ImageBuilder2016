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
using Livet;

namespace IBApp.Views.ControlPanels
{
    public class LayersViewCP : IBControlPanelBase
    {
        static LayersViewCP()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayersViewCP), new FrameworkPropertyMetadata(typeof(LayersViewCP)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            view = GetTemplateChild("View") as ListView;
            view.SelectionChanged += View_SelectionChanged;
        }

        private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SetTargetLayerCommand != null)
            {
                if (SetTargetLayerCommand.CanExecute(null))
                {
                    SetTargetLayerCommand.Execute(null);
                }
            }
        }



        public ICommand SetTargetLayerCommand
        {
            get { return (ICommand)GetValue(SetTargetLayerCommandProperty); }
            set { SetValue(SetTargetLayerCommandProperty, value); }
        }
        public static readonly DependencyProperty SetTargetLayerCommandProperty =
            DependencyProperty.Register("SetTargetLayerCommand", typeof(ICommand), typeof(LayersViewCP), new PropertyMetadata(null));



        ListView view;
    }
}
