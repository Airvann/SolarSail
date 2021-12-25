using System.Collections.Generic;
using System;
using MetaheuristicHelper;
using OdeSolver;

namespace SolarSail.SourceCode
{
    public class SA : IMetaAlgorithm
    {
        private int maxIterationCount;
        Agent agent;
        public override void CalculateResult(int populationNumber, double bottomBSL, double topBSL, double bottomBFC, double topBFC, long lambda1, long lambda2, long lambda3, long lambda4, int p, int P, params object[] list)
        {
            bottomBorderSectionLength = bottomBSL * 1000;
            topBorderSectionLength = topBSL * 1000;
            bottomBorderFuncCoeff = bottomBFC;
            topBorderFuncCoeff = topBFC;
            this.lambda1 = lambda1;
            this.lambda2 = lambda2;
            this.lambda3 = lambda3;
            this.lambda4 = lambda4;
            this.p = p;
            this.P = P;
            maxIterationCount = (int)list[0];
            Dim = 2 * P + 1;

            this.populationNumber = populationNumber;
#if DEBUG
            Report("Начало работы алгоритма");
            Console.WriteLine("-------------------------------------");
#endif
            solver = new OdeSolver.OdeSolver(p, P);
            agent = new Agent(Dim);

            double T = (double)list[1];
            double C = (double)list[2]; ; //>0
            double beta = (double)list[3]; //0.8; 0.99

            GenerateStartPoint();
            for (int i = 0; i < maxIterationCount; i++)
            {
                Agent newPoint = new Agent(Dim);

                for (int j = 0; j < Dim; j++)
                    newPoint.Coords[j] = agent.Coords[j] + (2 * rand.NextDouble() - 1) * agent.Coords[j] / 10;

                solver.EulerMethod(newPoint);
                I(newPoint);

                double diff = newPoint.Fitness - agent.Fitness;

                if (diff < 0)
                {
                    agent = newPoint;
                }
                else
                {
                    if (rand.NextDouble() < Math.Exp(-diff / (C * T)))
                    {
                        agent = newPoint;
                    }
                }

                solver.EulerMethod(agent);
                I(agent);
                T *= beta;
            }
            solver.EulerMethod(agent, Mode.SaveResults);
            I(agent);
        }

        public static Dictionary<string, object> AlgParams()
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("Максимальное число итераций", 100);
            par.Add("Размер популяции", 100);
            par.Add("Число разбиений", 10);
            par.Add("Temp", 10);
            par.Add("C", 10);
            par.Add("бета", 10);
            return par;
        }

        private void GenerateStartPoint()
        {

            double nextRandomSectionLength;
            double nextRandomFuncCoeff;


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

            solver.EulerMethod(agent);
            I(agent);
        }
    }
}
