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
using Microsoft.Xna.Framework;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.Game
{
    [DefaultKey(Keys.F12)]
    public class ToggleFullScreenAction: GameAction
    {
        public override void Execute()
        {
            if (!GameData.GraphicsDeviceManager.IsFullScreen)
                SetBackBufferSize(GameData.Configuration.FullScreenResolution);
            else
                SetBackBufferSize(GameData.Configuration.WindowedModeResolution);

            GameData.Map.Viewport = new Rectangle(0,
                0,
                GameData.GraphicsDeviceManager.PreferredBackBufferWidth,
                GameData.GraphicsDeviceManager.PreferredBackBufferHeight);

            GameData.Map.CenterViewportAt(GameData.ActivePlayer.AssignedEntity);
            GameData.GraphicsDeviceManager.ToggleFullScreen();
            GameData.OnResolutionChanged(EventArgs.Empty);
        }

        void SetBackBufferSize(Size size)
        {
            GameData.GraphicsDeviceManager.PreferredBackBufferWidth = size.Width;
            GameData.GraphicsDeviceManager.PreferredBackBufferHeight = size.Height;
        }
    }
}

