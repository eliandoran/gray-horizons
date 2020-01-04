namespace GrayHorizons.Actions.Debugging
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using GrayHorizons.Screens;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.OemTilde)]
    public class ToggleConsoleAction: GameAction
    {
        OnScreenConsole screen;

        public override void Execute()
        {
            GameData.DebuggingSettings.ShowConsole = !GameData.DebuggingSettings.ShowConsole;

            if (GameData.DebuggingSettings.ShowConsole)
            {
                if (screen.IsNull())
                    screen = new OnScreenConsole(GameData);
                
                GameData.ScreenManager.AddScreen(screen, null);
            }
            else if (screen.IsNotNull())
            {
                screen.ExitScreen();
            }
        }
    }
}

