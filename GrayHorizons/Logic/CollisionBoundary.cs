namespace GrayHorizons.Logic
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using GrayHorizons.Extensions;
    using GrayHorizons.ThirdParty;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a rectangle area on a <see cref="GrayHorizons.Logic.Map"/> in which every <see cref="GrayHorizons.Logic.Entity"/> collides with it.
    /// </summary>
    public class CollisionBoundary
    {
        static readonly XmlRootAttribute ListSerializationRoot = new XmlRootAttribute("CollisionBoundaries");

        /// <summary>
        /// Gets or sets the X position of the top-left corner of the collision rectangle.
        /// </summary>
        /// <value>The X position.</value>
        [XmlAttribute("x")]
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the Y position of the top-left corner of the collision rectangle.
        /// </summary>
        /// <value>The Y position.</value>
        [XmlAttribute("y")]
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the width of the collision rectangle.
        /// </summary>
        /// <value>The width.</value>
        [XmlAttribute("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the collision rectangle.
        /// </summary>
        /// <value>The height.</value>
        [XmlAttribute("height")]
        public int Height { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.CollisionBoundary"/> class with specified dimensions.
        /// </summary>
        /// <param name="x">The X coordinate of the collision rectangle's top-left corner.</param>
        /// <param name="y">The Y coordinate of the collision rectangle's top-left corner..</param>
        /// <param name="width">The width of the collision rectangle.</param>
        /// <param name="height">The height of the collision rectangle.</param>
        public CollisionBoundary(int x, int y, int width, int height)
        {
            Left = x;
            Top = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.CollisionBoundary"/> class.
        /// </summary>
        public CollisionBoundary()
        {
        }

        /// <summary> 
        /// Returns a <see cref="GrayHorizons.Logic.CollisionBoundary"/> with Left=0, Top=0, Width=0, Height=0.
        /// </summary>
        /// <value>The empty <see cref="GrayHorizons.Logic.CollisionBoundary"/>.</value>
        public static CollisionBoundary Empty
        {
            get
            {
                return new CollisionBoundary(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Returns a <see cref="Microsoft.Xna.Framework.Rectangle"/> that represents the current collision boundary.
        /// </summary>
        /// <returns>The rectangle.</returns>
        public Rectangle ToRectangle()
        {            
            return new Rectangle(Left, Top, Width, Height);
        }

        /// <summary>
        /// Returns a <see cref="GrayHorizons.ThirdParty.RotatedRectangle"/> that represents the current collision boundary.
        /// </summary>
        /// <returns>The rotated rectangle.</returns>
        /// <param name="rotation">The initial orientation (in radians) of the resulted <see cref="GrayHorizons.ThirdParty.RotatedRectangle"/>.</param>
        public RotatedRectangle ToRotatedRectangle(float rotation)
        {
            return new RotatedRectangle(ToRectangle(), rotation);
        }

        /// <summary>
        /// Returns a <see cref="GrayHorizons.ThirdParty.RotatedRectangle"/> that represents the current collision boundary with the orientation of 0 radians.
        /// </summary>
        /// <returns>The rotated rectangle.</returns>
        public RotatedRectangle ToRotatedRectangle()
        {
            return ToRotatedRectangle(0f);
        }

        /// <summary>
        /// Returns a <see cref="Microsoft.Xna.Framework.Vector2"/> that represents the position of the upper-left corner of the collision rectangle.
        /// </summary>
        /// <returns>The position of the upper-left corner of the collision rectangle.</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(Left, Top);
        }

        /// <summary>
        /// Returns a <see cref="GrayHorizons.Logic.Size"/> that represents the size of the collision rectangle. 
        /// </summary>
        /// <returns>The size of the collision rectangle.</returns>
        public Size ToSize()
        {
            return new Size(Width, Height);
        }

        #region Static Methods

        public static void SerializeList(List<CollisionBoundary> source, IInputOutputAgent ioAgent, string path)
        {
            using (Stream stream = ioAgent.GetStream(path, FileMode.Create))
            {
                source.SerializeInto(stream, ListSerializationRoot);
            }
        }

        public static List<CollisionBoundary> DeserializeList(IInputOutputAgent ioAgent, string path)
        {
            using (Stream stream = ioAgent.GetStream(path, FileMode.Open))
            {
                return stream.DeserializeTo<List<CollisionBoundary>>(ListSerializationRoot);
            }
        }

        #endregion
    }
}

