using System.Numerics;

using Raylib_cs;
using RGame.ECS;

namespace ECS
{
    public class TransformComponent : IComponent
    {
        private string m_Name = "Transform";
        private Vector2 m_Position;
        private Vector2 m_Size;

        string IComponent.Name { get => m_Name; set => m_Name = value; }

        public Vector2 Position { get => m_Position; set => m_Position = value; }
        public Vector2 Size { get => m_Size; set => m_Size = value; }

        public ref Vector2 PositionRef() => ref m_Position;
        public ref Vector2 SizeRef() => ref m_Size;

        public override string ToString() => m_Name;
    }

    public class SpriteRendererComponent : IComponent
    {
        private string m_Name = "Transform";
        private Vector4 m_Color;

        string IComponent.Name { get => m_Name; set => m_Name = value; }

        public Color Color
        {
            get => new((int)(m_Color.X * 255.0f), (int)(m_Color.Y * 255.0f), (int)(m_Color.Z * 255.0f), (int)(m_Color.W * 255.0f));
            set
            {
                m_Color.X = value.r / 255.0f;
                m_Color.Y = value.g / 255.0f;
                m_Color.Z = value.b / 255.0f;
                m_Color.W = value.a / 255.0f;
            }
        }

        public ref Vector4 ColorRef() => ref m_Color;
    }

    public class ScriptComponent : IComponent
    {
        private string m_Name = "Script";
        private ScriptableEntity? m_ScriptableEntity = null;

        public ScriptableEntity? Script 
        { 
            get => m_ScriptableEntity; 
            set
            {
                m_ScriptableEntity = value;
                m_ScriptableEntity?.OnInit();
            }
        }

        string IComponent.Name { get => m_Name; set => m_Name = value; }
    }
}