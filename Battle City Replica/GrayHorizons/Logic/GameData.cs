﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GrayHorizons.Objectives;

namespace GrayHorizons.Logic
{
    public class GameData
    {
        readonly List<Player> players = new List<Player>();
        readonly List<Tank> tankPool = new List<Tank>();
        readonly Dictionary<Type, Texture2D> mappedTextures = new Dictionary<Type, Texture2D>();
        readonly DebuggingSettings debuggingSettings = new DebuggingSettings();
        readonly ObjectiveList objectives = new ObjectiveList();
        StringBuilderTraceListener traceListener;

        public StringBuilderTraceListener TraceListener
        {
            get
            {
                return traceListener;
            }
            set
            {
                traceListener = value;

                if (value != null)
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

        public Configuration Configuration { get; set; }

        public Vector2 MapScale { get; set; }

        public Vector2 ViewportScale { get; set; }

        public bool IsPaused { get; set; }

        public ScreenManager ScreenManager { get; set; }

        public Texture2D BlankTexture;

        public InputOutputAgent IOAgent;

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
    }
}

