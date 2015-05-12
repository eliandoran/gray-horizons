using System;
using BattleCity.ThirdParty.GameStateManagement;
using System.Collections.Generic;
using BattleCity.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using BattleCity.Logic;
using BattleCity.Extensions;

namespace BattleCity.Screens
{
    public class MainMenuScreen: GameScreen
    {
        bool loaded;
        bool exiting;
        readonly Menu menu;
        readonly Game game;
        readonly GameData gameData;
        SpriteBatch spriteBatch;
        Texture2D backgroundImage;

        public Menu Menu
        {
            get
            {
                return menu;
            }
        }

        public MainMenuScreen (
            GameData gameData)
        {
            menu = new Menu ();
            this.gameData = gameData;
            game = gameData.Game;

            TransitionOnTime = TimeSpan.FromMilliseconds (250);
            TransitionOffTime = TimeSpan.FromMilliseconds (500);

            loaded = false;
        }

        void GetMenuItems ()
        {
            ScreenManager.AddScreen (menu, null);

            var menuItems = new [] {
                new MenuItem ("Single Player"),
                new MenuItem ("Map Editor"),
                new MenuItem ("Settings"),
                new MenuItem ("Exit")
            };

            menuItems [0].Activate += (
                sender,
                e) =>
            {
                ScreenManager.AddScreen (new BattlefieldScreen (gameData), null);
                //ExitScreen ();
            };

            menuItems [2].Activate += (
                sender,
                e) =>
            {
                ScreenManager.AddScreen (new SettingsMenuScreen (gameData, this), null);
            };

            menuItems [3].Activate += (
                sender,
                e) =>
            {
                if (!exiting)
                {
                    ScreenManager.Game.Exit ();
                    exiting = true;
                }
            };

            foreach (var menuItem in menuItems)
            {
                menuItem.CenterVertically = true;
                menuItem.TextPosition = new Vector2 (100, 0);
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

        void DrawFooter ()
        {
            var font = ScreenManager.Font;
            const string footerText = "Pentru \"Grigore Moisil\", Lugoj \n(C) 2015 Doran Adoris Elian";
            var metrics = font.MeasureString (footerText);
            const int padding = 10;
            var x = ScreenManager.Game.Window.ClientBounds.Width - metrics.X - padding;
            var y = ScreenManager.Game.Window.ClientBounds.Height - metrics.Y - padding;
            var color = new Color (255, 255, 255, 100);

            spriteBatch.DrawString (ScreenManager.Font, footerText, new Vector2 (x, y), color);
        }

        public override void Draw (
            GameTime gameTime)
        {
            spriteBatch.Begin ();
            spriteBatch.Draw (backgroundImage, destinationRectangle: ScreenManager.Game.Window.ClientBounds);
            DrawFooter ();

            spriteBatch.End ();

            if (IsExiting)
            {
                //menu.ExitScreen ();
                ScreenManager.FadeBackBufferToBlack (0.75f * TransitionPosition);
            }
        }

        public override void Update (
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update (gameTime, otherScreenHasFocus, coveredByOtherScreen);


            bool enabled = true;
            foreach (GameScreen screen in ScreenManager.GetScreens())
                if (screen is SettingsMenuScreen)
                    enabled = false;
            menu.Enabled = enabled;

            if (!loaded)
            {
                GetMenuItems ();
                loaded = true;
            }
        }

        public override void LoadContent ()
        {
            loaded = false;
            spriteBatch = ScreenManager.SpriteBatch;
            backgroundImage = ScreenManager.Game.Content.Load<Texture2D> ("TankWallpaper");
            Sound.UISounds.MenuSelect.Sounds.Add (ScreenManager.Game.Content.Load<SoundEffect> ("Sounds\\MenuSelect"));
        }

        public override void UnloadContent ()
        {
            base.UnloadContent ();
        }
    }
}

