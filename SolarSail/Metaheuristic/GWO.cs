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
            par.Add("Максимальное число итераций", 100);
            par.Add("Размер популяции", 100);
            par.Add("Число разбиений отрезка времени", 30);
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
            K = (int)list[2];
            P = (int)list[3];
            Dim = K * P;

            this.populationNumber = populationNumber;

            alfa = new Agent(K, P);
            beta = new Agent(K, P);
            delta = new Agent(K, P);

            FormingPopulation();

            for (int k = 1; k < maxIterationCount; k++)
            {
                Selection();
                NewPackGeneration();
                currentIteration++;
            }
            return alfa;
        }

        private void FormingPopulation()
        {
            double nextRandomSectionLength;
            double nextRandomFuncCoeff;

            for (int i = 0; i < populationNumber; i++)
            {
                Agent agent = new Agent(K,P);
                for (int j = 0; j < K; j++)
                {
                    nextRandomSectionLength = (Math.Abs(bottomBorderSectionLength) + Math.Abs(topBorderSectionLength)) * rand.NextDouble() - Math.Abs(bottomBorderSectionLength);
                    agent.Coords[j] = nextRandomSectionLength;
                }
                for (int j = K; j < Dim; j++)
                {
                    nextRandomFuncCoeff = (Math.Abs(bottomBorderFuncCoeff) + Math.Abs(topBorderFuncCoeff)) * rand.NextDouble() - Math.Abs(bottomBorderFuncCoeff);
                    agent.Coords[j] = nextRandomFuncCoeff;
                }

                List<double> h = new List<double>();
                for (int j = 0; j < K; j++)
                    h.Add(agent.Coords[j]);

                List<double> a = new List<double>();
                for (int j = K; j < Dim; j++)
                    a.Add(agent.Coords[j]);

                RungeKutta rk = new RungeKutta();
                Dictionary<string, Dictionary<double, double>> res = rk.RungeKuttaCaculate(h, a);

                agent.Fitness = 0;          //TODO: добавить вычисление ДУ и функции приспособленности
                individuals.Add(agent);
            }
        }
        private void Selection()
        {
            individuals = individuals.OrderByDescending(s => s.Fitness).ToList();

            //Выбираем наиболее приспосоленных волков (сделано так, чтобы была передача значений, а не ссылки) 
               
            for (int i = 0; i < populationNumber; i++) 
            {
                alfa.Coords[i] = individuals[0].Coords[i];
                beta.Coords[i] = individuals[1].Coords[i];
                delta.Coords[i] = individuals[2].Coords[i];
            }
                
            alfa.Fitness = individuals[0].Fitness;     
            beta.Fitness = individuals[1].Fitness;   
            delta.Fitness = individuals[2].Fitness;
        }
        private void NewPackGeneration()
        {
            double a;
            //Выбор функции изменения параметра а
            if (param == Params.Quadratic)
                a = 2 * (1 - ((currentIteration * currentIteration) / ((double)maxIterationCount * maxIterationCount)));
            else
                a = 2 * (1 - currentIteration / (double)(maxIterationCount));

            Vector A_alfa = new Vector(K);            Vector C_alfa = new Vector(K);          Vector D_alfa = new Vector(K); 
                        
            Vector A_beta = new Vector(K);            Vector C_beta = new Vector(K);          Vector D_beta = new Vector(K);

            Vector A_delta = new Vector(K);           Vector C_delta = new Vector(K);         Vector D_delta = new Vector(K);

            for (int k = 0; k < populationNumber; k++)
            {
                for (int m = 0; m < Dim; m++)
                {
                    A_alfa[m] = 2 * a * rand.NextDouble() - a;
                    A_beta[m] = 2 * a * rand.NextDouble() - a;
                    A_delta[m] = 2 * a * rand.NextDouble() - a;

                    C_alfa[m] = 2 * rand.NextDouble();
                    C_beta[m] = 2 * rand.NextDouble();
                    C_delta[m] = 2 * rand.NextDouble();
                }

                D_alfa = Vector.Abs(C_alfa * alfa.Coords - individuals[k].Coords);
                D_beta = Vector.Abs(C_beta * beta.Coords - individuals[k].Coords);
                D_delta = Vector.Abs(C_beta * delta.Coords - individuals[k].Coords);


                individuals[k].Coords = ((alfa.Coords - D_alfa * A_alfa) +
                                                (beta.Coords - D_beta * A_beta) +
                                                (delta.Coords - D_delta * A_delta)) / 3.0;

                for (int i = 0; i < K; i++)
                {
                    if (individuals[k].Coords[i] < bottomBorderSectionLength)
                        individuals[k].Coords[i] = bottomBorderSectionLength;
                    else if (individuals[k].Coords[i] > topBorderSectionLength)
                        individuals[k].Coords[i] = topBorderSectionLength;
                }

                for (int i = K; i < Dim; i++)
                {
                    if (individuals[k].Coords[i] < bottomBorderFuncCoeff)
                        individuals[k].Coords[i] = bottomBorderFuncCoeff;          
                    else if (individuals[k].Coords[i] > topBorderFuncCoeff)
                        individuals[k].Coords[i] = topBorderFuncCoeff;
                }
                individuals[k].Fitness = 0; //TODO: change it! (I());
            }
        }
    }
}
