using System;
using BattleCity.Entities;

namespace BattleCity.Logic
{
    public class Player
    {
        public Tank AssignedTank { get; set; }

        public int Score { get; set; }

        public Player (Tank assignedTank)
        {
            AssignedTank = assignedTank;
        }

        public Player () : this (null)
        {
            
        }
    }
}

