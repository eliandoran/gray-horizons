/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/
using GrayHorizons.Attributes;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;
using GrayHorizons.StaticObjects;
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrayHorizons.Entities.Cars
{
    [MappedTextures(@"Vehicles\Pickup")]
    public class MinelayerPickup: Vehicle
    {
        bool firstTime = true;
        Texture2D crosshairTexture;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.MineLayerPickup"/> class.
        /// </summary>
        public MinelayerPickup()
        {
            DefaultSize = new Point(160, 66);
            Acceleration = 0.025f;
            AxisPosition = new Point(36, 30);
            CanMoveOnSpot = true;
            CanBeRunOverByTank = true;
            Speed = 6;
            Health = 10;
        }

        public override void RenderHud()
        {
            if (firstTime)
            {
                crosshairTexture = GameData.ContentManager.Load<Texture2D>("MinelayerCrosshair");
                spriteBatch = GameData.ScreenManager.SpriteBatch;
                firstTime = false;
            }

            if (GameData.ActivePlayer.AssignedEntity != this)
                return;

            var rect = GetRect();
            spriteBatch.Draw(
                crosshairTexture,
                GameData.Map.CalculateViewportCoordinates(rect.UpperLeftCorner(), GameData.MapScale),
                rotation: rect.Rotation);
        }

        public override void Shoot()
        {
            GameData.Map.QueueAddition(new AntitankBarrier
                {
                    Position = GetRect(),
                    HasCollision = false
                });
        }

        RotatedRectangle GetRect()
        {
            if (crosshairTexture.IsNull())
                return null;

            var xOffset = new Point(-crosshairTexture.Width - 20, 0);
            var yOffset = new Point(0, (Position.CollisionRectangle.Height / 2) - (crosshairTexture.Height / 2));
            return Position.Offset(xOffset).Offset(yOffset);
        }
    }
}

