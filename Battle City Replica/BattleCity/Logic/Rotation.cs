using System;
using System.Diagnostics;

namespace BattleCity.Logic
{
    public class Rotation
    {
        public float degrees;

        public float Degrees { get; set; }

        public Rotation () : this (
                0)
        {
			
        }

        public Rotation (
            float degrees)
        {
            Degrees = degrees;
        }

        public float ToRadians ()
        {
            return (float)((Math.PI / 180) * Degrees);
        }

        public static Rotation FromRadians (
            float rads)
        {
            return new Rotation ((float)((180 / Math.PI) * rads));
        }

        public Rotation OffsetBy (
            float degrees)
        {
            var orientation = Degrees + degrees;
            orientation %= 360;

            return new Rotation (orientation);
        }

        public override string ToString ()
        {
            return string.Format ("[Rotation: Degrees={0}, Radians={1}]", Degrees, ToRadians ());
        }
    }
}

