using System;
using GrayHorizons.ThirdParty;

namespace GrayHorizons.Logic
{
    public class ControllableEntity: Entity
    {

        public Rotation TurretRotation { get; set; }

        public RotatedRectangle TurretRect { get; set; }

        public ControllableEntity ()
        {
        }

        public virtual void Use ()
        {
            
        }
    }
}

