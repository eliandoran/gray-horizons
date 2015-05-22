using System;
using System.Linq;
using GrayHorizons.Logic;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrayHorizons.Screens
{
    public class MessageScreen: GameScreen
    {
        readonly GameData gameData;
        SpriteFont font;

        public string Message { get; set; }

        TimeSpan timeToHide;

        bool AllowOverlap { get; set; }

        bool destroyOtherMessages;

        public TimeSpan TimeToHide
        {
            get
            {
                return timeToHide;
            }
            set
            {
                timeToHide = value;
                TimeLeft = value;
            }
        }

        public TimeSpan TimeLeft { get; set; }

        public MessageScreen(GameData gameData)
        {
            this.gameData = gameData;
            IsPopup = true;
            TimeToHide = TimeSpan.FromMilliseconds(2000);
            TransitionOnTime = TimeSpan.FromMilliseconds(400);
            TransitionOffTime = TimeSpan.FromMilliseconds(500);
        }

        public MessageScreen(GameData gameData, string message)
            : this(gameData)
        {
            Message = message;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!destroyOtherMessages)
            {
                ScreenManager.GetScreens().ToList().FindAll(obj => obj is MessageScreen && obj != this).ForEach(obj => obj.ExitScreen());
                destroyOtherMessages = false;
            }

            var metrics = ScreenManager.Font.MeasureString(Message);
            var padding = 20;
            var bgHeight = (int)(metrics.Y + padding * 3);
            var bgY = (int)((gameData.GraphicsDevice.Viewport.Height / 2) - metrics.Y / 2);

            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.Draw(
                gameData.BlankTexture,
                new Rectangle(0, bgY, gameData.GraphicsDevice.Viewport.Width, bgHeight),
                Color.Black * (.6f * TransitionAlpha)
            );

            ScreenManager.SpriteBatch.DrawString(
                font,
                Message,
                new Vector2(padding, bgY + padding),
                Color.White * TransitionAlpha
            );

            ScreenManager.SpriteBatch.End();
        }

        public override void LoadContent()
        {
            font = gameData.ContentManager.Load<SpriteFont>(@"Fonts\Large");
            destroyOtherMessages = !AllowOverlap;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (TimeLeft > gameTime.ElapsedGameTime)
                TimeLeft -= gameTime.ElapsedGameTime;
            else
                ExitScreen();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}

