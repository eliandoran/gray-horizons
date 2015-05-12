using System;

namespace BattleCity.Attributes
{
    /// <summary>
    /// Indicates that a <see cref="BattleCity.Logic.GameAction"/> can be executed continously while the <see cref="BattleCity.Input.InputBinding"/> it is bound to is active.  
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class AllowContinousPressAttribute: Attribute
    {
        // No implementation needed.
    }
}

