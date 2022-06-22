using System.Numerics;

using Raylib_cs;
using ImGuiNET;

using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace RGame.Core
{
    public class Window
    {
        private bool m_Running = true;

        public Action<float>? OnUpdate = null;
        public Action? OnDraw = null;
        public Action? OnImGuiDraw = null;

        private readonly Vector3 m_CubePosition;

        private readonly RCamera3D m_Camera;
        private readonly RColor m_Color;
        
        public Window(string label, int width, int height)
        {
            SetConfigFlags(ConfigFlags.FLAG_VSYNC_HINT | ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(width, height, label);
            rlImGui.Setup();

            m_CubePosition = Vector3.Zero;
            m_Camera = new RCamera3D();
            m_Color = Color.RED;
        }

        public void Run() 
        {
            while (!WindowShouldClose() && m_Running)
            {
                OnUpdate?.Invoke(GetFrameTime());
                m_Camera.Update(GetFrameTime());

                BeginDrawing();
                {
                    ClearBackground(Color.GRAY);

                    //BeginMode3D(m_Camera.GetCameraHandler());
                    m_Camera.BeginDraw();
                    {
                        DrawCube(m_CubePosition, 2.0f, 2.0f, 2.0f, (Color)m_Color);
                        DrawCubeWires(m_CubePosition, 2.0f, 2.0f, 2.0f, Color.MAROON);

                        DrawGrid(40, 1.0f);
                    }
                    m_Camera.EndDraw();

                    DrawFPS(10, 10);

                    rlImGui.Begin();
                    ImGui.Begin("Cube");
                    {
                        ImGui.ColorEdit4("Color", ref m_Color.GetColorRef());
                    }
                    ImGui.End();

                    ImGui.Begin("Camera");
                    {
                        ImGui.DragFloat2("Angle", ref m_Camera.Angle, 0.01f);
                        ImGui.DragFloat("Target Distance", ref m_Camera.TargetDistance);

                        ImGui.Separator();

                        ImGui.DragFloat3("Position", ref m_Camera.Position);
                        ImGui.DragFloat3("Target", ref m_Camera.Target);
                        ImGui.DragFloat3("Up", ref m_Camera.Up);
                        ImGui.DragFloat("Fov", ref m_Camera.Fovy);
                    }
                    ImGui.End();
                    rlImGui.End();
                }
                EndDrawing();
            }

            rlImGui.Shutdown();
            CloseWindow();
        }

        public void Close() => m_Running = false;
    }
}
