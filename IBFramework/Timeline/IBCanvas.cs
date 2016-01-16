﻿using System;
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
using IBFramework.OpenGL;
using Wintab;

namespace IBFramework.Timeline
{
    public partial class IBCanvas : Control
    {
        static IBCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IBCanvas), new FrameworkPropertyMetadata(typeof(IBCanvas)));
        }
        private static List<IBCanvas> all = new List<IBCanvas>();

        public static void RefreshAll()
        {
            foreach (IBCanvas c in all)
                c.glControl.Refresh();
        }

        public IBCanvas()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                glControl = new GLControl(new GraphicsMode(GraphicsMode.Default.AccumulatorFormat, 24, 0, 0));
                glControl.Load += GlControl_Load;
                glControl.SizeChanged += GlControl_SizeChanged;
                glControl.Paint += GlControl_Paint;
                glControl.MouseEnter += GlControl_MouseEnter;
                glControl.MouseDown += GlControl_MouseDown;
                glControl.MouseMove += GlControl_MouseMove;
                glControl.MouseWheel += GlControl_MouseWheel;
                glControl.LostFocus += GlControl_LostFocus;
                glControl.MouseLeave += GlControl_MouseLeave;
                glControl.MouseUp += GlControl_MouseUp;
                Application.Current.Exit += Current_Exit;

                System.Drawing.Bitmap cur = new System.Drawing.Bitmap("cursorTest.png");
                IntPtr h = cur.GetHicon();
                var icon = System.Drawing.Icon.FromHandle(h);
                glControl.Cursor = new System.Windows.Forms.Cursor(icon.Handle);

                all.Add(this);
            }
        }

        ~IBCanvas()
        {
            all.Remove(this);
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            // GLコントロールがDisposeされる前にコールしないとだめらしい
            BGRA32FormattedImage.FinalizeBGRA32FormattedImage();
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
        internal GLControl glControl;
        private bool IsCurrentGLControl = false;
        private IBTabControl Tabs;
        private int zoomListIndex = 5;
        private double[] zoomList = new double[] { 10, 12.5, 25, 100 / 3, 50, 200 / 3, 75, 100, 150, 200, 300, 400, 500, 600, 800 };
        private bool PenDraging;
        int preX, preY;
        private int offsetX = 0, offsetY = 0;
        internal int camX = 0, camY = 0;


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

        private void GlControl_LostFocus(object sender, EventArgs e)
        {
            IsCurrentGLControl = false;
            if (Brush != null)
                Brush.EndRequest();
        }

        private void GlControl_SizeChanged(object sender, EventArgs e)
        {
            glControl.MakeCurrent();

            SetCam();
            glControl.Refresh();
        }

        private void GlControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!IsCurrentGLControl)
            {
                glControl.MakeCurrent();
                IsCurrentGLControl = true;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (ShowingElement != null)
            {
                GL.Enable(EnableCap.Texture2D);
                RenderAll();

                GL.Disable(EnableCap.Texture2D);
                GL.Color3(1.0f, 0.3f, 1.0f);
                DrawOuterCenterMark();
                DrawCornerMark();
                DrawCinemaScopeFrame();
                GL.Color3(0.3f, 0.3f, 0.3f);
                DrawImageFrame();
                GL.Color3(1.0f, 1.0f, 1.0f);
            }

            glControl.SwapBuffers();
        }

        private void GlControlHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            glControlHost.Child.Refresh();
        }

        private void GlControl_MouseEnter(object sender, EventArgs e)
        {
            //glControl.Focus();
        }

        private void GlControl_MouseLeave(object sender, EventArgs e)
        {
            //this.Focus();
        }

        private void GlControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            preX = e.X;
            preY = e.Y;
            if (Brush != null)
                Brush.Set(this, ShowingElement, GetImageCoord(this, e.Location, ZoomPerCent / 100.0));
        }

        private void GlControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (WintabUtility.PenUsing) return;

            if(e.Delta > 0)
            {
                if (zoomListIndex + 1 < zoomList.Length)
                    zoomListIndex++;
            }
            else
            {
                if (zoomListIndex > 1)
                    zoomListIndex--;
            }

            ZoomPerCent = zoomList[zoomListIndex];
            SetCam();
            glControl.Refresh();
        }

        private void GlControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                offsetX -= e.X - preX;
                offsetY += e.Y - preY;
                SetCam();
                glControl.Refresh();

                preX = e.X;
                preY = e.Y;
            }
            else if (WintabUtility.PenButtonPressed)
            {
                if (!PenDraging)
                {
                    preX = e.X;
                    preY = e.Y;
                    PenDraging = true;
                }
                offsetX -= (e.X - preX) * 2;
                offsetY += (e.Y - preY) * 2;
                SetCam();
                glControl.Refresh();

                preX = e.X;
                preY = e.Y;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (Brush == null || ShowingElement == null) return;

                Brush.Draw(GetImageCoord(this, e.Location, ZoomPerCent / 100.0));
            }
            else
            {
                PenDraging = false;
            }
        }

        private void GlControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Brush != null)
                Brush.EndRequest();
        }

        private void OpenedElements_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ResetTabs();
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(IBProjectElement ele in OpenedElements)
            {
                if (ele.Type == IBProjectElementTypes.CellSource)
                    ((CellSource)ele).EndDrawingModeLayers();
            }

            if (Tabs.SelectedItem != null)
            {
                ShowingElement = ((SubTabItem)Tabs.SelectedItem).Element;
                if(ShowingElement != null && ShowingElement.Type == IBProjectElementTypes.CellSource)
                {
                    ((CellSource)ShowingElement).SetDrawingModeLayers();
                }
            }
            else
                ShowingElement = null;

            glControl.Refresh();
        }


        private void RenderAll()
        {
            double layer = 0;

            switch (ShowingElement.Type)
            {
                case IBProjectElementTypes.CellSource:
                    Render.RenderCellSource(ShowingElement as CellSource, ZoomPerCent, ref layer);
                    break;

                case IBProjectElementTypes.Cell:
                    Render.RenderCell(ShowingElement as Cell, ZoomPerCent, ref layer, glControl.Width / 2, glControl.Height / 2, offsetX, offsetY);
                    SetCam();
                    break;

                default:
                    break;
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
            int h = glControl.Height;
            int w = glControl.Width;

            if (w % 2 != 0) glControl.Width++;
            if (h % 2 != 0) glControl.Height++;

            int x = w / 2;
            int y = h / 2;

            int xc = (int)(x - 1920 * ZoomPerCent * 0.01 * 0.5);
            int yc = (int)(y - 1080 * ZoomPerCent * 0.01 * 0.5);
            camX = offsetX + x - xc;
            camY = offsetY + y - yc;

            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 proj = Matrix4.CreateOrthographic(w, h, 0.01f, 64.0f);
            GL.LoadMatrix(ref proj);

            GL.MatrixMode(MatrixMode.Modelview);

            Matrix4 look = Matrix4.LookAt(new Vector3(camX, camY, 32.0f), new Vector3(camX, camY, 0.0f), Vector3.UnitY);
            GL.LoadMatrix(ref look);
        }
    }
}
