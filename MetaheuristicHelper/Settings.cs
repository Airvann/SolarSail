
using System;

namespace MetaheuristicHelper
{
    public class Settings
    {
        private Settings() { }
        private static Settings Instance { get; set; }
        public static Settings Get()
        {
            if (Instance == null)
                Instance = new Settings();
            return Instance;
        }

        public double              brightness;
        public Orbit               orbit;
        public double              odeSolverStep;
        public OdeSolver.OdeSolver odeSolver;
        public int                 sectionsCount;
        public int                 splineCoeff;
        public double              bottomBorderSection;
        public double              topBorderSection;

        public double              bottomBorderFunc;
        public double              topBorderFunc;

        public long                lambda1;
        public long                lambda2;
        public long                lambda3;
        public long                lambda4;

        public void Clear() 
        {
            brightness      = -1;
            odeSolverStep   = -1;
            splineCoeff     = -1;
            sectionsCount   = -1;
        }

        public string PrintSettings()
        {
            string text = "===========================\nПараметры:\n\n";

            text += "Целевая орбита: "                     + orbit.GetName()        + '\n';
            text += "Метод решения системы ОДУ: "          + odeSolver.GetName()    + '\n';
            text += "Параметр яркости солнечного паруса: " + brightness             + '\n';
            text += "Шаг интегрирования: "                 + odeSolverStep          + '\n';
            text += "Число разбиений отрезка времени: "    + sectionsCount          + '\n';
            text += "Параметр сплайна: "                   + splineCoeff            + '\n';
            text += "Нижняя грань управления: "            + bottomBorderFunc       + '\n';
            text += "Верхняя грань управления: "           + topBorderFunc          + '\n';
            text += "лямбда 1: "                           + lambda1                + '\n';
            text += "лямбда 2: "                           + lambda2                + '\n';
            text += "лямбда 3: "                           + lambda3                + '\n';
            text += "лямбда 4: "                           + lambda4                + '\n';
            text += "-----------------------------\n";

            return text; 
        }
    }
}
