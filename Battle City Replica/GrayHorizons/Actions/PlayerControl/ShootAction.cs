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
using GrayHorizons.Entities;
using Microsoft.Xna.Framework.Input;

namespace GrayHorizons.Actions.PlayerControl
{
    [DefaultKey(Keys.Space)]
    [DefaultMouseButton(MouseButtons.Left)]
    [AllowContinuousPress]
    public class ShootAction: GameAction
    {
        public ShootAction(
            Player player)
            : base(
                player)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.ShootAction"/> class.
        /// </summary>
        public ShootAction()
            : this(
                null)
        {
        }

        public override void Execute()
        {
            Player.AssignedEntity.Shoot();
        }
    }
}

