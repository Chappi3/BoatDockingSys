using System;
using System.Collections.Generic;
using System.Text;

namespace BoatDockingSys.Objects
{
    class MotorBoat : Boat
    {
        public int HorsePower { get; set; }

        public MotorBoat(int horsePower, string id, int weight, int maxSpeed, int daysInPort = 3, int size = 1)
        {
            HorsePower = horsePower;
            Id = id;
            Weight = weight;
            MaxSpeed = maxSpeed;
            DaysInPort = daysInPort;
            Size = size;
        }

        public override string GetUniqueProp()
        {
            return $"Horsepower: {HorsePower}";
        }
    }
}
