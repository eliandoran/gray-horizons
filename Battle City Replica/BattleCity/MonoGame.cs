#region Using Statements
using Microsoft.Xna.Framework;
using BattleCity.ThirdParty.GameStateManagement;
using System.Diagnostics;
using BattleCity.Logic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using BattleCity.Extensions;
using Microsoft.Xna.Framework.Content;

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
        int screenCount = 0;

        public GameData GameData = new GameData ();

        public void LoadConfiguration ()
        {
            #if DEBUG
            Debug.WriteLine ("Loading configuration...", "INIT");
            #endif

            var fn = @"config.xml";
            if (File.Exists (fn))
            {
                configuration = Configuration.Load (fn);            
                GameData.Configuration = configuration;
                Configuration.ConfigureGameDataInputBindings (GameData);

                #if DEBUG
                Debug.WriteLine ("Configuration loaded successfully.", "INIT");
                #endif
            }
        }

        public void InitGraphics ()
        {
            #if DEBUG
            Debug.WriteLine ("Initializing GraphicsDeviceManager...", "INIT");
            #endif
                       
            graphics = new GraphicsDeviceManager (this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.CreateDevice ();
            this.CenterGameWindow ();

            GameData.GraphicsDeviceManager = graphics;

            Window.AllowUserResizing = true;
            Window.Title = "BattleCity Replica - (C) 2015 Elian Doran";

            Content.RootDirectory = new System.Uri (System.Reflection.Assembly.GetExecutingAssembly ().CodeBase + @"/../../../../BattleCity.Content/bin/Windows").LocalPath;

            IsMouseVisible = true;
            IsFixedTimeStep = true;
        }

        public void InitScreens ()
        {
            #if DEBUG
            Debug.WriteLine ("Initializing ScreenManager...", "INIT");
            #endif

            screenManager = new ScreenManager (this);
            GameData.ScreenManager = screenManager;
            Components.Add (screenManager);
            //screenManager.AddScreen (new Screens.MainMenuScreen (GameData), null);
            screenManager.AddScreen (new Screens.BattlefieldScreen (GameData), null);
        }

        public BattleCityGame ()
        {   
            #if DEBUG
            Debug.WriteLine ("BattleCity Replica");
            Debug.WriteLine ("(C) 2015 by Doran Adoris Elian\n");

            //Debug.Listeners.Add (new TextWriterTraceListener (new FileStream ("Log.txt", FileMode.Create)));
            #endif

            var player = new Player ();
            GameData.Players.Add (player);
            GameData.ActivePlayer = player;
            GameData.Game = this;

            LoadConfiguration ();
            InitGraphics ();
            InitScreens ();
        }

        protected override void Draw (
            GameTime gameTime)
        {
            var newScreenCount = GameData.ScreenManager.GetScreens ().Length;
            if (newScreenCount != screenCount)
            {
                Debug.WriteLine (newScreenCount, "SCREEN COUNT");
                screenCount = newScreenCount;
            }

            base.Draw (gameTime);
        }
    }
}

