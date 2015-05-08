using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using BattleCity.Entities;
using BattleCity.Logic;
using BattleCity.StaticObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using BattleCity.Extensions;
using BattleCity.Attributes;
using BattleCity.ThirdParty;

namespace BattleCity
{
    /// <summary>
    /// Represents a collection of types and MonoGame textures.
    /// </summary>
    public class Renderer
    {
        GameData gameData;

        readonly Dictionary<Type, Texture2D> mappedTextures = new Dictionary<Type, Texture2D> ();
        Texture2D dirtTexture, blankTexture;

        public Dictionary<Type, Texture2D> MappedTextures
        {
            get
            {
                return mappedTextures;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Renderer"/> class.
        /// </summary>
        public Renderer (GameData gameData)
        {
            this.gameData = gameData;

            dirtTexture = gameData.ContentManager.Load<Texture2D> ("dirt");
            blankTexture = gameData.ContentManager.Load<Texture2D> ("blank");

            MappedTextures attr = new MappedTextures (new [] { "Dirt\\01" });
            dirtTexture = gameData.ContentManager.Load<Texture2D> (attr.GetRandomTexture ());

            LoadMappedTextures ();
        }

        /// <summary>
        /// Loads the mapped textures.
        /// </summary>
        void LoadMappedTextures()
        {
            var namespaces = new List<string> () { "BattleCity.Entities", "BattleCity.StaticObjects" };

            var query = from type in Assembly.GetExecutingAssembly ().GetTypes ()
                                 where (type.IsClass && namespaces.Contains (type.Namespace) && (type.GetCustomAttributes (typeof(MappedTextures)).FirstOrDefault () != null))
                                 select type;
            query.ToList ().ForEach (type =>
            { 
                var attr = (MappedTextures)type.GetCustomAttributes (typeof(MappedTextures)).First ();
                var textureName = attr.GetRandomTexture ();

                try
                {
                    var texture = gameData.ContentManager.Load<Texture2D> (textureName);
                    MappedTextures.Add (type, texture);
                }
                catch (ContentLoadException e)
                {
                    #if DEBUG
                    Debug.WriteLine ("Unable to map type <{0}> with \"{1}\". Texture load error:\n{2}".FormatWith (type.Name,
                                                                                                                   textureName,
                                                                                                                   e.Message),
                                     "MAPPED TEXTURE");                    
                    #endif

                    throw;
                }

                #if DEBUG
                Debug.WriteLine ("Mapped the type of <{0}> with \"{1}\".".FormatWith (type.Name,
                                                                                      textureName),
                                 "MAPPED TEXTURE");
                #endif
            });
        }

        /// <summary>
        /// Renders the terrain.
        /// </summary>
        /// <param name="mapSize">Map size.</param>
        public void RenderTerrain(Vector2 mapSize)
        {            
            int rows = (int)mapSize.X / dirtTexture.Width;
            int cols = (int)mapSize.Y / dirtTexture.Height;

            for (var row = 0; row <= rows; row++)
                for (var col = 0; col <= cols; col++)
                    gameData.SpriteBatch.Draw (dirtTexture,
                                               new Vector2 (row * dirtTexture.Width,
                                                            col * dirtTexture.Height));
        }

        /// <summary>
        /// Draws the guides.
        /// </summary>
        /// <param name="obj">Object.</param>
        public void DrawGuides(ObjectBase obj)
        {
//            if (!gameData.DebuggingSettings.ShowGuides)
//                return;

            var rotation = obj.Position.Rotation;
            var lowerLeft = obj.Position.LowerLeftCorner ();
            var lowerRight = obj.Position.LowerRightCorner ();
            var upperLeft = obj.Position.UpperLeftCorner ();
            var upperRight = obj.Position.UpperRightCorner ();

            gameData.SpriteBatch.Draw (blankTexture, lowerLeft, rotation: rotation);
            gameData.SpriteBatch.Draw (blankTexture, lowerRight, rotation: rotation);
            gameData.SpriteBatch.Draw (blankTexture, upperLeft, rotation: rotation);
            gameData.SpriteBatch.Draw (blankTexture, upperRight, rotation: rotation);

            var tank = obj as Tank;
            if (tank != null)
            {
                var muzzlePos = tank.GetMuzzleRotatedRectangle ();

                gameData.SpriteBatch.Draw (blankTexture,
                                           destinationRectangle: muzzlePos.CollisionRectangle,
                                           rotation: muzzlePos.Rotation);
            }
        }

        public void RenderEntities(Collection<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                var entityType = entity.GetType ();

                if (MappedTextures.ContainsKey (entityType))
                {					
                    var texture = MappedTextures [entityType];

                    gameData.SpriteBatch.Draw (
                        texture,
                        position: entity.Position.CollisionRectangle.Center.ToVector2 (),
					    //destinationRectangle: new Rectangle (entity.RotatedPos.X, entity.RotatedPos.Y, entity.RotatedPos.Width, entity.RotatedPos.Height),
                        origin: new Vector2 (texture.Width / 2, texture.Height / 2),					    
                        rotation: entity.Position.Rotation
                    );

                    DrawGuides (entity);
                }
                else
                {
                    #if DEBUG
                    Debug.WriteLine ("There is no mapped texture for type <{0}>.".FormatWith (entityType.Name),
                                     "RENDERER");
                    #endif
                }
            }
        }

        public void RenderStaticObjects(Collection<StaticObject> staticObjects)
        {
            foreach (StaticObject staticObject in staticObjects)
            {
                var texture = MappedTextures [staticObject.GetType ()];

                if (staticObject.GetType () == typeof(Explosion))
                    RenderExplosion ((Explosion)staticObject);
                else
                    gameData.SpriteBatch.Draw (texture, staticObject.Position.UpperLeftCorner ());

                DrawGuides (staticObject);
            }
        }

        public Rectangle GetSpriteFromSpriteImage(Texture2D texture,
                                                  int num,
                                                  int rows,
                                                  int columns)
        {
            var width = texture.Width / columns;
            var height = texture.Height / rows;
            var row = (int)Math.Floor ((float)num / (float)columns);
            var col = num - (row * rows);
            return new Rectangle (col * width, row * height, width, height);
        }

        public void RenderExplosion(Explosion explosion)
        {
            var texture = MappedTextures [explosion.GetType ()];

            gameData.SpriteBatch.Draw (
                texture,
                origin: new Vector2 (0, 0),
                destinationRectangle: explosion.Position.CollisionRectangle,
                sourceRectangle: GetSpriteFromSpriteImage (texture, explosion.CurrentState, 5, 5)
            );
        }

        public Vector2 CoordinatesToVector2(Coordinate coordinate)
        {
            return new Vector2 (coordinate.X * 64, coordinate.Y * 64);
        }
    }
}