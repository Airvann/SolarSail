using System.Collections.Generic;



namespace SolarSail.SourceCode
{
    //Singletone class
    public class Result
    {
        private static Result instance;
        private Result() 
        {
            resultTable = new Dictionary<string, List<double>>();
        }

        public Dictionary<string, List<double>> resultTable;

        public static Result getInstance()
        {
            if (instance == null) 
                instance = new Result();
            return instance;
        }

        public void Clear() 
        {
            resultTable = new Dictionary<string, List<double>>();
        }

    }
}
