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

        List<double> r_tmp = new List<double>();
        List<double> thetta_tmp = new List<double>();
        List<double> u_tmp = new List<double>();
        List<double> v_tmp = new List<double>();

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
       double F1(double r, double thetta, double u, double v, double alfa) 
       {
          return u;
       }
       
       double F2(double r, double thetta, double u, double v, double alfa)
       {
          return v / r;
       }
       
       double F3(double r, double thetta, double u, double v, double alfa)
       {
          return (v * v) / r - (mu / (r * r)) * (1 - beta * Math.Pow(Math.Cos(alfa), 3));
       }
       
       double F4(double r, double thetta, double u, double v, double alfa)
       {
          return -u * v / r + mu * beta * ((Math.Sin(alfa)*Math.Pow(Math.Cos(alfa),2)) / (r*r));
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
            List<double> c = agent.GetC();
            //Инициализация начальными условиями
            r.Add(r_0);
            thetta.Add(thetta_0);
            u.Add(u_0);
            v.Add(v_0);

            int P = agent.P;
            List<double> tau = new List<double>();

            double step = 1f / P;
            for (double m = 0; m <= 1; m += step)
                tau.Add(m);
            double h_step = (tau[1] - tau[0]) / 20f;
            double start;       double stop;
            double currStart_r_0 = r_0;         double currStart_thetta_0 = thetta_0;
            double currStart_u_0 = u_0;         double currStart_v_0 = v_0;

            List<double> tauPart = new List<double>();            
            
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

                //Формируем отрезок разибений, на котором происходит решение ДУ 
                for (double gap = start;  gap <= stop + 0.00001f; gap+=h_step)
                    tauPart.Add(gap);

                //Рунге-Кутта
                for (int i = 0; i < tauPart.Count - 1; ++i)
                {
                   double K1 = F1(r_tmp[i], thetta_tmp[i], u_tmp[i], v_tmp[i], Alfa(tauPart[i], c))          * P * h[k];
                   double L1 = F2(r_tmp[i], thetta_tmp[i], u_tmp[i], v_tmp[i], Alfa(tauPart[i], c))          * P * h[k];
                   double M1 = F3(r_tmp[i], thetta_tmp[i], u_tmp[i], v_tmp[i], Alfa(tauPart[i], c))          * P * h[k];
                   double N1 = F4(r_tmp[i], thetta_tmp[i], u_tmp[i], v_tmp[i], Alfa(tauPart[i], c))          * P * h[k];
                 
                   double K2 = F1(r_tmp[i] + (h_step / 2) * K1, thetta_tmp[i] + (h_step / 2) * L1, u_tmp[i] + (h_step / 2) * M1, v_tmp[i] + (h_step / 2) * N1, Alfa(tauPart[i], c))       * P * h[k];
                   double L2 = F2(r_tmp[i] + (h_step / 2) * K1, thetta_tmp[i] + (h_step / 2) * L1, u_tmp[i] + (h_step / 2) * M1, v_tmp[i] + (h_step / 2) * N1, Alfa(tauPart[i], c))       * P * h[k];
                   double M2 = F3(r_tmp[i] + (h_step / 2) * K1, thetta_tmp[i] + (h_step / 2) * L1, u_tmp[i] + (h_step / 2) * M1, v_tmp[i] + (h_step / 2) * N1, Alfa(tauPart[i], c))       * P * h[k];
                   double N2 = F4(r_tmp[i] + (h_step / 2) * K1, thetta_tmp[i] + (h_step / 2) * L1, u_tmp[i] + (h_step / 2) * M1, v_tmp[i] + (h_step / 2) * N1, Alfa(tauPart[i], c))       * P * h[k];

                   double K3 = F1(r_tmp[i] + (h_step / 2) * K2, thetta_tmp[i] + (h_step / 2) * L2, u_tmp[i] + (h_step / 2) * M2, v_tmp[i] + (h_step / 2) * N2, Alfa(tauPart[i], c))       * P * h[k];
                   double L3 = F2(r_tmp[i] + (h_step / 2) * K2, thetta_tmp[i] + (h_step / 2) * L2, u_tmp[i] + (h_step / 2) * M2, v_tmp[i] + (h_step / 2) * N2, Alfa(tauPart[i], c))       * P * h[k];
                   double M3 = F3(r_tmp[i] + (h_step / 2) * K2, thetta_tmp[i] + (h_step / 2) * L2, u_tmp[i] + (h_step / 2) * M2, v_tmp[i] + (h_step / 2) * N2, Alfa(tauPart[i], c))       * P * h[k];
                   double N3 = F4(r_tmp[i] + (h_step / 2) * K2, thetta_tmp[i] + (h_step / 2) * L2, u_tmp[i] + (h_step / 2) * M2, v_tmp[i] + (h_step / 2) * N2, Alfa(tauPart[i], c))       * P * h[k];

                   double K4 = F1(r_tmp[i] + (h_step / 2) * K3, thetta_tmp[i] + (h_step / 2) * L3, u_tmp[i] + (h_step / 2) * M3, v_tmp[i] + (h_step / 2) * N3, Alfa(tauPart[i], c))       * P * h[k];
                   double L4 = F2(r_tmp[i] + (h_step / 2) * K3, thetta_tmp[i] + (h_step / 2) * L3, u_tmp[i] + (h_step / 2) * M3, v_tmp[i] + (h_step / 2) * N3, Alfa(tauPart[i], c))       * P * h[k];
                   double M4 = F3(r_tmp[i] + (h_step / 2) * K3, thetta_tmp[i] + (h_step / 2) * L3, u_tmp[i] + (h_step / 2) * M3, v_tmp[i] + (h_step / 2) * N3, Alfa(tauPart[i], c))       * P * h[k];
                   double N4 = F4(r_tmp[i] + (h_step / 2) * K3, thetta_tmp[i] + (h_step / 2) * L3, u_tmp[i] + (h_step / 2) * M3, v_tmp[i] + (h_step / 2) * N3, Alfa(tauPart[i], c))       * P * h[k];
                 
                   double next_r        = r_tmp[i]      + (h_step / 6) * (K1 + 2 * K2 + 2 * K3 + K4);
                   double next_thetta   = thetta_tmp[i] + (h_step / 6) * (L1 + 2 * L2 + 2 * L3 + L4);
                   double next_v        = v_tmp[i]      + (h_step / 6) * (N1 + 2 * N2 + 2 * N3 + N4);
                   double next_u        = u_tmp[i]      + (h_step / 6) * (M1 + 2 * M2 + 2 * M3 + M4);

                   r_tmp.Add(next_r);
                   thetta_tmp.Add(next_thetta);
                   u_tmp.Add(next_u);
                   v_tmp.Add(next_v);
                }

                double sum = 0;
                for (int j = 0; j < k; j++)
                    sum += h[j];

                for (int i = 1; i < tauPart.Count; i++)
                {
                    alfa.Add(Alfa(tauPart[i], c));
                    t.Add(T_tau(sum, tauPart[i], h[k], P, k));
                    r.Add(r_tmp[i]);
                    thetta.Add(thetta_tmp[i]);
                    u.Add(u_tmp[i]);
                    v.Add(v_tmp[i]);  
                }
                currStart_r_0       = r[r.Count - 1];
                currStart_thetta_0  = thetta[thetta.Count - 1];
                currStart_u_0       = u[u.Count - 1];
                currStart_v_0       = v[v.Count - 1];
            }

            agent.SetTf();
            agent.r_tf = r[r.Count - 1];
            agent.u_tf = u[u.Count - 1];
            agent.v_tf = v[v.Count - 1];


            if (mode == Mode.SaveParams) 
            {
                Result res = Result.getInstance();
                res.resultTable.Add("t", t);
                res.resultTable.Add("alpha", alfa);
                res.resultTable.Add("r", r);
            }
        }

        double T_tau(double sum, double tau, double H_k_1, int P, int k) 
        {
            return sum + H_k_1 * P * (tau - (double)(k) / P);
        }
    } 
}
