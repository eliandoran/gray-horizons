namespace GrayHorizons.Actions.Game
{
    using System.Diagnostics;
    using GrayHorizons.Attributes;
    using GrayHorizons.Logic;
    using GrayHorizons.Screens;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.Escape)]
    public class TogglePauseAction: GameAction
    {
        PauseScreen pauseScreen;

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

