using System.Numerics;

using Raylib_cs;
using ImGuiNET;
using RGame.Game;
using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace RGame.Core
{
    public class Application
    {
        private bool m_Running = true;

        private readonly EditorCamera m_EditorCamera;

        private bool m_DebugDraw = false;

        private Chunk m_Chunk;
        public Application(string label, int width, int height)
        {
            SetConfigFlags(ConfigFlags.FLAG_VSYNC_HINT | ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(width, height, label);
            rlImGui.Setup();
            AssetManager.Load();

            m_EditorCamera = new EditorCamera(60.0f, 0.5f, 0.8f)
            {
                FocalPoint = new Vector3(12.0f, 3.0f, 5.0f)
            };

            AssetManager.Textures?.AddTexture("Dirt", LoadTexture("assets/textures/dirt.png"));

            m_Chunk = new Chunk();
        }

        public void Run() 
        {
            while (!WindowShouldClose() && m_Running)
            {
                m_EditorCamera.Update(GetFrameTime());
                rlEnableBackfaceCulling();
                BeginDrawing();
                {
                    ClearBackground(Color.BLACK);

                    
                    m_EditorCamera.BeginDraw();
                    {
                        m_Chunk.Draw(m_DebugDraw);
                        //DrawCubeTexture(AssetManager.Textures!.GetTextureByName("Dirt"), Vector3.Zero, 0.5f, 0.5f, 0.5f, Color.WHITE);
                        //m_Block.Draw(m_DebugDraw);
                    }
                    m_EditorCamera.EndDraw();

                    DrawFPS(10, 10);

                    rlImGui.Begin();
                    ImGui.Begin("Cube");
                    {
                        ImGui.Checkbox("Debug draw", ref m_DebugDraw);
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
                        ImGui.DragFloat("Pitch", ref m_EditorCamera.Pitch);
                        ImGui.DragFloat("Yaw", ref m_EditorCamera.Yaw);
                    }
                    ImGui.End();
                    rlImGui.End();
                }
                EndDrawing();
            }
            AssetManager.Unload();
            rlImGui.Shutdown();
            CloseWindow();
        }

        public void Close() => m_Running = false;
    }
}
