using System;
using BattleCity.Logic;
using BattleCity.Attributes;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using BattleCity.Screens;
using BattleCity.ThirdParty.GameStateManagement;
using System.Diagnostics;

namespace BattleCity.Input.Actions
{
    [DefaultKey (Keys.F12)]
    public class ToggleFullScreenAction: GameAction
    {
        public ToggleFullScreenAction (
            GameData gameData) : base (
                gameData) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Input.Actions.ToggleFullScreenAction"/> class.
        /// </summary>
        public ToggleFullScreenAction () : this (
                null) { }

        public override void Execute ()
        {
            GameData.GraphicsDeviceManager.PreferredBackBufferWidth = GameData.Configuration.FullScreenResolution.Width;
            GameData.GraphicsDeviceManager.PreferredBackBufferHeight = GameData.Configuration.FullScreenResolution.Height;
            GameData.Map.Viewport = new Rectangle (0,
                0,
                GameData.Configuration.FullScreenResolution.Width,
                GameData.Configuration.FullScreenResolution.Height);

            GameData.Map.CenterViewportAt (GameData.ActivePlayer.AssignedEntity);
            GameData.GraphicsDeviceManager.ToggleFullScreen ();
        }
    }


    [DefaultKey (Keys.Escape)]
    public class TogglePauseAction: GameAction
    {
        PauseScreen pauseScreen;

        public TogglePauseAction (
            GameData gameData) : base (
                gameData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Input.Actions.TogglePauseAction"/> class.
        /// </summary>
        public TogglePauseAction () : this (
                null) { }

        public override void Execute ()
        {            
            GameData.IsPaused = !GameData.IsPaused;

            if (GameData.IsPaused)
            {
                #if DEBUG
                Debug.WriteLine ("Game paused.", "PAUSE");
                #endif

                pauseScreen = new PauseScreen (GameData);
                GameData.ScreenManager.AddScreen (pauseScreen, null);
            }
            else
            {
                #if DEBUG
                Debug.WriteLine ("Game unpaused.", "PAUSE");
                #endif

                pauseScreen.ExitScreen ();
            }
        }
    }
}

