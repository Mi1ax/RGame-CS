using System.Numerics;

using ImGuiNET;
using Raylib_cs;

using RGame.Core;

using static Raylib_cs.Raylib;

namespace RGame
{
    internal static class Program
    {
        private static void Main()
        {
            var window = new Window("RGame", 1920, 1080)
            {
                OnUpdate = (float dt) =>
                {

                }
            };

            window.Run();
        }
    }
}