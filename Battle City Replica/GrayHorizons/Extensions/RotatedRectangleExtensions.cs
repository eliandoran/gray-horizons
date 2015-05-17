using System;
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Extensions
{
    public static class RotatedRectangleExtensions
    {
        public static RotatedRectangle Offset(
            this RotatedRectangle rect,
            Point offsetBy)
        {
            var newRect = new RotatedRectangle(rect.CollisionRectangle, rect.Rotation);
            var rads = rect.Rotation;

            var deltaX = new Point(
                             (int)(Math.Cos(rads) * offsetBy.X),
                             (int)(Math.Sin(rads) * offsetBy.X));
            var deltaY = new Point(
                             (int)(Math.Sin(rads) * -offsetBy.Y),
                             (int)(Math.Cos(rads) * offsetBy.Y)
                         );

            newRect.CollisionRectangle.Offset(deltaX);
            newRect.CollisionRectangle.Offset(deltaY);
            return newRect;
        }
    }
}

