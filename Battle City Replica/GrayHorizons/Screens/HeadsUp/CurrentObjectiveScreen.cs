using System;
using GrayHorizons.Objectives;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework.Graphics;
using GrayHorizons.Extensions;

namespace GrayHorizons.Screens.HeadsUp
{
    public class CurrentObjectiveScreen: GameScreen
    {
        readonly ObjectiveList objectives;
        readonly GameData gameData;
        readonly SpriteBatch spriteBatch;

        public CurrentObjectiveScreen(GameData gameData, ObjectiveList objectives)
        {
            IsPopup = true;

            if (objectives.IsNotNull())
                this.objectives = objectives;
            else
                this.objectives = gameData.Objectives;

            spriteBatch = gameData.ScreenManager.SpriteBatch;
            this.gameData = gameData;
        }

        public CurrentObjectiveScreen(GameData gameData)
            : this(gameData, null)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Objective objective = objectives.GetFirstUncompletedObjective();

            if (objective.IsNotNull())
            {
                if (objective.Text.IsNull())
                    return;

                var texture = gameData.BlankTexture;
                var x = 5;
                var y = 5;
                var metrics = ScreenManager.Font.MeasureString(objective.Text);
                var rect = new Rectangle(x, y, (int)metrics.X + 10, (int)metrics.Y + 10);

                spriteBatch.Begin();

                spriteBatch.Draw(
                    texture,
                    rect,
                    Color.Black * .4f
                );

                spriteBatch.DrawString(
                    ScreenManager.Font,
                    objective.Text,
                    new Vector2(rect.X + 5, rect.Y + 5),
                    Color.White
                );

                spriteBatch.End();
            }
        }
    }
}

