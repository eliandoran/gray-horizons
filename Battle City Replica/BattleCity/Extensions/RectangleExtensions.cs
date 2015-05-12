using System;
using Microsoft.Xna.Framework;
using BattleCity.ThirdParty;

namespace BattleCity.Extensions
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
        public static Rectangle Scale (
            this Rectangle rect,
            float scale)
        {
            return new Rectangle (
                (int)(scale * rect.X),
                (int)(scale * rect.Y),
                (int)(scale * rect.Width),
                (int)(scale * rect.Height)
            );
        }
    }
}

