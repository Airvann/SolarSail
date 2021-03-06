using System.Collections.Generic;
using System.Linq;
using System;
using MetaheuristicHelper;
using OdeSolver;

namespace SolarSail.SourceCode
{
    public class GWO : IMetaAlgorithm
    {
        private int maxIterationCount;

        private Agent alfa;
        private Agent beta;
        private Agent delta;

        public GWO() {}

        public static Dictionary<string, object> AlgParams()
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("Максимальное число итераций",          100);
            par.Add("Размер популяции",                     100);
            par.Add("Число разбиений",                        6);
            return par;
        }

        /// <summary>
        /// Выполнение алгоритма
        /// </summary>
        /// <param name="populationNumber">Размер популяции</param>
        /// <param name="list">PARAMS: MaxIteration, A_Param, K, P</param>
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
#if DEBUG
            Report("Начало работы алгоритма");
            Console.WriteLine("-------------------------------------");
#endif
            alfa  = new Agent(Dim);
            beta  = new Agent(Dim);
            delta = new Agent(Dim);

            FormingPopulation();

            for (int k = 1; k <= maxIterationCount; k++)
            {
                Selection();
                NewPackGeneration();
                currentIteration++;
#if DEBUG
                Report("Итерация " + k + " из " + maxIterationCount);
#endif
            }
            Selection();
            odeSolver.Solve(alfa, Mode.SaveResults);
        }     

        private void Selection()
        {
            individuals = individuals.OrderBy(s => s.Fitness).ToList();

            //Выбираем наиболее приспосоленных волков (сделано так, чтобы была передача значений, а не ссылки) 
            for (int i = 0; i < Dim; i++) 
            {
                alfa.Coords[i] = individuals[0].Coords[i];
                beta.Coords[i] = individuals[1].Coords[i];
                delta.Coords[i] = individuals[2].Coords[i];
            }

            alfa.Fitness  = individuals[0].Fitness;
            beta.Fitness  = individuals[1].Fitness;
            delta.Fitness = individuals[2].Fitness;

            alfa.r_tf = individuals[0].r_tf;        
            alfa.u_tf = individuals[0].u_tf;
            alfa.v_tf = individuals[0].v_tf;
            alfa.tf   = individuals[0].tf;

            beta.r_tf = individuals[1].r_tf;
            beta.u_tf = individuals[1].u_tf;
            beta.v_tf = individuals[1].v_tf;
            beta.tf   = individuals[1].tf;

            delta.r_tf = individuals[2].r_tf;
            delta.u_tf = individuals[2].u_tf;
            delta.v_tf = individuals[2].v_tf;
            delta.tf   = individuals[2].tf;

#if DEBUG
            Console.WriteLine(alfa.Fitness.ToString() + '\n');
#endif
        }
        private void NewPackGeneration()
        {
            double a = 2 * (1 - currentIteration / (double)(maxIterationCount));
            //Выбор функции изменения параметра а

            Vector A_alfa = new Vector(Dim);            Vector C_alfa = new Vector(Dim);          Vector D_alfa;
            Vector A_beta = new Vector(Dim);            Vector C_beta = new Vector(Dim);          Vector D_beta;
            Vector A_delta = new Vector(Dim);           Vector C_delta = new Vector(Dim);         Vector D_delta;

            for (int k = 0; k < populationNumber; k++)
            {
                for (int m = 0; m < Dim; m++)
                {
                    A_alfa[m]   = 2 * a * rand.NextDouble() - a;
                    A_beta[m]   = 2 * a * rand.NextDouble() - a;
                    A_delta[m]  = 2 * a * rand.NextDouble() - a;

                    C_alfa[m]   = 2 * rand.NextDouble();
                    C_beta[m]   = 2 * rand.NextDouble();
                    C_delta[m]  = 2 * rand.NextDouble();
                }

                D_alfa = Vector.Abs(C_alfa * alfa.Coords - individuals[k].Coords);
                D_beta = Vector.Abs(C_beta * beta.Coords - individuals[k].Coords);
                D_delta = Vector.Abs(C_beta * delta.Coords - individuals[k].Coords);

                individuals[k].Coords = ((alfa.Coords - (D_alfa * A_alfa)) +
                                         (beta.Coords - (D_beta * A_beta)) +
                                         (delta.Coords - (D_delta * A_delta))) / 3.0;
                CheckBorders(individuals[k]);
                odeSolver.Solve(individuals[k]);
                I(individuals[k]);
            }
        }

        public override string PrintParams() 
        {
            string param = "";
            param += base.PrintParams();
            param += "Число итераций = " + maxIterationCount + '\n';
            return param + "\n";
        }
    }
}