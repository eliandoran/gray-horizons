using System;
using Microsoft.Xna.Framework;
using Microsoft.Office.Interop.PowerPoint;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace GrayHorizons.UI
{
    public class ProgressBar: GameScreen
    {
        public Rectangle Position { get; set; }

        public int CurrentValue { get; set; }

        public int MaximumValue { get; set; }

        SpriteBatch spriteBatch;

        Texture2D texture;

        public Color EmptyColor { get; set; }

        public Color FilledColor { get; set; }

        public double Percentage
        {
            get
            {
                Debug.WriteLine("CV: {0}, MV: {1}, R: {2}", CurrentValue, MaximumValue, CurrentValue / MaximumValue);
                return (double)CurrentValue / (double)MaximumValue;
            }
        }

        public ProgressBar()
        {
            IsPopup = true;
            EmptyColor = Color.Black * .25f;
            FilledColor = Color.Blue * .75f;

            MaximumValue = 100;
        }

        public override void LoadContent()
        {
            spriteBatch = ScreenManager.SpriteBatch;
            texture = ScreenManager.Game.Content.Load<Texture2D>("Blank");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.Draw(
                texture,
                destinationRectangle: Position,
                color: EmptyColor
            );

            var rect = new Rectangle(
                           Position.X,
                           Position.Y,
                           (int)(Percentage * Position.Width),
                           Position.Height
                       );
            ScreenManager.SpriteBatch.Draw(texture, rect, FilledColor);

            ScreenManager.SpriteBatch.End();
        }
    }
}

