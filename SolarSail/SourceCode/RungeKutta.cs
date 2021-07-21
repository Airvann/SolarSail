using System;
using System.Collections.Generic;

namespace SolarSail.SourceCode
{
    public class RungeKutta
    {
        public RungeKutta(double bottomAlfaBorder, double topAlfaBorder) 
        {
            this.topAlfaBorder = topAlfaBorder;
            this.bottomAlfaBorder = bottomAlfaBorder;
        }

        List<double> t =      new List<double>();
        List<double> r =      new List<double>();
        List<double> thetta = new List<double>();
        List<double> u =      new List<double>();
        List<double> v =      new List<double>();
        List<double> alfa =   new List<double>();

        //Параметр базисной функции
        int p = 3;
        //Начальное условие
        double r_0 = 1.496f * Math.Pow(10, 11);
        double thetta_0 = 0;
        double u_0 = 0;
        double v_0 = 2.98 * Math.Pow(10,4);
        
        double mu = 1.327474512f * Math.Pow(10, 20);
        double beta = 0.042f;

        double topAlfaBorder;
        double bottomAlfaBorder;
      
       /*
       r'(t)=F1,
       O'(t)=F2,
       u'(t)=F3,
       v'(t)=F4.
       */
       double F1(double u) 
       {
          return u;
       }
       
       double F2(double r, double v)
       {
          return v / r;
       }
       
       double F3(double r, double v, double alfa)
       {
            double tmp = Math.Cos(alfa);
            return ((v * v) / r) - (mu / (r * r)) * (1 - beta * tmp * tmp * tmp);
       }
       
       double F4(double r, double u, double v, double alfa)
       {
            double tmp = Math.Cos(alfa);
            return ((-u * v) / r) + mu * beta * ((Math.Sin(alfa) * tmp * tmp) / (r * r));
       }

       double BasisFunction(double t)
       {//p = 3;
           if ((t >= -1) && (t <= -0.5))
               return 2 * 2 * (1 + t) * (1 + t) * (1 + t);
           else if ((t >= -0.5) && (t <= 0.5))
           {
               double tmp = Math.Abs(t);
               return 1 - 2 * 2 * tmp * tmp * tmp;
           }
           else if ((t >= 0.5) && (t <= 1))
               return 2 * 2 * (1 - t) * (1 - t) * (1 - t);
           else
               return 0;
           //if ((t >= -1) && (t <= -0.5))
           //    return Math.Pow(2, p - 1) * Math.Pow(1 + t, p);
           //else if ((t >= -0.5) && (t <= 0.5))
           //    return 1 - Math.Pow(2, p - 1) * Math.Pow(Math.Abs(t), p);
           //else if ((t >= 0.5) && (t <= 1))
           //    return Math.Pow(2, p - 1) * Math.Pow(1 - t, p);
           //else
           //    return 0;
        }

        double Alfa(double t, List<double> c) 
       {
           int P = c.Count;
           double res = 0;
           for (int i = 0; i < P; i++)
               res += c[i] * BasisFunction(t * P - i);
           if (res < bottomAlfaBorder)
               res = bottomAlfaBorder;
           else if (res > topAlfaBorder)
               res = topAlfaBorder;
           return res;
       }

        /// <summary>
        /// Метод Рунге-Кутта для решения задачи с изначально известным временем окончания работы
        /// </summary>
        /// <param name="F"> Делегат, описывающий правую часть ДУ </param>
        /// <param name="x0"> Начальное условие x(0)=x0 </param>
        /// <returns>Словарь x(t)</returns>
        public Dictionary<double, double> RungeKuttaCaculate(Func<double, double, double> F, double x0)
        {
            Dictionary<double, double> res = new Dictionary<double, double>();

            List<double> x = new List<double>();
            List<double> y = new List<double>();

            y.Add(x0);                          //Начальное условие X(0)=X0
            double h = 0.01;                    //Шаг разбиения в методе Рунге-Кутта
            double tfin = 10;                   //Момент окончания работы алгоритма
            for (double a = 0; a < tfin; a+=h)
                x.Add(a);

            //Метод Рунге-Кутта решения ДУ
            for (int i = 0; i < x.Count - 1; ++i)
            {
                double K1 = F(x[i], y[i]);
                double K2 = F(x[i] + h / 2, y[i] + (h / 2) * K1);
                double K3 = F(x[i] + h / 2, y[i] + (h / 2) * K2);
                double K4 = F(x[i] + h, y[i] + h * K3);
                double next = y[i] + (h / 6) * (K1 + 2 * K2 + 2 * K3 + K4);
                y.Add(next);
            }

            //Формируем словарь решений: [t - x(t)]
            for (int i = 0; i < x.Count; i++)
                res.Add(x[i], y[i]);
            return res;
        }

        public void RungeKuttaCaculate(Agent agent, Mode mode = Mode.SkipParams)
        {
            List<double> h = agent.GetH();
            double tf = 0;
            List<double> c = agent.GetC();
            List<double> t = new List<double>();
            for (int i = 0; i < h.Count; i++)
                tf += h[i];

            //Инициализация начальными условиями
            r.Add(r_0);
            thetta.Add(thetta_0);
            u.Add(u_0);
            v.Add(v_0);

            double h_step = 0.1;

            //Формируем отрезок разибений, на котором происходит решение ДУ 
            for (double gap = 0; gap <= tf + 0.00001f; gap += h_step)
                t.Add(gap);

            for (int i = 0; i < t.Count; i++)
            {
                double next_r           = r[i]      + F1(u[i]) * h_step;
                double next_thetta      = thetta[i] + F2(r[i], v[i]) * h_step;
                double next_v           = v[i]      + F3(r[i], v[i], Alfa(t[i], c)) * h_step;
                double next_u           = u[i]      + F4(r[i], u[i], v[i], Alfa(t[i], c)) * h_step;

                alfa.Add(Alfa(t[i], c));
                r.Add(next_r);
                thetta.Add(next_thetta);
                u.Add(next_u);
                v.Add(next_v);
            }

            agent.SetTf();
            agent.r_tf = r[r.Count - 1];
            agent.u_tf = u[u.Count - 1];
            agent.v_tf = v[v.Count - 1];


            if (mode == Mode.SaveParams) 
            {
                Result res = Result.getInstance();
                res.resultTable.Add("t",      t);
                res.resultTable.Add("alpha",  alfa);
                res.resultTable.Add("r",      r);
                res.resultTable.Add("thetta", thetta);
                res.resultTable.Add("u",      u);
                res.resultTable.Add("v",      v);
            }
        }

        double T_tau(double sum, double tau, double H_k_1, int P, int k) 
        {
            return sum + H_k_1 * P * (tau - (double)(k) / P);
        }
    } 
}
