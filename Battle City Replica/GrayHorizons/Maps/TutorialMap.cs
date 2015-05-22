using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using GrayHorizons.Entities.Cars;
using GrayHorizons.Logic;
using GrayHorizons.Objectives;
using GrayHorizons.StaticObjects;
using Microsoft.Xna.Framework;
using GrayHorizons.Screens;
using GrayHorizons.Entities.Tanks;
using GrayHorizons.Extensions;

namespace GrayHorizons.Maps
{
    [MappedTextures(@"Maps\Tutorial")]
    public class TutorialMap: Map
    {
        readonly Soldier player;
        readonly Vehicle pickup;

        public TutorialMap(GameData gameData)
            : base(new Vector2(6400, 4800), gameData)
        {
            Action<string> msg = message => gameData.ScreenManager.AddScreen(new MessageScreen(gameData, message), null);

            #region Player
            player = new Soldier();
            player.GameData = gameData;
            //player.Position = new RotatedRectangle(new Rectangle(6220, 1120, player.DefaultSize.X, player.DefaultSize.Y), 0);
            //player.Location = new Point(1872, 3405);
            player.Location = new Point(6220, 1120);
            //player.Location = new Point(2337, 2075);
            player.Moved += (sender, e) => GameData.Map.CenterViewportAt(player);
            player.MiniMapColor = Color.White;

            Entities.Add(player);
            GameData.ActivePlayer.AssignedEntity = player;
            #endregion

            #region Collision Boundaries
            var boundaries = CollisionBoundary.DeserializeList(
                                 GameData.IOAgent,
                                 GameData.ContentManager.RootDirectory + @"\Data\Tutorial.collision.xml"
                             );

            collisionBoundaries = new Collection<CollisionBoundary>(boundaries);
            #endregion

            #region Pickup
            pickup = new MinelayerPickup
            {
                CanBeBoarded = true,
                GameData = gameData,
                Location = new Point(6061, 1584),
                Orientation = MathHelper.ToRadians(105)
            };
            Entities.Add(pickup);
            #endregion
            GameData.Map = this;

            GameData.Map.Add(new AmmoCrate
                {
                    Location = new Point(1797, 4196),
                    Orientation = 5.631316f
                });

            GameData.Map.Add(new AmmoCrate
                {
                    Location = new Point(1927, 4201),
                    Orientation = 4.15997f
                });

            var path = GameData.ContentManager.RootDirectory + @"\Data\Tutorial.dummies.xml";
            List<DummyTarget> dummyTargets;
            using (Stream stream = GameData.IOAgent.GetStream(path, FileMode.Open))
            {
                var serializer = new XmlSerializer(
                                     typeof(List<DummyTarget>),
                                     new XmlRootAttribute("Dummies"));

                dummyTargets = (List<DummyTarget>)serializer.Deserialize(stream);
            }

            #region Objectives
            var goToCarObjective = new ReachPointObjective(new Point(6140, 1630), 220)
            {
                Text = "Mergi către mașina albă indicată pe hartă.",
                InitialMessage =
                    "Bine ai venit în lumea orizonturilor cenușii, în continuare vom prezenta câteva elemente specifice jocului.\n\n" +
                "Pentru început, controlezi un soldat, acestea sunt tastele:\n" +
                "<W>/<S> - Mergi în față / în spate\n" +
                "<A>/<D> - Întoarce spre stânga / dreapta\n" +
                "<Spațiu> - Trage.",
                InitialMessageDuration = TimeSpan.FromSeconds(5)
            };

            var enterCarPickup = new BoardVehicleObjective(player, pickup)
            {
                Text = "Intră în mașină.",
                InitialMessage = "Așează-te lângă mașină și apasă tasta <F> pentru a intra în ea.",
                EndingMessage =
                "Mașina se controlează astfel:\n" +
                "<W> Accelerează\n" +
                "<S> Marșarier\n" +
                "<A> Vireză la stânga\n" +
                "<D> Virează la dreapta.\n"
            };

            var crossBridge = new ReachPointObjective(new Point(1872, 3405), 300)
            {
                Text = "Traversează podul pentru a ajunge pe cealaltă parte a malului."
            };

            var destroyTargets = new DestroyMultipleObjective(dummyTargets, "Distruge țintele ({0} din {1} rămase)\nTimp rămas: {2}")
            {
                InitialMessage = "Ai un minut la dispoziție să distrugi {0} ținte, marcate cu galben pe mini-hartă.".FormatWith(dummyTargets.Count),
                TimeLeft = TimeSpan.FromMinutes(1)          
            };

            destroyTargets.Starting += (sender, e) => dummyTargets.ForEach(Add);
            destroyTargets.Won += (sender, e) => msg("Felicitări!");
            destroyTargets.Lost += (sender, e) =>
            {
                msg("Din păcate nu ai reușit să distrugi țintele în timp util. Mergi înapoi pentru a reîncepe.");
                GameData.Objectives.SkipTo(2);
            };

            var playerTank = new TankE100
            {
                Location = new Point(2790, 2068),
                Orientation = 2.538849f,
                CanBeBoarded = false,
                AmmoCapacity = 25
            };
            GameData.Map.Add(playerTank);
            
            var goToTankObjective = new ReachPointObjective(playerTank.Position.CollisionRectangle.Center, 250)
            {
                Text = "Mergi spre tancul indicat pe hartă."
            };
            goToTankObjective.Starting += (sender, e) => playerTank.CanBeBoarded = true;

            var enterTank = new BoardVehicleObjective(player, playerTank)
            {
                Text = "Intră în tanc.",
                EndingMessage =
                        "Tancul se controlează astfel:\n" +
                "<W/A/S/D> Controlul direcției tancului\n" +
                "<Space> Trage\n" +
                "Pentru a controla turela de tragere, se folosește mouse-ul."
            };

            var enemyTanks = new List<ObjectBase>()
            {
                new TankKV2
                {
                    Location = new Point(375, 3474),
                    Orientation = 3.626071f,
                    AI = new AI.VanillaAI()
                },

                new TankM6
                {
                    Location = new Point(1720, 3330),
                    Orientation = -0.2536778f,
                    AI = new AI.VanillaAI()
                },

                new TankPzKpfwB
                {
                    Location = new Point(1890, 2568),
                    Orientation = -5.664198f,
                    AI = new AI.VanillaAI()
                },

                new TankVK3601h
                {
                    Location = new Point(300, 2032),
                    Orientation = 0.8284287f,
                    AI = new AI.VanillaAI()
                },

                new TankM6
                {
                    Location = new Point(666, 1458),
                    Orientation = -5.664198f,
                    AI = new AI.VanillaAI()
                },

                new TankPzKpfwB
                {
                    Location = new Point(2430, 1271),
                    Orientation = -0.2536778f,
                    AI = new AI.VanillaAI()
                },
            };

            var destroyEnemyTank = new DestroyMultipleObjective(
                                       enemyTanks,
                                       "Distruge țintele ({0} din {1})\n" +
                                       "Timp ramas: {2}",
                                       TimeSpan.FromMilliseconds(30)
                                   )
            {
                TimeLeft = TimeSpan.FromMinutes(10)
            };
            destroyEnemyTank.Starting += (sender, e) => enemyTanks.ForEach(GameData.Map.Add);
            destroyEnemyTank.Ending += (sender, e) => msg("Felicitări, ai terminat cu succes acest tutorial!");

            GameData.Objectives.AddRange(new Objective[]
                {
                    goToCarObjective,
                    enterCarPickup,
                    crossBridge,
                    destroyTargets,
                    goToTankObjective,
                    enterTank,
                    destroyEnemyTank
                });

            GameData.Objectives.GameData = GameData;

            //GameData.Objectives.SkipTo(4);
            #endregion
        }
    }
}

