using Raylib_cs;
using ImGuiNET;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace RGame.Core
{
    public class Window
    {
        private bool m_Running = true;

        public Action<float>? OnUpdate = null;
        public Action? OnDraw = null;
        public Action? OnImGuiDraw = null;

        public Window(string label, int width, int height)
        {
            InitWindow(width, height, label);

            rlImGui.Setup();
        }

        public void Run() 
        {
            while (!WindowShouldClose() && m_Running)
            {
                OnUpdate?.Invoke(GetFrameTime());

                BeginDrawing();
                ClearBackground(Color.PINK);
                {
                    OnDraw?.Invoke();

                    rlImGui.Begin();
                    OnImGuiDraw?.Invoke();
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
