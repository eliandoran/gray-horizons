namespace GrayHorizons.Logic
{
    using System.Xml.Serialization;
    using System.ComponentModel;

    /// <summary>
    /// Represents information about the damage a projectile causes when it hits a specific kind of object.
    /// </summary>
    public class DamageInfo
    {
        /// <summary>
        /// Gets or sets the health penalty when the projectile hits an object.
        /// </summary>
        /// <value>The health penalty.</value>
        [XmlAttribute("healthPenalty"), DefaultValue(1)]
        public int HealthPenalty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the projectile passes through the object, without being destroyed.
        /// </summary>
        /// <value><c>true</c> if the projectile passes through; otherwise, <c>false</c>.</value>
        [XmlAttribute("passThrough"), DefaultValue(false)]
        public bool PassThrough { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.DamageInfo"/> class with specific parameters.
        /// </summary>
        /// <param name="healthPenalty">The health penalty of the object it hits.</param>
        /// <param name="passThrough">Whether the projectile should pass through.</param>
        public DamageInfo(int healthPenalty, bool passThrough)
        {
            HealthPenalty	= healthPenalty;
            PassThrough = passThrough;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.DamageInfo"/> class with 1 damage and no pass through.
        /// </summary>
        public DamageInfo()
            : this(1, false)
        {

        }
    }
}

