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
using GrayHorizons.Screens;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.Game
{
    [DefaultKey(Keys.Escape)]
    public class TogglePauseAction: GameAction
    {
        PauseScreen pauseScreen;

        public override void Execute()
        {            
            GameData.IsPaused = !GameData.IsPaused;

            if (GameData.IsPaused)
            {
                #if DEBUG
                Debug.WriteLine("[PAUSE] Game paused.");
                #endif

                pauseScreen = new PauseScreen(GameData);
                GameData.ScreenManager.AddScreen(pauseScreen, null);
            }
            else
            {
                #if DEBUG
                Debug.WriteLine("[PAUSE] Game unpaused.");
                #endif

                pauseScreen.ExitScreen();
            }
        }
    }
}

