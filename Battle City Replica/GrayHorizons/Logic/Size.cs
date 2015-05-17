using System;
using System.Xml.Serialization;

namespace GrayHorizons.Logic
{
    public class Size
    {
        [XmlAttribute ("width")]
        public int Width { get; set; }

        [XmlAttribute ("height")]
        public int Height { get; set; }

        public Size (int width,
                     int height)
        {
            Width = width;
            Height = height;
        }

        public Size () : this (0,
                               0)
        {

        }
    }
}

