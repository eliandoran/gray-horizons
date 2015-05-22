using System;
using GrayHorizons.Logic;
using GrayHorizons.Entities;
using System.Diagnostics;

namespace GrayHorizons.Objectives
{
    public class BoardVehicleObjective: Objective
    {
        readonly Soldier passenger;
        readonly Vehicle vehicleToBoard;

        public Vehicle VehicleToBoard { get { return vehicleToBoard; } }

        public BoardVehicleObjective(Soldier passenger, Vehicle vehicleToBoard)
        {
            this.passenger = passenger;
            this.vehicleToBoard = vehicleToBoard;
        }

        public override void CheckCompletion()
        {
            IsCompleted = vehicleToBoard.Passengers.Contains(passenger);
        }
    }
}

