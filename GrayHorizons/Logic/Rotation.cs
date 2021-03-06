﻿namespace GrayHorizons.Logic
{
    using System;
    using GrayHorizons.Extensions;

    public class Rotation
    {
        public float Degrees { get; set; }

        public Rotation()
            : this(
                0)
        {
			
        }

        public Rotation(
            float degrees)
        {
            Degrees = degrees;
        }

        public float ToRadians()
        {
            return (float)((Math.PI / 180) * Degrees);
        }

        public static Rotation FromRadians(
            float radians)
        {
            return new Rotation((float)((180 / Math.PI) * radians));
        }

        public Rotation OffsetBy(
            float degrees)
        {
            return new Rotation((Degrees + degrees) % 360);
        }

        public override string ToString()
        {
            return "[Rotation: Degrees={0}, Radians={1}]".FormatWith(Degrees, ToRadians());
        }
    }
}

