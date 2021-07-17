using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SolarSail.SourceCode
{
    public class Result
    {
        private static Result instance;
        private Result() { }
        public static Result getInstance() 
        {
            if (instance == null)
                instance = new Result();
            return instance;
        }

        public Dictionary<string, List<double>> resultTable;
        
        public double tf;
        public double r_tf;
        public double u_tf;
        public double v_tf;

        public double Tf 
        {
            set { tf = value > 0 ? tf = value : tf = 0; }
            get { return tf; }
        }

        public void Clear() 
        {
            resultTable = new Dictionary<string, List<double>>();
            tf = 0;
            r_tf = 0;
            u_tf = 0;
            v_tf = 0;
        }

    }
}
