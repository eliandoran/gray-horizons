using BattleCity.Logic;
using BattleCity.Entities;

namespace BattleCity.Logic
{
    /// <summary>
    /// Represents the artificial intelligence that controls the computer's tanks.
    /// </summary>
    public abstract class AIBase
    {
        /// <summary>
        /// Gets or sets the tank that is being controlled by the AI.
        /// </summary>
        /// <value>The controlling tank.</value>
        public Tank ControllingTank { get; set; }

        /// <summary>
        /// Gets or sets the map containing the tank.
        /// </summary>
        /// <value>The parent map.</value>
        public Map ParentMap { get; set; }

        /// <summary>
        /// It should be called when the game requires the AI to make the next step.
        /// </summary>
        public virtual void NextStep()
        {
			
        }
    }
}

