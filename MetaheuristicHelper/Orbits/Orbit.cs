using System;

namespace MetaheuristicHelper
{
    public abstract class Orbit
    {
        public abstract double GetR();
        public double GetU() { return 0; }
        public double GetV() { return 1 / Math.Sqrt(GetR()); }
        public abstract string GetName();

        public static Orbit ReturnOrbit(string name)
        {
            Orbit orbit;
            switch(name)
            {
                case "Меркурий":
                    orbit = Orbits.Mercury.Get();
                    break;
                case "Венера":
                    orbit = Orbits.Venus.Get();
                    break;
                case "Марс":
                    orbit = Orbits.Mars.Get();
                    break;
                case "Земля":
                    orbit = Orbits.Earth.Get();
                    break;
                default:
                    orbit = Orbits.Mercury.Get();
                    break;
            }
            return orbit;
        }
    }
}
