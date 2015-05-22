using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using GrayHorizons.ThirdParty;

namespace GrayHorizons.Logic
{
    public class CollisionBoundary
    {
        const string ListSerializationRoot = "CollisionBoundaries";

        [XmlAttribute("x")]
        public int X { get; set; }

        [XmlAttribute("y")]
        public int Y { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        public CollisionBoundary(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public CollisionBoundary()
        {

        }

        public static CollisionBoundary Empty
        {
            get
            {
                return new CollisionBoundary(0, 0, 0, 0);
            }
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }

        public RotatedRectangle ToRotatedRectangle(float rotation)
        {
            return new RotatedRectangle(ToRectangle(), rotation);
        }

        public RotatedRectangle ToRotatedRectangle()
        {
            return ToRotatedRectangle(0f);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public Size ToSize()
        {
            return new Size(Width, Height);
        }

        #region Static Methods

        public static void SerializeList(List<CollisionBoundary> source, InputOutputAgent ioAgent, string path)
        {
            using (Stream stream = ioAgent.GetStream(path, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            {
                var serializer = new XmlSerializer(
                                     source.GetType(),
                                     new XmlRootAttribute(ListSerializationRoot));
                serializer.Serialize(writer, source);
            }
        }

        public static List<CollisionBoundary> DeserializeList(InputOutputAgent ioAgent, string path)
        {
            using (Stream stream = ioAgent.GetStream(path, FileMode.Open))
            {
                var serializer = new XmlSerializer(
                                     typeof(List<CollisionBoundary>),
                                     new XmlRootAttribute(ListSerializationRoot));
                return (List<CollisionBoundary>)serializer.Deserialize(stream);
            }
        }

        #endregion
    }
}

