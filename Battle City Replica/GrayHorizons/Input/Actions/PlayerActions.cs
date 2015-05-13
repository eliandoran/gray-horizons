using System;
using GrayHorizons.Logic;
using GrayHorizons.Entities;
using System.Diagnostics;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;

namespace GrayHorizons.Input.Actions
{
    [AllowContinuousPress ()]
    public abstract class TankMovementAction: GameAction
    {
        /// <summary>
        /// Represents whether a tank is moving (forward or backward) or turning (left or right).
        /// </summary>
        protected enum MovementType
        {
            Moving,
            Turning
        }

        MovementType movementType;

        protected TankMovementAction (
            Player player,
            InputBinding parentInputBinding,
            MovementType movementType) : base (
                null,
                player,
                parentInputBinding)
        {
            this.movementType = movementType;

            var keyBinding = ParentInputBinding as KeyBinding;
            if (keyBinding != null)
            {
                keyBinding.KeyDown += ParentKeyBinding_KeyDown;
                keyBinding.KeyUp += ParentKeyBinding_KeyUp;
            }
        }

        void ParentKeyBinding_KeyUp (
            object sender,
            EventArgs e)
        {
            SetValue (false);
        }

        void ParentKeyBinding_KeyDown (
            object sender,
            EventArgs e)
        {
            SetValue (true);
        }

        void SetValue (
            bool value)
        {
            switch (movementType)
            {
                case MovementType.Moving:
                    Player.AssignedEntity.IsMoving = value;
                    break;

                case MovementType.Turning:
                    Player.AssignedEntity.IsTurning = value;
                    break;
            }
        }
    }


    [DefaultKey (Keys.W)]
    public class MoveForwardAction: TankMovementAction
    {
        public MoveForwardAction (
            Player player,
            InputBinding parentInputBinding) : base (
                player,
                parentInputBinding,
                MovementType.Moving) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.MoveForwardAction"/> class.
        /// </summary>
        public MoveForwardAction () : this (
                null,
                null) { }

        public override void Execute ()
        {
            Player.AssignedEntity.IsMoving = true;
            Player.AssignedEntity.Move (Entity.MoveDirection.Forward, 10, false);
        }
    }


    [DefaultKey (Keys.S)]
    public class MoveBackwardAction: TankMovementAction
    {
        public MoveBackwardAction (
            Player player,
            InputBinding parentInputBinding) : base (
                player,
                parentInputBinding,
                MovementType.Moving) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.MoveBackwardAction"/> class.
        /// </summary>
        public MoveBackwardAction () : this (
                null,
                null) { }

        public override void Execute ()
        {
            Player.AssignedEntity.IsMoving = true;
            Player.AssignedEntity.Move (Entity.MoveDirection.Backward, 10, false);
        }
    }


    [DefaultKey (Keys.A)]
    public class TurnLeftAction: TankMovementAction
    {
        public TurnLeftAction (
            Player player,
            InputBinding parentInputBinding) : base (
                player,
                parentInputBinding,
                MovementType.Turning) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.TurnLeftAction"/> class.
        /// </summary>
        public TurnLeftAction () : this (
                null,
                null) { }

        public override void Execute ()
        {
            Player.AssignedEntity.Turn (Entity.TurnDirection.Left);
        }
    }


    [DefaultKey (Keys.D)]
    public class TurnRightAction: TankMovementAction
    {
        public TurnRightAction (
            Player player,
            InputBinding parentInputBinding) : base (
                player,
                parentInputBinding,
                MovementType.Turning) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.TurnRightAction"/> class.
        /// </summary>
        public TurnRightAction () : this (
                null,
                null) { }

        public override void Execute ()
        {
            Player.AssignedEntity.Turn (Entity.TurnDirection.Right);
        }
    }


    [DefaultKey (Keys.Space)]
    [DefaultMouseButton (MouseButtons.Left)]
    public class ShootAction: GameAction
    {
        public ShootAction (
            Player player) : base (
                player) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.ShootAction"/> class.
        /// </summary>
        public ShootAction () : this (
                null) { }

        public override void Execute ()
        {
            var tank = Player.AssignedEntity as Tank;
            if (tank != null)
            {
                tank.Shoot (new Projectile ());
                Sound.TankSounds.Firing.Play ();
            }
        }
    }


    [DefaultKey (Keys.F)]
    public class MoveAction: GameAction
    {
        public MoveAction (
            Player player) : base (
                player) { }

        public MoveAction () : this (
                null) {
            
        }

        public override void Execute ()
        {
            if (Player.AssignedEntity != null)
            {
                Player.AssignedEntity.Use ();
            }
            base.Execute ();
        }
    }
}
