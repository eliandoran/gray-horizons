using System;
using GrayHorizons.Attributes;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Entities.Cars
{
    [MappedTextures(@"Vehicles\Car")]
    public class OrangeCar: Vehicle
    {
        public OrangeCar()
        {
            DefaultSize = new Point(154, 66);
            Acceleration = 0.005f;
            AxisPosition = new Point(36, 30);
            CanMoveOnSpot = true;
            CanBeRunOverByTank = true;
            Speed = 40;
        }
    }
}

