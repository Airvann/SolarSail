using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSail.SourceCode
{
    public static class RungeKutta
    {
        static int p = 3;
        static double r_0 = 1.496f * Math.Pow(10, 11);
        static double thetta_0 = 0;
        static double u_0 = 0;
        static double v_0 = 2.98 * Math.Pow(10,4);
        static double BasisFunction(double t)
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
    }
}
