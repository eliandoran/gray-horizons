﻿namespace GrayHorizons.Logic
{
    /// <summary>
    /// Represents the artificial intelligence that controls the computer's tanks.
    /// </summary>
    public abstract class AIBase
    {
        /// <summary>
        /// Gets or sets the <see cref="GrayHorizons.Logic.Entity"/> that is being controlled by the AI.
        /// </summary>
        /// <value>The entity that the AI is controlling.</value>
        public Entity ControllingEntity { get; set; }

        public GameData GameData { get; set; }

        /// <summary>
        /// It should be called when the game requires the AI to make the next step.
        /// </summary>
        public virtual void NextStep()
        {
            ControllingEntity.Move(Entity.MoveDirection.Forward, false);
        }
    }
}

