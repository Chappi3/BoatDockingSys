using System;
using System.Collections.Generic;
using System.Text;

namespace BoatDockingSys.Objects
{
    class RowBoat : Boat
    {
        public int MaxPassenger { get; set; }

        public RowBoat(int maxPassenger, string id, int weight, int maxSpeed, int daysInPort = 1, int size = 1)
        {
            MaxPassenger = maxPassenger;
            Id = id;
            Weight = weight;
            MaxSpeed = maxSpeed;
            DaysInPort = daysInPort;
            Size = size;
        }

        public override string GetUniqueProp()
        {
            return $"Max passengers: {MaxPassenger}";
        }
    }
}
