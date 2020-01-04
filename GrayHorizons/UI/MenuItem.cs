using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameStateManagement;
using System.Diagnostics;
using GrayHorizons.Extensions;

namespace GrayHorizons.UI
{
    public class MenuItem: GameScreen
    {
        public event EventHandler<EventArgs> Select;
        public event EventHandler<EventArgs> Deselect;
        public event EventHandler<EventArgs> Executed;

        SpriteBatch spriteBatch;
        Texture2D texture;

        public string Text { get; set; }

        public Rectangle Dimensions { get; set; }

        public Vector2 TextPosition { get; set; }

        public TimeSpan ActivationCooltime { get; set; }

        public TimeSpan CurrentActivationCooltime { get; set; }

        public bool Enabled { get; set; }

        public bool Selected { get; set; }

        public Color Color { get; set; }

        public Color SelectedColor { get; set; }

        public Color SelectedBackColor { get; set; }

        public SpriteFont Font { get; set; }

        public bool CenterHorizontally { get; set; }

        public bool CenterVertically { get; set; }

        public MenuItem(
            string text)
        {
            Text = text;
            Enabled = true;

            Color = SelectedColor = Color.Black;
            SelectedBackColor = new Color(0, 0, 0, 150);

            TransitionOnTime = TimeSpan.FromMilliseconds(500);
            TransitionOffTime = TimeSpan.FromMilliseconds(500);

            ActivationCooltime = TimeSpan.FromMilliseconds(100);
            CurrentActivationCooltime = TimeSpan.FromMilliseconds(ActivationCooltime.TotalMilliseconds);

            IsPopup = true;
        }

        public override void Activate(bool instancePreserved)
        {
            base.Activate(instancePreserved);                
            spriteBatch = ScreenManager.SpriteBatch;
            texture = ScreenManager.Game.Content.Load<Texture2D>("Blank");
        }

        public override void Draw(
            GameTime gameTime)
        {
            spriteBatch.Begin();

            if (Font.IsNotNull())
            {
                var color = (Selected ? SelectedColor : Color);
                color = new Color(color, TransitionAlpha);

                if (Selected && Enabled)
                {
                    var backColor = new Color(SelectedBackColor, (int)(TransitionAlpha * SelectedBackColor.A));

                    spriteBatch.Draw(
                        texture,
                        Dimensions,
                        backColor);
                }

                var metrics = Font.MeasureString(Text);
                var pos = new Vector2(
                              (CenterHorizontally ? Dimensions.X + ((Dimensions.Width / 2) - (metrics.X / 2)) : Dimensions.X + TextPosition.X),
                              (CenterVertically ? Dimensions.Y + ((Dimensions.Height / 2) - (metrics.Y / 2)) : Dimensions.Y + TextPosition.Y)
                          );
                spriteBatch.DrawString(Font, Text, pos, color);
            }

            spriteBatch.End();
        }

        internal void OnSelect(
            EventArgs e)
        {
            if (Select.IsNotNull())
                Select(this, e);
        }

        internal void OnDeselect(
            EventArgs e)
        {
            if (Deselect.IsNotNull())
                Deselect(this, e);
        }

        internal void OnExecute(
            EventArgs e)
        {
            if (CurrentActivationCooltime.TotalMilliseconds < 1 && Executed.IsNotNull())
            {
                Executed(this, e);
                CurrentActivationCooltime = ActivationCooltime;
            }
        }

        public virtual void Execute()
        {
            OnExecute(EventArgs.Empty);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (CurrentActivationCooltime > gameTime.ElapsedGameTime)
                CurrentActivationCooltime -= gameTime.ElapsedGameTime;
            else
                CurrentActivationCooltime = TimeSpan.Zero;

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override string ToString()
        {
            return "[MenuItem: {0}]".FormatWith(Text);
        }
    }
}

