using System.Numerics;
using Raylib_cs;
using RGame.Core;
using static Raylib_cs.Raylib;

namespace RGame.Game;

public enum BlockType : byte
{
    Air = 0,
    Dirt = 1
}

public class Block
{
    private BlockType m_Type;
    private Texture2D? m_Texture;
    private Vector3 m_Position;
    private RColor m_Color;
    private float m_Scale;

    public bool Active = false;

    private bool m_EnableFront = false;
    private bool m_EnableBack = false;
    private bool m_EnableTop = false;
    private bool m_EnableBottom = false;
    private bool m_EnableRight = false;
    private bool m_EnableLeft = false;

    public ref bool DrawFront => ref m_EnableFront;
    public ref bool DrawBack => ref m_EnableBack;
    public ref bool DrawTop => ref m_EnableTop;
    public ref bool DrawBottom => ref m_EnableBottom;
    public ref bool DrawRight => ref m_EnableRight;
    public ref bool DrawLeft => ref m_EnableLeft;

    public ref Vector3 Position => ref m_Position;
    public ref float Scale => ref m_Scale;
    public BlockType Type => m_Type;
    public Texture2D? Texture => m_Texture;
    public RColor TintColor => m_Color;

    public Block(BlockType type = BlockType.Air)
    {
        m_Type = type;
        m_Texture = type switch
        {
            BlockType.Air => null,
            BlockType.Dirt => AssetManager.Textures!.GetTextureByName("Dirt"),
            _ => null
        };
        m_Position = Vector3.Zero;
        m_Color = Color.WHITE;
        m_Scale = 1.0f;
    }

    public void Draw(bool debug = false)
    {
        if (!Active) return;
        if (m_Texture == null) return;
            
        Renderer.DrawBlock(this, m_EnableFront, m_EnableBack, m_EnableTop, m_EnableBottom, m_EnableRight, m_EnableLeft);
    }
}