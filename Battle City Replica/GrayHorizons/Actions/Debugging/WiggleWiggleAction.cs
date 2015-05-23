namespace GrayHorizons.Actions.Debugging
{
    using System.Diagnostics;
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.F4)]
    public class WiggleWiggleAction: GameAction
    {
        public override void Execute()
        {
            if (GameData.Map.ShakeFactor < 5)
                GameData.Map.ShakeFactor += 1;
            else if (GameData.Map.ShakeFactor < 20)
                GameData.Map.ShakeFactor += 5;
            else
                GameData.Map.ShakeFactor = 0;

            Debug.WriteLine("Shake factor set to {0}x.".FormatWith(GameData.Map.ShakeFactor), "WIGGLE");
        }
    }
}

