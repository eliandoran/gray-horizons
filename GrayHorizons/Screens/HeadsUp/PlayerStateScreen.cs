﻿using System;
using GameStateManagement;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrayHorizons.UI;

namespace GrayHorizons.Screens.HeadsUp
{
    public class PlayerStateScreen: GameScreen
    {
        readonly GameData gameData;
        SpriteBatch spriteBatch;
        bool firstTime = true;

        public Player Player { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Padding { get; set; }

        Texture2D healthFullIcon;
        Texture2D healthHalfIcon;
        Texture2D healthEmptyIcon;
        Texture2D ammunitionIcon;

        ProgressBar healthProgressBar = new ProgressBar()
        {
            FilledColor = Color.Red * .70f
        };

        ProgressBar ammunitionProgressBar = new ProgressBar();

        public PlayerStateScreen(GameData gameData)
        {
            this.gameData = gameData;
            Player = gameData.ActivePlayer;
            IsPopup = true;
            Width = 170;
            Height = 50;
            Padding = 10;
        }

        public override void Draw(GameTime gameTime)
        {
            if (firstTime)
            {
                spriteBatch = ScreenManager.SpriteBatch;
                firstTime = false;
            }

            var screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            var screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            var rect = new Rectangle(
                           screenWidth - Width - Padding,
                           screenHeight - Height - Padding,
                           Width,
                           Height
                       );

            spriteBatch.Begin();

//            spriteBatch.Draw(
//                gameData.BlankTexture,
//                destinationRectangle: rect,
//                color: Color.Black * .25f
//            );

            Texture2D healthIcon;
            var health = gameData.ActivePlayer.AssignedEntity.Health;
            var percentage = gameData.ActivePlayer.AssignedEntity.HealthPercentage;
            if (percentage > .5)
                healthIcon = healthFullIcon;
            else if (percentage > .25)
                healthIcon = healthHalfIcon;
            else
                healthIcon = healthEmptyIcon;

            var iconWidth = healthIcon.Width;
            var iconHeight = healthIcon.Height;

            spriteBatch.Draw(
                healthIcon,
                new Vector2(rect.X + Padding, rect.Y + Padding)
            );

            var playerEntity = gameData.ActivePlayer.AssignedEntity;
            var progressBarWidth = rect.Width - iconWidth - Padding * 2;
            var progressBarHeight = iconHeight / 2;
            healthProgressBar.CurrentValue = health;
            healthProgressBar.MaximumValue = playerEntity.MaximumHealth;
            var healthY = rect.Y + ((iconHeight / 2) - (progressBarHeight / 2)) + Padding;
            healthProgressBar.Position = new Rectangle(
                rect.X + iconWidth + Padding * 2,
                healthY,
                progressBarWidth,
                progressBarHeight
            );

            spriteBatch.Draw(
                ammunitionIcon,
                position: new Vector2(rect.X + Padding, rect.Y + Padding * 2 + healthIcon.Height)
            );

            ammunitionProgressBar.CurrentValue = playerEntity.AmmoLeft;
            ammunitionProgressBar.MaximumValue = playerEntity.AmmoCapacity;
            ammunitionProgressBar.Position = new Rectangle(
                rect.X + iconWidth + Padding * 2,
                healthY + ((iconHeight / 2) - (progressBarHeight / 2)) + Padding * 2,
                progressBarWidth,
                progressBarHeight
            );

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Activate(bool instancePreserved)
        {
            healthFullIcon = gameData.ContentManager.Load<Texture2D>(@"Icons\HealthFull");
            healthHalfIcon = gameData.ContentManager.Load<Texture2D>(@"Icons\HealthHalf");
            healthEmptyIcon = gameData.ContentManager.Load<Texture2D>(@"Icons\HealthEmpty");
            ammunitionIcon = gameData.ContentManager.Load<Texture2D>(@"Icons\Ammunition");

            ScreenManager.AddScreen(healthProgressBar, null);
            ScreenManager.AddScreen(ammunitionProgressBar, null);
            base.Activate(instancePreserved);
        }
    }
}

