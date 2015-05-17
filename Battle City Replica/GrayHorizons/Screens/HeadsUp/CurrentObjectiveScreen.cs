using System;
using GrayHorizons.Objectives;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework.Graphics;

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

            if (objectives != null)
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

            if (objective != null)
            {
                var texture = gameData.BlankTexture;
                var x = 5;
                var y = 5;
                var rect = new Rectangle(x, y, 400, 20);

                spriteBatch.Begin();

                spriteBatch.DrawString(
                    ScreenManager.Font,
                    objective.Text,
                    new Vector2(rect.X + 5, rect.Y + 5),
                    color: Color.White
                );

                spriteBatch.End();
            }
        }
    }
}

