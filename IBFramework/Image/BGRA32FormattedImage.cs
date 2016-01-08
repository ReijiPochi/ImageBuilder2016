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

            texNumber = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texNumber);
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Filter4Sgis);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);

            TextureUpdate();
        }

        public byte[] data;
        public IBRectangle size = new IBRectangle();
        public int texNumber;

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
            GL.BindTexture(TextureTarget.Texture2D, texNumber);
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                (int)size.Width, (int)size.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, data);
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
