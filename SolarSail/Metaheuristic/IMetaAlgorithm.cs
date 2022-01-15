using System;
using MetaheuristicHelper;
using OdeSolver;
namespace SolarSail
{
    abstract public class IMetaAlgorithm
    {
        protected Random rand = new Random();
        protected OdeSolver.OdeSolver solver;

        public int Dim = 0;
        public int P = 0;

        protected int currentIteration;
        protected int p;

        protected double lambda1 = Math.Pow(10, 5);
        protected double lambda2 = Math.Pow(10, 5);
        protected double lambda3 = Math.Pow(10, 5);
        protected double lambda4 = Math.Pow(10, 5);

        protected double brightness;
        protected TargetOrbit targetOrbit = TargetOrbit.Unknown;
        protected double stepSolver;
        protected ODE_Solver odeSolver = ODE_Solver.Unknown;

        protected void I(Agent agent)
        {
            double r_tf_Error = agent.r_tf - Result.const_rf;
            double u_tf_Error = agent.u_tf - Result.const_uf;
            double v_tf_Error = agent.v_tf - Result.const_vf;

            agent.Fitness = lambda1 * ((agent.tf) / 86400f) + lambda2 * Math.Pow(r_tf_Error, 2) + lambda3 * Math.Pow(u_tf_Error, 2) + lambda4 * Math.Pow(v_tf_Error, 2);
        }

        protected double bottomBorderSectionLength = 0;
        protected double topBorderSectionLength = 0;

        protected double bottomBorderFuncCoeff = 0;
        protected double topBorderFuncCoeff = 0;

        protected int populationNumber = 0;

        public abstract void CalculateResult(TargetOrbit orbit, double brightness, double odeStep, ODE_Solver odeSolver, int populationNumber,
            double bottomBSL, double topBSL, double bottomBFC, double topBFC, long lambda1, long lambda2, long lambda3, long lambda4, int p, int P, params object[] list);

        public void CheckBorders(Agent agent) 
        {
            for (int i = 0; i < P; i++)
            {
                if (agent.Coords[i] < bottomBorderSectionLength)
                    agent.Coords[i] = bottomBorderSectionLength;
                else if (agent.Coords[i] > topBorderSectionLength)
                    agent.Coords[i] = topBorderSectionLength;
            }

            for (int i = P; i < Dim; i++)
            {
                if (agent.Coords[i] < bottomBorderFuncCoeff)
                    agent.Coords[i] = bottomBorderFuncCoeff;
                else if (agent.Coords[i] > topBorderFuncCoeff)
                    agent.Coords[i] = topBorderFuncCoeff;
            }
        }

        protected void Report(string text) 
        {
            Console.WriteLine(DateTime.Now + ": " + text);
        }

        public virtual string PrintParams() 
        {
            string param = "";

            param += "=============================\n";
            param += DateTime.Now.ToString() + "\n";
            param += "=============================\n";
            param += "Размер популяции = " + populationNumber + "\n";
            param += "Число разбиений = " + P + "\n";
            param += "Параметр сплайна = " + p + "\n";

            param += "λ1 = " + lambda1 + "\n";
            param += "λ2 = " + lambda2 + "\n";
            param += "λ3 = " + lambda3 + "\n";
            param += "λ4 = " + lambda4 + "\n";

            param += "Нижняя граница управления = "  + bottomBorderFuncCoeff + "\n";
            param += "Верхняя граница управления = " + topBorderFuncCoeff + "\n";

            param += "Нижняя граница отрезка времени = "  + bottomBorderSectionLength/1000f + "\n";
            param += "Верхняя граница отрезка времени = " + topBorderSectionLength/1000f + "\n";
            return param;
        }
    }
}
