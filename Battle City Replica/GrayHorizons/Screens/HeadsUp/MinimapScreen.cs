using System;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using GrayHorizons.Logic;
using GrayHorizons.Extensions;

namespace GrayHorizons.Screens.HeadsUp
{
    public class MiniMapScreen: GameScreen
    {
        public enum MiniMapPosition
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        GameData gameData;

        public Point Size { get; set; }

        MiniMapPosition position;

        public MiniMapPosition Position
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

        public MiniMapScreen(GameData gameData, MiniMapPosition defaultPosition)
        {
            IsPopup = true;
            this.gameData = gameData;

            Size = new Point(256, 256);
            Padding = new Point(10, 10);
            Position = defaultPosition;

            gameData.ResolutionChanged += GameData_ResolutionChanged;
            CalculatePosition();
        }

        void GameData_ResolutionChanged(object sender, EventArgs e)
        {
            CalculatePosition();
        }

        public override void Draw(GameTime gameTime)
        {
            rect = GetRectangle();

            ScreenManager.SpriteBatch.Begin();

            DrawFrame(5);
            DrawBackground();
            DrawEntities();
            if (gameData.DebuggingSettings.ShowGuides)
                DrawCollisionRectangles();

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
                if (!entity.MiniMapColor.HasValue)
                    continue;

                var pos = RealPosition.ToVector2() + (entity.Position.CollisionRectangle.Center.ToVector2() * scale);
                ScreenManager.SpriteBatch.Draw(
                    gameData.BlankTexture,
                    pos,
                    color: entity.MiniMapColor.Value,
                    rotation: entity.Position.Rotation);
            }
        }

        void DrawCollisionRectangles()
        {
            var scale = new Vector2(Size.X / gameData.Map.MapSize.X, Size.Y / gameData.Map.MapSize.Y);

            foreach (CollisionBoundary boundary in gameData.Map.CollisionBoundaries)
                ScreenManager.SpriteBatch.Draw(
                    gameData.BlankTexture,
                    boundary.ToRectangle().ScaleTo(scale).OffsetBy(RealPosition),
                    Color.Red * .60f
                );
        }

        void CalculatePosition()
        {
            var screenWidth = gameData.GraphicsDevice.Viewport.Width;
            var screenHeight = gameData.GraphicsDevice.Viewport.Height;
            var x = (Position == MiniMapPosition.TopRight || Position == MiniMapPosition.BottomRight ? screenWidth - Size.X - Padding.X : Padding.Y);
            var y = (Position == MiniMapPosition.BottomLeft || Position == MiniMapPosition.BottomRight ? screenHeight - Size.Y - Padding.Y : Padding.Y);
            RealPosition = new Point(x, y);
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
