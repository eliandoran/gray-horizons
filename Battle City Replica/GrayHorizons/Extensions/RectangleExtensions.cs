/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/
using Microsoft.Xna.Framework;
using GrayHorizons.ThirdParty;

namespace GrayHorizons.Extensions
{
    /// <summary>
    /// Represents a set of <see cref="Microsoft.Xna.Framework.Rectangle"/> extensions.
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// Scale the specified <see cref="Microsoft.Xna.Framework.Rectangle"/> at the specified scale.
        /// </summary>
        /// <param name="rect">The <see cref="Microsoft.Xna.Framework.Rectangle"/> to be scaled.</param>
        /// <param name="scale">The scale factor of the <see cref="Microsoft.Xna.Framework.Rectangle"/> (1 is 100%).</param>
        public static Rectangle ScaleTo(
            this Rectangle rect,
            Vector2 scale)
        {
            return new Rectangle(
                (int)(scale.X * rect.X),
                (int)(scale.Y * rect.Y),
                (int)(scale.X * rect.Width),
                (int)(scale.Y * rect.Height)
            );
        }

        public static Rectangle OffsetBy(
            this Rectangle rect,
            Point offset)
        {
            return new Rectangle(
                rect.X + offset.X,
                rect.Y + offset.Y,
                rect.Width,
                rect.Height
            );
        }

        public static RotatedRectangle ToRotatedRectangle(
            this Rectangle rect,
            float rotation)
        {
            return new RotatedRectangle(rect, rotation);
        }

        public static RotatedRectangle ToRotatedRectangle(
            this Rectangle rect)
        {
            return rect.ToRotatedRectangle(0f);
        }
    }
}

