using System;
using BattleCity.Logic;
using BattleCity.Entities;
using System.Diagnostics;
using BattleCity.Attributes;
using Microsoft.Xna.Framework.Input;

namespace BattleCity.Input.Actions
{
    [AllowContinousPress ()]
    public abstract class TankMovementAction: GameAction
    {
        protected TankMovementAction (Player player,
                                      InputBinding parentInputBinding) : base (null,
                                                                               player,
                                                                               parentInputBinding)
        {
            var keyBinding = ParentInputBinding as KeyBinding;
            if (keyBinding != null)
            {
                keyBinding.KeyDown += ParentKeyBinding_KeyDown;
                keyBinding.KeyUp += ParentKeyBinding_KeyUp;
            }
        }

        void ParentKeyBinding_KeyUp(object sender,
                                    EventArgs e)
        {
            Debug.WriteLine ("KEY UP");
            Player.AssignedTank.IsMoving = false;
        }

        void ParentKeyBinding_KeyDown(object sender,
                                      EventArgs e)
        {
            Debug.WriteLine ("KEY DOWN");
            Player.AssignedTank.IsMoving = true;
        }
    }

    [DefaultKey (Keys.W)]
    public class MoveForwardAction: TankMovementAction
    {
        public MoveForwardAction (Player player,
                                  InputBinding parentInputBinding) : base (player,
                                                                           parentInputBinding) { }

        public MoveForwardAction () : this (null,
                                            null) { }

        public override void Execute()
        {
            Player.AssignedTank.IsMoving = true;
            Player.AssignedTank.Move (Entity.MoveDirection.Forward, 10, false);
        }
    }

    [DefaultKey (Keys.S)]
    public class MoveBackwardAction: TankMovementAction
    {
        public MoveBackwardAction (Player player,
                                   InputBinding parentInputBinding) : base (player,
                                                                            parentInputBinding) { }

        public MoveBackwardAction () : this (null,
                                             null) { }

        public override void Execute()
        {
            Player.AssignedTank.IsMoving = true;
            Player.AssignedTank.Move (Entity.MoveDirection.Backward, 10, false);
        }
    }

    [DefaultKey (Keys.A)]
    public class TurnLeftAction: TankMovementAction
    {
        public TurnLeftAction (Player player,
                               InputBinding parentInputBinding) : base (player,
                                                                        parentInputBinding) { }

        public TurnLeftAction () : this (null,
                                         null) { }

        public override void Execute()
        {
            Player.AssignedTank.IsMoving = true;
            Player.AssignedTank.Turn (Entity.TurnDirection.Left);
        }
    }

    [DefaultKey (Keys.D)]
    public class TurnRightAction: TankMovementAction
    {
        public TurnRightAction (Player player,
                                InputBinding parentInputBinding) : base (player,
                                                                         parentInputBinding) { }

        public TurnRightAction () : this (null,
                                          null) { }

        public override void Execute()
        {
            Player.AssignedTank.IsMoving = true;
            Player.AssignedTank.Turn (Entity.TurnDirection.Right);
        }
    }

    [DefaultKey (Keys.Space)]
    public class ShootAction: GameAction
    {
        public ShootAction (Player player) : base (player) {}

        public ShootAction () : this (null) { }

        public override void Execute()
        {
            Player.AssignedTank.Shoot (new Projectile ());
        }
    }
}
