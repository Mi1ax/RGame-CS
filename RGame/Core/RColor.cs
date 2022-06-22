using System.Numerics;
using Raylib_cs;

namespace RGame.Core;

public class RColor
{
    private Vector4 m_Color;

    public RColor(float r, float g, float b, float a)
    {
        m_Color = new Vector4(r, g, b, a);
    }

    public ref Vector4 GetColorRef() => ref m_Color;

    public static implicit operator RColor(Color color) => new (
        color.r / 255.0f, 
        color.g / 255.0f, 
        color.b / 255.0f, 
        color.a / 255.0f);

    public static explicit operator Color(RColor color) => new (
        (int)(color.m_Color.X * 255.0f),
        (int)(color.m_Color.Y * 255.0f),
        (int)(color.m_Color.Z * 255.0f),
        (int)(color.m_Color.W * 255.0f));
}