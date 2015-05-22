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
using GrayHorizons.Logic;
using Microsoft.Xna.Framework.Input;
using System.IO;
using GrayHorizons.Extensions;
using System.Diagnostics;

namespace GrayHorizons.Actions.Debugging
{
    [DefaultKey(Keys.F5)]
    public class SavePositionAction: GameAction
    {
        public override void Execute()
        {            
            const string path = "Positions.txt";

            using (var stream = GameData.IOAgent.GetStream(path, FileMode.Append))
            using (var streamWriter = new StreamWriter(stream))
            {
                var pos = GameData.ActivePlayer.AssignedEntity.Position;             
                streamWriter.WriteLine(
                    "[X={0}, Y={1}, Width={2}, Height={3}, Orientation={4}]".FormatWith(
                        pos.CollisionRectangle.X,
                        pos.CollisionRectangle.Y,
                        pos.CollisionRectangle.Width,
                        pos.CollisionRectangle.Height,
                        pos.Rotation
                    ));

                Debug.WriteLine("Positions saved to {0}.".FormatWith(Path.GetFileName(path)));
            }
        }
    }
}

