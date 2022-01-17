
namespace MetaheuristicHelper.Orbits
{
    public class Mercury : Orbit
    {
        private static Mercury instance;

        private Mercury() { }

        public static Mercury Get()
        {
            if (instance == null)
                instance = new Mercury();
            return instance;
        }

        public override string GetName() { return "Меркурий"; }

        public override double GetR() { return 0.39; }
    }
}