using System.Collections.Generic;
using System.Linq;
using System;
using MetaheuristicHelper;
using OdeSolver;

namespace SolarSail.SourceCode
{
    public class GA : IMetaAlgorithm
    {
        private Agent best;
        private int maxIterationCount;
        private List<Agent> individuals = new List<Agent>();

        public GA() { }

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

            best = new Agent(Dim);

            this.populationNumber = populationNumber;
#if DEBUG
            Report("Начало работы алгоритма");
            Console.WriteLine("-------------------------------------");
#endif
            solver = new OdeSolver.OdeSolver(p, P);

            FormingPopulation();
            for (int k = 1; k <= maxIterationCount; k++)
            {
                SelectionCrossMutation();
                currentIteration++;
#if DEBUG
                Report("Итерация " + k + " из " + maxIterationCount);
#endif
            }

            Selection();
            solver.EulerMethod(best, Mode.SaveResults);
        }

        public static Dictionary<string, object> AlgParams()
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("Максимальное число итераций", 100);
            par.Add("Размер популяции", 100);
            par.Add("Число разбиений", 10);
            return par;
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
            best.tf = individuals[0].tf;
        }

        private void SelectionCrossMutation()
        {
            //individuals.OrderBy(s => s.Fitness).ToList();
            //Agent curr_best = new Agent(Dim);
            //curr_best = individuals[0];
            for (int k = 0; k < individuals.Count; k++)
            {
                //Панмиксия
                Agent parent1 = individuals[rand.Next(0, individuals.Count)];
                Agent parent2 = individuals[rand.Next(0, individuals.Count)];

                Agent child = new Agent(Dim);
                //плоский кроссовер
                for (int i = 0; i < Dim; i++)
                {
                    if (parent2.Coords[i] < parent1.Coords[i])
                        child.Coords[i] = parent2.Coords[i] + (parent1.Coords[i] - parent2.Coords[i]) * rand.NextDouble();
                    else
                        child.Coords[i] = parent1.Coords[i] + (parent2.Coords[i] - parent1.Coords[i]) * rand.NextDouble();
                }

                //Мутация
                var hMutation = rand.Next(0, Dim / 2 - 1);    //Мутация отрезка времени разбиения
                var cMutation = rand.Next(Dim / 2, Dim);    //Мутация управления

                child.Coords[hMutation] = bottomBorderSectionLength + (topBorderSectionLength - bottomBorderSectionLength) * rand.NextDouble();
                child.Coords[cMutation] = bottomBorderFuncCoeff + (topBorderFuncCoeff - bottomBorderFuncCoeff) * rand.NextDouble();
                 
                solver.EulerMethod(child);
                I(child);

                individuals = individuals.OrderBy(s => s.Fitness).ToList();
                individuals[individuals.Count - 1] = child;
            }
           // individuals[individuals.Count - 1] = curr_best;
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
    }
}
