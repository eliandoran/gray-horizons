using System;
using System.Diagnostics;
using GrayHorizons.Extensions;
using GrayHorizons.Input;
using GrayHorizons.Logic;
using GrayHorizons.StaticObjects;
using GrayHorizons.ThirdParty;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrayHorizons.UI;

namespace GrayHorizons.Screens
{
    public class BattlefieldScreen: ExtendedGameScreen
    {
        bool coveredByOtherScreen;
        bool firstTime = true;
        SpriteBatch spriteBatch;
        Renderer renderer;
        readonly GameData gameData;
        HeadsUpScreen hud;
        Map map;
        ControllableEntity playerVehicle = new Entities.Tanks.TankE100();

        public BattlefieldScreen(
            GameData gameData,
            Map map)
        {
            this.gameData = gameData;
            this.map = map;
            gameData.Map = map;
            gameData.Map.Viewport = new Rectangle(
                0, 0,
                gameData.GraphicsDevice.Viewport.Width,
                gameData.GraphicsDevice.Viewport.Height);
            gameData.Map.ScaledViewport = gameData.Map.Viewport.ScaleTo(new Vector2(1, 1));

            TransitionOnTime = TimeSpan.FromMilliseconds(1000);
            TransitionOffTime = TimeSpan.FromMilliseconds(1000);

            Debug.WriteLine("[BINDINGS] Count: {0}.".FormatWith(gameData.Configuration.InputBindings.Count));
        }

        static void WallLine(
            Map map,
            int startX,
            int endX,
            int y)
        {
            for (int x = startX; x <= endX; x++)
                map.Add(new Wall
                    {
                        Position = new RotatedRectangle(new Rectangle(x * 64, y * 64, 64, 64), 0)
                    });
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (gameData.IsPaused)
                return;

            foreach (InputBinding binding in gameData.Configuration.InputBindings)
                binding.UpdateState();
            base.HandleInput(gameTime, input);
        }

        public override void Activate(bool instancePreserved)
        {
            base.Activate(instancePreserved);

            spriteBatch = ScreenManager.SpriteBatch;

            Loader.LoadSoundEffects(gameData);

            gameData.MapScale = new Vector2(1, 1);
            gameData.ViewportScale = new Vector2(1, 1);
            renderer = new Renderer(gameData);

            gameData.Map = map;
            gameData.IsPaused = false;
            gameData.Map.CenterViewportAt(gameData.ActivePlayer.AssignedEntity);
        }

        public override void Draw(
            GameTime gameTime)
        {
            if (gameTime.ElapsedGameTime.TotalMilliseconds > 20)
                Debug.WriteLine(String.Format("{0}ms".FormatWith(
                            gameTime.ElapsedGameTime.TotalMilliseconds.ToString())),
                    "LAG");

            if (spriteBatch.IsNull())
                return;

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
                gameData.Objectives.GetFirstUncompletedObjective().Startup();
            }

            gameData.Objectives.Update(gameTime.ElapsedGameTime);
            HandleInput(gameTime, new InputState());

            this.coveredByOtherScreen = coveredByOtherScreen;
            gameData.GameTime = gameTime;

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void OnClientSizeChanged()
        {
            gameData.Map.Viewport = new Rectangle(
                    0, 0,
                    gameData.GraphicsDevice.Viewport.Width,
                    gameData.GraphicsDevice.Viewport.Height);
            gameData.Map.ScaledViewport = gameData.Map.Viewport.ScaleTo(gameData.ViewportScale);
            gameData.Map.CenterViewportAt(gameData.ActivePlayer.AssignedEntity);
        }

    }
}

