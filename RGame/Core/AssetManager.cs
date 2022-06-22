using Raylib_cs;
using static Raylib_cs.Raylib;

namespace RGame.Core;

public static class AssetManager
{
    public static TexturesManager? Textures;

    public class TexturesManager
    {
        private Dictionary<string, Texture2D> m_Textures = new ();
        
        public bool AddTexture(string name, Texture2D texture)
        {
            if (m_Textures.ContainsKey(name)) return false;
            m_Textures.Add(name, texture);
            return true;
        }

        public Texture2D GetTextureByName(string name) => m_Textures[name];

        public void UnloadAllTextures()
        {
            foreach (var (_, texture) in m_Textures)
                UnloadTexture(texture);
        }
    }

    public static void Load()
    {
        Textures = new TexturesManager();
    }

    public static void Unload()
    {
        Textures?.UnloadAllTextures();
    }
}