using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using GrayHorizons.Logic;
using GameStateManagement;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Screens
{
    public class OnScreenConsole: GameScreen
    {
        readonly StringBuilder console = new StringBuilder();
        readonly GameData gameData;
        Vector2? metrics;
        Rectangle rect;

        public int Padding { get; set; }

        public StringBuilder Console
        {
            get
            {
                return console;
            }
        }

        public OnScreenConsole(GameData gameData)
        {
            this.gameData = gameData;
            IsPopup = true;

            gameData.ResolutionChanged += GameData_ResolutionChanged;
        }

        void GameData_ResolutionChanged(object sender, EventArgs e)
        {
            rect = GetRectangle();
        }

        public override void Draw(GameTime gameTime)
        {
            if (!metrics.HasValue)
            {
                metrics = ScreenManager.Font.MeasureString(" ");
                rect = GetRectangle();
            }

            if (!gameData.DebuggingSettings.ShowConsole)
                return;

            var lines = (int)(rect.Height / metrics.Value.Y) + 1;

            ScreenManager.SpriteBatch.Begin();

            var frameSize = 5;
            ScreenManager.SpriteBatch.Draw(
                gameData.BlankTexture,
                destinationRectangle: new Rectangle(0, rect.Bottom, rect.Width, frameSize),
                color: Color.Black * 0.3f
            );

            ScreenManager.SpriteBatch.Draw(
                gameData.BlankTexture,
                destinationRectangle: new Rectangle(rect.X, rect.Y, rect.Width, rect.Height),
                color: Color.Black * 0.6f
            );

            ScreenManager.SpriteBatch.DrawString(
                ScreenManager.Font,
                Process(TakeLastLines(gameData.TraceListener.Builder.ToString(), lines)),
                new Vector2(rect.Location.X + frameSize, rect.Location.Y * frameSize),
                Color.White * 0.8f
            );
            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        string Process(string message)
        {            
            StringBuilder builder = new StringBuilder();

            foreach (char ch in message)
            {
                if (ch == '\n' || ch >= 32 && ch <= 127)
                    builder.Append(ch);
                else
                    builder.Append(" ");
            }

            return builder.ToString();
        }

        Rectangle GetRectangle()
        {
            var windowWidth = gameData.GraphicsDevice.Viewport.Width;
            var windowHeight = gameData.GraphicsDevice.Viewport.Height;
            var height = (int)(windowHeight / 2.5);
            return new Rectangle(0, 0, windowWidth, height);
        }

        static string TakeLastLines(string text, int count)
        {
            ;

            StringBuilder builder = new StringBuilder();
            List<string> lines = new List<string>();
            Match match = Regex.Match(text, "^.*$", RegexOptions.Multiline | RegexOptions.RightToLeft);

            while (match.Success && lines.Count < count)
            {
                lines.Insert(0, match.Value);
                match = match.NextMatch();
            }

            foreach (var line in lines)
                builder.AppendLine(line);

            return builder.ToString();
        }
    }
}

