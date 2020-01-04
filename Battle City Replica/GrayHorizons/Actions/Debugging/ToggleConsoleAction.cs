﻿/*
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

