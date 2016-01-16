using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace IBFramework.Image
{
    public class BGRA32FormattedImage
    {
        private static List<BGRA32FormattedImage> allList = new List<BGRA32FormattedImage>();

        /// <summary>
        /// 先に、IBCanvasが表示されておかないと、テクスチャの生成に失敗します。
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public BGRA32FormattedImage(int w, int h)
        {
            imageSize.Width = w;
            imageSize.Height = h;
            imageSize.OffsetX = 0;
            imageSize.OffsetY = 0;

            data = new byte[4];

            textureNumber = GL.GenTexture();
            allList.Add(this);
            IBFramework.OpenGL.Texture.BindTexture(textureNumber);
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            }
            //IBFramework.OpenGL.Texture.BindTexture(0);

            TextureUpdate();
        }

        /// <summary>
        /// IBFramework.Timeline.IBCanvas の中でコールされます
        /// </summary>
        public static void FinalizeBGRA32FormattedImage()
        {
            IBFramework.OpenGL.Texture.BindTexture(0);

            foreach (BGRA32FormattedImage i in allList)
            {
                // GLコントロールがDisposeされる前にコールしないとだめらしい
                GL.DeleteTexture(i.textureNumber);
            }
        }

        public void SetDrawingMode()
        {
            if (CanDraw == true) return;

            byte[] buffer = new byte[(int)imageSize.Width * (int)imageSize.Height * 4];

            int drawXS = (int)drawingAreaSize.OffsetX;
            int drawXE = drawXS + (int)drawingAreaSize.Width;
            int drawYS = (int)drawingAreaSize.OffsetY;
            int drawYE = drawYS + (int)drawingAreaSize.Height;

            int yInData = 0;
            int xInData = 0;

            for (int y = drawYS; y < drawYE; y++)
            {
                int offsetData = yInData * (int)actualSize.Width * 4;
                int offset = y * (int)imageSize.Width * 4;
                for (int x = drawXS * 4; x < drawXE * 4; x++)
                {
                    buffer[offset + x] = data[offsetData + xInData];
                    xInData++;
                }
                xInData = 0;
                yInData++;
            }

            data = buffer;
            CanDraw = true;

            actualSize.OffsetX = imageSize.OffsetX;
            actualSize.OffsetY = imageSize.OffsetY;
            actualSize.Width = imageSize.Width;
            actualSize.Height = imageSize.Height;

            TextureUpdate();
        }

        public void EndDrawingMode()
        {
            if (CanDraw == false) return;
            CanDraw = false;

            int drawXS = 114514;
            int drawXE = 0;
            int drawYS = 114514;
            int drawYE = 0;

            for (int y = 0; y < actualSize.Height; y++)
            {
                int offset = y * (int)actualSize.Width * 4;
                for(int x = 0; x < actualSize.Width; x++)
                {
                    int index = offset + x * 4 + 3;
                    if (data[index] != 0)
                    {
                        if (y < drawYS) drawYS = y;
                        if (x < drawXS) drawXS = x;
                        if (y >= drawYE) drawYE = y + 1;
                        if (x >= drawXE) drawXE = x + 1;
                    }
                }
            }

            drawingAreaSize.OffsetX = drawXS;
            drawingAreaSize.OffsetY = drawYS;
            drawingAreaSize.Width = drawXE - drawXS;
            drawingAreaSize.Height = drawYE - drawYS;

            byte[] buffer;

            if (drawingAreaSize.Width < 0 || drawingAreaSize.Height < 0)
            {
                buffer = new byte[(int)actualSize.Width * (int)actualSize.Height * 4];
            }
            else
            {
                buffer = new byte[(int)drawingAreaSize.Width * (int)drawingAreaSize.Height * 4];
            }


            int yInData = drawYS;
            int xInDataOffset = drawXS * 4;

            for (int y = 0; y < drawingAreaSize.Height; y++)
            {
                int offset = y * (int)drawingAreaSize.Width * 4;
                int offsetData = yInData * (int)actualSize.Width * 4;
                for (int x = 0; x < drawingAreaSize.Width * 4; x++)
                {
                    buffer[offset + x] = data[offsetData + xInDataOffset + x];
                }

                yInData++;
            }

            data = buffer;

            actualSize.OffsetX = drawingAreaSize.OffsetX;
            actualSize.OffsetY = drawingAreaSize.OffsetY;
            actualSize.Width = drawingAreaSize.Width;
            actualSize.Height = drawingAreaSize.Height;

            TextureUpdate();
        }

        public bool CanDraw { get; private set; }

        /// <summary>
        /// データを書き換えた後に、TextureUpdate()をコールしてください
        /// </summary>
        public byte[] data;

        /// <summary>
        /// dataの実際の大きさ
        /// </summary>
        public IBRectangle actualSize = new IBRectangle(1, 1, 0, 0);

        /// <summary>
        /// 実際に画像が描かれている範囲
        /// </summary>
        public IBRectangle drawingAreaSize = new IBRectangle(1, 1, 0, 0);

        /// <summary>
        /// この画像の仮想的な大きさ
        /// </summary>
        public IBRectangle imageSize = new IBRectangle();

        public int textureNumber;

        public void ClearData(PixelData color)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color.b;
                i++;
                data[i] = color.g;
                i++;
                data[i] = color.r;
                i++;
                data[i] = color.a;
            }
        }

        public void TextureUpdate()
        {
            IBFramework.OpenGL.Texture.BindTexture(textureNumber);
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                (int)actualSize.Width, (int)actualSize.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, data);
            }
            //IBFramework.OpenGL.Texture.BindTexture(0);
        }

        public void TextureDelete()
        {
            IBFramework.OpenGL.Texture.BindTexture(textureNumber);
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                0, 0, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, new byte[0]);
            }
            //IBFramework.OpenGL.Texture.BindTexture(0);
        }
    }
}
