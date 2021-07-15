using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSail
{
    public class Agent
    {
        public Agent(int K, int P) 
        {
            Fitness = 0;
            Coords = new Vector(K * P);
        }
        public double Fitness { set; get; }
        public Vector Coords { set; get; }
    }
}
