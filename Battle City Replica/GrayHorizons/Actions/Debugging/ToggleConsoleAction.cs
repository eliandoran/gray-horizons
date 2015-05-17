using System;
using GrayHorizons.Logic;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;

namespace GrayHorizons.Actions.Debugging
{
    [DefaultKey(Keys.OemTilde)]
    public class ToggleConsoleAction: GameAction
    {
        public override void Execute()
        {
            GameData.DebuggingSettings.ShowConsole = !GameData.DebuggingSettings.ShowConsole;
        }
    }
}

