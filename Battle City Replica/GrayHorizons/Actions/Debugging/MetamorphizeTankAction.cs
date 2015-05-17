using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using System.Reflection;
using GrayHorizons.ThirdParty;
using System.Linq;
using GrayHorizons.Entities;
using System.Diagnostics;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Actions.Debugging
{
    [DefaultKey(Keys.F2)]
    public class MetamorphizeTankAction: GameAction
    {
        public MetamorphizeTankAction(
            GameData gameData,
            Player player)
            : base(
                gameData,
                player)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.MetamorphosizeTank"/> class.
        /// </summary>
        public MetamorphizeTankAction()
            : this(
                null,
                null)
        {
        }

        public override void Execute()
        {
            var query = from type in Assembly.GetExecutingAssembly().GetTypes()
                                 where (type.BaseType == typeof(Tank))
                                 select type;
            var random = new Random();
            var tankType = query.Skip(random.Next(query.Count())).Take(1).First();
            var newTank = (Tank)tankType.GetConstructor(new Type[] { }).Invoke(new Type[] { });

            newTank.Position = new RotatedRectangle(new Rectangle(
                    Player.AssignedEntity.Position.CollisionRectangle.X,
                    Player.AssignedEntity.Position.CollisionRectangle.Y,
                    newTank.DefaultSize.X,
                    newTank.DefaultSize.Y), Player.AssignedEntity.Position.Rotation
            );

            var playerTank = Player.AssignedEntity as Tank;
            //newTank.MuzzlePosition = newTank.GetMuzzleRotatedRectangle ();

            newTank.GameData = GameData;

            GameData.Map.Entities.Remove(Player.AssignedEntity);

            if (Player == GameData.ActivePlayer)
                GameData.ActivePlayer.AssignedEntity = newTank;

            Player.AssignedEntity = newTank;
            Player.AssignedEntity.Moved += (
                sender,
                e) => GameData.Map.CenterViewportAt(Player.AssignedEntity);

            #if DEBUG
            Debug.WriteLine("[METAMORPHOSIZE] {0} changed to <{1}>.", Player.AssignedEntity.ToString(), newTank);
            #endif

            GameData.Map.Entities.Add(Player.AssignedEntity);
        }
    }
}

