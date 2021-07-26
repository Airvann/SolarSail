using System;
using System.Collections.Generic;

namespace SolarSail.SourceCode
{
    public class ODESolver
    {
        public ODESolver(int p, int P) 
        {
            this.p = p;
            this.P = P;
        }

        List<double> t =      new List<double>();
        List<double> r =      new List<double>();
        List<double> thetta = new List<double>();
        List<double> u =      new List<double>();
        List<double> v =      new List<double>();
        List<double> alfa =   new List<double>();

        List<double> tau =    new List<double>();
        List<double> tauPart = new List<double>();

        List<double> r_tmp =        new List<double>();
        List<double> thetta_tmp =   new List<double>();
        List<double> u_tmp =        new List<double>();
        List<double> v_tmp =        new List<double>();

        //Параметр базисной функции
        int p = 0;
        //Число разбиений отрезка времени
        int P = 0;
        //Начальное условие
        double r_0 = 1.49597870691 * Math.Pow(10, 11);
        double thetta_0 = 0;
        double u_0 = 0;
        double v_0 = 2.98 * Math.Pow(10,4);
        
        double mu = 1.327474512 * Math.Pow(10, 20);
        double beta = 0.042;
      
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
            return ((v * v) / r) - (mu / (r * r)) + (beta * tmp * tmp * tmp) / (r * r);
       }
       
       double F4(double r, double u, double v, double alfa)
       {
            double tmp = Math.Cos(alfa);
            return (-u * v / r) + (mu * beta * ((Math.Sin(alfa) * tmp * tmp))/ (r * r));
       }

       double BasisFunction(double t)
       {
           if ((t >= -1) && (t <= -0.5))
               return Math.Pow(2, p - 1) * Math.Pow(1 + t, p);
           else if ((t >= -0.5) && (t <= 0.5))
               return 1 - Math.Pow(2, p - 1) * Math.Pow(Math.Abs(t), p);
           else if ((t >= 0.5) && (t <= 1))
               return Math.Pow(2, p - 1) * Math.Pow(1 - t, p);
           else
               return 0;
        }

        double Alfa(double t, List<double> c) 
        {
           int P = c.Count - 1;
           double res = 0;
           for (int i = 0; i <= P; i++)
               res += c[i] * BasisFunction(t * P - i);
           if (res < -Math.PI/2)
               res = -Math.PI/2;
           else if (res > Math.PI / 2)
               res = Math.PI/2;
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

        public void EulerMethod(Agent agent, Mode mode = Mode.SkipParams)
        {
            ResetToDefault();

            List<double> c = agent.GetC();
            List<double> h = agent.GetH();
            
            double step = 1f / P;
            for (double m = 0; m <= 1 + 0.00001f; m += step)
                tau.Add(m);
            double currStart_r_0 = r_0;         double currStart_thetta_0 = thetta_0;
            double currStart_u_0 = u_0;         double currStart_v_0 = v_0;
            double h_step = (tau[1] - tau[0])/1000;
            double start; double stop;

            for (int k = 0; k < P; k++)
            {
                tauPart.Clear();
                r_tmp.Clear();
                thetta_tmp.Clear();
                u_tmp.Clear();
                v_tmp.Clear();

                start = tau[k];
                stop = tau[k + 1];

                r_tmp.Add(currStart_r_0);
                thetta_tmp.Add(currStart_thetta_0);
                u_tmp.Add(currStart_u_0);
                v_tmp.Add(currStart_v_0);

                for (double gap = start; gap <= stop + 0.00001f; gap += h_step)
                    tauPart.Add(gap);
                
                for (int i = 0; i < tauPart.Count; ++i)
                {
                    double next_r      = r_tmp[i]            + F1(u_tmp[i])                                                  * h_step  * P * h[k];
                    double next_thetta = thetta_tmp[i]       + F2(r_tmp[i], v_tmp[i])                                        * h_step  * P * h[k];
                    double next_u      = u_tmp[i]            + F3(r_tmp[i], v_tmp[i], Alfa(tauPart[i], c))                   * h_step  * P * h[k];
                    double next_v      = v_tmp[i]            + F4(r_tmp[i], u_tmp[i], v_tmp[i], Alfa(tauPart[i], c))         * h_step  * P * h[k];

                    thetta_tmp.Add(next_thetta);
                    r_tmp.Add(next_r);
                    u_tmp.Add(next_u);
                    v_tmp.Add(next_v);
                }

                double sum = 0;
                for (int j = 0; j < k; j++)
                    sum += h[j];

                for (int i = 1; i < tauPart.Count; i++)
                {
                    alfa.Add(Alfa(tauPart[i], c));
                    t.Add(T_tau(sum, tauPart[i], h[k], k));
                    r.Add(r_tmp[i]);
                    thetta.Add(thetta_tmp[i]);
                    u.Add(u_tmp[i]);
                    v.Add(v_tmp[i]);
                }

                currStart_r_0 = r[r.Count - 1];
                currStart_thetta_0 = thetta[thetta.Count - 1];
                currStart_u_0 = u[u.Count - 1];
                currStart_v_0 = v[v.Count - 1];
            }

            agent.SetTf();
            agent.r_tf = r[r.Count - 1];
            agent.u_tf = u[u.Count - 1];
            agent.v_tf = v[v.Count - 1];

            if (mode == Mode.SaveResults) 
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

        private double T_tau(double sum, double tau, double H_k_1, int k) 
        {
            return sum + H_k_1 * P * (tau - (double)(k) / P);
        }

        private void ResetToDefault() 
        {
            t.Clear();          t.Add(0);
            r.Clear();          r.Add(r_0);
            thetta.Clear();     thetta.Add(thetta_0);
            u.Clear();          u.Add(u_0);
            v.Clear();          v.Add(v_0);
            alfa.Clear();
            tau.Clear();
        }
    } 
}
