using System.Collections.Generic;
using System;

namespace SolarSail.SourceCode
{
    //Singletone class
    public class Result
    {
        public static double const_rf       = 5.8344 * Math.Pow(10, 10);
        public static double const_uf       = 0;
        public static double const_vf       = 4.79 * Math.Pow(10, 4);

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
