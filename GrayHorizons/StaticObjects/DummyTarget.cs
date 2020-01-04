using System;
using GrayHorizons.Logic;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework;

namespace GrayHorizons.StaticObjects
{
    [MappedTextures(@"DummyTarget")]
    public class DummyTarget: StaticObject
    {
        readonly Random random = new Random();

        public DummyTarget()
        {
            HasCollision = true;
            DefaultSize = new Point(75, 33);
            CustomTextureCrop = new Rectangle(0, 0, DefaultSize.X, DefaultSize.Y);
            Health = 4;
            MiniMapColor = Color.Red;
        }

        public override void Destroy()
        {
            GameData.Map.QueueAddition(
                new Wreck(GameData, this)
                {
                    Position = Position,
                    Orientation = MathHelper.ToRadians(random.Next(0, 360))
                });
            
            base.Destroy();
        }
    }
}

