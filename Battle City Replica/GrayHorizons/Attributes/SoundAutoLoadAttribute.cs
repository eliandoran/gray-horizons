namespace GrayHorizons.Attributes
{
    using System;

    /// <summary>
    /// Indicates that a <see cref="GrayHorizons.Sound.SoundEffect"/> should be automatically loaded by the <see cref="GrayHorizons.Loader"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SoundAutoLoadAttribute: Attribute
    {
        // No implementation needed.
    }
}

