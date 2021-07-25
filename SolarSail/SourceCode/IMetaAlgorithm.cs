﻿using System;
namespace SolarSail
{
    public enum Mode 
    {
        SkipParams,
        SaveResults
    }
    abstract public class IMetaAlgorithm
    {
        protected Random rand = new Random();

        public int Dim = 0;
        public int P = 0;

        protected int currentIteration;
        protected double const_r_tf = 5.8344 * Math.Pow(10, 10);
        protected double const_u_tf = 0;
        protected double const_v_tf = 4.79 * Math.Pow(10, 4);

        protected double lambda1 = Math.Pow(10, 5);
        protected double lambda3 = Math.Pow(10, 5);
        protected double lambda2 = Math.Pow(10, 5);

        protected void I(Agent agent)
        {
            double r_tf_Error = agent.r_tf - const_r_tf;
            double u_tf_Error = agent.u_tf - const_u_tf;
            double v_tf_Error = agent.v_tf - const_v_tf;
            agent.Fitness = ((agent.tf) / 86400) * lambda1 * Math.Pow(r_tf_Error, 2) + lambda2 * Math.Pow(u_tf_Error, 2) + lambda3 * Math.Pow(v_tf_Error, 2);
        }

        protected double bottomBorderSectionLength = 0;
        protected double topBorderSectionLength = 0;

        protected double bottomBorderFuncCoeff = 0;
        protected double topBorderFuncCoeff = 0;

        protected int populationNumber = 0;

        public abstract Agent CalculateResult(int populationNumber, double bottomBSL, double topBSL, double bottomBFC,double topBFC,int lambda1, int lambda2, int lambda3, int p,  params object[] list);

        public void CheckBorders(Agent agent) 
        {
            for (int i = 0; i < P; i++)
            {
                if (agent.Coords[i] < bottomBorderSectionLength)
                    agent.Coords[i] = bottomBorderSectionLength;
                else if (agent.Coords[i] > topBorderSectionLength)
                    agent.Coords[i] = topBorderSectionLength;
            }

            for (int i = P; i < Dim; i++)
            {
                if (agent.Coords[i] < bottomBorderFuncCoeff)
                    agent.Coords[i] = bottomBorderFuncCoeff;
                else if (agent.Coords[i] > topBorderFuncCoeff)
                    agent.Coords[i] = topBorderFuncCoeff;
            }
        }
    }
}
