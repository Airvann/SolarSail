using System.Collections.Generic;
using System;

namespace MetaheuristicHelper
{
    //Singletone class
    public class Result
    {
        public double brightnessSolarSail;
        public Orbit orbit;
        public double oDE_Solver_Step;
        public OdeSolver.OdeSolver OdeSolver;

        private Dictionary<string, List<double>> resultTable;

        public double tf;
        public double rf;
        public double uf;
        public double vf;
        public int sectionsCount;
        public int splineCoeff;

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
        public List<double> GetH() { return resultTable["h"]; }

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
            string selectedTargetOrbit;
            string selectedODESolver;

            selectedTargetOrbit = orbit.GetName();
            selectedODESolver = OdeSolver.GetName();

            string text = "";

            text += "Целевая орбита: орбита " + selectedTargetOrbit                             + '\n'; 
            text += "Метод решения системы ОДУ: "                        + selectedODESolver                               + '\n';
            text += "Параметр яркости солнечного паруса: "               + brightnessSolarSail.ToString()                  + '\n';
            text += "Время окончания движения: "                         + ConvertFromSecToDays(tf)                        + '\n';
            text += "Точность попадания по r: "                          + Math.Abs(rf - orbit.GetR()).ToString("0.00")        + '\n';
            text += "Точность попадания по u: "                          + Math.Abs(uf - orbit.GetU()).ToString("0.00")        + '\n';
            text += "Точность попадания по v: "                          + Math.Abs(vf - orbit.GetV()).ToString("0.00")        + '\n';
            text += "-----------------------------\n";
            text += "Коэффициенты управления: \n";

            foreach(var item in GetControl()) text += (item.ToString() + '\n');
            text += "-----------------------------\n\n";
            return text;
        }

        public static double ConvertFromSecToDays(double t)
        {
            return t / 60f / 60f / 24f;
        }
    }
}
