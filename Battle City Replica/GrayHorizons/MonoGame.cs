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
using GrayHorizons.Objectives;

namespace GrayHorizons
{
    #region Using Statements
    using System;
    using System.Diagnostics;
    using System.IO;
    using GrayHorizons.Logic;
    using GrayHorizons.ThirdParty.GameStateManagement;
    using GrayHorizons.Windows.DirectX;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    #endregion

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GrayHorizonsGame : Game
    {
        internal const int defaultResolutionWidth = 800;
        internal const int defaultResolutionHeight = 600;

        bool isExiting;
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        Configuration configuration;
        int screenCount = 0;

        public GameData GameData = new GameData();

        public void LoadConfiguration()
        {
            #if DEBUG
            Debug.WriteLine("[INIT] Loading configuration...");
            #endif

            GameData.IOAgent = new FileStreamInputOutputAgent();

            const string fn = @"config.xml";
            if (File.Exists(fn))
            {
                configuration = Configuration.Load(GameData.IOAgent, fn);            
                GameData.Configuration = configuration;
                Configuration.ConfigureGameDataInputBindings(GameData);

                #if DEBUG
                Debug.WriteLine("[INIT] Configuration loaded successfully.");
                #endif
            }
        }

        void CenterWindow(Game game)
        {
            var screenWidth = game.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            var screenHeight = game.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

            var windowWidth = game.Window.ClientBounds.Width;
            var windowHeight = game.Window.ClientBounds.Height;

            var newX = (screenWidth - windowWidth) / 2;
            var newY = (screenHeight - windowHeight) / 2;

            game.Window.Position = new Point(newX, newY);
        }

        public void InitGraphics()
        {
            #if DEBUG
            Debug.WriteLine("[INIT] Initializing GraphicsDeviceManager...");
            #endif

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameData.Configuration.WindowedModeResolution.Width;
            graphics.PreferredBackBufferHeight = GameData.Configuration.WindowedModeResolution.Height;
            graphics.ApplyChanges();

            CenterWindow(this);

            GameData.ContentManager = Content;
            GameData.GraphicsDevice = graphics.GraphicsDevice;
            GameData.GraphicsDeviceManager = graphics;
            GameData.DebuggingSettings.ShowGuides = true;
            GameData.DebuggingSettings.ShowConsole = true;

            Window.AllowUserResizing = true;
            Window.Title = "Gray Horizons – © 2015 Doran Adoris Elian";

            #if DEBUG
            var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            Content.RootDirectory = new Uri(codeBase + @"/../../../../../../GrayHorizons.Content/bin/Windows").LocalPath;
            #else
            Content.RootDirectory = "Content";
            #endif

            GameData.BlankTexture = Content.Load<Texture2D>("blank");

            IsMouseVisible = true;
            IsFixedTimeStep = true;
        }

        public void InitScreens()
        {
            #if DEBUG
            Debug.WriteLine("INIT: Initializing ScreenManager...");
            #endif

            screenManager = new ScreenManager(this);
            GameData.ScreenManager = screenManager;
            Components.Add(screenManager);
            //screenManager.AddScreen (new Screens.MainMenuScreen (GameData), null);
            screenManager.AddScreen(new Screens.BattlefieldScreen(GameData), null);
            screenManager.AddScreen(new Screens.OnScreenConsole(GameData), null);
        }

        public GrayHorizonsGame()
        {   
            #if DEBUG
            GameData.TraceListener = new StringBuilderTraceListener();
            Debug.Listeners.Add(GameData.TraceListener);

            Debug.WriteLine(
                "   _____                   _    _            _                    \n" +
                "  / ____|                 | |  | |          (_)                   \n" +
                " | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ \n" +
                " | | |_ | '__/ _` | | | | |  __  |/ _ \\| '__| |_  / _ \\| '_ \\/ __|\n" +
                " | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \\__ \\\n" +
                "  \\_____|_|  \\__,_|\\__, | |_|  |_|\\___/|_|  |_/___\\___/|_| |_|___/\n" +
                "                    __/ |                                         \n" +
                "                   |___/              © 2015 by Doran Adoris Elian\n"
            );                
            #endif

            var player = new Player();
            GameData.Players.Add(player);
            GameData.ActivePlayer = player;
            GameData.Game = this;

            GameData.Objectives.Add(new Objective { Text = "Dolor sit amet adipiscing elit consticteur." });
            GameData.Objectives.Add(new Objective { Text = "There" });

            LoadConfiguration();
            InitGraphics();
            InitScreens();
        }

        protected override void Draw(
            GameTime gameTime)
        {
            #if DEBUG
            var newScreenCount = GameData.ScreenManager.GetScreens().Length;
            if (newScreenCount != screenCount)
            {
                Debug.WriteLine(String.Empty);
                Debug.WriteLine("SCREENS ({0}):", newScreenCount);

                var index = 1;
                foreach (var screen in GameData.ScreenManager.GetScreens())
                    Debug.WriteLine("\t{0}. {1}", index++, screen);

                Debug.WriteLine(String.Empty);

                screenCount = newScreenCount;
            }
            #endif

            base.Draw(gameTime);
        }

        public void ExitOnce()
        {
            if (!isExiting)
            {
                isExiting = true;
                Exit();
            }
        }
    }
}

