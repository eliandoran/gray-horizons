using System;
using System.Diagnostics;
using GrayHorizons.Attributes;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace GrayHorizons.Entities
{
    [MappedTextures ("Soldier")]
    public class Soldier: ControllableEntity
    {
        const int moveStatesCount = 7;
        int currentMoveState = 0;
        TimeSpan moveStateTime, currentMoveStateTime;
        TimeSpan currentIsMovingTime, isMovingTime;
        bool isMoving, firstTimeMoving;
        SoundEffectInstance footstepsSound;
        RotatedRectangle usageRect;

        public Soldier ()
        {
            Health = 1;
            AccelerationFactor = 0.03f;
            Acceleration = 0.15f;
            DefaultSize = new Point (75, 50);
            moveStateTime = TimeSpan.FromMilliseconds (75);
            isMovingTime = TimeSpan.FromMilliseconds (200);
            HasCollision = true;
        }

        public override bool Turn (
            TurnDirection turnDirection,
            MoveDirection? moveDirection,
            bool onSpot = false,
            bool noClip = false)
        {
            return base.Turn (turnDirection, moveDirection, true, noClip);
        }

        public override bool Move (
            MoveDirection direction,
            int moveStep,
            bool noClip)
        {
            isMoving = true;
            currentIsMovingTime = isMovingTime;
            return base.Move (direction, moveStep, noClip);
        }

        public override void Render ()
        {
            var texture = GameData.MappedTextures [GetType ()];
            var position = GameData.Map.CalculateViewportCoordinates (Position.CollisionRectangle.Center.ToVector2 (),
                                                                      GameData.MapScale);

            int x = 0, y = currentMoveState * 90;

            var rect = new Rectangle (x, y, 90, 90);

            GameData.SpriteBatch.Draw (
                texture,
                sourceRectangle: rect,
                position: position,
                origin: new Vector2 (29, 22),
                rotation: Position.Rotation,
                scale: GameData.MapScale
            );

            position = GameData.Map.CalculateViewportCoordinates (
                usageRect.CollisionRectangle.Location.ToVector2 (),
                GameData.MapScale);

            if (GameData.DebuggingSettings.ShowGuides)
            {
                var blankTexture = GameData.ContentManager.Load<Texture2D> ("blank");
                GameData.SpriteBatch.Draw (
                    blankTexture,
                    destinationRectangle: new Rectangle (
                        (int)position.X,
                        (int)position.Y,
                        usageRect.CollisionRectangle.Width,
                        usageRect.CollisionRectangle.Height),
                    rotation: usageRect.Rotation
                );
            }
        }

        public override void Update (
            TimeSpan gameTime)
        {
            usageRect = new RotatedRectangle (
                new Rectangle (
                    (int)Position.UpperRightCorner ().X,
                    (int)Position.UpperRightCorner ().Y,
                    25,
                    Position.CollisionRectangle.Height),
                Position.Rotation);

            var rads = Position.Rotation;
            var x = 15;
            var y = 0;
            var deltaX = new Point (
                             (int)(Math.Cos (rads) * x),
                             (int)(Math.Sin (rads) * x));
            var deltaY = new Point (
                             (int)(Math.Sin (rads) * -y),
                             (int)(Math.Cos (rads) * y)
                         );

            var rect = Position.CollisionRectangle;
            rect.Offset (deltaX);
            rect.Offset (deltaY);

            if (currentIsMovingTime > gameTime)
                currentIsMovingTime -= gameTime;
            else
            {
                isMoving = false;
                firstTimeMoving = true;
            }

            if (isMoving)
            {
                if (firstTimeMoving)
                {
                    footstepsSound = Sound.SoldierSounds.Footsteps.PlayLooped ();
                    firstTimeMoving = false;
                }

                if (currentMoveStateTime > gameTime)
                {
                    currentMoveStateTime -= gameTime;
                }
                else
                {
                    if (currentMoveState < moveStatesCount - 1)
                    {
                        currentMoveState++;
                    }
                    else
                        currentMoveState = 1;

                    currentMoveStateTime = moveStateTime;
                }
            }
            else
            {
                currentMoveState = 0;

                if (footstepsSound != null)
                    footstepsSound.Stop ();
            }                

            base.Update (gameTime);
        }

        public override void Use ()
        {
            foreach (ObjectBase obj in GameData.Map.SearchMapObjects (usageRect))
            {
                if (obj == this)
                    continue;

                var vehicle = obj as Vehicle;
                if (vehicle != null && vehicle.CanBeBoarded)
                {
                    #if DEBUG
                    Debug.WriteLine ("Identified vehicle of type <{0}>.".FormatWith (vehicle.GetType ()));
                    #endif

                    isMoving = false;
                    vehicle.BoardPassenger (this);

                    if (GameData.ActivePlayer.AssignedEntity == this)
                    {
                        GameData.Map.CenterViewportAt (vehicle);

                        vehicle.Moved += (
                            sender,
                            e) => GameData.Map.CenterViewportAt (vehicle);
                        GameData.ActivePlayer.AssignedEntity = vehicle;

                        #if DEBUG
                        Debug.WriteLine (
                            "Player's assigned entity was changed to <{0}>.".FormatWith (vehicle.GetType ()),
                            "USE");
                        #endif
                    }

                    return;
                }
            }

            #if DEBUG
            Debug.WriteLine ("No suitable objects found.", "USE");
            #endif
        }
    }
}

