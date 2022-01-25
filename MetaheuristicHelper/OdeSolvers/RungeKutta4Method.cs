using MetaheuristicHelper;

namespace OdeSolver
{
    public class RungeKutta4Method : OdeSolver
    {
        public RungeKutta4Method(int p, int P, double brightness, double odeStep) : base(p, P, brightness, odeStep) { }
        public override string GetName() { return "Метод Рунге-Кутта 4-го порядка"; }

        public override void Solve(Agent agent, Mode mode = Mode.SkipParams)
        {
            ResetToDefault();

            c = agent.GetC();
            h = agent.GetH();

            double step = 1f / P;
            for (double m = 0; m <= 1 + 0.00001f; m += step)
                tau.Add(m);

            double currStart_r_0 = MetaheuristicHelper.Orbits.Earth.Get().GetR();       double currStart_thetta_0 = 0;
            double currStart_u_0 = MetaheuristicHelper.Orbits.Earth.Get().GetU();       double currStart_v_0 = MetaheuristicHelper.Orbits.Earth.Get().GetV();
            double h_step = (tau[1] - tau[0]) / odeStep;
            double start;       double stop;

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
                    double K1 = F1(u_tmp[i])                                          * h[k] * P;
                    double L1 = F2(r_tmp[i], v_tmp[i])                                * h[k] * P;
                    double M1 = F3(r_tmp[i], v_tmp[i], Alfa(tauPart[i], c))           * h[k] * P;
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

                    double next_r        = r_tmp[i]         + (h_step / 6f) * (K1 + 2 * K2 + 2 * K3 + K4);
                    double next_thetta   = thetta_tmp[i]    + (h_step / 6f) * (L1 + 2 * L2 + 2 * L3 + L4);
                    double next_u        = u_tmp[i]         + (h_step / 6f) * (M1 + 2 * M2 + 2 * M3 + M4);
                    double next_v        = v_tmp[i]         + (h_step / 6f) * (N1 + 2 * N2 + 2 * N3 + N4);

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

            if (mode == Mode.SaveResults) { SaveParams(agent); }
        }
    }
}
