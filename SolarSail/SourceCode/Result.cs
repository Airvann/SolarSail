using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SolarSail.SourceCode
{
    public class Result
    {
        static Result instance;
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
