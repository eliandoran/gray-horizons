using System;
using BattleCity.Entities;

namespace BattleCity.Logic
{
    public class Player
    {
        public ControllableEntity AssignedEntity { get; set; }

        public int Score { get; set; }

        public Player (
            ControllableEntity assignedEntity)
        {
            AssignedEntity = assignedEntity;
        }

        public Player () : this (
                null)
        {
            
        }
    }
}

