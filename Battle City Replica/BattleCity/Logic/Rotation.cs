using System;

namespace BattleCity.Logic
{
    public class Rotation
    {
        public float Degrees { get; set; }

        public Rotation () : this (0)
        {
			
        }

        public Rotation (float degrees)
        {
            Degrees = degrees;
        }

        public float ToRadians()
        {
            return (float)((Math.PI / 180) * Degrees);
        }

        public static Rotation FromRadians(float rads)
        {
            return new Rotation ((float)(rads * 180 / Math.PI));
        }

        public Rotation OffsetBy(float degrees)
        {
            if (Degrees + degrees > 360)
                return new Rotation ((Degrees + degrees) - 360);

            if (Degrees + degrees < 0)
                return new Rotation (360 - (Degrees + degrees));		

            return new Rotation (Degrees + degrees);
        }

        public override string ToString()
        {
            return string.Format ("[Rotation: Degrees={0}, Radians={1}]", Degrees, ToRadians ());
        }
    }
}

