using GrayHorizons.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrayHorizons.Logic;
using GrayHorizons.Extensions;
using System.Diagnostics;
using GrayHorizons.ThirdParty;
using GrayHorizons.StaticObjects;

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
        }

        public override void RenderHUD()
        {
            if (firstTime)
            {
                crosshairTexture = GameData.ContentManager.Load<Texture2D>("MinelayerCrosshair");
                spriteBatch = GameData.ScreenManager.SpriteBatch;
                firstTime = false;
            }

            if (GameData.ActivePlayer.AssignedEntity != this)
                return;

            spriteBatch.Begin();

            var rect = GetRect();
            spriteBatch.Draw(crosshairTexture,
                position: GameData.Map.CalculateViewportCoordinates(rect.UpperLeftCorner(),
                    GameData.MapScale),
                rotation: rect.Rotation);

            spriteBatch.End();
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
            if (crosshairTexture == null)
                return null;

            var xOffset = new Point(-crosshairTexture.Width - 20, 0);
            var yOffset = new Point(0, (Position.CollisionRectangle.Height / 2) - (crosshairTexture.Height / 2));
            return Position.Offset(xOffset).Offset(yOffset);
        }
    }
}

