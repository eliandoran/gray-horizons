using System;

namespace GrayHorizons.Attributes
{
    /// <summary>
    /// Indicates that a <see cref="GrayHorizons.Logic.GameAction"/> can be executed continously while the <see cref="GrayHorizons.Input.InputBinding"/> it is bound to is active.  
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public sealed class AllowContinuousPressAttribute: Attribute
    {
        // No implementation needed.
    }
}

