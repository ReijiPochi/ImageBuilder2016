using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace IBGUI
{
    public class IBPanel : Grid
    {
        public IBPanel()
        {
            AllIBPanelList.Add(this);
        }

        /// <summary>
        /// このIBPanleの現在の位置
        /// </summary>
        public Position CurrentPos { get; set; } = Position.center;

        private IBPanelSplitter PS = new IBPanelSplitter { Focusable = false };

        public IBTabControl TC;

        public IBPanel P1;
        public IBPanel P2;

        private static List<IBPanel> AllIBPanelList = new List<IBPanel>();

        /// <summary>
        /// このIBPanelにIBTabControlを移動します
        /// </summary>
        /// <param name="addIBTabControl">移動するIBTabControl</param>
        /// <param name="pos">移動させる場所</param>
        public void AddIBTabControl(IBTabControl tc2, Position pos)
        {
            if (RowDefinitions.Count > 1 || ColumnDefinitions.Count > 1)
                throw new IBLayoutException("この IBGrid に、これ以上 IBTabControl を追加できません");

            if (TC == null && Children.Count != 0)
            {
                TC = Children[0] as IBTabControl;
            }

            if (TC != null) TC.RemoveFromParent();
            tc2.RemoveFromParent();

            switch (pos)
            {
                case Position.left:
                    AddToLeft(tc2);
                    break;

                case Position.right:
                    AddToRight(tc2);
                    break;

                case Position.top:
                    AddToTop(tc2);
                    break;

                case Position.bottom:
                    AddToBottom(tc2);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// このIBPanelを親のIBPanelから削除します
        /// </summary>
        public void RemoveIBPanel()
        {
            IBPanel parent = Parent as IBPanel;
            if (parent == null)
                return;

            IBPanel p = null;
            if (parent.P1 == this) p = parent.P2;
            else p = parent.P1;

            if (parent.P1 != null)
            {
                parent.P1.RemoveFromParent();
                parent.P1 = null;
            }

            if (parent.P2 != null)
            {
                parent.P2.RemoveFromParent();
                parent.P2 = null;
            }

            parent.Children.Clear();

            IBTabControl tc = null;
            if (p.Children.Count != 0) tc = p.Children[0] as IBTabControl;

            if (tc != null)
            {
                p.Children.Remove(tc);
                parent.Children.Add(tc);

                parent.TC = tc;

                parent.ColumnDefinitions.Clear();
                parent.RowDefinitions.Clear();
            }
            else
            {
                Window parent_parentW = parent.Parent as Window;
                IBPanel parent_parentP = parent.Parent as IBPanel;
                IBWorkspace parent_parentWS = parent.Parent as IBWorkspace;

                if (parent_parentW != null)
                {
                    parent_parentW.Content = p;
                }

                if (parent_parentP != null)
                {
                    parent_parentP.Children.Add(p);

                    IBPanel panel = null;
                    foreach (object o in parent_parentP.Children)
                    {
                        panel = o as IBPanel;
                        if (panel != null)
                        {
                            if (panel.Children.Count == 0)
                                break;
                        }
                    }
                    parent_parentP.Children.Remove(panel);
                }

                if (parent_parentWS != null)
                {
                    parent_parentWS.Content = p;
                }
            }
        }

        public void SetColumnRow()
        {
            switch (CurrentPos)
            {
                case Position.left:
                    SetValue(ColumnProperty, 0);
                    SetValue(ColumnSpanProperty, 1);
                    SetValue(RowProperty, 0);
                    SetValue(RowSpanProperty, 1);
                    break;

                case Position.right:
                    SetValue(ColumnProperty, 1);
                    SetValue(ColumnSpanProperty, 2);
                    SetValue(RowProperty, 0);
                    SetValue(RowSpanProperty, 1);
                    break;

                case Position.top:
                    SetValue(ColumnProperty, 0);
                    SetValue(ColumnSpanProperty, 1);
                    SetValue(RowProperty, 0);
                    SetValue(RowSpanProperty, 1);
                    break;

                case Position.bottom:
                    SetValue(ColumnProperty, 0);
                    SetValue(ColumnSpanProperty, 1);
                    SetValue(RowProperty, 1);
                    SetValue(RowSpanProperty, 2);
                    break;

                case Position.center:
                    SetValue(ColumnProperty, 0);
                    SetValue(ColumnSpanProperty, 1);
                    SetValue(RowProperty, 0);
                    SetValue(RowSpanProperty, 1);
                    break;

                default:
                    break;
            }
        }

        private void AddToLeft(IBTabControl tc2)
        {
            if (P1 == null)
            {
                P1 = new IBPanel();
                P1.Children.Add(tc2);
                P1.TC = tc2;
            }
            else
                P1.RemoveFromParent();

            if (P2 == null)
            {
                P2 = new IBPanel();
                P2.Children.Add(TC);
                P2.TC = TC;
            }
            else
                P2.RemoveFromParent();

            ColumnDefinition newColumn1 = new ColumnDefinition() { Width = new GridLength(200, GridUnitType.Pixel) , MinWidth = 50.0};
            ColumnDefinition newColumn2 = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), MinWidth = 50.0 };

            ColumnDefinitions.Add(newColumn1);
            ColumnDefinitions.Add(newColumn2);

            P1.CurrentPos = Position.left;
            P1.SetColumnRow();

            PS.Height = double.NaN;
            PS.Margin = new Thickness(-3, 0, 0, 0);
            PS.VerticalAlignment = VerticalAlignment.Stretch;
            PS.HorizontalAlignment = HorizontalAlignment.Left;
            PS.SetValue(ColumnProperty, 1);

            P2.CurrentPos = Position.right;
            P2.SetColumnRow();

            Children.Add(P1);
            Children.Add(PS);
            Children.Add(P2);
        }

        private void AddToRight(IBTabControl tc2)
        {
            if (P1 == null)
            {
                P1 = new IBPanel();
                P1.Children.Add(TC);
                P1.TC = TC;
            }
            else
                P1.RemoveFromParent();

            if (P2 == null)
            {
                P2 = new IBPanel();
                P2.Children.Add(tc2);
                P2.TC = tc2;
            }
            else
                P2.RemoveFromParent();

            ColumnDefinition newColumn1 = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), MinWidth = 50.0 };
            ColumnDefinition newColumn2 = new ColumnDefinition() { Width = new GridLength(200, GridUnitType.Pixel), MinWidth = 50.0 };

            ColumnDefinitions.Add(newColumn1);
            ColumnDefinitions.Add(newColumn2);

            P1.CurrentPos = Position.left;
            P1.SetColumnRow();

            PS.Height = double.NaN;
            PS.Margin = new Thickness(0, 0, -3, 0);
            PS.VerticalAlignment = VerticalAlignment.Stretch;
            PS.HorizontalAlignment = HorizontalAlignment.Right;
            PS.SetValue(ColumnProperty, 0);

            P2.CurrentPos = Position.right;
            P2.SetColumnRow();

            Children.Add(P1);
            Children.Add(PS);
            Children.Add(P2);
        }

        private void AddToTop(IBTabControl tc2)
        {
            if (P1 == null)
            {
                P1 = new IBPanel();
                P1.Children.Add(tc2);
                P1.TC = tc2;
            }
            else
                P1.RemoveFromParent();

            if (P2 == null)
            {
                P2 = new IBPanel();
                P2.Children.Add(TC);
                P2.TC = TC;
            }
            else
                P2.RemoveFromParent();

            RowDefinition newRow1 = new RowDefinition() { Height = new GridLength(200, GridUnitType.Pixel), MinHeight = 50.0 };
            RowDefinition newRow2 = new RowDefinition() { Height = new GridLength(1, GridUnitType.Star), MinHeight = 50.0 };

            RowDefinitions.Add(newRow1);
            RowDefinitions.Add(newRow2);

            P1.CurrentPos = Position.top;
            P1.SetColumnRow();

            PS.Width = double.NaN;
            PS.Margin = new Thickness(0, -3, 0, 0);
            PS.VerticalAlignment = VerticalAlignment.Top;
            PS.HorizontalAlignment = HorizontalAlignment.Stretch;
            PS.SetValue(RowProperty, 1);

            P2.CurrentPos = Position.bottom;
            P2.SetColumnRow();

            Children.Add(P1);
            Children.Add(PS);
            Children.Add(P2);
        }

        private void AddToBottom(IBTabControl tc2)
        {
            if (P1 == null)
            {
                P1 = new IBPanel();
                P1.Children.Add(TC);
                P1.TC = TC;
            }
            else
                P1.RemoveFromParent();

            if (P2 == null)
            {
                P2 = new IBPanel();
                P2.Children.Add(tc2);
                P2.TC = tc2;
            }
            else
                P2.RemoveFromParent();

            RowDefinition newRow1 = new RowDefinition() { Height = new GridLength(1, GridUnitType.Star), MinHeight = 50.0 };
            RowDefinition newRow2 = new RowDefinition() { Height = new GridLength(200, GridUnitType.Pixel), MinHeight = 50.0 };

            RowDefinitions.Add(newRow1);
            RowDefinitions.Add(newRow2);

            P1.CurrentPos = Position.top;
            P1.SetColumnRow();

            PS.Width = double.NaN;
            PS.Margin = new Thickness(0, 0, 0, -3);
            PS.VerticalAlignment = VerticalAlignment.Bottom;
            PS.HorizontalAlignment = HorizontalAlignment.Stretch;
            PS.SetValue(RowProperty, 0);

            P2.CurrentPos = Position.bottom;
            P2.SetColumnRow();

            Children.Add(P1);
            Children.Add(PS);
            Children.Add(P2);
        }

        /// <summary>
        /// 全てのIBPanelを調べてレイアウトの不整合を修正します
        /// </summary>
        public static void ResetLayout()
        { 
            foreach(IBPanel p in AllIBPanelList)
            {
                IBPanel p1 = null, p2 = null;
                foreach (object o in p.Children)
                {
                    if (p1 == null)
                    {
                        p1 = o as IBPanel;
                    }
                    else if (p2 == null)
                    {
                        p2 = o as IBPanel;
                    }
                }

                if (p1 != null && p2 != null)
                {
                    switch (p1.CurrentPos)
                    {
                        case Position.left:
                            p2.CurrentPos = Position.right;
                            break;

                        case Position.right:
                            p2.CurrentPos = Position.left;
                            break;

                        case Position.top:
                            p2.CurrentPos = Position.bottom;
                            break;

                        case Position.bottom:
                            p2.CurrentPos = Position.top;
                            break;

                        default:
                            break;
                    }

                    p1.SetColumnRow();
                    p2.SetColumnRow();

                    if (p.TC != null) p.TC.EndDrag();
                    if (p1.TC != null) p1.TC.EndDrag();
                    if (p2.TC != null) p2.TC.EndDrag();

                    if (p.P1 == null) p.P1 = p1;
                    if (p.P2 == null) p.P2 = p2;
                }
            }
        }

        /// <summary>
        /// 指定された位置にあるIBPanelを検索して返します
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IBPanel GetChildIBPanelAt(Position p)
        {
            IBPanel result = null;

            foreach(UIElement u in Children)
            {
                result = u as IBPanel;

                if(result != null)
                {
                    if (result.CurrentPos == p)
                        return result;
                }
            }

            return result;
        }

        public void RemoveFromParent()
        {
            Panel parent = Parent as Panel;
            Window parentW = Parent as Window;
            IBWorkspace parentWS = Parent as IBWorkspace;

            if (parent != null)
                parent.Children.Remove(this);

            if (parentW != null)
                parentW.Content = null;

            if (parentWS != null)
                parentWS.Content = null;

        }

        public object GetVisualParent()
        {
            return VisualParent;
        }
    }
}
