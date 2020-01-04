using System;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework;
using GrayHorizons.Attributes;

namespace GrayHorizons.StaticObjects
{
    [MappedTextures("Circle")]
    public class Waypoint: StaticObject
    {
        int delta = 0;
        bool deltaDecreasing, isShrinkingAway;
        int deltaStep = 2;
        int maximumDelta = 10;
        int minimumDelta = -10;
        TimeSpan deltaTime, currentDeltaTime;

        public Waypoint()
        {
            MiniMapColor = Color.Purple;
            DefaultSize = new Point(20, 20);
            deltaTime = TimeSpan.FromMilliseconds(50);
            currentDeltaTime = TimeSpan.FromMilliseconds(deltaTime.TotalMilliseconds);
        }

        public void ShrinkAway()
        {
            isShrinkingAway = true;
            deltaStep = 15;
            deltaTime = TimeSpan.FromMilliseconds(15);
        }

        public override void Render()
        {
            var spriteBatch = GameData.ScreenManager.SpriteBatch;
            var viewport = GameData.Map.CalculateViewportCoordinates(Position.CollisionRectangle.Location.ToVector2(), GameData.MapScale);
            spriteBatch.Draw(
                GameData.MappedTextures[GetType()],
                destinationRectangle: new Rectangle(
                    (int)viewport.X - delta / 2,
                    (int)viewport.Y - delta / 2,
                    Position.CollisionRectangle.Width + delta,
                    Position.CollisionRectangle.Height + delta
                ),
                rotation: Position.Rotation,
                color: Color.Purple * .55f
            );
        }

        public override void Update(TimeSpan gameTime)
        {
            //Debug.WriteLine(isShrinkingAway)

            if (currentDeltaTime <= TimeSpan.Zero)
            {
                if (!deltaDecreasing && !isShrinkingAway)
                {
                    var newDelta = delta + deltaStep;

                    if (newDelta <= maximumDelta)
                        delta = newDelta;
                    else
                        deltaDecreasing = true;
                }
                else
                {
                    var newDelta = delta - deltaStep;

                    if (newDelta >= minimumDelta || (isShrinkingAway && newDelta > -Position.CollisionRectangle.Width))
                        delta = newDelta;
                    else
                    {
                        if (!isShrinkingAway)
                            deltaDecreasing = false;
                        else
                            GameData.Map.QueueRemoval(this);
                    }
                }

                currentDeltaTime = TimeSpan.FromMilliseconds(deltaTime.TotalMilliseconds);
            }
            else
            {
                currentDeltaTime -= gameTime;
            }

            base.Update(gameTime);
        }
    }
}

