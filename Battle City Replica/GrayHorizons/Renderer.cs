using System;
using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using GrayHorizons.Logic;
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrayHorizons
{
    /// <summary>
    /// Represents a collection of types and MonoGame textures.
    /// </summary>
    public class Renderer
    {
        readonly GameData gameData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Renderer"/> class.
        /// </summary>
        public Renderer (
            GameData gameData)
        {
            this.gameData = gameData;

            Loader.LoadMappedTextures (gameData);
        }

        /// <summary>
        /// Renders the terrain.
        /// </summary>
        /// <param name="mapSize">Map size.</param>
        public void RenderTerrain (
            Vector2 mapSize)
        {            
            var texture = gameData.Map.Texture;
            if (texture == null)
            {
                MappedTexturesAttribute attr = new MappedTexturesAttribute (new [] { "Dirt\\01" });
                texture = gameData.ContentManager.Load<Texture2D> (attr.GetRandomTexture ());
            }

            if (texture.Width >= mapSize.X && texture.Height >= mapSize.Y)
            {
                var pos = gameData.Map.CalculateViewportCoordinates (Vector2.Zero, gameData.MapScale);
                gameData.SpriteBatch.Draw (texture, pos);
            }
            else
            {
                int rows = (int)(Math.Ceiling (mapSize.Y / texture.Height));
                int cols = (int)(Math.Ceiling (mapSize.X / texture.Width));

                for (var row = 0; row <= rows; row++)
                    for (var col = 0; col <= cols; col++)
                    {
                        var rect = new Rectangle (
                                       col * texture.Width,
                                       row * texture.Height,
                                       texture.Width,
                                       texture.Height
                                   );

                        var pos = gameData.Map.CalculateViewportCoordinates (new Vector2 (
                                          rect.X,
                                          rect.Y
                                      ), gameData.MapScale);

                        gameData.SpriteBatch.Draw (texture, pos, scale: gameData.MapScale);
                    }
            }
        }

        public static void DrawRotatedRect (
            GameData gameData,
            RotatedRectangle rect)
        {
            var rotation = rect.Rotation;
            var lowerLeft = rect.LowerLeftCorner ();
            var lowerRight = rect.LowerRightCorner ();
            var upperLeft = rect.UpperLeftCorner ();
            var upperRight = rect.UpperRightCorner ();

            var blankTexture = gameData.ContentManager.Load<Texture2D> ("blank");

            gameData.SpriteBatch.Draw (blankTexture,
                                       gameData.Map.CalculateViewportCoordinates (lowerLeft, gameData.MapScale),
                                       rotation: rotation,
                                       scale: gameData.MapScale);
            gameData.SpriteBatch.Draw (blankTexture,
                                       gameData.Map.CalculateViewportCoordinates (lowerRight, gameData.MapScale),
                                       rotation: rotation,
                                       scale: gameData.MapScale);
            gameData.SpriteBatch.Draw (blankTexture,
                                       gameData.Map.CalculateViewportCoordinates (upperLeft, gameData.MapScale),
                                       rotation: rotation,
                                       scale: gameData.MapScale);
            gameData.SpriteBatch.Draw (blankTexture,
                                       gameData.Map.CalculateViewportCoordinates (upperRight, gameData.MapScale),
                                       rotation: rotation,
                                       scale: gameData.MapScale);
        }

        /// <summary>
        /// Draws the guides.
        /// </summary>
        /// <param name="obj">Object.</param>
        public static void DrawGuides (
            GameData gameData,
            ObjectBase obj)
        {
            if (!gameData.DebuggingSettings.ShowGuides)
                return;//return;

            DrawRotatedRect (gameData, obj.Position);

            var tank = obj as Tank;
            if (tank != null && tank.MuzzlePosition != null)
            {
                if (tank.TurretRect == null)
                    return;

                var muzzlePos = tank.GetMuzzleRotatedRectangle ();
                var muzzleX = muzzlePos.CollisionRectangle.X;
                var muzzleY = muzzlePos.CollisionRectangle.Y;

                var muzzleViewportPos = gameData.Map.CalculateViewportCoordinates (new Vector2 (muzzleX, muzzleY),
                                                                                   gameData.MapScale);
                var rect = new Rectangle ((int)muzzleViewportPos.X,
                                          (int)muzzleViewportPos.Y,
                                          muzzlePos.CollisionRectangle.Width,
                                          muzzlePos.CollisionRectangle.Height);

                var blankTexture = gameData.ContentManager.Load<Texture2D> ("blank");

                gameData.SpriteBatch.Draw (blankTexture,
                                           destinationRectangle: rect,
                                           rotation: muzzlePos.Rotation,
                                           scale: gameData.MapScale);

                DrawRotatedRect (gameData, tank.TurretRect);
            }
        }

        public static Rectangle GetSpriteFromSpriteImage (
            Texture2D texture,
            int spriteNumber,
            int rows,
            int columns)
        {
            var width = texture.Width / columns;
            var height = texture.Height / rows;
            var row = (int)Math.Floor ((float)spriteNumber / (float)columns);
            var col = spriteNumber - (row * rows);

            if (col < 0)
                col = 0;

            return new Rectangle (col * width, row * height, width, height);
        }

        public Vector2 CoordinatesToVector2 (
            Coordinate coordinate)
        {
            return new Vector2 (coordinate.X * 64, coordinate.Y * 64);
        }

        public void Render ()
        {
            RenderTerrain (gameData.Map.MapSize);

            foreach (ObjectBase obj in gameData.Map.GetObjects())
            {
                obj.Render ();
                Renderer.DrawGuides (gameData, obj);
            }
        }
    }
}