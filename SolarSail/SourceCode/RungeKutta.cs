using System;
using System.Collections.Generic;

namespace SolarSail.SourceCode
{
    public enum Variables 
    { 
        R,
        Thetta,
        U,
        V
    }
    public class RungeKutta
    {
        //static int p = 3;
        //static double r_0 = 1.496f * Math.Pow(10, 11);
        //static double thetta_0 = 0;
        //static double u_0 = 0;
        //static double v_0 = 2.98 * Math.Pow(10,4);
        //
        //static double mu = 1.327474512f * Math.Pow(10, 20);
        //static double beta = 0.042f;
        //
        //static Dictionary<double, double> r = new Dictionary<double, double>();
        //static Dictionary<double, double> thetta = new Dictionary<double, double>();
        //static Dictionary<double, double> u = new Dictionary<double, double>();
        //static Dictionary<double, double> v = new Dictionary<double, double>();
        /*
         r'(t)=F1,
         O'(t)=F2,
         u'(t)=F3,
         v'(t)=F4.
         */
       // static double F1(double t) 
       // {
       //     if (u.TryGetValue(t, out double res))
       //         return res;
       //     else
       //         throw new Exception("Не вычислено значение в время: " + t);
       // }
       //
       // static double F2(double t)
       // {
       //     if (v.TryGetValue(t, out double res_v) && r.TryGetValue(t, out double res_r))
       //         return res_v/res_r;
       //     else
       //         throw new Exception("Не вычислено значение в время: " + t);
       // }
       //
       // static double F3(double t)
       // {
       //     if (v.TryGetValue(t, out double res_v) && r.TryGetValue(t, out double res_r)) 
       //     {
       //         double alfa_t = Alfa(t, 1, new Vector(), 2, 2);     //!
       //         return (Math.Pow(res_v, 2) / res_r) - (mu / Math.Pow(res_r, 2)) * (1 - beta * Math.Pow(Math.Cos(alfa_t), 3));
       //     }
       //     
       //     else
       //         throw new Exception("Не вычислено значение в время: " + t);
       // }
       //
       // static double F4(double t)
       // {
       //     if (v.TryGetValue(t, out double res_v) && r.TryGetValue(t, out double res_r) && u.TryGetValue(t, out double res_u))
       //     {
       //         double alfa_t = Alfa(t, 1, new Vector(), 2, 2);
       //         return -((res_u * res_v) / (res_r)) + mu*beta*(Math.Sin(alfa_t)*Math.Pow(Math.Cos(alfa_t), 2))/(Math.Pow(res_r,2));
       //     }
       //
       //     else
       //         throw new Exception("Не вычислено значение в время: " + t);
       // }

    //    static double BasisFunction(double t)
    //    {
    //        if ((t >= -1) && (t <= -0.5))
    //            return Math.Pow(2, p - 1) * Math.Pow(1 + t, p);
    //        else if ((t >= -0.5) && (t <= 0.5))
    //            return 1 - Math.Pow(2, p - 1) * Math.Pow(Math.Abs(t), p);
    //        else if ((t >= 0.5) && (t <= 1))
    //            return Math.Pow(2, p - 1) * Math.Pow(1 - t, p);
    //        else
    //            return 0;
    //    }

     //   static double Alfa(double t, int P, Vector c, double topBorder, double bottomBorder) 
     //   {
     //       double res = 0;
     //       for (int i = 0; i < P; i++)
     //       {
     //           res += c[i] * BasisFunction(t * P - i);
     //       }
     //       if (res < bottomBorder)
     //           res = bottomBorder;
     //       if (res > topBorder)
     //           res = topBorder;
     //       return res;
     //   }

        //h -точность вычислений - отрезок разбиений
        public Dictionary<double, double> RungeKuttaCaculate(Func<double, double, double> F)
        {

            Dictionary<double, double> res = new Dictionary<double, double>();
            //r.Add(0, r_0);
            //thetta.Add(0, thetta_0);
            //u.Add(0, u_0);
            //v.Add(0, v_0);



            List<double> x = new List<double>();
            List<double> y = new List<double>();
            y.Add(1);   //Начальное условие
            double h = 0.2;
            double tfin = 2;
            //double t_fin = 1000;
            for (double a = 0; a < tfin; a+=h)
                x.Add(a);

            for (int i = 0; i < x.Count - 1; ++i)
            {
                double K1 = F(x[i], y[i]);
                double K2 = F(x[i] + h / 2, y[i] + (h / 2) * K1);
                double K3 = F(x[i] + h / 2, y[i] + (h / 2) * K2);
                double K4 = F(x[i] + h, y[i] + h * K3);
                double next = y[i] + (h / 6) * (K1 + 2 * K2 + 2 * K3 + K4);
                y.Add(next);
            }

            for (int i = 0; i < x.Count; i++)
            {
                res.Add(x[i], y[i]);
            }
            return res;
        }


        public Dictionary<double, double> RungeKuttaCaculate(List<double> h, Func<double, double, double> F)
        {
            Dictionary<double, double> res = new Dictionary<double, double>();

            res.Add(0, 1);  //!
            int P = h.Count;
            List<double> tau = new List<double>();
            double step = 1f / P;
            for (double m = 0; m <= 1; m += step)
                tau.Add(m);

            double start;       double stop;

            double curr = res[0];
            for (int k = 0; k < P; k++)
            {
                start = tau[k];
                stop = tau[k + 1];

                List<double> tau2 = new List<double>();
                List<double> x = new List<double>();

                x.Add(curr);

                double h_step = 0.1;
                for (double gap = start;  gap <= stop; gap+=h_step)
                {
                    tau2.Add(gap);
                }

                for (int i = 0; i < tau2.Count - 1; ++i)
                {
                    double K1 = UpdFunc(F(tau2[i], x[i]), P, h[k]);
                    double K2 = UpdFunc(F(tau2[i] + h_step / 2, x[i] + (h_step / 2) * K1), P, h[k]);
                    double K3 = UpdFunc(F(tau2[i] + h_step / 2, x[i] + (h_step / 2) * K2), P, h[k]);
                    double K4 = UpdFunc(F(tau2[i] + h_step, x[i] + h_step * K3), P, h[k]);
                    double next = x[i] + (h_step / 6) * (K1 + 2 * K2 + 2 * K3 + K4);
                    x.Add(next);
                }

                double sum = 0;
                for (int j = 0; j < k; j++)
                {
                    sum += h[j + 1];
                }

                for (int i = 1; i < tau2.Count; i++)
                {
                    double timeAdd = T_tau(sum, tau2[i], h[k], P, k);
                    res.Add(timeAdd, x[i]);

                    if (i == tau2.Count - 1)
                        curr = res[timeAdd];
                }
            }
            
            return res;
        }

        double T_tau(double sum, double tau, double H_k_1, int P, int k) 
        {
            return sum + H_k_1 * P * (tau - (double)(k) / P);
        }

        private double UpdFunc(double f, double P, double h_k_1) 
        {
            return f * P * h_k_1;
        }
    } 
}
