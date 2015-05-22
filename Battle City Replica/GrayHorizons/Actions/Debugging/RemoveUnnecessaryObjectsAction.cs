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

using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Logic;
using GrayHorizons.Entities;
using System.Diagnostics;
using GrayHorizons.Extensions;

namespace GrayHorizons.Actions.Debugging
{
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

