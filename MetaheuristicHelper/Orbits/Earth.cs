using System;

namespace MetaheuristicHelper.Orbits
{
    public class Earth : Orbit
    {
        private static Earth instance;

        private Earth() { }

        public static Earth Get()
        {
            if (instance == null)
                instance = new Earth();
            return instance;
        }

        public override string GetName() { return "Земля"; }

        public override double GetR() { return 1; }
    }
}
