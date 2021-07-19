using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarSail.SourceCode
{
    public enum Params 
    {
        Linear,
        Quadratic
    }

    public class GWO : IMetaAlgorithm
    {
        private Params param;       //Параметр изменения функции a

        private int maxIterationCount;

        private Agent alfa;
        private Agent beta;
        private Agent delta;
        private List<Agent> individuals = new List<Agent>();

        public GWO() {}

        public static Dictionary<string, object> PAR()
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("Максимальное число итераций",          500);
            par.Add("Размер популяции",                     100);
            par.Add("Число разбиений",                       10);
            return par;
        }

        /// <summary>
        /// Выполнение алгоритма
        /// </summary>
        /// <param name="populationNumber">Размер популяции</param>
        /// <param name="list">PARAMS: MaxIteration, A_Param, K, P</param>
        /// <returns></returns>
        public override Agent CalculateResult(int populationNumber, double bottomBSL, double topBSL, double bottomBFC, double topBFC, params object[] list) 
        {
            bottomBorderSectionLength = bottomBSL;
            topBorderSectionLength = topBSL;
            bottomBorderFuncCoeff = bottomBFC;
            topBorderFuncCoeff = topBFC;

            maxIterationCount = (int)list[0];
            param = (Params)list[1];
            P = (int)list[2];
            Dim = 2 * P;

            this.populationNumber = populationNumber;

            alfa  = new Agent(2 * P);
            beta  = new Agent(2 * P);
            delta = new Agent(2 * P);

            FormingPopulation();

            for (int k = 1; k < maxIterationCount; k++)
            {
                Selection();
                NewPackGeneration();
                currentIteration++;
            }
            Selection();

            RungeKutta rk = new RungeKutta(bottomBorderFuncCoeff, topBorderFuncCoeff);
            rk.RungeKuttaCaculate(alfa, Mode.SaveParams);

            return alfa;
        }

        private void FormingPopulation()
        {
            double nextRandomSectionLength;
            double nextRandomFuncCoeff;

            for (int i = 0; i < populationNumber; i++)
            {
                Agent agent = new Agent(2 * P);
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

                CheckBorders(agent);
                RungeKutta rk = new RungeKutta(bottomBorderFuncCoeff, topBorderFuncCoeff);
                rk.RungeKuttaCaculate(agent);

                I(agent);
                individuals.Add(agent);
            }
        }

        private void Selection()
        {
            individuals = individuals.OrderByDescending(s => s.Fitness).ToList();

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
        }
        private void NewPackGeneration()
        {
            double a;
            //Выбор функции изменения параметра а
            if (param == Params.Quadratic)
                a = 2 * (1 - ((currentIteration * currentIteration) / ((double)maxIterationCount * maxIterationCount)));
            else
                a = 2 * (1 - currentIteration / (double)(maxIterationCount));

            Vector A_alfa = new Vector(Dim);            Vector C_alfa = new Vector(Dim);          Vector D_alfa = new Vector(Dim); 
            Vector A_beta = new Vector(Dim);            Vector C_beta = new Vector(Dim);          Vector D_beta = new Vector(Dim);
            Vector A_delta = new Vector(Dim);           Vector C_delta = new Vector(Dim);         Vector D_delta = new Vector(Dim);

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

                individuals[k].Coords = ((alfa.Coords - D_alfa * A_alfa) +
                                                (beta.Coords - D_beta * A_beta) +
                                                (delta.Coords - D_delta * A_delta)) / 3.0;

                CheckBorders(individuals[k]);
                RungeKutta rk = new RungeKutta(bottomBorderFuncCoeff, topBorderFuncCoeff);
                rk.RungeKuttaCaculate(individuals[k]);
                I(individuals[k]);
            }
        }
    }
}
