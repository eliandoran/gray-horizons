namespace GrayHorizons.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using GameStateManagement;
    using GrayHorizons.Extensions;
    using GrayHorizons.Objectives;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class GameData
    {
        readonly List<Player> players = new List<Player>();
        readonly List<Tank> tankPool = new List<Tank>();
        readonly Dictionary<Type, Texture2D> mappedTextures = new Dictionary<Type, Texture2D>();
        readonly DebuggingSettings debuggingSettings = new DebuggingSettings();
        readonly ObjectiveList objectives = new ObjectiveList();
        StringBuilderTraceListener traceListener;

        public event EventHandler ResolutionChanged;

        public StringBuilderTraceListener TraceListener
        {
            get
            {
                return traceListener;
            }
            set
            {
                traceListener = value;

                if (value.IsNotNull())
                    Debug.Listeners.Add(value);
            }
        }

        public Game Game { get; set; }

        public Map Map { get; set; }

        public ContentManager ContentManager { get; set; }

        public GraphicsDevice GraphicsDevice { get; set; }

        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        public GameTime GameTime { get; set; }

        public Player ActivePlayer { get; set; }

        public GameConfig Configuration { get; set; }

        public Vector2 MapScale { get; set; }

        public Vector2 ViewportScale { get; set; }

        public bool IsPaused { get; set; }

        public ScreenManager ScreenManager { get; set; }

        public Texture2D BlankTexture { get; set; }

        public IInputOutputAgent IOAgent { get; set; }

        public Matrix TranslationMatrix { get; set; }

        public List<Player> Players
        {
            get
            {
                return players;
            }
        }

        public List<Tank> TankPool
        {
            get
            {
                return tankPool;
            }
        }

        public Dictionary<Type, Texture2D> MappedTextures
        {
            get
            {
                return mappedTextures;
            }
        }

        public DebuggingSettings DebuggingSettings
        {
            get
            {
                return debuggingSettings;
            }
        }

        public ObjectiveList Objectives
        {
            get
            {
                return objectives;
            }
        }

        internal void OnResolutionChanged(EventArgs e)
        {
            if (ResolutionChanged.IsNotNull())
                ResolutionChanged(this, e);
        }
    }
}

