using GameStateManagement;

namespace GrayHorizons.UI
{
    /// <summary>
    /// Extension of <see cref="GameScreen"/> which adds game-specific features and events (such as being informed when the available drawing size is changed).
    /// </summary>
    public abstract class ExtendedGameScreen: GameScreen
    {

        /// <summary>
        /// Called upon when the game drawing surface size is changed (such as being resized by the user).
        /// </summary>
        public virtual void OnClientSizeChanged()
        {
            // Do nothing by default.
        }

    }
}
