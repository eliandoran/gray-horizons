namespace GrayHorizons.Actions.Debugging
{
    using System.Diagnostics;
    using System.IO;
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework.Input;

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

