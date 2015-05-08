using System;
using BattleCity.Logic;
using BattleCity.Attributes;

namespace BattleCity.StaticObjects
{
    [MappedTextures ("Explosion")]
    public class Explosion: StaticObject
    {
        int currentState;

        public int CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                if (value > MaximumState)
                    Destroy ();
                else
                    currentState = value;
            }
        }

        public int MaximumState { get; set; }

        public Explosion ()
        {
            MaximumState = 25;
            CurrentState = -1;
            HasCollision = false;
            IsInvincible = true;
        }

        public override void Update(TimeSpan gameTime)
        {
            CurrentState += 1;
        }
    }
}

