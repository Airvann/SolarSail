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
        private const double const_r_tf = 5.8344f * 10000000000;
        private const double const_u_tf = 0;
        private const double const_v_tf = 4.79f * 10000;

        private double lambda1 = Math.Pow(10, 5);
        private double lambda2 = Math.Pow(10, 5);
        private double lambda3 = Math.Pow(10, 5);

        private Params param;       //Параметр изменения функции a
        private Random rand = new Random();

        private double bottomBorderControl;
        private double topBorderControl;

        private double bottomBorderFuncCoeff;
        private double topBorderFuncCoeff;

        private int maxIterationCount;

        private int currentIteration;
        /// <summary>
        /// число разбиений времени
        /// </summary>
        private int K;
        /// <summary>
        /// число коэффициентов в функции управления
        /// </summary>
        private int P;

        private Agent alfa;
        private Agent beta;
        private Agent delta;
        private List<Agent> individuals = new List<Agent>();

        private double I(double tf, double r_tf, double u_tf, double v_tf) 
        {
            double r_tf_Error = r_tf - const_r_tf;
            double u_tf_Error = u_tf - const_u_tf;
            double v_tf_Error = v_tf - const_v_tf;

            return (tf / 86400) * lambda1 * Math.Pow(r_tf_Error, 2) + lambda2 * Math.Pow(u_tf_Error, 2) + lambda3 * Math.Pow(v_tf_Error, 2);
        }

        public GWO() { }

        /// <summary>
        /// Выполнение алгоритма
        /// </summary>
        /// <param name="populationNumber">Размер популяции</param>
        /// <param name="list">PARAMS: MaxIteration, A_Param, bottomBorderControl, topBorderControl, bottomBorderFuncCoeff, topBorderFuncCoeff</param>
        /// <returns></returns>
        public override Agent CalculateResult(int populationNumber, params object[] list) 
        {
            maxIterationCount =             (int)list[0];
            param =                         (Params)list[1];
            bottomBorderControl =           (double)list[2];
            topBorderControl =              (double)list[3];
            bottomBorderFuncCoeff =         (double)list[4];
            topBorderFuncCoeff =            (double)list[5];
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
            double nextRandomControl;
            double nextRandomCoeff;

            for (int i = 0; i < populationNumber; i++)
            {
                Agent agent = new Agent(K,P);
                for (int j = 0; j < P; j++)
                {
                    nextRandomControl = (Math.Abs(bottomBorderControl) + Math.Abs(topBorderControl)) * rand.NextDouble() - Math.Abs(bottomBorderControl);
                    agent.SectionLength[i] = nextRandomControl;  
                }
                for (int j = 0; j < K; j++)
                {
                    nextRandomCoeff = (Math.Abs(bottomBorderFuncCoeff) + Math.Abs(topBorderFuncCoeff)) * rand.NextDouble() - Math.Abs(bottomBorderFuncCoeff);
                    agent.FuncCoeffs[i] = nextRandomCoeff;
                }
                agent.Fitness = 0;          //TODO: добавить вычисление ДУ и функции приспособленности
                individuals.Add(agent);
            }
        }
        private void Selection() 
        {
            individuals = individuals.OrderByDescending(s => s.Fitness).ToList();

            //Выбираем наиболее приспосоленных волков (сделано так, чтобы была передача значений, а не ссылки) 
            for (int i = 0; i < K; i++) 
            {
                alfa.SectionLength[i] = individuals[0].SectionLength[i];
                beta.SectionLength[i] = individuals[1].SectionLength[i];
                delta.SectionLength[i] = individuals[2].SectionLength[i];
            }
               
            for (int i = 0; i < P; i++) 
            {
                alfa.FuncCoeffs[i] = individuals[0].FuncCoeffs[i];
                beta.FuncCoeffs[i] = individuals[1].FuncCoeffs[i];
                delta.FuncCoeffs[i] = individuals[2].FuncCoeffs[i];
            }
                
            alfa.Fitness = individuals[0].Fitness;     
            beta.Fitness = individuals[1].Fitness;   
            delta.Fitness = individuals[2].Fitness;
        }

        private void NewPackGeneration()
        {
        
        }

        private double Tf(Agent agent) 
        {
            double sum = 0;
            for (int i = 0; i < K; i++)
                sum += agent.FuncCoeffs[i];
            return sum;
        }
    }
}
