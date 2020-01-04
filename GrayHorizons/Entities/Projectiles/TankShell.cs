namespace GrayHorizons.Entities.Projectiles
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Logic;

    [MappedTextures("Projectile")]
    public class TankShell: Projectile
    {
        public TankShell()
        {
            Damage = 1;
            Speed = 10;
        }
    }
}

