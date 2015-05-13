using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GrayHorizons.ThirdParty.GameStateManagement;
using System.Diagnostics;

namespace GrayHorizons.UI
{
    public class MenuItem: GameScreen
    {
        public event EventHandler<EventArgs> Select;
        public event EventHandler<EventArgs> Deselect;
        public event EventHandler<EventArgs> Activate;

        SpriteBatch spriteBatch;
        Texture2D texture;

        public string Text { get; set; }

        public Rectangle Dimensions { get; set; }

        public Vector2 TextPosition { get; set; }

        public bool Enabled { get; set; }

        public bool Selected { get; set; }

        public Color Color { get; set; }

        public Color SelectedColor { get; set; }

        public Color SelectedBackColor { get; set; }

        public SpriteFont Font { get; set; }

        public bool CenterHorizontally { get; set; }

        public bool CenterVertically { get; set; }

        public MenuItem (
            string text)
        {
            Text = text;
            Enabled = true;

            Color = SelectedColor = Color.Black;
            SelectedBackColor = new Color (0, 0, 0, 150);

            TransitionOnTime = TimeSpan.FromMilliseconds (500);
            TransitionOffTime = TimeSpan.FromMilliseconds (500);

            IsPopup = true;
        }

        public override void LoadContent ()
        {
            spriteBatch = ScreenManager.SpriteBatch;
            texture = ScreenManager.Game.Content.Load<Texture2D> ("Blank");
        }

        public override void Draw (
            GameTime gameTime)
        {
            spriteBatch.Begin ();

            if (Font != null)
            {
                var color = (Selected ? SelectedColor : Color);
                color = new Color (color, TransitionAlpha);

                if (Selected && Enabled)
                {
                    var backColor = new Color (SelectedBackColor, (int)(TransitionAlpha * SelectedBackColor.A));

                    spriteBatch.Draw (
                        texture,
                        Dimensions,
                        backColor);
                }

                var metrics = Font.MeasureString (Text);
                var pos = new Vector2 (
                              (CenterHorizontally ? Dimensions.X + ((Dimensions.Width / 2) - (metrics.X / 2)) : Dimensions.X + TextPosition.X),
                              (CenterVertically ? Dimensions.Y + ((Dimensions.Height / 2) - (metrics.Y / 2)) : Dimensions.Y + TextPosition.Y)
                          );
                spriteBatch.DrawString (Font, Text, pos, color);
            }

            spriteBatch.End ();
        }

        internal void OnSelect (
            EventArgs e)
        {
            if (Select != null)
                Select (this, e);
        }

        internal void OnDeselect (
            EventArgs e)
        {
            if (Deselect != null)
                Deselect (this, e);
        }

        internal void OnActivate (
            EventArgs e)
        {
            if (Activate != null)
                Activate (this, e);
        }

        public virtual void Execute ()
        {
            OnActivate (EventArgs.Empty);
        }

        public override string ToString ()
        {
            return string.Format (
                "[MenuItem: Text={0}, Dimensions={1}, TextPosition={2}, Enabled={3}, Selected={4}, Color={5}, SelectedColor={6}, SelectedBackColor={7}, Font={8}, CenterHorizontally={9}, CenterVertically={10}]",
                Text,
                Dimensions,
                TextPosition,
                Enabled,
                Selected,
                Color,
                SelectedColor,
                SelectedBackColor,
                Font,
                CenterHorizontally,
                CenterVertically);
        }
    }
}

