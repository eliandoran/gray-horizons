/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/

namespace GrayHorizons.Actions.Debugging
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using GrayHorizons.Attributes;
    using GrayHorizons.Entities;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using GrayHorizons.ThirdParty;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.F2)]
    public class MetamorphizeTankAction: GameAction
    {
        public override void Execute()
        {
            var query = from type in Assembly.GetExecutingAssembly().GetTypes()
                                 where (type.BaseType == typeof(Tank))
                                 select type;
            var random = new Random();
            var enumerable = query as System.Collections.Generic.IList<Type> ?? query.ToList();
            var tankType = enumerable.Skip(random.Next(enumerable.Count())).Take(1).First();
            var newTank = (Tank)tankType.GetConstructor(new Type[] { }).Invoke(new Type[] { });

            newTank.Position = new RotatedRectangle(new Rectangle(
                    Player.AssignedEntity.Position.CollisionRectangle.X,
                    Player.AssignedEntity.Position.CollisionRectangle.Y,
                    newTank.DefaultSize.X,
                    newTank.DefaultSize.Y), Player.AssignedEntity.Position.Rotation
            );

            newTank.GameData = GameData;

            var playerTank = Player.AssignedEntity as Tank;
            if (playerTank.IsNotNull())
            {
                newTank.Passengers.AddRange(playerTank.Passengers);

                GameData.Map.Entities.Remove(Player.AssignedEntity);

                if (Player == GameData.ActivePlayer)
                    GameData.ActivePlayer.AssignedEntity = newTank;
            }

            var playerSoldier = Player.AssignedEntity as Soldier;
            if (playerSoldier.IsNotNull())
            {
                newTank.BoardPassenger(playerSoldier);
            }

            Player.AssignedEntity = newTank;
            Player.AssignedEntity.Moved += (
                sender,
                e) => GameData.Map.CenterViewportAt(Player.AssignedEntity);

            Debug.WriteLine("[METAMORPHIZE] {0} changed to <{1}>.".FormatWith(Player.AssignedEntity.ToString(), newTank));

            GameData.Map.Entities.Add(Player.AssignedEntity);
        }
    }
}

