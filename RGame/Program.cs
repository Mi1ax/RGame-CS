using RGame.Core;

using ECS;
using ImGuiNET;
using Raylib_cs;
using static Raylib_cs.Raylib;
using RGame.ECS;
using System.Numerics;

namespace RGame
{
    internal static class Program
    {
        public class SquareScript : ScriptableEntity
        {
            private TransformComponent m_Transform;
            private SpriteRendererComponent m_SpriteRenderer;

            public SquareScript(Entity entity)
                : base(entity) {}

            public override void OnInit()
            {
                TraceLog(TraceLogLevel.LOG_INFO, $"{Entity.Name} created");

                m_Transform = (TransformComponent)Entity.GetComponent<TransformComponent>();
                m_SpriteRenderer = (SpriteRendererComponent)Entity.GetComponent<SpriteRendererComponent>();
            }

            public override void OnDraw()
            {
                DrawRectangleV(m_Transform.Position, m_Transform.Size, m_SpriteRenderer.Color);
            }

            public override void OnUpdate(float dt)
            {

            }

            public override void OnDestroy()
            {
                
            }
        }

        private static void Main()
        {
            var system = new ECSRegister();
            var entity = system.CreateEntity("Entity");
            var tc = entity.GetComponent<TransformComponent>() as TransformComponent;
            var src = entity.AddComponent<SpriteRendererComponent>() as SpriteRendererComponent;
            var sec = entity.AddComponent<ScriptComponent>() as ScriptComponent;
            sec!.Script = new SquareScript(entity);
            tc!.Size = new(128, 128);
            src!.Color = Color.RED;

            var window = new Window()
            {
                OnUpdate = (float dt) =>
                {
                    system.UpdateEntities(dt);
                },
                OnTargetDraw = () =>
                {
                    system.DrawEntities();
                },
                OnImGuiDraw = () =>
                {
                    ImGui.Begin("Properties");
                    {
                        ImGui.DragFloat2("Position", ref tc.PositionRef());
                        ImGui.DragFloat2("Size", ref tc.SizeRef());
                        ImGui.ColorEdit4("Color", ref src.ColorRef());
                    }
                    ImGui.End();
                }
            };

            window.Run();
        }
    }
}