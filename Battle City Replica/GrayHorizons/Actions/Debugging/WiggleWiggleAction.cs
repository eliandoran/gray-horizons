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
using GrayHorizons.Logic;
using System.Diagnostics;
using GrayHorizons.Extensions;

namespace GrayHorizons.Actions.Debugging
{
    [DefaultKey(Keys.F4)]
    public class WiggleWiggleAction: GameAction
    {
        public override void Execute()
        {
            if (GameData.Map.ShakeFactor < 5)
                GameData.Map.ShakeFactor += 1;
            else if (GameData.Map.ShakeFactor < 20)
                GameData.Map.ShakeFactor += 5;
            else
                GameData.Map.ShakeFactor = 0;

            Debug.WriteLine("Shake factor set to {0}x.".FormatWith(GameData.Map.ShakeFactor), "WIGGLE");
        }
    }
}

