using Raylib_cs;
using ImGuiNET;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace RGame.Core
{
    public class Window
    {
        private Vector2 m_ViewportSize;
        private readonly RenderTarget m_Target;
        private bool m_Running = true;

        public Action<float>? OnUpdate = null;
        public Action? OnDraw = null;
        public Action? OnTargetDraw = null;
        public Action? OnImGuiDraw = null;

        public Window()
        {
            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_VSYNC_HINT);
            InitWindow(1280, 800, "Window");
            SetTargetFPS(144);

            rlImGui.Setup();
            m_Target = new RenderTarget();
            m_ViewportSize = Vector2.Zero;
        }

        private void Update()
        {
            if (m_ViewportSize != new Vector2(m_Target.GetWidth(), m_Target.GetHeight()))
                m_Target.Resize(m_ViewportSize);

            OnUpdate?.Invoke(GetFrameTime());
        }

        private void DrawToTexture(RenderTarget target)
        {
            BeginTextureMode((RenderTexture2D)target);
            ClearBackground(Color.DARKGRAY);

            OnTargetDraw?.Invoke();

            EndTextureMode();
        }

        private void Draw()
        {
            rlImGui.Begin();
            Docking.Draw(() => {
                m_ViewportSize = ImGui.GetContentRegionAvail();
                ImGui.Image(
                    m_Target.GetTextureIDPtr(),
                    m_ViewportSize,
                    new Vector2(0, 1),
                    new Vector2(1, 0)
                    );
            }, OnImGuiDraw);
            rlImGui.End();

            OnDraw?.Invoke();
        }

        public void Run() 
        {
            while (!WindowShouldClose() && m_Running)
            {
                Update();

                DrawToTexture(m_Target);

                BeginDrawing();
                ClearBackground(Color.PINK);
                {
                    Draw();
                }
                EndDrawing();
            }

            rlImGui.Shutdown();
            m_Target.Free();
            CloseWindow();
        }

        public void Close() => m_Running = false;
    }
}
