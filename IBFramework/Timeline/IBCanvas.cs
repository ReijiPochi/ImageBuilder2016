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

using System.Drawing.Imaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms.Integration;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using IBFramework.Image;
using IBFramework.Image.Blend;
using IBFramework.Timeline.TimelineElements;
using IBGUI;
using IBFramework.Project;
using IBFramework.Project.IBProjectElements;
using IBFramework.OpenCL;
using OpenCLFunctions;
using OpenCLFunctions.Utilities;

namespace IBFramework.Timeline
{
    public class IBCanvas : Control
    {
        static IBCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBCanvas), new FrameworkPropertyMetadata(typeof(IBCanvas)));
        }

        public IBCanvas()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                glControl = new GLControl(new GraphicsMode(GraphicsMode.Default.AccumulatorFormat, 24, 0, 0));
                glControl.Load += GlControl_Load;
                glControl.SizeChanged += GlControl_SizeChanged;
                glControl.Paint += GlControl_Paint;
                glControl.MouseMove += GlControl_MouseMove;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            glControlHost = GetTemplateChild("GLControlHost") as WindowsFormsHost;
            glControlHost.SizeChanged += GlControlHost_SizeChanged;
            glControlHost.Child = glControl;

            Tabs = GetTemplateChild("Tabs") as IBTabControl;
            Tabs.ItemsChanged += Tabs_ItemsChanged;
            Tabs.SelectionChanged += Tabs_SelectionChanged;
            Tabs.Items.Add(new SubTabItem() { isDummyItem = true, Header = "*** NoItems ***" });

            OpenedElements.CollectionChanged += OpenedElements_CollectionChanged;

            glControl.Refresh();
        }

        private WindowsFormsHost glControlHost;
        private GLControl glControl;
        private IBTabControl Tabs;


        [Description("開かれているエレメント"), Category("IBFramework")]
        public ObservableCollection<IBProjectElement> OpenedElements
        {
            get { return (ObservableCollection<IBProjectElement>)GetValue(OpenedElementsProperty); }
            set { SetValue(OpenedElementsProperty, value); }
        }
        public static readonly DependencyProperty OpenedElementsProperty =
            DependencyProperty.Register("OpenedElements", typeof(ObservableCollection<IBProjectElement>), typeof(IBCanvas), new PropertyMetadata(new ObservableCollection<IBProjectElement>()));

        [Description("表示している（選択されている）エレメント"), Category("IBFramework")]
        public IBProjectElement ShowingElement
        {
            get { return (IBProjectElement)GetValue(ShowingElementProperty); }
            set { SetValue(ShowingElementProperty, value); }
        }
        public static readonly DependencyProperty ShowingElementProperty =
            DependencyProperty.Register("ShowingElement", typeof(IBProjectElement), typeof(IBCanvas), new PropertyMetadata(null));

        [Description("ズーム"), Category("IBFramework")]
        public double ZoomPerCent
        {
            get { return (double)GetValue(ZoomPerCentProperty); }
            set { SetValue(ZoomPerCentProperty, value); }
        }
        public static readonly DependencyProperty ZoomPerCentProperty =
            DependencyProperty.Register("ZoomPerCent", typeof(double), typeof(IBCanvas), new PropertyMetadata(66.6));

        [Description("アクティブなブラシ"), Category("IBFramework")]
        public IBBrush Brush
        {
            get { return (IBBrush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }
        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register("Brush", typeof(IBBrush), typeof(IBCanvas), new PropertyMetadata(new Image.Pixel.Pen()));

        [Description("ドローイングエリアの背景色"), Category("IBFramework")]
        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(IBCanvas), new PropertyMetadata(Color.FromArgb(255,50,50,50), new PropertyChangedCallback(OnBackgroundColorChanged)));

        private static void OnBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IBCanvas sender = d as IBCanvas;
            if (sender != null)
            {
                sender.glControl.MakeCurrent();
                GL.ClearColor(sender.BackgroundColor.R / 255.0f, sender.BackgroundColor.G / 255.0f, sender.BackgroundColor.B / 255.0f, 255);
                sender.glControl.Refresh();
            }
        }

        private void GlControl_Load(object sender, EventArgs e)
        {
            glControl.MakeCurrent();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            SetCam();
        }

        private void GlControl_SizeChanged(object sender, EventArgs e)
        {
            glControl.MakeCurrent();

            SetCam();
            glControl.Refresh();
        }

        private void GlControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            glControl.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Render();

            glControl.SwapBuffers();
        }

        private void GlControlHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            glControlHost.Child.Refresh();
        }

        private void OpenedElements_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ResetTabs();
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tabs.SelectedItem != null)
                ShowingElement = ((SubTabItem)Tabs.SelectedItem).Element;

            glControl.Refresh();
        }


        private void GlControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //cur = e.Location;
                //if ((pre.X - cur.X) * (pre.X - cur.X) + (pre.Y - cur.Y) * (pre.Y - cur.Y) >= 2.0)
                //{
                //pre = cur;
                glControl.Refresh();
                //}
            }
        }


        private void Render()
        {
            if (Tabs.SelectedItem == null) return;
            if (((SubTabItem)Tabs.SelectedItem).Element as CellSource == null) return;

            double zoom = ZoomPerCent / 100.0;
            double layer = 0;

            for(int c = ((CellSource)((SubTabItem)Tabs.SelectedItem).Element).Layers.Count - 1; c >= 0; c--)
            {
                IBImage i = ((CellSource)((SubTabItem)Tabs.SelectedItem).Element).Layers[c];

                GL.BindTexture(TextureTarget.Texture2D, i.imageData.texNumber);
                {
                    GL.Begin(PrimitiveType.Quads);
                    {
                        double offsetX = i.Rect.OffsetX * zoom, offsetY = i.Rect.OffsetY * zoom;
                        double w = i.Rect.Width * zoom, h = i.Rect.Height * zoom;
                        double texMin = 0, texMax = 1.0;

                        if (i.LayerType == ImageTypes.SingleColor)
                        {
                            texMin = 0.5;
                            texMax = 0.5;
                        }

                        GL.TexCoord2(texMax, texMax);
                        GL.Vertex3(offsetX + w, offsetY + h, layer);
                        GL.TexCoord2(texMin, texMax);
                        GL.Vertex3(offsetX, offsetY + h, layer);
                        GL.TexCoord2(texMin, texMin);
                        GL.Vertex3(offsetX, offsetY, layer);
                        GL.TexCoord2(texMax, texMin);
                        GL.Vertex3(offsetX + w, offsetY, layer);
                    }
                    GL.End();
                    layer += 0.01;
                }
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
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
                IBProjectElement trgC = OpenedElements[count];

                SubTabItem s = new SubTabItem();
                s.Element = trgC;
                s.Header = trgC.Name;
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
        private void Tabs_ItemsChanged(object sender, ItemsChangedEventArgs e)
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

                IBProjectElement c = s.Element as IBProjectElement;
                if (c == null) break;

                OpenedElements.Add(c);

            }

            ResetTabs_LOCK = false;
        }
        #endregion

        private void SetCam()
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            GL.MatrixMode(MatrixMode.Projection);
            float h = glControl.Height;
            float w = glControl.Width;
            Matrix4 proj = Matrix4.CreateOrthographic(w, h, 0.01f, 64.0f);
            GL.LoadMatrix(ref proj);

            GL.MatrixMode(MatrixMode.Modelview);

            float x = w / 2;
            float y = h / 2;
            Matrix4 look = Matrix4.LookAt(new Vector3(x, y, 32.0f), new Vector3(x, y, 0.0f), Vector3.UnitY);
            GL.LoadMatrix(ref look);
        }
    }
}
