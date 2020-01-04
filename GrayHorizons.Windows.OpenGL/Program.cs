namespace GrayHorizons.Windows.OpenGL
{
    using System;
    using GrayHorizons;

    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new GrayHorizonsGame())
                game.Run();
        }
    }
}

