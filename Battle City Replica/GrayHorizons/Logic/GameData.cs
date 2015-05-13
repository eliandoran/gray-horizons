using System;
using System.Collections.Generic;
using GrayHorizons.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GrayHorizons.Entities;
using GrayHorizons.ThirdParty.GameStateManagement;

namespace GrayHorizons.Logic
{
    public class GameData
    {
        readonly List<Player> players = new List<Player> ();
        readonly List<InputBinding> inputBindings = new List<InputBinding> ();
        readonly List<Tank> tankPool = new List<Tank> ();
        readonly Dictionary<Type, Texture2D> mappedTextures = new Dictionary<Type, Texture2D> ();
        readonly DebuggingSettings debuggingSettings = new DebuggingSettings ();

        public Game Game { get; set; }

        public Map Map { get; set; }

        public ContentManager ContentManager { get; set; }

        public GraphicsDevice GraphicsDevice { get; set; }

        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public GameTime GameTime { get; set; }

        public Player ActivePlayer { get; set; }

        public Configuration Configuration { get; set; }

        public Vector2 MapScale { get; set; }

        public Vector2 ViewportScale { get; set; }

        public bool IsPaused { get; set; }

        public ScreenManager ScreenManager { get; set; }

        public Texture2D blankTexture;

        public List<Player> Players
        {
            get
            {
                return players;
            }
        }

        public List<InputBinding> InputBindings
        {
            get
            {
                return inputBindings;
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
    }
}

