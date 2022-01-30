using System;
using System.Collections.Generic;
using System.Linq;
using MetaheuristicHelper;
using OdeSolver;

namespace SolarSail.SourceCode
{
    public class WOA : IMetaAlgorithm
    {
        private int maxIterationCount;

        private double b;
        private Agent best;


        public WOA() {}
        public static Dictionary<string, object> AlgParams()
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("Максимальное число итераций",           100);
            par.Add("Размер популяции",                      100);
            par.Add("Число разбиений",                       3);
            par.Add("Параметр логарифмической спирали b",    2);
            return par;
        }

        /// <summary>
        /// Выполнение алгоритма
        /// </summary>
        /// <param name="populationNumber">Размер популяции</param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override void CalculateResult(params object[] list)
        {
            Settings set = Settings.Get();

            bottomBorderSectionLength = set.bottomBorderSection;
            topBorderSectionLength = set.topBorderSection;
            bottomBorderFuncCoeff = set.bottomBorderFunc;
            topBorderFuncCoeff = set.topBorderFunc;
            lambda1 = set.lambda1;
            lambda2 = set.lambda2;
            lambda3 = set.lambda3;
            lambda4 = set.lambda4;
            p = set.splineCoeff;
            P = set.sectionsCount;
            maxIterationCount = (int)list[0];
            Dim = 2 * P + 1;

            targetOrbit = set.orbit;
            brightness = set.brightness;
            stepSolver = set.odeSolverStep;
            odeSolver = set.odeSolver;

            populationNumber = (int)list[1];
            b                = (double)list[2];

#if DEBUG
            Report("Начало работы алгоритма");
            Console.WriteLine("-------------------------------------");
#endif
            best = new Agent(Dim);

            FormingPopulation();

            for (int k = 1; k < maxIterationCount; k++)
            {
                Selection();
                NewPackGeneration();
                currentIteration++;
#if DEBUG
                Report("Итерация " + k + " из " + maxIterationCount);
#endif
            }
            Selection();
            odeSolver.Solve(best, Mode.SaveResults);
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
                odeSolver.Solve(individuals[k]);
                I(individuals[k]);
            }
        }
    }
}