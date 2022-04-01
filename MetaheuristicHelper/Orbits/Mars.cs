namespace MetaheuristicHelper.Orbits
{
    public class Mars : Orbit
    {
        private static Mars instance;

        private Mars() { }

        public static Mars Get()
        {
            if (instance == null)
                instance = new Mars();
            return instance;
        }

        public override string GetName() { return "Марс"; }

        public override double GetR() { return 1.5235; }
    }
}
