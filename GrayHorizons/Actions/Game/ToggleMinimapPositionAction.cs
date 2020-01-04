/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/

using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Logic;
using GrayHorizons.Screens.HeadsUp;
using System.Linq;
using GrayHorizons.Extensions;

namespace GrayHorizons.Actions.Game
{
    [DefaultKey(Keys.F9)]
    public class ToggleMiniMapPositionAction: GameAction
    {
        public override void Execute()
        {
            GameData.ScreenManager.GetScreens().ToList().ForEach(screen =>
                {
                    var miniMap = screen as MiniMapScreen;
                    if (miniMap.IsNotNull())
                    {
                        if (miniMap.Position < MiniMapScreen.MiniMapPosition.BottomRight)
                            miniMap.Position += 1;
                        else
                            miniMap.Position = MiniMapScreen.MiniMapPosition.TopLeft;
                    }
                });
        }
    }
}

