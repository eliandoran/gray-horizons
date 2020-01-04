namespace GrayHorizons.Actions.Debugging
{
    using System;
    using System.Diagnostics;
    using GrayHorizons.Attributes;
    using GrayHorizons.Entities;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.F3)]
    public class RemoveUnnecessaryObjectsAction: GameAction
    {
        public override void Execute()
        {
            var count = 0;

            foreach (ObjectBase obj in GameData.Map.GetObjects())
            {
                if (obj == GameData.ActivePlayer.AssignedEntity)
                    continue;

                var soldier = obj as Soldier;
                if (soldier.IsNotNull())
                {
                    var vehicle = GameData.ActivePlayer.AssignedEntity as Vehicle;
                    if (vehicle.IsNotNull() && vehicle.Passengers.Contains(soldier))
                        continue;
                }

                GameData.Map.QueueRemoval(obj);
                count++;
            }

            Debug.WriteLine("{0} objects were removed.".FormatWith(count), "CLEAR");
        }
    }
}

