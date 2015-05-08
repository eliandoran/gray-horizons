using System;
using System.Collections.Generic;
using BattleCity.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleCity.Logic
{
    public class GameData
    {
        readonly List<Player> players;
        readonly List<InputBinding> inputBindings;
        readonly DebuggingSettings debuggingSettings = new DebuggingSettings ();

        public Map Map { get; set; }

        public ContentManager ContentManager { get; set; }

        public GraphicsDevice GraphicsDevice { get; set; }

        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public GameTime GameTime { get; set; }

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

        public DebuggingSettings DebuggingSettings
        {
            get
            {
                return debuggingSettings;
            }
        }

        public GameData ()
        {
            players = new List<Player> (2);
            inputBindings = new List<InputBinding> (10);
        }
    }
}

