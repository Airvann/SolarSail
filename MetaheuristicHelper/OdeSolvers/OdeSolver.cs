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
        public OdeSolver(int p, int P, double brightness, double odeStep, Orbit targetOrbit)
        {
            this.p = p;
            this.P = P;

            this.odeStep = odeStep;
            beta = brightness;
            this.targetOrbit = targetOrbit;
        }

        protected readonly List<double> t = new List<double>();
        protected readonly List<double> r = new List<double>();
        protected readonly List<double> thetta = new List<double>();
        protected readonly List<double> u = new List<double>();
        protected readonly List<double> v = new List<double>();
        protected readonly List<double> alfa = new List<double>();
        
        protected readonly List<double> tau = new List<double>();
        protected readonly List<double> tauPart = new List<double>();
        
        protected readonly List<double> r_tmp = new List<double>();
        protected readonly List<double> thetta_tmp = new List<double>();
        protected readonly List<double> u_tmp = new List<double>();
        protected readonly List<double> v_tmp = new List<double>();

        protected readonly double odeStep = -1;
        protected readonly Orbit targetOrbit;

        //Параметр базисной функции
        protected readonly int p = 0;
        //Число разбиений отрезка времени
        protected readonly int P = 0;
        //Начальное условие        

        protected readonly double mu = 1.327474512 * Math.Pow(10, 20);
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
            return ((v * v) / r) + (beta * tmp * tmp * tmp - mu) / (r * r);
        }

        protected double F4(double r, double u, double v, double alfa)
        {
            double tmp = Math.Cos(alfa);
            return (-u * v / r) + (mu * beta * ((Math.Sin(alfa) * tmp * tmp)) / (r * r));
        }

        public abstract string GetName();

        public static OdeSolver ReturnOdeSolver(string name, int p, int P, double brightness, double odeStep, Orbit targetOrbit)
        {
            OdeSolver solver;
            switch (name)
            {
                case "Метод Эйлера":
                    solver = new EulerMethod(p, P, brightness, odeStep, targetOrbit);
                    break;
                case "Метод Рунге-Кутта 4-го порядка":
                    solver = new EulerMethod(p, P, brightness, odeStep, targetOrbit);
                    break;
                default:
                    solver = new EulerMethod(p, P, brightness, odeStep, targetOrbit);
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

/*
        public void RungeKuttaMethod(Agent agent, Mode mode = Mode.SkipParams) 
        {
            ResetToDefault();

            List<double> c = agent.GetC();
            List<double> h = agent.GetH();

            double step = 1f / P;
            for (double m = 0; m <= 1 + 0.00001f; m += step)
                tau.Add(m);

            double currStart_r_0 = r_0; double currStart_thetta_0 = thetta_0;
            double currStart_u_0 = u_0; double currStart_v_0 = v_0;
            double h_step = (tau[1] - tau[0]) / odeStep;
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
                    double K1 = F1(u_tmp[i]) * h[k] * P;
                    double L1 = F2(r_tmp[i], v_tmp[i]) * h[k] * P;
                    double M1 = F3(r_tmp[i], v_tmp[i], Alfa(tauPart[i], c)) * h[k] * P;
                    double N1 = F4(r_tmp[i], u_tmp[i], v_tmp[i], Alfa(tauPart[i], c)) * h[k] * P;

                    double K2 = F1(u_tmp[i] + 0.5 * h_step * M1) * h[k] * P;
                    double L2 = F2(r_tmp[i] + 0.5 * h_step * K1, v_tmp[i] + 0.5 * h_step * N1) * h[k] * P;
                    double M2 = F3(r_tmp[i] + 0.5 * h_step * K1, v_tmp[i] + 0.5 * h_step * N1, Alfa(tauPart[i], c)) * h[k] * P;
                    double N2 = F4(r_tmp[i] + 0.5 * h_step * K1, u_tmp[i] + 0.5 * h_step * M1, v_tmp[i] + 0.5 * h_step * N1, Alfa(tauPart[i], c)) * h[k] * P;

                    double K3 = F1(u_tmp[i] + 0.5 * h_step * M2) * h[k] * P;
                    double L3 = F2(r_tmp[i] + 0.5 * h_step * K2, v_tmp[i] + 0.5 * h_step * N2) * h[k] * P;
                    double M3 = F3(r_tmp[i] + 0.5 * h_step * K2, v_tmp[i] + 0.5 * h_step * N2, Alfa(tauPart[i], c)) * h[k] * P;
                    double N3 = F4(r_tmp[i] + 0.5 * h_step * K2, u_tmp[i] + 0.5 * h_step * M2, v_tmp[i] + 0.5 * h_step * N2, Alfa(tauPart[i], c)) * h[k] * P;

                    double K4 = F1(u_tmp[i] + h_step * M3) * h[k] * P;
                    double L4 = F2(r_tmp[i] + h_step * K3, v_tmp[i] + h_step * N3) * h[k] * P;
                    double M4 = F3(r_tmp[i] + h_step * K3, v_tmp[i] + h_step * N3, Alfa(tauPart[i], c)) * h[k] * P;
                    double N4 = F4(r_tmp[i] + h_step * K3, u_tmp[i] + h_step * M3, v_tmp[i] + h_step * N3, Alfa(tauPart[i], c)) * h[k] * P;

                    double next_r      = r_tmp[i]       + (h_step / 6f) * (K1 + 2 * K2 + 2 * K3 + K4);
                    double next_thetta = thetta_tmp[i]  + (h_step / 6f) * (L1 + 2 * L2 + 2 * L3 + L4);
                    double next_u      = u_tmp[i]       + (h_step / 6f) * (M1 + 2 * M2 + 2 * M3 + M4);
                    double next_v      = v_tmp[i]       + (h_step / 6f) * (N1 + 2 * N2 + 2 * N3 + N4);

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
                res.Add("t", t);
                res.Add("alpha", alfa);
                res.Add("r", r);
                res.Add("thetta", thetta);
                res.Add("u", u);
                res.Add("v", v);
                res.Add("c", c);
                res.Add("h", h);

                res.brightnessSolarSail = beta;
                res.oDE_Solver_Step = odeStep;
                res.oDE_Solver = odeSolver;
                res.targetOrbit = targetOrbit;

                res.sectionsCount = P;
                res.splineCoeff = p;
                res.rf = agent.r_tf;
                res.uf = agent.u_tf;
                res.vf = agent.v_tf;
                res.tf = agent.tf;
                res.fitness = agent.Fitness;
            }
        }
*/
        protected double T_tau(double sum, double tau, double H_k_1, int k)
        {
            return sum + H_k_1 * P * (tau - (double)(k) / P);
        }

        protected void ResetToDefault()
        {
            t.Clear(); t.Add(0);
            r.Clear(); r.Add(MetaheuristicHelper.Orbits.Earth.Get().GetR());
            thetta.Clear(); thetta.Add(0);
            u.Clear(); u.Add(MetaheuristicHelper.Orbits.Earth.Get().GetU());
            v.Clear(); v.Add(MetaheuristicHelper.Orbits.Earth.Get().GetV());
            alfa.Clear();
            tau.Clear();
        }
    }
}