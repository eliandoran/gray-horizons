using GrayHorizons.Logic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System;

namespace GrayHorizons.AI
{
    /// <summary>
    /// Represents a computer artificial intelligence controlling the tanks that is similar to the original game, Battle City.
    /// </summary>
    public class VanillaAI: AIBase
    {
        int MaximumVisibilityRange { get; set; }

        public VanillaAI()
        {
            MaximumVisibilityRange = 300;
        }

        /// <summary>
        /// It should be called when the game requires the AI to make the next step.
        /// </summary>
        public override void NextStep()
        {	
            //ControllingTank.IsMoving = true;
            //ControllingTank.Move(Entity.MoveDirection.Backward, false);

            //ControllingTank.TurretRotation = ControllingTank.TurretRotation.OffsetBy(5);
            //ControllingTank.Shoot (new Projectile ());

            GetNearestThreat();
        }

        Entity GetNearestThreat()
        {
            float? minimumDist = null;
            Entity nearestEntity = null;

            foreach (Entity entity in GameData.Map.Entities)
            {
                var dist = GameData.Map.CalculateDistance(ControllingEntity, entity);

                if (CheckInterest(entity) && (!minimumDist.HasValue || minimumDist > dist))
                {
                    nearestEntity = entity;
                    minimumDist = dist;
                }
            }

            if (nearestEntity == null)
                return null;

            if (minimumDist <= MaximumVisibilityRange)
                RotateTowards(nearestEntity.Position.CollisionRectangle.Center.ToVector2());

            //Debug.WriteLine(nearestEntity + "/" + minimumDist);

            return null;
        }

        bool CheckInterest(ObjectBase obj)
        {
            return obj != this.ControllingEntity && obj is Tank;
        }


        void RotateTowards(Vector2 position)
        {
            var tank = ControllingEntity as Tank;

            if (tank != null)
            {
                var a = ControllingEntity.Position.CollisionRectangle.Center.ToVector2();
                var b = position;
                var pos = a - b;
                var currentRotation = tank.TurretRotation.ToRadians();
                var rotation = Rotation.FromRadians((float)Math.Atan2(pos.Y, pos.X)).OffsetBy(180);
                var delta = currentRotation - rotation.ToRadians();
                var sign = (delta > 1) ? -1 : 1;
                var step = new Rotation(70).ToRadians();
                Debug.WriteLine("Current: {0}, Rotation: {1}, Delta: {2}", currentRotation, rotation, delta);

                tank.TurretRect = new GrayHorizons.ThirdParty.RotatedRectangle(
                    tank.Position.CollisionRectangle,
                    tank.Position.Rotation);

                if (Math.Abs(delta) < 0.16f)
                {
                    tank.Shoot();
                    return;
                }

                if (Math.Abs(delta) < step)
                {
                    step = new Rotation(20).ToRadians();
                }
                tank.TurretRotation = tank.TurretRotation.OffsetBy(sign * step);
                //tank.TurretRotation = rotation.OffsetBy(180);
            }
        }
    }
}

