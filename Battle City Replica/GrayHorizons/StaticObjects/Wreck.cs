using System;
using GrayHorizons.Entities;
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;

namespace GrayHorizons.StaticObjects
{
    public class Wreck: StaticObject
    {
        public Wreck(GameData gameData, ObjectBase vehicle)
        {
            GameData = gameData;
            var texture = GameData.MappedTextures[vehicle.GetType()];
            Position = new RotatedRectangle(vehicle.Position.CollisionRectangle, vehicle.Position.Rotation);
            CustomTexture = texture;
            HasCollision = true;
            IsInvincible = false;

            if (vehicle is Tank)
                CustomTextureCrop = Renderer.GetSpriteFromSpriteImage(texture, 1, 2, 1);
            else
                CustomTextureCrop = Renderer.GetSpriteFromSpriteImage(texture, 1, 2, 1);

            MiniMapColor = Color.DarkGray;
        }
    }
}

