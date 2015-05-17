using System;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using GrayHorizons.Logic;

namespace GrayHorizons.Screens.HeadsUp
{
    public class MinimapScreen: GameScreen
    {
        int lastScreenWidth, lastScreenHeight;

        public enum MinimapPosition
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        GameData gameData;

        public Point Size { get; set; }

        MinimapPosition position;

        public MinimapPosition Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                CalculatePosition();
            }
        }

        Point RealPosition { get; set; }

        public Point Padding { get; set; }

        Rectangle rect;

        public MinimapScreen(GameData gameData, MinimapPosition defaultPosition)
        {
            IsPopup = true;
            this.gameData = gameData;

            Size = new Point(256, 256);
            Padding = new Point(10, 10);
            Position = defaultPosition;
            CalculatePosition();
        }

        public override void Draw(GameTime gameTime)
        {
            rect = GetRectangle();

            ScreenManager.SpriteBatch.Begin();

            DrawFrame(5);
            DrawBackground();
            DrawEntities();

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawFrame(int padding)
        {
            ScreenManager.SpriteBatch.Draw(gameData.Map.Texture,
                new Rectangle(rect.X - padding,
                    rect.Y - padding,
                    rect.Width + padding * 2,
                    rect.Height + padding * 2),
                new Color(0, 0, 0, 50));
        }

        void DrawBackground()
        {
            ScreenManager.SpriteBatch.Draw(gameData.Map.Texture,
                rect,
                Color.White * 0.7f);
        }

        void DrawEntities()
        {
            var scale = new Vector2(Size.X / gameData.Map.MapSize.X, Size.Y / gameData.Map.MapSize.Y);

            foreach (ObjectBase entity in gameData.Map.GetObjects())
            {
                if (!entity.MinimapColor.HasValue)
                    continue;

                var pos = RealPosition.ToVector2() + (entity.Position.CollisionRectangle.Center.ToVector2() * scale);
                ScreenManager.SpriteBatch.Draw(gameData.BlankTexture,
                    pos,
                    color: entity.MinimapColor.Value,
                    rotation: entity.Position.Rotation);
            }
        }

        void CalculatePosition()
        {
            var screenWidth = gameData.GraphicsDevice.Viewport.Width;
            var screenHeight = gameData.GraphicsDevice.Viewport.Height;
            var x = (Position == MinimapPosition.TopRight || Position == MinimapPosition.BottomRight ? screenWidth - Size.X - Padding.X : Padding.Y);
            var y = (Position == MinimapPosition.BottomLeft || Position == MinimapPosition.BottomRight ? screenHeight - Size.Y - Padding.Y : Padding.Y);
            lastScreenWidth = screenWidth;
            lastScreenHeight = screenHeight;
            RealPosition = new Point(x, y);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (lastScreenWidth != gameData.GraphicsDevice.Viewport.Width ||
                lastScreenHeight != gameData.GraphicsDevice.Viewport.Height)
            {
                CalculatePosition();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        Rectangle GetRectangle()
        {
            return new Rectangle(
                RealPosition.X,
                RealPosition.Y,
                Size.X,
                Size.Y
            );
        }
    }
}
