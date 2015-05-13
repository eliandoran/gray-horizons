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

namespace GrayHorizons.Input.Actions
{
    [DefaultKey (Keys.F1)]
    public class ToggleGuidesTraceAction: GameAction
    {
        public ToggleGuidesTraceAction (
            GameData gameData) : base (
                gameData) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.ToggleGuidesTraceAction"/> class.
        /// </summary>
        public ToggleGuidesTraceAction () : this (
                null) { }

        public override void Execute ()
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
    public class MetamorphizeTank: GameAction
    {
        public MetamorphizeTank (
            GameData gameData,
            Player player) : base (
                gameData,
                player) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.MetamorphosizeTank"/> class.
        /// </summary>
        public MetamorphizeTank () : this (
                null,
                null) { }

        public override void Execute ()
        {
            var query = from type in Assembly.GetExecutingAssembly ().GetTypes ()
                                 where (type.BaseType == typeof(Tank))
                                 select type;
            var random = new Random ();
            var tankType = query.Skip (random.Next (query.Count ())).Take (1).First ();
            var newTank = (Tank)tankType.GetConstructor (new Type[] { }).Invoke (new Type[] { });

            newTank.Position = new RotatedRectangle (new Rectangle (
                    Player.AssignedEntity.Position.CollisionRectangle.X,
                    Player.AssignedEntity.Position.CollisionRectangle.Y,
                    newTank.DefaultSize.X,
                    newTank.DefaultSize.Y), Player.AssignedEntity.Position.Rotation
            );

            var playerTank = Player.AssignedEntity as Tank;
            //newTank.MuzzlePosition = newTank.GetMuzzleRotatedRectangle ();

            newTank.GameData = GameData;

            GameData.Map.Entities.Remove (Player.AssignedEntity);

            if (Player == GameData.ActivePlayer)
                GameData.ActivePlayer.AssignedEntity = newTank;

            Player.AssignedEntity = newTank;
            Player.AssignedEntity.Moved += (
                sender,
                e) => GameData.Map.CenterViewportAt (Player.AssignedEntity);

            #if DEBUG
            Debug.WriteLine ("{0} changed to <{1}>.".FormatWith (Player.AssignedEntity.ToString (), newTank),
                             "METAMORPHOSIZE");
            #endif

            GameData.Map.Entities.Add (Player.AssignedEntity);
        }
    }
}

