using System;
using System.Collections.Generic;
using System.Text;

namespace BoatDockingSys.Objects
{
    class SailBoat : Boat
    {
        public int Length { get; set; }

        public SailBoat(int length, string id, int weight, int maxSpeed, int daysInPort = 4, int size = 2)
        {
            Length = length;
            Id = id;
            Weight = weight;
            MaxSpeed = maxSpeed;
            DaysInPort = daysInPort;
            Size = size;
        }

        public override string GetUniqueProp()
        {
            return $"Length: {Length}";
        }
    }
}
