using System;
using System.Diagnostics;
using BattleCity.AIs;
using BattleCity.Entities;
using BattleCity.Extensions;
using BattleCity.Input;
using BattleCity.Input.Actions;
using BattleCity.Logic;
using BattleCity.StaticObjects;
using BattleCity.ThirdParty;
using BattleCity.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleCity.Screens
{
    public class BattlefieldScreen: GameScreen
    {
        ContentManager content;
        SpriteFont gameFont;
        SpriteBatch spriteBatch;
        Renderer renderer;
        GameData gameData;

        Tank playerTank = new Tank01 ();

        public BattlefieldScreen (GameData gameData)
        {
            this.gameData = gameData;
        }

        void WallLine(Map map,
                      int startX,
                      int endX,
                      int y)
        {
            for (int x = startX; x <= endX; x++)
                map.Add (new Wall () { Position = new RotatedRectangle (new Rectangle (x * 64, y * 64, 64, 64), 0) });
        }

        Map GetTestMap()
        {
            var map = new Map (new Vector2 (2000, 2000));

            playerTank.Position = new RotatedRectangle (new Rectangle (6 * 64,
                                                                       6 * 64,
                                                                       playerTank.DefaultSize.X,
                                                                       playerTank.DefaultSize.Y), 0);
            map.Viewport = new Rectangle (0, 0, 768, 768);

            var ur = playerTank.Position.UpperRightCorner ();
            var lr = playerTank.Position.LowerRightCorner ();
            playerTank.MuzzlePosition = new RotatedRectangle (new Rectangle (192, 37, 1, 8),
                                                              playerTank.Position.Rotation);            

            map.Add (playerTank);
            WallLine (map, 0, 9, 9);
            WallLine (map, 0, 9, 0);

            Player player = new Player () {
                AssignedTank = playerTank,
                Score = 100
            };

            var moveForwardKeyBinding = new KeyBinding (gameData, null, Keys.W, true);
            moveForwardKeyBinding.BoundAction = new MoveForwardAction (player, moveForwardKeyBinding);

            var moveBackwardKeyBinding = new KeyBinding (gameData, null, Keys.S, true);
            moveBackwardKeyBinding.BoundAction = new MoveBackwardAction (player, moveBackwardKeyBinding);

            var turnLeftKeyBinding = new KeyBinding (gameData, null, Keys.A, true);
            turnLeftKeyBinding.BoundAction = new TurnLeftAction (player, turnLeftKeyBinding);

            var turnRightKeyBinding = new KeyBinding (gameData, null, Keys.D, true);
            turnRightKeyBinding.BoundAction = new TurnRightAction (player, turnRightKeyBinding);

            gameData.InputBindings.AddRange (new InputBinding[] {
                moveForwardKeyBinding,
                moveBackwardKeyBinding,
                turnLeftKeyBinding,
                turnRightKeyBinding,
                new KeyBinding (gameData, new ShootAction (player), allowContinousPress: true),

                new KeyBinding (gameData, new ToggleGuidesTraceAction (gameData)),
                new KeyBinding (gameData, new MetamorphosizeTank (gameData, player)),
                new KeyBinding (gameData, new ToggleFullScreenAction (gameData))
            });                

            gameData.Players.Add (player);

            var config = new Configuration ();
            foreach (KeyBinding binding in gameData.InputBindings)
                config.InputBindings.Add (binding);

            config.Save (@"C:\Users\Elian\Desktop\config.xml");

            return map;
        }

        public override void HandleInput(InputState input)
        {
            foreach (InputBinding binding in gameData.InputBindings)
            {
                binding.UpdateState ();
            }
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager (ScreenManager.Game.Services, @"Content");

            gameFont = content.Load<SpriteFont> ("ArcadeFont");
            spriteBatch = new SpriteBatch (ScreenManager.GraphicsDevice);

            gameData.SpriteBatch = spriteBatch;
            gameData.ContentManager = content;
            gameData.GraphicsDevice = ScreenManager.GraphicsDevice;
            renderer = new Renderer (gameData);

            gameData.Map = GetTestMap ();
        }

        public override void Draw(GameTime gameTime)
        {
            #if DEBUG
            if (gameTime.ElapsedGameTime.TotalMilliseconds > 17)
                Debug.WriteLine (String.Format ("{0}ms".FormatWith (gameTime.ElapsedGameTime.TotalMilliseconds.ToString ())),
                                 "LAG");
            #endif


            spriteBatch.Begin ();
            gameData.Map.Update (gameTime.ElapsedGameTime);
            renderer.RenderTerrain (gameData.Map.MapSize);
            renderer.RenderStaticObjects (gameData.Map.Objects);
            renderer.RenderEntities (gameData.Map.Entities);
            spriteBatch.End ();
        }

        public override void UnloadContent()
        {
            base.UnloadContent ();
        }

        public override void Update(GameTime gameTime,
                                    bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            gameData.GameTime = gameTime;
        }
    }
}

