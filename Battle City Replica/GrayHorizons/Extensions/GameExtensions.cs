namespace GrayHorizons.Extensions
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a set of extensions attached to the <see cref="Microsoft.Xna.Framework.Game"/> class.
    /// </summary>
    public static class GameExtensions
    {
        /// <summary>
        /// Positions the game window at the center of the screen.
        /// </summary>
        /// <param name="game">The game whose window to center.</param>
        public static void CenterGameWindow(this Game game)
        {
            game.Window.Position = new Point(
                (game.GraphicsDevice.Adapter.CurrentDisplayMode.Width - game.Window.ClientBounds.Width) / 2,
                (game.GraphicsDevice.Adapter.CurrentDisplayMode.Height - game.Window.ClientBounds.Height) / 2
            );
        }
    }
}

