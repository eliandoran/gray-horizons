using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.Debugging
{
    [DefaultKey(Keys.F1)]
    public class ToggleGuidesAction: GameAction
    {
        public ToggleGuidesAction(
            GameData gameData)
            : base(
                gameData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.ToggleGuidesTraceAction"/> class.
        /// </summary>
        public ToggleGuidesAction()
            : this(
                null)
        {
        }

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

