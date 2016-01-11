﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBFramework.Image;
using IBFramework.Timeline.TimelineElements;
using IBFramework.Project.IBProjectElements;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace IBFramework.OpenGL
{
    public class Render
    {
        public static void DrawOneImage(IBImage i, double zoomPerCent, ref double layer)
        {
            double zoom = zoomPerCent / 100.0;

            IBFramework.OpenGL.Texture.BindTexture(i.imageData.textureNumber);
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

                    GL.TexCoord2(texMax, texMin);
                    GL.Vertex3(offsetX + w, offsetY + h, layer);
                    GL.TexCoord2(texMin, texMin);
                    GL.Vertex3(offsetX, offsetY + h, layer);
                    GL.TexCoord2(texMin, texMax);
                    GL.Vertex3(offsetX, offsetY, layer);
                    GL.TexCoord2(texMax, texMax);
                    GL.Vertex3(offsetX + w, offsetY, layer);
                }
                GL.End();
                layer += 0.01;
            }
            //IBFramework.OpenGL.Texture.BindTexture(0);
        }

        public static void RenderCellSource(CellSource c, double zoomPerCent, ref double layer)
        {
            if (c == null) return;

            for (int index = c.Layers.Count - 1; index >= 0; index--)
            {
                IBImage i = c.Layers[index];
                DrawOneImage(i, zoomPerCent, ref layer);
            }
        }

        public static void RenderCell(Cell c, double zoomPerCent, ref double layer, int x, int y, int offsetX, int offsetY)
        {
            double zoom = zoomPerCent / 100.0;

            int w = (int)(c.Width * zoom);
            int h = (int)(c.Height * zoom);
            int xc = (int)(x - c.Width * zoom * 0.5);
            int yc = (int)(y - c.Height * zoom * 0.5);

            GL.Viewport(xc - offsetX, yc - offsetY, w, h);
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 proj = Matrix4.CreateOrthographic(w, h, 0.01f, 64.0f);
            GL.LoadMatrix(ref proj);

            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 look = Matrix4.LookAt(new Vector3(w/2, h/2, 32.0f), new Vector3(w/2, h/2, 0.0f), Vector3.UnitY);
            GL.LoadMatrix(ref look);

            RenderCellSource(c.Source, zoomPerCent, ref layer);
        }
    }
}