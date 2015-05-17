using System;
using GrayHorizons.Logic;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework;

namespace GrayHorizons.StaticObjects
{
    [MappedTextures ("Explosion")]
    public class Explosion: StaticObject
    {
        int currentState;

        public int CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                if (value > MaximumState)
                    Destroy ();
                else
                    currentState = value;
            }
        }

        public int MaximumState { get; set; }

        public Explosion ()
        {
            MaximumState = 25;
            CurrentState = -1;
            HasCollision = false;
            IsInvincible = true;
            MinimapColor = null;
        }

        public override void Update (
            TimeSpan gameTime)
        {
            CurrentState += 1;
        }

        public override void Render ()
        {
            var texture = GameData.MappedTextures [GetType ()];
            var position = GameData.Map.CalculateViewportCoordinates (Position.UpperLeftCorner (), GameData.MapScale);

            GameData.ScreenManager.SpriteBatch.Draw (
                texture,
                origin: new Vector2 (0, 0),
                destinationRectangle: new Rectangle ((int)position.X,
                                                     (int)position.Y,
                                                     Position.CollisionRectangle.Width,
                                                     Position.CollisionRectangle.Height),
                sourceRectangle: Renderer.GetSpriteFromSpriteImage (texture, CurrentState, 5, 5),
                scale: GameData.MapScale
            );
        }
    }
}

