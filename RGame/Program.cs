using RGame.Core;

namespace RGame
{
    internal static class Program
    {
        private static void Main()
        {
            var window = new Application("RGame", 1920, 1080);
            window.Run();
        }
    }
}