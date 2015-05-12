using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using BattleCity.Attributes;
using BattleCity.Entities;
using BattleCity.Extensions;
using BattleCity.Logic;
using BattleCity.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleCity
{
    /// <summary>
    /// Represents a collection of types and MonoGame textures.
    /// </summary>
    public class Renderer
    {
        readonly GameData gameData;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Renderer"/> class.
        /// </summary>
        public Renderer (
            GameData gameData)
        {
            this.gameData = gameData;

            LoadMappedTextures ();
        }

        /// <summary>
        /// Loads the mapped textures.
        /// </summary>
        void LoadMappedTextures (
            bool fullReload = false)
        {
            var namespaces = new List<string> () { "BattleCity.Entities", "BattleCity.StaticObjects" };

            var query = from type in Assembly.GetExecutingAssembly ().GetTypes ()
                                 where (type.IsClass && namespaces.Contains (type.Namespace) && (type.GetCustomAttributes (
                                         typeof(MappedTexturesAttribute),
                                         true).FirstOrDefault () != null))
                                 select type;

            #if DEBUG
            Debug.WriteLine ("MAPPED TEXTURES:");
            Debug.Indent ();
            #endif

            query.ToList ().ForEach (
                    type =>
                { 
                    if (!fullReload && gameData.MappedTextures.ContainsKey (type))
                        return;

                    var attr = (MappedTexturesAttribute)type.GetCustomAttributes (
                                   typeof(MappedTexturesAttribute),
                                   true).First ();
                    var textureName = attr.GetRandomTexture ();

                    try
                    {
                        var texture = gameData.ContentManager.Load<Texture2D> (textureName);
                        gameData.MappedTextures.Add (type, texture);
                    }
                    catch (ContentLoadException e)
                    {
                        #if DEBUG
                        Debug.WriteLine ("Unable to map type <{0}> with \"{1}\". Texture load error:\n{2}".FormatWith (
                            type.Name,
                            textureName,
                            e.Message));                    
                        #endif

                        throw;
                    }

                    #if DEBUG
                    Debug.WriteLine ("Mapped the type of <{0}> with \"{1}\".".FormatWith (type.Name,
                        textureName));
                    #endif
                });

            #if DEBUG
            Debug.Unindent ();
            #endif
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
                var pos = gameData.Map.CalculateViewportCoordinates (Vector2.Zero, gameData.Scale);
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
                                  ), gameData.Scale);

                        gameData.SpriteBatch.Draw (texture, pos, scale: gameData.Scale);
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
                gameData.Map.CalculateViewportCoordinates (lowerLeft, gameData.Scale),
                rotation: rotation,
                scale: gameData.Scale);
            gameData.SpriteBatch.Draw (blankTexture,
                gameData.Map.CalculateViewportCoordinates (lowerRight, gameData.Scale),
                rotation: rotation,
                scale: gameData.Scale);
            gameData.SpriteBatch.Draw (blankTexture,
                gameData.Map.CalculateViewportCoordinates (upperLeft, gameData.Scale),
                rotation: rotation,
                scale: gameData.Scale);
            gameData.SpriteBatch.Draw (blankTexture,
                gameData.Map.CalculateViewportCoordinates (upperRight, gameData.Scale),
                rotation: rotation,
                scale: gameData.Scale);
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
                                            gameData.Scale);
                var rect = new Rectangle ((int)muzzleViewportPos.X,
                               (int)muzzleViewportPos.Y,
                               muzzlePos.CollisionRectangle.Width,
                               muzzlePos.CollisionRectangle.Height);

                var blankTexture = gameData.ContentManager.Load<Texture2D> ("blank");

                gameData.SpriteBatch.Draw (blankTexture,
                    destinationRectangle: rect,
                    rotation: muzzlePos.Rotation,
                    scale: gameData.Scale);

                DrawRotatedRect (gameData, tank.TurretRect);
            }
        }

        public static Rectangle GetSpriteFromSpriteImage (
            Texture2D texture,
            int num,
            int rows,
            int columns)
        {
            var width = texture.Width / columns;
            var height = texture.Height / rows;
            var row = (int)Math.Floor ((float)num / (float)columns);
            var col = num - (row * rows);

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