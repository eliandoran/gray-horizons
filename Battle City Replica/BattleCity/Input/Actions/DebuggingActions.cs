using System;
using BattleCity.Logic;
using System.Diagnostics;
using BattleCity.Entities;
using System.Reflection;
using System.Linq;
using BattleCity.Extensions;
using Microsoft.Xna.Framework;
using BattleCity.ThirdParty;
using BattleCity.Attributes;
using Microsoft.Xna.Framework.Input;

namespace BattleCity.Input.Actions
{
    [DefaultKey (Keys.F1)]
    public class ToggleGuidesTraceAction: GameAction
    {
        public ToggleGuidesTraceAction (GameData gameData) : base (gameData) { }

        public ToggleGuidesTraceAction () : this (null) { }

        public override void Execute()
        {
            GameData.DebuggingSettings.ShowGuides = !GameData.DebuggingSettings.ShowGuides;

            #if DEBUG
            if (GameData.DebuggingSettings.ShowGuides)
                Debug.WriteLine ("Guides activated.", "GUIDES");
            else
                Debug.WriteLine ("Guides deactivated.", "GUIDES");
            #endif
        }
    }

    [DefaultKey (Keys.F2)]
    public class MetamorphosizeTank: GameAction
    {
        public MetamorphosizeTank (GameData gameData,
                                   Player player) : base (gameData,
                                                          player) { }


        public MetamorphosizeTank () : this (null,
                                             null) { }

        public override void Execute()
        {
            var query = from type in Assembly.GetExecutingAssembly ().GetTypes ()
                                 where (type.BaseType == typeof(Tank))
                                 select type;
            var random = new Random ();
            var tankType = query.Skip (random.Next (query.Count ())).Take (1).First ();
            var newTank = (Tank)tankType.GetConstructor (new Type[] { }).Invoke (new Type[] { });

            newTank.Position = new RotatedRectangle (new Rectangle (
                Player.AssignedTank.Position.CollisionRectangle.X,
                Player.AssignedTank.Position.CollisionRectangle.Y,
                newTank.DefaultSize.X,
                newTank.DefaultSize.Y), Player.AssignedTank.Position.Rotation
            );
            
            newTank.MuzzlePosition = Player.AssignedTank.MuzzlePosition;
            newTank.ParentMap = GameData.Map;              

            GameData.Map.Entities.Remove (Player.AssignedTank);
            Player.AssignedTank = newTank;

            #if DEBUG
            Debug.WriteLine ("{0} changed to <{1}>.".FormatWith (Player.AssignedTank.ToString (), newTank),
                             "METAMORPHOSIZE");
            #endif

            GameData.Map.Entities.Add (Player.AssignedTank);
        }
    }
}

