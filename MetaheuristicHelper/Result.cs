using System.Collections.Generic;
using System;

namespace MetaheuristicHelper
{
    public class Result
    {
        private Dictionary<string, List<double>> resultTable = new Dictionary<string, List<double>>();

        public double tf;
        public double rf;
        public double uf;
        public double vf;

        public double fitness;

        private static Result instance;
        private Result() { }

        public static Result Get()
        {
            if (instance == null) 
                instance = new Result();
            return instance;
        }

        public List<double> GetT()  {      return resultTable["t"];         }
        public List<double> GetR() {       return resultTable["r"];         }
        public List<double> GetTheta() {   return resultTable["thetta"];    }
        public List<double> GetU() {       return resultTable["u"];         }
        public List<double> GetAlpha() {   return resultTable["alpha"];     }
        public List<double> GetV() {       return resultTable["v"];         }
        public List<double> GetControl() { return resultTable["c"];         }
        public List<double> GetH() {       return resultTable["h"];         }

        public void Add(string id, List<double> data) { resultTable.Add(id, data); }

        public void Clear() 
        {
            resultTable = new Dictionary<string, List<double>>();
            tf          = -1;
            uf          = -1;
            vf          = -1;
            rf          = -1;
            fitness     = -1;
        }

        public string PrintResult()
        {
            string text = "Результаты: \n";

            text += "Время окончания движения: "           + tf.ToString("0.00")                                            + '\n';
            text += "Точность попадания по r:  "            + Math.Abs(rf - Settings.Get().orbit.GetR()).ToString("0.000")    + '\n';
            text += "Точность попадания по u:  "           + Math.Abs(uf - Settings.Get().orbit.GetU()).ToString("0.000")    + '\n';
            text += "Точность попадания по v:  "           + Math.Abs(vf - Settings.Get().orbit.GetV()).ToString("0.000")    + '\n';
            text += "-------------------------------\n";
            text += "Коэффициенты управления: \n";

            foreach(var item in GetControl()) text += (item.ToString("0.000") + '\n');
            text += "-------------------------------\n";
            text += "Отрезки разбиения времени: \n";
            foreach (var item in GetH())      text += (item.ToString() + '\n');
            text += "-------------------------------\n";
            return text;
        }
    }
}
