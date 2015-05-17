using System;
using System.Diagnostics;
using GrayHorizons.Entities;
using GrayHorizons.Extensions;
using GrayHorizons.Input;
using GrayHorizons.StaticObjects;
using GrayHorizons.ThirdParty;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrayHorizons.Maps;
using GrayHorizons.Logic;

namespace GrayHorizons.Screens
{
    public class BattlefieldScreen: GameScreen
    {
        bool coveredByOtherScreen;
        bool firstTime = true;
        SpriteBatch spriteBatch;
        Renderer renderer;
        readonly GameData gameData;
        HeadsUpScreen hud;
        ControllableEntity playerVehicle = new Entities.Tanks.TankE100();

        public BattlefieldScreen(
            GameData gameData)
        {
            this.gameData = gameData;

            TransitionOnTime = TimeSpan.FromMilliseconds(1000);
            TransitionOffTime = TimeSpan.FromMilliseconds(1000);

            Debug.WriteLine("[BINDINGS] Count: {0}.", gameData.Configuration.InputBindings.Count);
        }

        static void WallLine(
            Map map,
            int startX,
            int endX,
            int y)
        {
            for (int x = startX; x <= endX; x++)
                map.Add(new Wall() { Position = new RotatedRectangle(new Rectangle(x * 64, y * 64, 64, 64), 0) });
        }

        Map GetTestMap()
        {
            var map = new DesertMap(gameData);

            gameData.ActivePlayer.AssignedEntity = playerVehicle;
            playerVehicle.Position = new RotatedRectangle(new Rectangle(5 * 64,
                    5 * 64,
                    playerVehicle.DefaultSize.X,
                    playerVehicle.DefaultSize.Y),
                0);

            map.Viewport = new Rectangle(
                0, 0,
                gameData.Game.GraphicsDevice.Viewport.Width,
                gameData.Game.GraphicsDevice.Viewport.Height);

            gameData.ViewportScale = new Vector2(1, 1);
            map.ScaledViewport = map.Viewport.ScaleTo(gameData.ViewportScale.X);
            map.GameData = gameData;

            playerVehicle.Moved += (
                sender,
                e) => gameData.Map.CenterViewportAt(playerVehicle);

            map.Add(playerVehicle);

            var enemyTank = new Entities.Tanks.TankVK3601h();
            enemyTank.CanBeBoarded = true;
            enemyTank.Position = new RotatedRectangle(new Rectangle(8 * 64,
                    6 * 64,
                    enemyTank.DefaultSize.X,
                    enemyTank.DefaultSize.Y),
                90);
            enemyTank.AI = new AI.VanillaAI();
            enemyTank.AI.GameData = gameData;
            map.Add(enemyTank);

            var car = new Entities.Cars.MinelayerPickup();
            car.Position = new RotatedRectangle(
                new Rectangle(12 * 64, 12 * 64, car.DefaultSize.X, car.DefaultSize.Y),
                0);
            car.CanBeBoarded = true;
            map.Add(car);

            WallLine(map, 0, 9, 9);
            WallLine(map, 0, 9, 0);

            if (gameData.Configuration != null)
                gameData.Configuration.Save(gameData.IOAgent, @"config.xml");

            return map;
        }

        public override void HandleInput(
            InputState input)
        {
            if (gameData.IsPaused)
                return;

            foreach (InputBinding binding in gameData.Configuration.InputBindings)
            {
                binding.UpdateState();
            }
        }

        public override void LoadContent()
        {
            spriteBatch = ScreenManager.SpriteBatch;

            Loader.LoadSoundEffects(gameData);

            gameData.MapScale = new Vector2(1, 1);
            renderer = new Renderer(gameData);

            gameData.Map = GetTestMap();
            gameData.IsPaused = false;

            //new GrayHorizons.Input.Actions.ToggleFullScreenAction (gameData).Execute ();
        }

        public override void Draw(
            GameTime gameTime)
        {
            #if DEBUG
            if (gameTime.ElapsedGameTime.TotalMilliseconds > 17)
                Debug.WriteLine(String.Format("{0}ms".FormatWith(gameTime.ElapsedGameTime.TotalMilliseconds.ToString())),
                    "LAG");
            #endif

            spriteBatch.Begin();

            if (!gameData.IsPaused)
                gameData.Map.Update(gameTime.ElapsedGameTime);

            renderer.Render();
            spriteBatch.End();

            ScreenManager.FadeBackBufferToBlack(TransitionPosition);
        }

        public override void Update(
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            if (firstTime)
            {
                hud = new HeadsUpScreen(gameData);
                ScreenManager.AddScreen(hud, null);
                firstTime = false;
            }

            HandleInput(new InputState());

            this.coveredByOtherScreen = coveredByOtherScreen;
            gameData.GameTime = gameTime;

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}

