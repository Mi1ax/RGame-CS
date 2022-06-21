using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace RGame.Core
{
    public class RenderTarget
    {
        private RenderTexture2D m_RenderTexture2D;
        private int m_Width, m_Height;

        public RenderTarget()
        {
            m_Width = 0;
            m_Height = 0;
        }

        public RenderTexture2D GetTarget() => m_RenderTexture2D;

        public int GetWidth() => m_Width;
        public int GetHeight() => m_Height;

        public IntPtr GetTextureIDPtr() => new (m_RenderTexture2D.texture.id);

        public void Resize(Vector2 Size) => Resize((int)Size.X, (int)Size.Y);
        public void Resize(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new Exception($"Attemted to resize RenderTarget to {width}:{height}");

            m_Width = width;
            m_Height = height;
            UnloadRenderTexture(m_RenderTexture2D);
            m_RenderTexture2D = LoadRenderTexture(m_Width, m_Height);
        }

        public void Free() => UnloadRenderTexture(m_RenderTexture2D); 

        public static explicit operator RenderTexture2D(RenderTarget target) => target.m_RenderTexture2D;
    }
}
