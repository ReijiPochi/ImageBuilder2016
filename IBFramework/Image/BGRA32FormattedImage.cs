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
        /// <param name="color"></param>
        public BGRA32FormattedImage(int w, int h, PixelData color)
        {
            size.Width = w;
            size.Height = h;
            size.OffsetX = 0;
            size.OffsetY = 0;

            data = new byte[w * h * 4];

            ClearData(color);

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
        /// 先に、IBCanvasが表示されておかないと、テクスチャの生成に失敗します。
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public BGRA32FormattedImage(int w, int h)
        {
            size.Width = w;
            size.Height = h;
            size.OffsetX = 0;
            size.OffsetY = 0;

            data = new byte[w * h * 4];

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

        /// <summary>
        /// データを書き換えた後に、TextureUpdate()をコールしてください
        /// </summary>
        public byte[] data;

        public IBRectangle size = new IBRectangle();

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
                (int)size.Width, (int)size.Height, 0,
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
