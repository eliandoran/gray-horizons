#region Using Statements

using Microsoft.Xna.Framework;
using BattleCity.ThirdParty.GameStateManagement;
using System.Diagnostics;
using BattleCity.Logic;
using System.IO;

#endregion

namespace BattleCity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BattleCityGame : Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        Configuration configuration;

        public GameData GameData;

        public void LoadConfiguration()
        {
            #if DEBUG
            Debug.WriteLine ("Loading configuration...", "INIT");
            #endif

            configuration = Configuration.Load (@"C:\Users\Elian\Desktop\config.xml");

            #if DEBUG
            Debug.WriteLine ("Configuration loaded successfully.", "INIT");
            #endif
        }

        public void InitGraphics()
        {
            #if DEBUG
            Debug.WriteLine ("Initializing GraphicsDeviceManager...", "INIT");
            #endif

            GameData = new GameData ();
            graphics = new GraphicsDeviceManager (this);
            graphics.PreferredBackBufferWidth = graphics.PreferredBackBufferHeight = 764;
            GameData.GraphicsDeviceManager = graphics;

            Window.AllowUserResizing = true;
            Window.Title = "BattleCity Replica - (C) 2015 Elian Doran";

            Content.RootDirectory = @"Content";

            IsMouseVisible = true;
            IsFixedTimeStep = true;
        }

        public void InitScreens()
        {
            #if DEBUG
            Debug.WriteLine ("Initializing ScreenManager...", "INIT");
            #endif

            screenManager = new ScreenManager (this);
            screenManager.AddScreen (new Screens.BattlefieldScreen (GameData), null);
            Components.Add (screenManager);
        }

        public BattleCityGame ()
        {   
            #if DEBUG
            Debug.WriteLine ("BattleCity Replica");
            Debug.WriteLine ("(C) 2015 by Doran Adoris Elian\n");

            Debug.Listeners.Add (new TextWriterTraceListener (new FileStream ("Log.txt", FileMode.Create)));
            #endif

            LoadConfiguration ();
            InitGraphics ();
            InitScreens ();
        }
    }
}

