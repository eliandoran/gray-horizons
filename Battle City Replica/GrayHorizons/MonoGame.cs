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


#region Using Statements
using System.Diagnostics;
using System.IO;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace GrayHorizons
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GrayHorizonsGame : Game
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
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            graphics.CreateDevice ();
            this.CenterGameWindow ();

            GameData.GraphicsDeviceManager = graphics;

            Window.AllowUserResizing = true;
            Window.Title = "Gray Horizons - (C) 2015 Elian Doran";

            #if DEBUG
            Content.RootDirectory = new System.Uri (System.Reflection.Assembly.GetExecutingAssembly ().CodeBase + @"/../../../../GrayHorizons.Content/bin/Windows").LocalPath;
            #else
            Content.RootDirectory = "Content";
            #endif

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

        public GrayHorizonsGame ()
        {   
            #if DEBUG
            Debug.WriteLine ("Gray Horizons");
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

