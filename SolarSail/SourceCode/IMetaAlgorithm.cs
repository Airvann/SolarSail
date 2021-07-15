using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSail
{
    abstract public class IMetaAlgorithm
    {
        protected double I(double tf, double r_tf, double u_tf, double v_tf)
        {
            double r_tf_Error = r_tf - const_r_tf;
            double u_tf_Error = u_tf - const_u_tf;
            double v_tf_Error = v_tf - const_v_tf;
            return -1;
            return (tf / 86400) * lambda1 * Math.Pow(r_tf_Error, 2) + lambda2 * Math.Pow(u_tf_Error, 2) + lambda3 * Math.Pow(v_tf_Error, 2);
        }

        protected double bottomBorderSectionLength;
        protected double topBorderSectionLength;

        protected double bottomBorderFuncCoeff;
        protected double topBorderFuncCoeff;

        protected int populationNumber = 0;
        public abstract Agent CalculateResult(int populationNumber, double bottomBSL, double topBSL, double bottomBFC,double topBFC, params object[] list);

        protected const double const_r_tf = 5.8344f * 10000000000;
        protected const double const_u_tf = 0;
        protected const double const_v_tf = 4.79f * 10000;

        protected double lambda1 = Math.Pow(10, 5);
        protected double lambda3 = Math.Pow(10, 5);
        protected double lambda2 = Math.Pow(10, 5);
        
        protected Random rand = new Random();

        public double Dim;

        protected int currentIteration;
        /// <summary>
        /// число разбиений времени
        /// </summary>
        protected int K;
        /// <summary>
        /// число коэффициентов в функции управления
        /// </summary>
        protected int P;

        protected double Tf(Agent agent)
        {
            double sum = 0;
            for (int i = 0; i < K; i++)
                sum += agent.Coords[i];
            return sum;
        }
    }
}
