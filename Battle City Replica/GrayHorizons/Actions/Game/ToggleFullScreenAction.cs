using System;
using Microsoft.Xna.Framework;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.Game
{
    [DefaultKey(Keys.F12)]
    public class ToggleFullScreenAction: GameAction
    {
        public ToggleFullScreenAction(
            GameData gameData)
            : base(
                gameData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.ToggleFullScreenAction"/> class.
        /// </summary>
        public ToggleFullScreenAction()
            : this(
                null)
        {
        }

        public override void Execute()
        {
            if (!GameData.GraphicsDeviceManager.IsFullScreen)
                SetBackBufferSize(GameData.Configuration.FullScreenResolution);
            else
                SetBackBufferSize(GameData.Configuration.WindowedModeResolution);

            GameData.Map.Viewport = new Rectangle(0,
                0,
                GameData.GraphicsDeviceManager.PreferredBackBufferWidth,
                GameData.GraphicsDeviceManager.PreferredBackBufferHeight);

            GameData.Map.CenterViewportAt(GameData.ActivePlayer.AssignedEntity);
            GameData.GraphicsDeviceManager.ToggleFullScreen();
        }

        void SetBackBufferSize(Size size)
        {
            GameData.GraphicsDeviceManager.PreferredBackBufferWidth = size.Width;
            GameData.GraphicsDeviceManager.PreferredBackBufferHeight = size.Height;
        }
    }
}

