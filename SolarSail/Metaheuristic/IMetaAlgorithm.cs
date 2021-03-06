using System;
using MetaheuristicHelper;
using System.Collections.Generic;
using OdeSolver;
namespace SolarSail
{
    abstract public class IMetaAlgorithm
    {
        protected Random rand = new Random();

        protected List<Agent> individuals = new List<Agent>();

        public int Dim = 0;
        public int P = 0;

        protected int currentIteration;
        protected int p;

        protected double lambda1 = 1;
        protected double lambda2 = 1;
        protected double lambda4 = 1;
        protected double lambda3 = 1;

        protected double brightness;
        protected Orbit targetOrbit;
        protected double stepSolver;
        protected OdeSolver.OdeSolver odeSolver;

        protected void I(Agent agent)
        {
            double r_tf_Error = agent.r_tf - targetOrbit.GetR();
            double u_tf_Error = agent.u_tf - targetOrbit.GetU();
            double v_tf_Error = agent.v_tf - targetOrbit.GetV();

            double fitness = lambda1 * agent.tf + lambda2 * Math.Pow(r_tf_Error, 2) + lambda3 * Math.Pow(u_tf_Error, 2) + lambda4 * Math.Pow(v_tf_Error, 2);

            if ((double.IsNaN(fitness) || double.IsPositiveInfinity(fitness) || double.IsNegativeInfinity(fitness))) 
                agent.Fitness = double.MaxValue;
            else
                agent.Fitness = fitness;
        }

        protected double bottomBorderSectionLength  = 0;
        protected double topBorderSectionLength     = 0;

        protected double bottomBorderFuncCoeff      = 0;
        protected double topBorderFuncCoeff         = 0;

        protected int populationNumber              = 0;

        public abstract void CalculateResult(params object[] list);

        protected void FormingPopulation()
        {
            double nextRandomSectionLength;
            double nextRandomFuncCoeff;

            for (int i = 0; i < populationNumber; i++)
            {
                Agent agent = new Agent(Dim);
                for (int j = 0; j < P; j++)
                {
                    nextRandomSectionLength = bottomBorderSectionLength + (topBorderSectionLength - bottomBorderSectionLength) * rand.NextDouble();
                    agent.Coords[j] = nextRandomSectionLength;
                }
                for (int j = P; j < Dim; j++)
                {
                    nextRandomFuncCoeff = bottomBorderFuncCoeff + (topBorderFuncCoeff - bottomBorderFuncCoeff) * rand.NextDouble();
                    agent.Coords[j] = nextRandomFuncCoeff;
                }

                odeSolver.Solve(agent);
                I(agent);
                individuals.Add(agent);
            }
        }

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
