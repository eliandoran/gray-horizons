using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.ThirdParty.GameStateManagement;
using GrayHorizons.Logic;
using GrayHorizons.Screens.HeadsUp;

namespace GrayHorizons.Actions.Game
{
    [DefaultKey(Keys.F9)]
    public class ToggleMinimapPositionAction: GameAction
    {
        public ToggleMinimapPositionAction(GameData gameData)
            : base(gameData)
        {
        }

        public ToggleMinimapPositionAction()
            : this(null)
        {
        }

        public override void Execute()
        {
            foreach (GameScreen screen in GameData.ScreenManager.GetScreens())
            {
                var minimap = screen as MinimapScreen;
                if (minimap != null)
                {
                    if (minimap.Position < MinimapScreen.MinimapPosition.BottomRight)
                        minimap.Position += 1;
                    else
                        minimap.Position = MinimapScreen.MinimapPosition.TopLeft;
                }
            }
        }
    }
}

