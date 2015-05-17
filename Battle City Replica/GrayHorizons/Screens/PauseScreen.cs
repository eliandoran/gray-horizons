using System;
using GrayHorizons.ThirdParty.GameStateManagement;
using GrayHorizons.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using GrayHorizons.Logic;

namespace GrayHorizons.Screens
{
    public class PauseScreen: GameScreen
    {
        bool loaded;
        readonly Menu menu;
        readonly Game game;
        readonly GameData gameData;
        SpriteBatch spriteBatch;

        public PauseScreen(
            GameData gameData)
        {
            menu = new Menu();
            this.gameData = gameData;
            game = gameData.Game;

            TransitionOffTime = TimeSpan.FromMilliseconds(500);
            TransitionOnTime = TimeSpan.FromMilliseconds(500);
            IsPopup = true;
        }

        void GetMenuItems()
        {
            ScreenManager.AddScreen(menu, null);

            var menuItems = new []
            {
                new MenuItem("Resume"),
                new MenuItem("Main Menu"),
                new MenuItem("Exit Game")
            };

            menuItems[0].Activate += (
                sender,
                e) =>
            {
                menu.Unload();
                ExitScreen();
                gameData.IsPaused = false;
            };

            menuItems[1].Activate += (
                sender,
                e) =>
            {
                var screens = gameData.ScreenManager.GetScreens();
                foreach (var screen in screens)
                {
                    screen.ExitScreen();
                }

                gameData.ScreenManager.AddScreen(new MainMenuScreen(gameData), null);
            };

            menuItems[2].Activate += (
                sender,
                e) =>
                ScreenManager.Game.Exit();

            foreach (var menuItem in menuItems)
            {
                menuItem.CenterVertically = true;
                menuItem.CenterHorizontally = true;
                menuItem.TextPosition = new Vector2(100, 0);
                menuItem.Color = menuItem.SelectedColor = new Color(255, 255, 255, 100);
                menuItem.SelectedBackColor = new Color(204, 102, 0, 50);
            }

            var screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            var screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            menu.ItemSize = new Point(screenWidth, 60);
            menu.ItemPadding = new Point(0, 15);
            menu.MenuItems.AddRange(menuItems);
            menu.Position = new Point(0, (screenHeight - menu.Height) / 2);
            menu.AddComponents();
            menu.SelectedMenuItem = menuItems[0];
        }

        public override void Draw(
            GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.End();

            ScreenManager.FadeBackBufferToBlack(0.5f * TransitionAlpha);
        }

        public override void LoadContent()
        {
            spriteBatch = ScreenManager.SpriteBatch;
        }

        public override void UnloadContent()
        {
            menu.ExitScreen();
            base.UnloadContent();
        }

        public override void HandleInput(
            InputState input)
        {
        }

        public override void Update(
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            menu.Enabled = !coveredByOtherScreen;

            if (!loaded)
            {
                GetMenuItems();

                loaded = true;
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}

