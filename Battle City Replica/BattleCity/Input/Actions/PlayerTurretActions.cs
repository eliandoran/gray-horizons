using System;
using BattleCity.Logic;
using System.Diagnostics;
using BattleCity.Entities;

namespace BattleCity.Input.Actions
{
    public class TankMouseTurretControl: GameAction
    {
        public TankMouseTurretControl (
            GameData gameData = null,
            Player player = null,
            InputBinding parentInputBinding = null) : base (
                gameData,
                player,
                parentInputBinding)
        {
            ParentInputBindingChanged += TankMouseTurretControl_ParentInputBindingChanged;
            OnParentInputBindingChanged (EventArgs.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Input.Actions.TankMouseTurretControl"/> class.
        /// </summary>
        public TankMouseTurretControl () : this (
                null) { }

        void TankMouseTurretControl_ParentInputBindingChanged (
            object sender,
            EventArgs e)
        {
            var mouseAxisBinding = ParentInputBinding as MouseAxisBinding;
            if (mouseAxisBinding != null)
            {
                mouseAxisBinding.AxisChanged += MouseAxisChanged;
            }
        }

        void MouseAxisChanged (
            object sender,
            AxisChangedEventArgs e)
        {
            var playerTank = Player.AssignedEntity as Tank;

            if (playerTank != null)
            {
                var pos = Player.AssignedEntity.Position.CollisionRectangle.Center - e.State.Position;
                pos = GameData.Map.CalculateViewportCoordinates (pos.ToVector2 (), GameData.Scale).ToPoint ();

                var currentRotation = playerTank.TurrelRotation;
                var rotation = Rotation.FromRadians ((float)Math.Atan2 (pos.Y, pos.X));

                playerTank.TurretRect = new BattleCity.ThirdParty.RotatedRectangle (
                    playerTank.Position.CollisionRectangle,
                    playerTank.Position.Rotation);
                playerTank.TurrelRotation = rotation;
            }
        }

        public override void Execute ()
        {

        }
    }

}