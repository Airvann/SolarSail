﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace SolarSail.SourceCode
{
    public class WOA : IMetaAlgorithm
    {
        private int maxIterationCount;
        private double b;
        private Params param;
        private List<Agent> individuals = new List<Agent>();
        private Agent best;

        public static Dictionary<string, object> PAR()
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("Максимальное число итераций",           100);
            par.Add("Размер популяции",                      100);
            par.Add("Параметр логарифмической спирали b",    2);
            par.Add("Число разбиений",                       10);
            return par;
        }

        public override Agent CalculateResult(int populationNumber, double bottomBSL, double topBSL, double bottomBFC, double topBFC, params object[] list) 
        {
            bottomBorderSectionLength = bottomBSL;
            topBorderSectionLength = topBSL;
            bottomBorderFuncCoeff = bottomBFC;
            topBorderFuncCoeff = topBFC;

            maxIterationCount = (int)list[0];
            param = (Params)list[1];
            P = (int)list[2];            
            b = (double)list[4];

            Dim = 2 * P;

            best = new Agent(2 * P);
            FormingPopulation();
            for (int k = 1; k < maxIterationCount; k++)
            {
                Selection();
                NewPackGeneration();
                currentIteration++;
            }
            Selection();
            return best;
        }

        private void FormingPopulation()
        {
            double nextRandomLength;
            double nextRandomFuncCoeff;

            for (int i = 0; i < populationNumber; i++)
            {
                Agent agent = new Agent(2*P);
                for (int j = 0; j < P; j++)
                {
                    nextRandomLength = (Math.Abs(bottomBorderSectionLength) + Math.Abs(topBorderSectionLength)) * rand.NextDouble() - Math.Abs(bottomBorderSectionLength);
                    agent.Coords[i] = nextRandomLength;
                }
                for (int j = P; j < Dim; j++)
                {
                    nextRandomFuncCoeff = (Math.Abs(bottomBorderFuncCoeff) + Math.Abs(topBorderFuncCoeff)) * rand.NextDouble() - Math.Abs(bottomBorderFuncCoeff);
                    agent.Coords[i] = nextRandomFuncCoeff;
                }
                //TODO: тут должно быть вычисление через Рунге-Кутта
                I(agent);          //TODO: сделать подсчет функции приспособленности
                individuals.Add(agent);
            }
        }

        private void Selection() 
        {
            individuals = individuals.OrderByDescending(s => s.Fitness).ToList();

            //Выбираем наиболее приспосоленных волков (сделано так, чтобы была передача значений, а не ссылки) 
            for (int i = 0; i < Dim; i++)
                best.Coords[i] = individuals[0].Coords[i];
            
            best.Fitness = individuals[0].Fitness;
        }

        private void NewPackGeneration() 
        {
            double a;
            //Выбор функции изменения параметра а
            if (param == Params.Quadratic)
                a = 2 * (1 - ((currentIteration * currentIteration) / ((double)maxIterationCount * maxIterationCount)));
            else
                a = 2 * (1 - currentIteration / (double)(maxIterationCount));

            Vector l = new Vector();
            Vector D = new Vector();
                    
            Vector A = new Vector(P);
            Vector C = new Vector(P);

            for (int k = 0; k < populationNumber; k++)
            {
                if (rand.NextDouble() < 0.5f)
                {
                    for (int i = 0; i < P; i++)
                    {
                        A[i] = 2 * a * rand.NextDouble() - a;
                        C[i] = 2 * rand.NextDouble();
                    }
                    for (int i = P; i < Dim; i++)
                    {
                        A[i] = 2 * a * rand.NextDouble() - a;
                        C[i] = 2 * rand.NextDouble();
                    }

                    bool lowerThan1 = true;           
                    for (int i = 0; i < Dim; i++)     
                    {                                 
                        if (Math.Abs(A[i]) >= 1)      
                        {                             
                            lowerThan1 = false;       
                            break;                    
                        }                             
                    }                                 

                    if (lowerThan1)
                    {
                        D = Vector.Abs(C * best.Coords - individuals[k].Coords);
                        individuals[k].Coords = best.Coords - D * A;
                    }
                    else
                    {
                        Agent WhaleRand = individuals[rand.Next(0, populationNumber - 1)];

                        D = Vector.Abs(C * WhaleRand.Coords - individuals[k].Coords);
                        individuals[k].Coords = WhaleRand.Coords - D * A;
                    }
                }
                else 
                {
                    Vector tmp = new Vector();

                    D = Vector.Abs(best.Coords - individuals[k].Coords);

                    for (int i = 0; i < P; i++)
                    {
                        l[i] = 2 * rand.NextDouble() - 1;
                        tmp[i] = Math.Cos(2 * Math.PI * l[i]) * Math.Exp(b * l[i]);
                    }
                    individuals[k].Coords = D * tmp + best.Coords;  //TODO:?
                }

                CheckBorders(individuals[k]);
                I(individuals[k]); //TODO: Рунге-Кутта
            }
        }
    }
}
