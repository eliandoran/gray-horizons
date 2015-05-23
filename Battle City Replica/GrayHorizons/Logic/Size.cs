namespace GrayHorizons.Logic
{
    using System.Xml.Serialization;
    using Microsoft.Xna.Framework;

    public class Size
    {
        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        public Size(int width,
                    int height)
        {
            Width = width;
            Height = height;
        }

        public Size()
            : this(0, 0)
        {

        }

        public Point ToPoint()
        {
            return new Point(Width, Height);
        }
    }
}

