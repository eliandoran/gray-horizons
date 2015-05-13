using System.Xml.Serialization;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Logic
{
    /// <summary>
    /// Represents an in-game coordinate that is relative to a map.
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Gets or sets the horizontal value (X) of this coordinate.
        /// </summary>
        /// <value>The X value.</value>
        [XmlAttribute ("x"), DefaultValue (0)]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the vertical value (Y) of this coordinate..
        /// </summary>
        /// <value>The Y value.</value>
        [XmlAttribute ("y"), DefaultValue (0)]
        public int Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.Coordinate"/> class with the specified X and Y values.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Coordinate (int x,
                           int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.Coordinate"/> class with X and Y zero.
        /// </summary>
        public Coordinate () : this (0,
                                     0)
        {

        }

        /// <summary>
        /// Determines whether the specified coordinate is equal to the current coordinate.
        /// </summary>
        /// <returns>true if the specified coordinate is equal to the current coordinate; otherwise, false.</returns>
        /// <filterpriority>2</filterpriority>
        /// <param name="compareWith">The <see cref="GrayHorizons.Logic.Coordinate"/> to compare with the current <see cref="GrayHorizons.Logic.Coordinate"/>.</param>
        public bool Equals(Coordinate compareWith)
        {
            return X == compareWith.X && Y == compareWith.Y;
        }

        /// <summary>
        /// Adds two coordinates together.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        public static Coordinate Combine(Coordinate first,
                                         Coordinate second)
        {
            return new Coordinate (
                first.X + second.X,
                first.Y + second.Y);
        }

        /// <summary>
        /// Multiplies two coordinates.
        /// </summary>
        /// <param name="multiplicand">The coordinate that is being multiplied.</param>
        /// <param name="multiplier">The coordinate that it is multiplied by.</param>
        public static Coordinate Multiply(Coordinate multiplicand,
                                          Coordinate multiplier)
        {
            return new Coordinate (
                multiplicand.X * multiplier.X,
                multiplicand.Y * multiplier.Y);
        }

        /// <summary>
        /// Converts the coordinate to a XNA Framework 2D Vector.
        /// </summary>
        /// <returns>The converted coordinate.</returns>
        public Vector2 ToVector2D()
        {
            return new Vector2 (X, Y);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format ("[Coordinate: X={0}, Y={1}]", X, Y);
        }
    }
}

