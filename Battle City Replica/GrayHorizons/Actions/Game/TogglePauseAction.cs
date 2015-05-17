using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using GrayHorizons.Screens;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.Game
{
    [DefaultKey(Keys.Escape)]
    public class TogglePauseAction: GameAction
    {
        PauseScreen pauseScreen;

        public TogglePauseAction(
            GameData gameData)
            : base(
                gameData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.TogglePauseAction"/> class.
        /// </summary>
        public TogglePauseAction()
            : this(
                null)
        {
        }

        public override void Execute()
        {            
            GameData.IsPaused = !GameData.IsPaused;

            if (GameData.IsPaused)
            {
                #if DEBUG
                Debug.WriteLine("[PAUSE] Game paused.");
                #endif

                pauseScreen = new PauseScreen(GameData);
                GameData.ScreenManager.AddScreen(pauseScreen, null);
            }
            else
            {
                #if DEBUG
                Debug.WriteLine("[PAUSE] Game unpaused.");
                #endif

                pauseScreen.ExitScreen();
            }
        }
    }
}

