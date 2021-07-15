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
    public static class RungeKutta
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
        public static List<double> RungeKuttaCaculate(Func<double, double, double> F)
        {
            //r.Add(0, r_0);
            //thetta.Add(0, thetta_0);
            //u.Add(0, u_0);
            //v.Add(0, v_0);

            List<double> x = new List<double>();
            List<double> y = new List<double>();
            y.Add(0);   //Начальное условие
            double h = 0.2;
            double tfin = 5;
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
            return y;
        }
    } 
}
