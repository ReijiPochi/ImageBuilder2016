using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace IBFramework.OpenGL
{
    public class Texture
    {
        private static int CurrentBindingTexture = 0;

        public static void BindTexture(int texNum)
        {
            if(CurrentBindingTexture != texNum)
            {
                GL.BindTexture(TextureTarget.Texture2D, texNum);
                CurrentBindingTexture = texNum;
            }
        }
    }
}
