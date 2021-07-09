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

        private double bottomBorderSectionLength;
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
        /// <param name="list">PARAMS: MaxIteration, A_Param, bottomBorderSectionLength, topBorderControl, bottomBorderFuncCoeff, topBorderFuncCoeff, K, P</param>
        /// <returns></returns>
        public override Agent CalculateResult(int populationNumber, params object[] list) 
        {
            maxIterationCount =             (int)list[0];
            param =                         (Params)list[1];
            bottomBorderSectionLength =     (double)list[2];
            topBorderControl =              (double)list[3];
            bottomBorderFuncCoeff =         (double)list[4];
            topBorderFuncCoeff =            (double)list[5];
            K =                             (int)list[6];
            P =                             (int)list[7];
          
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
                    nextRandomControl = (Math.Abs(bottomBorderSectionLength) + Math.Abs(topBorderControl)) * rand.NextDouble() - Math.Abs(bottomBorderSectionLength);
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
            double a;
            //Выбор функции изменения параметра а
            if (param == Params.Quadratic)
                a = 2 * (1 - ((currentIteration * currentIteration) / ((double)maxIterationCount * maxIterationCount)));
            else
                a = 2 * (1 - currentIteration / (double)(maxIterationCount));

            Vector A_alfa_K = new Vector(K);            Vector C_alfa_K = new Vector(K);          Vector D_alfa_K = new Vector(K);
            Vector A_alfa_P = new Vector(P);            Vector C_alfa_P = new Vector(P);          Vector D_alfa_P = new Vector(P);  

            Vector A_beta_K = new Vector(K);            Vector C_beta_K = new Vector(K);          Vector D_beta_K = new Vector(K);
            Vector A_beta_P = new Vector(P);            Vector C_beta_P = new Vector(P);          Vector D_beta_P = new Vector(P);

            Vector A_delta_K = new Vector(K);           Vector C_delta_K = new Vector(K);         Vector D_delta_K = new Vector(K);
            Vector A_delta_P = new Vector(P);           Vector C_delta_P = new Vector(P);         Vector D_delta_P = new Vector(P);

            for (int k = 0; k < populationNumber; k++)
            {
                for (int m = 0; m < K; m++)
                {
                    A_alfa_K[m] = 2 * a * rand.NextDouble() - a;
                    A_beta_K[m] = 2 * a * rand.NextDouble() - a;
                    A_delta_K[m] = 2 * a * rand.NextDouble() - a;

                    C_alfa_K[m] = 2 * rand.NextDouble();
                    C_beta_K[m] = 2 * rand.NextDouble();
                    C_delta_K[m] = 2 * rand.NextDouble();
                }

                for (int m = 0; m < P; m++)
                {
                    A_alfa_P[m] = 2 * a * rand.NextDouble() - a;
                    A_beta_P[m] = 2 * a * rand.NextDouble() - a;
                    A_delta_P[m] = 2 * a * rand.NextDouble() - a;

                    C_alfa_P[m] = 2 * rand.NextDouble();
                    C_beta_P[m] = 2 * rand.NextDouble();
                    C_delta_P[m] = 2 * rand.NextDouble();
                }
                D_alfa_K = Vector.Abs(C_alfa_K * alfa.SectionLength - individuals[k].SectionLength);
                D_beta_K = Vector.Abs(C_beta_K * beta.SectionLength - individuals[k].SectionLength);
                D_delta_K = Vector.Abs(C_beta_K * delta.SectionLength - individuals[k].SectionLength);

                D_alfa_P = Vector.Abs(C_alfa_P * alfa.FuncCoeffs - individuals[k].FuncCoeffs);
                D_beta_P = Vector.Abs(C_beta_P * beta.FuncCoeffs - individuals[k].FuncCoeffs);
                D_delta_P = Vector.Abs(C_delta_P * delta.FuncCoeffs - individuals[k].FuncCoeffs);

                individuals[k].SectionLength = ((alfa.SectionLength - D_alfa_K * A_alfa_K) +
                                                (beta.SectionLength - D_beta_K * A_beta_K) +
                                                (delta.SectionLength - D_delta_K * A_delta_K)) / 3.0;

                individuals[k].FuncCoeffs = ((alfa.FuncCoeffs - D_alfa_P * A_alfa_P) +
                                (beta.FuncCoeffs - D_beta_P * A_beta_P) +
                                (delta.FuncCoeffs - D_delta_P * A_delta_P)) / 3.0;

                for (int i = 0; i < K; i++)
                {
                    if (individuals[k].SectionLength[i] < bottomBorderSectionLength)
                        individuals[k].SectionLength[i] = bottomBorderSectionLength;
                    else if(individuals[k].SectionLength[i] > topBorderControl)
                        individuals[k].SectionLength[i] = topBorderControl;

                    if (individuals[k].FuncCoeffs[i] < bottomBorderFuncCoeff)
                        individuals[k].FuncCoeffs[i] = bottomBorderFuncCoeff;
                    else if (individuals[k].FuncCoeffs[i] > topBorderFuncCoeff)
                        individuals[k].FuncCoeffs[i] = topBorderFuncCoeff;
                }
                individuals[k].Fitness = 0; //TODO: change it! (I());
            }
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
