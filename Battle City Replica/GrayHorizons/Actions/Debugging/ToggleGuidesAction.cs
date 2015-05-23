namespace GrayHorizons.Actions.Debugging
{
    using System.Diagnostics;
    using GrayHorizons.Attributes;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.F1)]
    public class ToggleGuidesAction: GameAction
    {
        public override void Execute()
        {
            GameData.DebuggingSettings.ShowGuides = !GameData.DebuggingSettings.ShowGuides;

            #if DEBUG
            if (GameData.DebuggingSettings.ShowGuides)
                Debug.WriteLine("[GUIDES] Guides activated.");
            else
                Debug.WriteLine("[GUIDES] Guides deactivated.");
            #endif
        }
    }
}

