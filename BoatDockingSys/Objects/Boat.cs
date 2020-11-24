using System;
using System.Collections.Generic;
using System.Text;

namespace BoatDockingSys.Objects
{
    abstract class Boat
    {
        public string Id { get; set; }
        public int Weight { get; set; }
        public int MaxSpeed { get; set; }
        public int DaysInPort { get; set; }
        public int Size { get; set; }

        public virtual string GetUniqueProp()
        {
            return null;
        }
    }
}
