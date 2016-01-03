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

            // glControlが表示されてからテクスチャを生成しないといけない
            textureNumber = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureNumber);
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);

            Tabs = GetTemplateChild("Tabs") as IBTabControl;
            Tabs.ItemsChanged += Tabs_ItemsChanged;
            Tabs.SelectionChanged += Tabs_SelectionChanged;
            Tabs.Items.Add(new SubTabItem() { isDummyItem = true, Header = "*** NoItems ***" });

            OpenedElements.CollectionChanged += OpenedElements_CollectionChanged;

            Render();
            //ResetTabs();
        }

        private WindowsFormsHost glControlHost;
        private GLControl glControl;
        private IBTabControl Tabs;
        private int textureNumber;

        public BGRA32FormattedImage RenderData = new BGRA32FormattedImage(1920, 1080, new PixelData() { b = 0, g = 0, r = 0, a = 0 });


        [Description("開かれているタイムラインエレメント"), Category("IBFramework")]
        public ObservableCollection<IBProjectElement> OpenedElements
        {
            get { return (ObservableCollection<IBProjectElement>)GetValue(OpenedElementsProperty); }
            set { SetValue(OpenedElementsProperty, value); }
        }
        public static readonly DependencyProperty OpenedElementsProperty =
            DependencyProperty.Register("OpenedElements", typeof(ObservableCollection<IBProjectElement>), typeof(IBCanvas), new PropertyMetadata(new ObservableCollection<IBProjectElement>()));


        public IBProjectElement ShowingElement
        {
            get { return (IBProjectElement)GetValue(ShowingElementProperty); }
            set { SetValue(ShowingElementProperty, value); }
        }
        public static readonly DependencyProperty ShowingElementProperty =
            DependencyProperty.Register("ShowingElement", typeof(IBProjectElement), typeof(IBCanvas), new PropertyMetadata(null));



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
            GL.ClearColor(0.0f, 1.0f, 1.0f, 1.0f);

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

            GL.BindTexture(TextureTarget.Texture2D, textureNumber);
            {
                GL.Begin(PrimitiveType.Quads);
                {
                    double w = 1920, h = 1080;
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(w, h);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(0, h);
                    GL.TexCoord2(0, 0);
                    GL.Vertex2(0, 0);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(w, 0);
                }
                GL.End();
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);

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

            Render();
        }


        private void GlControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //cur = e.Location;
                //if ((pre.X - cur.X) * (pre.X - cur.X) + (pre.Y - cur.Y) * (pre.Y - cur.Y) >= 2.0)
                //{
                    //pre = cur;
                    Render();
                //}
            }
        }


        private void Render()
        {
            RenderData.ClearData(new PixelData() { b = 0, g = 0, r = 0, a = 0 });

            if (Tabs.SelectedItem == null) return;
            if (((SubTabItem)Tabs.SelectedItem).Element as CellSource == null) return;

            foreach(IBImage i in ((CellSource)((SubTabItem)Tabs.SelectedItem).Element).Layers)
            {
                i.RenderTo(RenderData);
            }

            GL.BindTexture(TextureTarget.Texture2D, textureNumber);
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    (int)RenderData.size.Width, (int)RenderData.size.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, RenderData.data);
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);

            glControl.Refresh();
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

            Tabs_ItemsChanged_LOCK = false;
        }

        private bool Tabs_ItemsChanged_LOCK = false;
        /// <summary>
        /// Tabs.Itemsの変更をCellsに反映
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
