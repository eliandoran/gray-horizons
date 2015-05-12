using System;
using Microsoft.Xna.Framework;

namespace BattleCity.Extensions
{
    public static class GameExtensions
    {
        public static void CenterGameWindow (
            this Game game)
        {
            var screenWidth = game.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            var screenHeight = game.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

            var windowWidth = game.Window.ClientBounds.Width;
            var windowHeight = game.Window.ClientBounds.Height;

            var newX = (screenWidth - windowWidth) / 2;
            var newY = (screenHeight - windowHeight) / 2;

            game.Window.Position = new Point (newX, newY);
        }
    }
}

