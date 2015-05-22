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

using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.Game
{
    [DefaultKey(Keys.OemPlus)]
    [AllowContinuousPress]
    public class ZoomInAction: GameAction
    {
        public override void Execute()
        {
            if (GameData.MapScale.X < 2.5f)
            {
                const float step = 0.01f;
                var mapScale = GameData.MapScale.X + step;
                var viewportScale = GameData.ViewportScale.X - step;
                GameData.MapScale = new Vector2(mapScale, mapScale);
                GameData.ViewportScale = new Vector2(viewportScale, viewportScale);
                GameData.Map.ScaledViewport = GameData.Map.Viewport.ScaleTo(GameData.ViewportScale);
                GameData.Map.CenterViewportAt(GameData.ActivePlayer.AssignedEntity);
            }
            #if DEBUG
            else
            {
                Debug.WriteLine("Maximum zoom reached.", "ZOOM");
            }
            #endif
        }
    }
}

