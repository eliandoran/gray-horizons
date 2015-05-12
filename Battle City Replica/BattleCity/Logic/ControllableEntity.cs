using System;
using BattleCity.ThirdParty;

namespace BattleCity.Logic
{
    public class ControllableEntity: Entity
    {

        public Rotation TurrelRotation { get; set; }

        public RotatedRectangle TurretRect { get; set; }

        public ControllableEntity ()
        {
        }

        public virtual void Use ()
        {
            
        }
    }
}

