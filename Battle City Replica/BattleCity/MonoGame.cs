#region Using Statements

using Microsoft.Xna.Framework;
using BattleCity.ThirdParty.GameStateManagement;
using System.Diagnostics;
using BattleCity.Logic;

#endregion

namespace BattleCity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BattleCityGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        readonly ScreenManager screenManager;

        public GameData GameData;

        public BattleCityGame ()
        {
            #if DEBUG
            Debug.WriteLine ("Initialising GraphicsDeviceManager...", "INIT");
            #endif

            GameData = new GameData ();
            graphics = new GraphicsDeviceManager (this);
            graphics.PreferredBackBufferWidth = graphics.PreferredBackBufferHeight = 764;
            GameData.GraphicsDeviceManager = graphics;

            Content.RootDirectory = @"Content";

            IsMouseVisible = true;
            IsFixedTimeStep = true;

            #if DEBUG
            Debug.WriteLine ("Initialising ScreenManager...", "INIT");
            #endif

            screenManager = new ScreenManager (this);
            screenManager.AddScreen (new Screens.BattlefieldScreen (GameData), null);
            Components.Add (screenManager);
        }
    }
}

