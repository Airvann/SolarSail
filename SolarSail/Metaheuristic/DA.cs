using System;
using System.Collections.Generic;
using System.Linq;
using MetaheuristicHelper;
using OdeSolver;
using MathNet.Numerics;

namespace SolarSail.SourceCode
{
    public class DA : IMetaAlgorithm
    {
        private int maxIterationCount;

        private Agent best;
        private Agent worst;
        //разделение стрекоз в стае
        private double s;
        //выравнивание стрекоз в стае
        private double a;
        //сплоченность стрекоз в стае
        private double c;
        //стремление к лучшему решению
        private double f;
        //уклонение от лучшего решения
        private double e;
        //память о предыстории
        private double w;

        private Vector R         = new Vector();
        private Vector lb;
        private Vector ub;
        private Vector MaxDelta;
        private List<Agent> pool = new List<Agent>();
        private List<Vector> steps;

        public DA() { }

        public static Dictionary<string, object> AlgParams()
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("Максимальное число итераций", 100);
            par.Add("Размер популяции", 100);
            par.Add("Число разбиений", 3);
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
            best = new Agent(Dim);
            worst = new Agent(Dim);

            lb = new Vector(Dim);
            ub = new Vector(Dim);

            for (int i = 0; i < P; i++)
            {
                lb[i] = bottomBorderSectionLength;
                ub[i] = topBorderSectionLength;
            }
            for (int i = P; i < Dim; i++)
            {
                lb[i] = bottomBorderFuncCoeff;
                ub[i] = topBorderFuncCoeff;
            }

            MaxDelta = (ub - lb) / 10;

            FormingPopulation();
            SetZeros();
            currentIteration = 0;
            for (int k = 0; k <= maxIterationCount; k++)
            {
                UpdateParams(currentIteration);
                PopulationOrder();
                NewPackGeneration();
                currentIteration++;
#if DEBUG
                Report("Итерация " + k + " из " + maxIterationCount);
#endif
            }
            best = PoolBest();
            odeSolver.Solve(best, Mode.SaveResults);
        }

        private void NewPackGeneration()
        {
            R = ((ub - lb) / 4) + ((ub - lb) * ((double)currentIteration / maxIterationCount) * 2);

            for (int i = 0; i < populationNumber; i++)
            {
                List<Agent> neighbourhood = new List<Agent>();

                for (int j = 0; j < populationNumber; j++)
                {
                    if (i == j)
                        continue;
                    Vector dist = new Vector(Dim);

                    for (int k = 0; k < Dim; k++)
                        dist[k] = Math.Abs(individuals[i].Coords[k] - individuals[j].Coords[k]);

                    bool ok = true;
                    for (int k = 0; k < Dim; k++)
                        if (dist[k] > R[k])
                        {
                            ok = false;
                            break;
                        }    
                    if (ok == true)
                        neighbourhood.Add(individuals[j]);
                }
                if (neighbourhood.Count > 1)
                {
                    //Разделение
                    Vector S = new Vector(Dim);
                    for (int m = 0; m < neighbourhood.Count; m++)
                        S += -(neighbourhood[m].Coords - individuals[i].Coords);

                    //Выравнивание
                    Vector A = new Vector(Dim);
                    for (int m = 0; m < neighbourhood.Count; m++)
                        A += steps[m];
                    A /= neighbourhood.Count;

                    //Сплоченность
                    Vector C = new Vector(Dim);
                    for (int m = 0; m < neighbourhood.Count; m++)
                        C += neighbourhood[m].Coords - individuals[i].Coords;
                    C /= neighbourhood.Count;

                    Vector F = best.Coords - individuals[i].Coords;
                    Vector E = worst.Coords + individuals[i].Coords;

                    steps[i] = s * S + a * A + c * C + f * F + e * E + w * steps[i];

                    for (int k = 0; k < Dim; k++)
                    {
                        if (steps[i][k] > MaxDelta[k])
                            steps[i][k] = MaxDelta[k];

                        if (steps[i][k] < -MaxDelta[k])
                            steps[i][k] = -MaxDelta[k];
                    }

                    individuals[i].Coords += steps[i];
                }
                else
                {
                    double beta = 1.5f;
                    double sigma = Math.Pow(SpecialFunctions.Gamma(1 + beta) * Math.Sin(Math.PI * beta / 2f) / (SpecialFunctions.Gamma((1 + beta) / 2f) * beta * Math.Pow(2, (beta - 1) / 2f)), 1 / beta);
                    Vector Levy = new Vector(Dim);

                    for (int k = 0; k < Dim; k++)
                        Levy.vector[k] = 0.01 * rand.NextDouble() * 2 * sigma / Math.Pow(Math.Abs(rand.NextDouble() * 2), 1 / beta);

                    individuals[i].Coords = individuals[i].Coords + Levy * individuals[i].Coords;
                    SetZeros();
                }
                CheckBorders(individuals[i]);
                odeSolver.Solve(individuals[i]);
                I(individuals[i]);
            }
        }

        public override string PrintParams()
        {
            string param = "";
            param += base.PrintParams();
            param += "Число итераций = " + maxIterationCount + '\n';
            return param + "\n";
        }

        private void SetZeros()
        {
            steps = new List<Vector>();
            for (int i = 0; i < populationNumber; i++)
            {
                steps.Add(new Vector(Dim));
                for (int j = 0; j < Dim; j++)
                    steps[i][j] = 0;
            }
        }

        private Agent PoolBest()
        {
            pool = pool.OrderBy(s => s.Fitness).ToList();
            return pool[0];
        }

        private void UpdateParams(int k)
        {
            w = 0.9 - k * ((0.9 - 0.4) / maxIterationCount);
            double my_c = 0.1 - k * ((0.9 - 0.4) / (0.5 * maxIterationCount));
            if (my_c < 0)
                my_c = 0;
            s = 2 * rand.NextDouble() * my_c;
            a = 2 * rand.NextDouble() * my_c;
            c = 2 * rand.NextDouble() * my_c;
            f = 2 * rand.NextDouble();
            e = my_c;
        }

        private void PopulationOrder()
        {
            best = new Agent(Dim);
            worst = new Agent(Dim);

            individuals = individuals.OrderBy(s => s.Fitness).ToList();

            for (int i = 0; i < Dim; i++)
            {
                best.Coords[i] = individuals[0].Coords[i];
                worst.Coords[i] = individuals[populationNumber-1].Coords[i];
            }

            best.Fitness   = individuals[0].Fitness;
            worst.Fitness  = individuals[populationNumber - 1].Fitness;

            best.r_tf = individuals[0].r_tf;
            best.u_tf = individuals[0].u_tf;
            best.v_tf = individuals[0].v_tf;
            best.tf   = individuals[0].tf;

            worst.r_tf = individuals[populationNumber - 1].r_tf;
            worst.u_tf = individuals[populationNumber - 1].u_tf;
            worst.v_tf = individuals[populationNumber - 1].v_tf;
            worst.tf   = individuals[populationNumber - 1].tf;

            pool.Add(best);
#if DEBUG
           Console.WriteLine(best.Fitness.ToString() + '\n');
#endif
        }
    }
}
