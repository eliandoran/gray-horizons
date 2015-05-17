using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework;
using GrayHorizons.ThirdParty;
using GrayHorizons.AI;
using GrayHorizons.Logic;

namespace GrayHorizons.Maps
{
    [MappedTextures(@"Maps\Desert")]
    public class DesertMap: Map
    {
        public DesertMap(GameData gameData)
            : base(new Vector2(4000, 4000), gameData)
        {
            Texture = gameData.MappedTextures[GetType()];

            var tank = new Entities.Tanks.TankE100();
            tank.Position = new RotatedRectangle(
                new Rectangle(
                    16 * 64, 16 * 64,
                    tank.DefaultSize.X,
                    tank.DefaultSize.Y
                ), 90
            );
            tank.AI = new VanillaAI();
            tank.AI.GameData = gameData;

            Add(tank);
        }
    }
}

