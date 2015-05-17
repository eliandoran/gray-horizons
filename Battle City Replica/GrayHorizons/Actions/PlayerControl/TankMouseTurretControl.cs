using System;
using GrayHorizons.Input;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Entities;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.PlayerControl
{
    public class TankMouseTurretControl: GameAction
    {
        public TankMouseTurretControl(
            GameData gameData = null,
            Player player = null,
            InputBinding parentInputBinding = null)
            : base(
                gameData,
                player,
                parentInputBinding)
        {
            ParentInputBindingChanged += TankMouseTurretControl_ParentInputBindingChanged;
            OnParentInputBindingChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.TankMouseTurretControl"/> class.
        /// </summary>
        public TankMouseTurretControl()
            : this(
                null)
        {
        }

        void TankMouseTurretControl_ParentInputBindingChanged(
            object sender,
            EventArgs e)
        {
            var mouseAxisBinding = ParentInputBinding as MouseAxisBinding;
            if (mouseAxisBinding != null)
            {
                mouseAxisBinding.MouseStateChanged += MouseAxisChanged;
            }
        }

        void MouseAxisChanged(
            object sender,
            MouseStateChangedEventArgs e)
        {
            var playerTank = Player.AssignedEntity;

            if (playerTank != null)
            {
                var pos = Player.AssignedEntity.Position.CollisionRectangle.Center - e.State.Position;
                pos = GameData.Map.CalculateViewportCoordinates(pos.ToVector2(), GameData.MapScale).ToPoint();

                var currentRotation = playerTank.TurretRotation;
                var rotation = Rotation.FromRadians((float)Math.Atan2(pos.Y, pos.X));

                playerTank.TurretRect = new GrayHorizons.ThirdParty.RotatedRectangle(
                    playerTank.Position.CollisionRectangle,
                    playerTank.Position.Rotation);
                playerTank.TurretRotation = rotation.OffsetBy(180);
            }

            if (e.State.LeftButton == ButtonState.Pressed)
            {
                playerTank.Shoot();
            }
        }

        public override void Execute()
        {

        }
    }
}

