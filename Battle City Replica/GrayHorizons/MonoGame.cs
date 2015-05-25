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
namespace GrayHorizons
{
    #region Using Statements
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using GameStateManagement;
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
        GameConfig configuration;
        int screenCount = 0;

        public GameData GameData = new GameData();

        public void LoadConfiguration()
        {
            Debug.WriteLine("[INIT] Loading configuration...");

            GameData.IOAgent = new FileStreamInputOutputAgent();

            const string fn = @"config.xml";
            if (File.Exists(fn))
            {
                configuration = GameConfig.Load(GameData.IOAgent, fn);            
                GameData.Configuration = configuration;
                GameConfig.ConfigureGameDataInputBindings(GameData);

                Debug.WriteLine("[INIT] Configuration loaded successfully.");
            }
        }

        static void CenterWindow(Game game)
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
            Debug.WriteLine("[INIT] Initializing GraphicsDeviceManager...");

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameData.Configuration.WindowedModeResolution.Width;
            graphics.PreferredBackBufferHeight = GameData.Configuration.WindowedModeResolution.Height;
            graphics.CreateDevice();

            CenterWindow(this);

            GameData.ContentManager = Content;
            GameData.GraphicsDevice = graphics.GraphicsDevice;
            GameData.GraphicsDeviceManager = graphics;

            Window.AllowUserResizing = true;
            Window.Title = "Gray Horizons – © 2015 Doran Adoris Elian";

            #if DEBUG
            Content.RootDirectory = new Uri(Assembly.GetExecutingAssembly().CodeBase + @"/../../../../GrayHorizons.Content/bin/Windows").LocalPath;
            #else
            Content.RootDirectory = "Content";
            #endif

            GameData.BlankTexture = Content.Load<Texture2D>("blank");

            IsMouseVisible = true;
            IsFixedTimeStep = true;
        }

        public void InitScreens()
        {
            Debug.WriteLine("INIT: Initializing ScreenManager...");

            screenManager = new ScreenManager(this);
            GameData.ScreenManager = screenManager;
            Components.Add(screenManager);
            screenManager.AddScreen(new Screens.MainMenuScreen(GameData), null);
            //screenManager.AddScreen(new Screens.BattlefieldScreen(GameData, new Maps.TutorialMap(GameData)), null);
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
                Debug.WriteLine("SCREENS ({0}):".FormatWith(newScreenCount));

                var index = 1;
                GameData.ScreenManager.GetScreens().ToList().ForEach(screen => Debug.WriteLine("{0}. {1}".FormatWith(index++, screen)));

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

