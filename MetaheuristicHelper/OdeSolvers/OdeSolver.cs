using System;
using System.Collections.Generic;
using MetaheuristicHelper;

namespace OdeSolver
{
    public enum Mode
    {
        SkipParams,
        SaveResults
    }

    public abstract class OdeSolver
    {
        public OdeSolver(int p, int P, double brightness, double odeStep)
        {
            this.p = p;
            this.P = P;

            this.odeStep = odeStep;
            beta = brightness;
        }

        protected readonly List<double> t           = new List<double>();
        protected readonly List<double> r           = new List<double>();
        protected readonly List<double> thetta      = new List<double>();
        protected readonly List<double> u           = new List<double>();
        protected readonly List<double> v           = new List<double>();
        protected readonly List<double> alfa        = new List<double>();
        
        protected readonly List<double> tau         = new List<double>();
        protected readonly List<double> tauPart     = new List<double>();
        
        protected readonly List<double> r_tmp       = new List<double>();
        protected readonly List<double> thetta_tmp  = new List<double>();
        protected readonly List<double> u_tmp       = new List<double>();
        protected readonly List<double> v_tmp       = new List<double>();

        protected List<double> c;
        protected List<double> h;

        protected readonly double odeStep = -1;

        //Параметр базисной функции
        protected readonly int p = 0;
        //Число разбиений отрезка времени
        protected readonly int P = 0;
        //Начальное условие        

        protected readonly double beta = 0.042;

        protected double F1(double u)
        {
            return u;
        }
        
        protected double F2(double r, double v)
        {
            return v / r;
        }

        protected double F3(double r, double v, double alfa)
        {
            double tmp = Math.Cos(alfa);
            return ((v * v) / r) + (beta * tmp * tmp * tmp - 1) / (r * r);
        }

        protected double F4(double r, double u, double v, double alfa)
        {
            double tmp = Math.Cos(alfa);
            return (-u * v / r) + (beta * Math.Sin(alfa) * tmp * tmp) / (r * r);
        }

        public abstract string GetName();

        public static OdeSolver ReturnOdeSolver(string name, int p, int P, double brightness, double odeStep)
        {
            OdeSolver solver;
            switch (name)
            {
                case "Метод Эйлера":
                    solver = new EulerMethod(p, P, brightness, odeStep);
                    break;
                case "Метод Рунге-Кутта 4-го порядка":
                    solver = new RungeKutta4Method(p, P, brightness, odeStep);
                    break;
                default:
                    solver = new EulerMethod(p, P, brightness, odeStep);
                    break;
            }
            return solver;
        }

        protected double BasisFunction(double t)
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

        protected double Alfa(double t, List<double> c)
        {
            int P = c.Count - 1;
            double res = 0;
            for (int i = 0; i <= P; i++)
                res += c[i] * BasisFunction(t * P - i);
            if (res < -Math.PI / 2)
                res = -Math.PI / 2;
            else if (res > Math.PI / 2)
                res = Math.PI / 2;
            return res;
        }

        public abstract void Solve(Agent agent, Mode mode = Mode.SkipParams);

        protected double T_tau(double sum, double tau, double H_k_1, int k)
        {
            return sum + H_k_1 * P * (tau - (double)(k) / P);
        }

        protected void ResetToDefault()
        {
            t.Clear();               t.Add(0);
            r.Clear();
            thetta.Clear();          thetta.Add(0);
            u.Clear();
            v.Clear();
            alfa.Clear();
            tau.Clear();
        }

        protected void SaveParams(Agent agent)
        {
            Result res = Result.Get();
            res.Add("t", t);
            res.Add("alpha", alfa);
            res.Add("r", r);
            res.Add("thetta", thetta);
            res.Add("u", u);
            res.Add("v", v);
            res.Add("c", c);
            res.Add("h", h);

            res.rf = agent.r_tf;
            res.uf = agent.u_tf;
            res.vf = agent.v_tf;
            res.tf = agent.tf;
            res.fitness = agent.Fitness;
        }
    }
}