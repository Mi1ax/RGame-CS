using System.Numerics;

using Raylib_cs;
using ImGuiNET;

using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace RGame.Core
{
    public class Application
    {
        private bool m_Running = true;

        private readonly Vector3 m_CubePosition;

        private readonly EditorCamera m_EditorCamera;
        private readonly RColor m_Color;
        
        public Application(string label, int width, int height)
        {
            SetConfigFlags(ConfigFlags.FLAG_VSYNC_HINT | ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(width, height, label);
            rlImGui.Setup();
            
            m_CubePosition = Vector3.Zero;
            m_EditorCamera = new EditorCamera();
            m_Color = Color.RED;
        }

        public void Run() 
        {
            while (!WindowShouldClose() && m_Running)
            {
                m_EditorCamera.Update(GetFrameTime());

                BeginDrawing();
                {
                    ClearBackground(Color.GRAY);
                    
                    m_EditorCamera.BeginDraw();
                    {
                        DrawCube(m_CubePosition, 2.0f, 2.0f, 2.0f, (Color)m_Color);
                        DrawCubeWires(m_CubePosition, 2.0f, 2.0f, 2.0f, Color.MAROON);

                        DrawGrid(40, 1.0f);
                    }
                    m_EditorCamera.EndDraw();

                    DrawFPS(10, 10);

                    rlImGui.Begin();
                    ImGui.Begin("Cube");
                    {
                        ImGui.ColorEdit4("Color", ref m_Color.GetColorRef());
                    }
                    ImGui.End();

                    ImGui.Begin("EditorCamera Settings");
                    {
                        ImGui.DragFloat("Fov", ref m_EditorCamera.Fov);
                        ImGui.DragFloat("Distance", ref m_EditorCamera.Distance);
                        ImGui.DragFloat("Rotation Speed", ref m_EditorCamera.RotationSpeed);
                        ImGui.DragFloat("Zoom Speed", ref m_EditorCamera.ZoomSpeed);
                        ImGui.Separator();
                        ImGui.DragFloat3("Focal Point", ref m_EditorCamera.FocalPoint);
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
