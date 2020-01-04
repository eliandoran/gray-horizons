namespace GrayHorizons.Extensions
{
    using System;
    using GrayHorizons.ThirdParty;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a set of <see cref="GrayHorizons.ThirdParty.RotatedRectangle"/> extensions.
    /// </summary>
    public static class RotatedRectangleExtensions
    {
        /// <summary>
        /// Offsets this <see cref="GrayHorizons.ThirdParty.RotatedRectangle"/> by the specified coordinates.
        /// </summary>
        /// <param name="rect">The <see cref="GrayHorizons.ThirdParty.RotatedRectangle"/> to be offset.</param>
        /// <param name="offsetBy">The coordinates the <see cref="GrayHorizons.ThirdParty.RotatedRectangle"/> will be offset by.</param>
        public static RotatedRectangle Offset(
            this RotatedRectangle rect,
            Point offsetBy)
        {
            var newRect = new RotatedRectangle(rect.CollisionRectangle, rect.Rotation);

            newRect.CollisionRectangle.Offset(new Point(
                    (int)(Math.Cos(rect.Rotation) * offsetBy.X),
                    (int)(Math.Sin(rect.Rotation) * offsetBy.X)));
            
            newRect.CollisionRectangle.Offset(new Point(
                    (int)(Math.Sin(rect.Rotation) * -offsetBy.Y),
                    (int)(Math.Cos(rect.Rotation) * offsetBy.Y)
                ));

            return newRect;
        }
    }
}

