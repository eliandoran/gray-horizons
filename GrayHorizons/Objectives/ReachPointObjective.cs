using System;
using Microsoft.Xna.Framework;
using GrayHorizons.StaticObjects;
using GrayHorizons.Logic;
using System.Diagnostics;

namespace GrayHorizons.Objectives
{
    public class ReachPointObjective: Objective
    {
        readonly Point pointToReach;
        readonly int size;
        Waypoint waypoint;

        public Point PointToReach { get { return pointToReach; } }

        public int Size { get { return size; } }

        public ReachPointObjective(Point pointToReach)
        {
            this.pointToReach = pointToReach;
            this.size = 50;
        }

        public ReachPointObjective(Point pointToReach, int size)
        {
            this.pointToReach = pointToReach;
            this.size = size;
        }

        public override void Startup()
        {
            waypoint = new Waypoint();

            waypoint.Position = new GrayHorizons.ThirdParty.RotatedRectangle(
                new Rectangle(
                    PointToReach.X - (size / 2),
                    PointToReach.Y - (size / 2),
                    size,
                    size
                ), 0
            );

            GameData.Map.Add(waypoint);
            base.Startup();
        }

        public override void CheckCompletion()
        {
            IsCompleted = waypoint.Position.Intersects(GameData.ActivePlayer.AssignedEntity.Position);
            if (IsCompleted)
                End(true);
        }

        public override void End(bool won)
        {
            waypoint.ShrinkAway();
        }
    }
}