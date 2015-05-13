﻿using GrayHorizons.Entities;
using GrayHorizons.Logic;

namespace GrayHorizons.AI
{
    /// <summary>
    /// Represents a computer artificial intelligence controlling the tanks that is similar to the original game, Battle City.
    /// </summary>
    public class VanillaAI: AIBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.AIs.VanillaAI"/> class.
        /// </summary>
        public VanillaAI ()
        {
			
        }

        /// <summary>
        /// It should be called when the game requires the AI to make the next step.
        /// </summary>
        public override void NextStep ()
        {			
            ControllingTank.Shoot (new Projectile ());
        }
    }
}

