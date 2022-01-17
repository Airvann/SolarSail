
namespace MetaheuristicHelper.Orbits
{
    public class Venus : Orbit
    {
        private static Venus instance;

        private Venus() { }

        public static Venus Get()
        {
            if (instance == null)
                instance = new Venus();
            return instance;
        }

        public override string GetName() { return "Венера"; }

        public override double GetR() { return 0.72; }
    }
}
