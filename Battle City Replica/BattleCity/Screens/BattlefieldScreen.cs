using System;
using System.Diagnostics;
using BattleCity.Entities;
using BattleCity.Extensions;
using BattleCity.Input;
using BattleCity.Input.Actions;
using BattleCity.Logic;
using BattleCity.StaticObjects;
using BattleCity.ThirdParty;
using BattleCity.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleCity.Screens
{
    public class BattlefieldScreen: GameScreen
    {
        bool coveredByOtherScreen;
        ContentManager content;
        SpriteBatch spriteBatch;
        Renderer renderer;
        readonly GameData gameData;
        Soldier playerVehicle = new Soldier ();
        Loader loader;

        public BattlefieldScreen (
            GameData gameData)
        {
            this.gameData = gameData;

            TransitionOnTime = TimeSpan.FromMilliseconds (1000);
            TransitionOffTime = TimeSpan.FromMilliseconds (1000);
        }

        static void WallLine (
            Map map,
            int startX,
            int endX,
            int y)
        {
            for (int x = startX; x <= endX; x++)
                map.Add (new Wall () { Position = new RotatedRectangle (new Rectangle (x * 64, y * 64, 64, 64), 0) });
        }

        Map GetTestMap ()
        {
            var map = new Map (new Vector2 (2000, 2000), gameData);

            gameData.ActivePlayer.AssignedEntity = playerVehicle;
            playerVehicle.Position = new RotatedRectangle (new Rectangle (5 * 64,
                5 * 64,
                playerVehicle.DefaultSize.X,
                playerVehicle.DefaultSize.Y),
                0);

            map.Viewport = new Rectangle (0, 0, 768, 768);
            map.ScaledViewport = map.Viewport.Scale (1);

            playerVehicle.Moved += (
                sender,
                e) => gameData.Map.CenterViewportAt (playerVehicle);

            map.Add (playerVehicle);

            var enemyTank = new TankVK3601h ();
            enemyTank.CanBeBoarded = true;
            enemyTank.Position = new RotatedRectangle (new Rectangle (8 * 64,
                6 * 64,
                enemyTank.DefaultSize.X,
                enemyTank.DefaultSize.Y),
                90);
                                                         
            enemyTank.MuzzlePosition = new RotatedRectangle (new Rectangle (192, 37, 5, 8),
                playerVehicle.Position.Rotation);  
            map.Add (enemyTank);

            WallLine (map, 0, 9, 9);
            WallLine (map, 0, 9, 0);

            var player = gameData.ActivePlayer;
            var moveForwardKeyBinding = new KeyBinding(gameData, null, Keys.W, true);
            moveForwardKeyBinding.BoundAction = new MoveForwardAction(player, moveForwardKeyBinding);

            var moveBackwardKeyBinding = new KeyBinding(gameData, null, Keys.S, true);
            moveBackwardKeyBinding.BoundAction = new MoveBackwardAction(player, moveBackwardKeyBinding);

            var turnLeftKeyBinding = new KeyBinding(gameData, null, Keys.A, true);
            turnLeftKeyBinding.BoundAction = new TurnLeftAction(player, turnLeftKeyBinding);

            var turnRightKeyBinding = new KeyBinding(gameData, null, Keys.D, true);
            turnRightKeyBinding.BoundAction = new TurnRightAction(player, turnRightKeyBinding);

            var turretAxisBinding = new MouseAxisBinding(gameData);
            turretAxisBinding.BoundAction = new TankMouseTurretControl(gameData, player, turretAxisBinding);

            gameData.InputBindings.AddRange(new InputBinding[] {
                moveForwardKeyBinding,
                moveBackwardKeyBinding,
                turnLeftKeyBinding,
                turnRightKeyBinding,
                new KeyBinding (gameData, new ShootAction (player)),
                turretAxisBinding,

                new KeyBinding (gameData, new ToggleGuidesTraceAction (gameData)),
                new KeyBinding (gameData, new MetamorphosizeTank (gameData, player)),
                new KeyBinding (gameData, new ToggleFullScreenAction (gameData)),
                new KeyBinding (gameData, new ZoomInAction (gameData), allowContinousPress: true),
                new KeyBinding (gameData, new ZoomOutAction (gameData), allowContinousPress: true),
                new KeyBinding (gameData, new TogglePauseAction (gameData))
            });

            if (gameData.Configuration == null)
            {
                var config = new Configuration();
                foreach (InputBinding binding in gameData.InputBindings)
                    config.InputBindings.Add(binding);
                config.FullScreenResolution = new Size(1280, 800);
                gameData.Configuration = config;
            }

            if (gameData.Configuration != null)
                gameData.Configuration.Save (@"config.xml");

            return map;
        }

        public override void HandleInput (
            InputState input)
        {
            if (coveredByOtherScreen || gameData.IsPaused)
                return;

            foreach (InputBinding binding in gameData.Configuration.InputBindings)
            {
                binding.UpdateState ();
            }
        }

        public override void LoadContent ()
        {

            spriteBatch = ScreenManager.SpriteBatch;

            content = gameData.Game.Content;
            loader = new Loader (content);
            loader.LoadSoundEffects ();

            gameData.SpriteBatch = spriteBatch;
            gameData.ContentManager = content;
            gameData.GraphicsDevice = ScreenManager.GraphicsDevice;
            gameData.Scale = new Vector2 (1, 1);
            renderer = new Renderer (gameData);

            gameData.Map = GetTestMap ();
        }

        public override void Draw (
            GameTime gameTime)
        {
            #if DEBUG
            if (gameTime.ElapsedGameTime.TotalMilliseconds > 17)
                Debug.WriteLine (String.Format ("{0}ms".FormatWith (gameTime.ElapsedGameTime.TotalMilliseconds.ToString ())),
                    "LAG");
            #endif

            spriteBatch.Begin ();

            if (!gameData.IsPaused)
                gameData.Map.Update (gameTime.ElapsedGameTime);

            renderer.Render ();
            spriteBatch.End ();

            ScreenManager.FadeBackBufferToBlack (TransitionPosition);
        }

        public override void Update (
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            this.coveredByOtherScreen = coveredByOtherScreen;
            gameData.GameTime = gameTime;

            base.Update (gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}

