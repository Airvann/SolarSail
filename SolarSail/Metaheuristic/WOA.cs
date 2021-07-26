using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarSail.SourceCode
{
    public class WOA : IMetaAlgorithm
    {
        private int maxIterationCount;

        private double b;
        private Agent best;
        private List<Agent> individuals = new List<Agent>();

        public WOA() {}
        public static Dictionary<string, object> AlgParams()
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("Максимальное число итераций",           100);
            par.Add("Размер популяции",                      100);
            par.Add("Параметр логарифмической спирали b",    2);
            par.Add("Число разбиений",                       10);
            return par;
        }

        /// <summary>
        /// Выполнение алгоритма
        /// </summary>
        /// <param name="populationNumber">Размер популяции</param>
        /// <param name="list">PARAMS: MaxIteration, A_Param, K, P</param>
        /// <returns></returns>
        public override Agent CalculateResult(int populationNumber, double bottomBSL, double topBSL, double bottomBFC, double topBFC, int lambda1, int lambda2, int lambda3, int p, params object[] list)
        {
            bottomBorderSectionLength = bottomBSL * 1000;
            topBorderSectionLength = topBSL * 1000;
            bottomBorderFuncCoeff = bottomBFC;
            topBorderFuncCoeff = topBFC;
            this.lambda1 = lambda1;
            this.lambda2 = lambda2;
            this.lambda3 = lambda3;
            maxIterationCount = (int)list[0];
            P = (int)list[1];
            b = (int)list[2];
            Dim = 2 * P;

            this.populationNumber = populationNumber;

            solver = new ODESolver(p, P);
            best = new Agent(Dim);
                      
            FormingPopulation();

            for (int k = 1; k < maxIterationCount; k++)
            {
                Selection();
                NewPackGeneration();
                currentIteration++;
            }
            Selection();

            solver.EulerMethod(best, Mode.SaveResults);
            return best;
        }

        private void FormingPopulation()
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

                solver.EulerMethod(agent);

                I(agent);
                individuals.Add(agent);
            }
        }

        private void Selection() 
        {
            individuals = individuals.OrderBy(s => s.Fitness).ToList();

            //Выбираем наиболее приспосоленных волков (сделано так, чтобы была передача значений, а не ссылки) 
            for (int i = 0; i < Dim; i++)
                best.Coords[i] = individuals[0].Coords[i];
            
            best.Fitness = individuals[0].Fitness;

            best.r_tf = individuals[0].r_tf;
            best.u_tf = individuals[0].u_tf;
            best.v_tf = individuals[0].v_tf;
            best.tf   = individuals[0].tf;
        }

        private bool IsLowerThan1(Vector vec) 
        {
            for (int i = 0; i < Dim; i++)
                if (Math.Abs(vec[i]) >= 1)
                    return false;
            return true;
        }

        private void NewPackGeneration() 
        {
            double a = 2 * (1 - currentIteration / (double)(maxIterationCount));

            Vector l = new Vector(Dim);
            Vector D;
                    
            Vector A = new Vector(Dim);
            Vector C = new Vector(Dim);

            for (int k = 0; k < populationNumber; k++)
            {
                if (rand.NextDouble() < 0.5f)
                {
                    for (int i = 0; i < Dim; i++)
                    {
                        A[i] = 2 * a * rand.NextDouble() - a;
                        C[i] = 2 * rand.NextDouble();
                    }          

                    if (IsLowerThan1(A))
                    {
                        D = Vector.Abs(C * best.Coords - individuals[k].Coords);
                        individuals[k].Coords = best.Coords - (D * A);
                    }
                    else
                    {
                        Agent WhaleRand = individuals[rand.Next(0, populationNumber - 1)];

                        D = Vector.Abs(C * WhaleRand.Coords - individuals[k].Coords);
                        individuals[k].Coords = WhaleRand.Coords - (D * A);
                    }
                }
                else 
                {
                    Vector tmp = new Vector(Dim);
                    D = Vector.Abs(best.Coords - individuals[k].Coords);
                    for (int i = 0; i < Dim; i++)
                    {
                        l[i] = 2 * rand.NextDouble() - 1;
                        tmp[i] = Math.Cos(2 * Math.PI * l[i]) * Math.Exp(b * l[i]);
                    }
                    individuals[k].Coords = (D * tmp) + best.Coords;
                }

                CheckBorders(individuals[k]);
                solver.EulerMethod(individuals[k]);
                I(individuals[k]);
            }
        }
    }
}
