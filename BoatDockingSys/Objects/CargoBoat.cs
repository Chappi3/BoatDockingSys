using System;
using System.Collections.Generic;
using System.Text;

namespace BoatDockingSys.Objects
{
    class CargoBoat : Boat
    {
        public int Containers { get; set; }

        public CargoBoat(int containers, string id, int weight, int maxSpeed, int daysInPort = 6, int size = 4)
        {
            Id = id;
            Weight = weight;
            MaxSpeed = maxSpeed;
            DaysInPort = daysInPort;
            Size = size;
            Containers = containers;
        }

        public override string GetUniqueProp()
        {
            return $"Containers: {Containers}";
        }
    }
}
