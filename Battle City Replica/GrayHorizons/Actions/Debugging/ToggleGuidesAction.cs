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
using System.Diagnostics;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.Debugging
{
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

