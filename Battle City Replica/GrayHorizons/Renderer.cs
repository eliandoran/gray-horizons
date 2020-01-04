﻿namespace GrayHorizons
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using GrayHorizons.Logic;
    using GrayHorizons.ThirdParty;
    using GrayHorizons.Extensions;

    /// <summary>
    /// Represents a collection of types and MonoGame textures.
    /// </summary>
    public class Renderer
    {
        readonly GameData gameData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Renderer"/> class.
        /// </summary>
        public Renderer(
            GameData gameData)
        {
            this.gameData = gameData;

            Loader.LoadMappedTextures(gameData);
        }

        public static void DrawRotatedRect(
            GameData gameData,
            RotatedRectangle rect)
        {
            var rotation = rect.Rotation;
            var lowerLeft = rect.LowerLeftCorner();
            var lowerRight = rect.LowerRightCorner();
            var upperLeft = rect.UpperLeftCorner();
            var upperRight = rect.UpperRightCorner();

            var blankTexture = gameData.ContentManager.Load<Texture2D>("blank");

            var frontColor = new Color(0, 0, 255);
            var rearColor = new Color(255, 0, 0);

            gameData.ScreenManager.SpriteBatch.Draw(blankTexture,
                gameData.Map.CalculateViewportCoordinates(lowerLeft,
                    gameData.MapScale),
                rotation: rotation,
                scale: gameData.MapScale,
                color: rearColor);
            gameData.ScreenManager.SpriteBatch.Draw(blankTexture,
                gameData.Map.CalculateViewportCoordinates(lowerRight,
                    gameData.MapScale),
                rotation: rotation,
                scale: gameData.MapScale,
                color: frontColor);
            gameData.ScreenManager.SpriteBatch.Draw(blankTexture,
                gameData.Map.CalculateViewportCoordinates(upperLeft,
                    gameData.MapScale),
                rotation: rotation,
                scale: gameData.MapScale,
                color: rearColor);
            gameData.ScreenManager.SpriteBatch.Draw(blankTexture,
                gameData.Map.CalculateViewportCoordinates(upperRight,
                    gameData.MapScale),
                rotation: rotation,
                scale: gameData.MapScale,
                color: frontColor);
        }

        static void DrawCollisionBoundaries(GameData gameData)
        {
            if (!gameData.DebuggingSettings.ShowGuides)
                return;

            var boundaries = gameData.Map.CollisionBoundaries;

            foreach (CollisionBoundary boundary in boundaries)
            {
                var scaled = boundary.ToRectangle().ScaleTo(gameData.MapScale);

                if (gameData.Map.IntersectsViewport(boundary.ToRotatedRectangle()))
                {
                    var viewportRect =
                        new Rectangle(
                            gameData.Map.CalculateViewportCoordinates(boundary.ToVector2(), gameData.MapScale).ToPoint(),
                            scaled.Size);

                    gameData.ScreenManager.SpriteBatch.Draw(
                        gameData.BlankTexture,
                        destinationRectangle: viewportRect,
                        color: Color.Red * .45f,
                        rotation: 0,
                        origin: Vector2.Zero
                    );
                }
            }
        }

        /// <summary>
        /// Draws the guides.
        /// </summary>
        /// <param name="obj">Object.</param>
        public static void DrawGuides(
            GameData gameData,
            ObjectBase obj)
        {
            if (!gameData.DebuggingSettings.ShowGuides)
                return;

            DrawRotatedRect(gameData, obj.Position);

            var tank = obj as Tank;
            if (tank.IsNotNull() && tank.MuzzleRectangle.IsNotNull())
            {
                if (tank.TurretRect.IsNull())
                    return;

                var muzzlePos = tank.GetMuzzleRotatedRectangle();
                var muzzleX = muzzlePos.CollisionRectangle.X;
                var muzzleY = muzzlePos.CollisionRectangle.Y;

                var muzzleViewportPos = gameData.Map.CalculateViewportCoordinates(
                                            new Vector2(muzzleX, muzzleY),
                                            gameData.MapScale);
                var rect = new Rectangle(
                               (int)muzzleViewportPos.X,
                               (int)muzzleViewportPos.Y,
                               muzzlePos.CollisionRectangle.Width,
                               muzzlePos.CollisionRectangle.Height);

                gameData.ScreenManager.SpriteBatch.Draw(
                    gameData.BlankTexture,
                    destinationRectangle: rect,
                    rotation: muzzlePos.Rotation,
                    scale: gameData.MapScale);

                DrawRotatedRect(gameData, tank.TurretRect);
            }
        }

        public static Rectangle GetSpriteFromSpriteImage(
            Texture2D texture,
            int spriteNumber,
            int rows,
            int columns)
        {
            var width = texture.Width / columns;
            var height = texture.Height / rows;
            var row = (int)Math.Floor((float)spriteNumber / (float)columns);
            var col = spriteNumber - (row * rows);

            if (col < 0)
                col = 0;

            return new Rectangle(col * width, row * height, width, height);
        }

        public Vector2 CoordinatesToVector2(
            Coordinate coordinate)
        {
            return new Vector2(coordinate.X * 64, coordinate.Y * 64);
        }

        public void Render()
        {
            gameData.ScreenManager.SpriteBatch.End();
            gameData.ScreenManager.SpriteBatch.Begin(
                transformMatrix: gameData.TranslationMatrix);

            gameData.Map.Render();
            DrawCollisionBoundaries(gameData);

            gameData.Map.GetObjects().ForEach(obj =>
                {
                    obj.Render();
                    Renderer.DrawGuides(gameData, obj);
                });

            gameData.ScreenManager.SpriteBatch.End();
        }
    }
}