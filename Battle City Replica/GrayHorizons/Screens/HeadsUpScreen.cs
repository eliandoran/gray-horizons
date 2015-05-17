using System;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework.Graphics;
using GrayHorizons.Extensions;

namespace GrayHorizons.Screens
{
    public class HeadsUpScreen: GameScreen
    {
        readonly HeadsUp.MinimapScreen minimapScreen;
        readonly HeadsUp.CurrentObjectiveScreen currentObjectiveScreen;
        readonly HeadsUp.PlayerStateScreen playerStateScreen;

        bool firstTime = true;
        GameData gameData;

        public HeadsUpScreen(GameData gameData)
        {
            minimapScreen = new HeadsUp.MinimapScreen(gameData, HeadsUp.MinimapScreen.MinimapPosition.TopRight);
            currentObjectiveScreen = new HeadsUp.CurrentObjectiveScreen(gameData);
            playerStateScreen = new HeadsUp.PlayerStateScreen(gameData);

            this.gameData = gameData;
            IsPopup = true;
        }

        public HeadsUp.MinimapScreen MinimapScreen
        {
            get
            {
                return minimapScreen;
            }
        }

        public HeadsUp.CurrentObjectiveScreen CurrentObjective
        {
            get
            {
                return currentObjectiveScreen;
            }
        }

        public override void Update(GameTime gameTime,
                                    bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            if (firstTime)
            {
                ScreenManager.AddScreen(minimapScreen, null);
                ScreenManager.AddScreen(currentObjectiveScreen, null);
                ScreenManager.AddScreen(playerStateScreen, null);
                firstTime = false;
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {            
            foreach (ObjectBase obj in gameData.Map.GetObjects())
                obj.RenderHUD();

//            ScreenManager.SpriteBatch.Begin();
//            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Entities: {0}\nStatic Objects: {1}".FormatWith(gameData.Map.Entities.Count, gameData.Map.StaticObjects.Count), new Vector2(0, 0), Color.White);
//            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

