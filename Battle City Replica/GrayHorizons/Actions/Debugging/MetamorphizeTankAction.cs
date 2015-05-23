namespace GrayHorizons.Actions.Debugging
{
    using System;
    using System.Diagnostics;
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
        Random random = new Random();

        public override void Execute()
        {        
            var newTank = (Tank)Tank.GetTankTypes().RandomElement().GetConstructor(new Type[] { }).Invoke(new Type[] { });

            newTank.Position = new RotatedRectangle(
                new Rectangle(
                    Player.AssignedEntity.Position.CollisionRectangle.X,
                    Player.AssignedEntity.Position.CollisionRectangle.Y,
                    newTank.DefaultSize.X,
                    newTank.DefaultSize.Y),
                Player.AssignedEntity.Position.Rotation
            );

            newTank.GameData = GameData;

            Player.AssignedEntity.TryCast<Tank>(tank =>
                {
                    newTank.Passengers.AddRange(tank.Passengers);
                    GameData.Map.Entities.Remove(tank);

                    if (Player == GameData.ActivePlayer)
                        GameData.ActivePlayer.AssignedEntity = newTank;
                });

            Player.AssignedEntity.TryCast<Soldier>(newTank.BoardPassenger);
            Player.AssignedEntity = newTank;
            Player.AssignedEntity.Moved += (sender, e) => GameData.Map.CenterViewportAt(Player.AssignedEntity);
            GameData.Map.Entities.Add(Player.AssignedEntity);

            Debug.WriteLine("[METAMORPHIZE] {0} changed to <{1}>.".FormatWith(
                    Player.AssignedEntity.ToString(),
                    newTank));
        }
    }
}

