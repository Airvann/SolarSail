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

        private Dictionary<string, List<double>> resultTable;

        public double tf;
        public double rf;
        public double uf;
        public double vf;

        public double fitness;

        private static Result instance;
        private Result() 
        {
            resultTable = new Dictionary<string, List<double>>();
        }

        public static Result getInstance()
        {
            if (instance == null) 
                instance = new Result();
            return instance;
        }

        public List<double> GetT() { return resultTable["t"]; }
        public List<double> GetR() { return resultTable["r"]; }
        public List<double> GetTheta() { return resultTable["thetta"]; }
        public List<double> GetU() { return resultTable["u"]; }
        public List<double> GetV() { return resultTable["v"]; }
        public List<double> GetAlpha() { return resultTable["alpha"]; }
        public List<double> GetControl() { return resultTable["c"]; }
        public void Add(string id, List<double> data) { resultTable.Add(id, data); }

        public void Clear() 
        {
            resultTable = new Dictionary<string, List<double>>();
            tf = -1;
            uf = -1;
            vf = -1;
            rf = -1;
            fitness = -1;
        }

        public string PrintResult() 
        {
            string text = "";
            text += "Время окончания движения: "        + ConvertFromSecToDays(tf) + '\n';
            text += "Точность попадания по r: "         + Math.Abs(rf - const_rf).ToString("0.00")        + '\n';
            text += "Точность попадания по u: "         + Math.Abs(uf - const_uf).ToString("0.00")        + '\n';
            text += "Точность попадания по v: "         + Math.Abs(vf - const_vf).ToString("0.00")        + '\n';
            text += "-----------------------------\n";
            text += "Коэффициенты управления: \n";

            foreach(var item in GetControl()) text += (item.ToString() + '\n');
            text += "-----------------------------\n\n";
            return text;
        }

        public static double ConvertFromSecToDays(double t)
        {
            return (t / 60f / 60f / 24f);
        }
    }
}
