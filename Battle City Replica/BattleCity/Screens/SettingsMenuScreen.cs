using System;
using BattleCity.UI;
using Microsoft.Xna.Framework;
using BattleCity.Logic;
using Microsoft.Xna.Framework.Graphics;
using BattleCity.ThirdParty.GameStateManagement;

namespace BattleCity.Screens
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

        public SettingsMenuScreen (
            GameData gameData,
            GameScreen parentScreen)
        {
            menu = new Menu ();
            this.gameData = gameData;
            this.parentScreen = parentScreen;
            game = gameData.Game;

            IsPopup = true;
        }

        void GetMenuItems ()
        {
            ScreenManager.AddScreen (menu, null);

            var menuItems = new MenuItem[] {
                new MenuItem ("Input"),
                new MenuItem ("Audio"),
                new MenuItem ("Video"),
                new MenuItem ("< Back")
            };

            menuItems [3].Activate += (
                sender,
                e) =>
            {
                menu.Unload ();
                ExitScreen ();
            };

            foreach (var menuItem in menuItems)
            {
                menuItem.CenterVertically = true;
                menuItem.TextPosition = new Vector2 (400, 0);
                menuItem.Color = menuItem.SelectedColor = new Color (255, 255, 255, 100);
            }

            var screenWidth = ScreenManager.Game.Window.ClientBounds.Width;
            var screenHeight = ScreenManager.Game.Window.ClientBounds.Height;

            menu.ItemSize = new Point (screenWidth, 60);
            menu.ItemPadding = new Point (0, 15);
            menu.MenuItems.AddRange (menuItems);
            menu.Position = new Point (0, screenHeight - menu.Height - 50);
            menu.AddComponents ();
            menu.SelectedMenuItem = menuItems [0];
        }

        public override void Draw (
            GameTime gameTime)
        {
            spriteBatch.Begin ();
            spriteBatch.End ();
        }

        public override void LoadContent ()
        {
            spriteBatch = ScreenManager.SpriteBatch;
        }

        public override void Update (
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            if (!loaded)
            {
                GetMenuItems ();

                loaded = true;
            }
        }
    }
}

