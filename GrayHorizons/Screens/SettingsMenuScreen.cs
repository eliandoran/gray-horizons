using System;
using GrayHorizons.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
using GrayHorizons.Logic;

namespace GrayHorizons.Screens
{
    public class SettingsMenuScreen: GameScreen
    {
        bool loaded;
        readonly Menu menu;
        readonly Game game;
        readonly GameData gameData;
        readonly GameScreen parentScreen;
        SpriteBatch spriteBatch;

        public Menu Menu
        {
            get
            {
                return menu;
            }
        }

        public SettingsMenuScreen(
            GameData gameData,
            GameScreen parentScreen)
        {
            menu = new Menu();
            this.gameData = gameData;
            this.parentScreen = parentScreen;
            game = gameData.Game;

            IsPopup = true;
        }

        void GetMenuItems()
        {
            ScreenManager.AddScreen(menu, null);

            var menuItems = new []
            {
                new MenuItem("Input"),
                new MenuItem("Audio"),
                new MenuItem("Video"),
                new MenuItem("< Back")
            };

            menuItems[3].Executed += (
                sender,
                e) =>
            {
                menu.Unload();
                ExitScreen();
            };

            foreach (var menuItem in menuItems)
            {
                menuItem.CenterVertically = true;
                menuItem.TextPosition = new Vector2(400, 0);
                menuItem.Color = menuItem.SelectedColor = new Color(255, 255, 255, 100);
            }

            var screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            var screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            menu.ItemSize = new Point(screenWidth, 60);
            menu.ItemPadding = new Point(0, 15);
            menu.MenuItems.AddRange(menuItems);
            menu.Position = new Point(0, screenHeight - menu.Height - 50);
            menu.AddComponents();
            menu.SelectedMenuItem = menuItems[0];
        }

        public override void Draw(
            GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.End();
        }

        public override void Activate(bool instancePreserved)
        {
            spriteBatch = ScreenManager.SpriteBatch;
        }

        public override void Update(
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            if (!loaded)
            {
                GetMenuItems();

                loaded = true;
            }
        }
    }
}

