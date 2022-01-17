using System.Collections.Generic;

namespace MetaheuristicHelper
{
    public class Agent
    {
        public int P = 0;
        public Agent(int dim)
        {
            P = dim / 2;
            Fitness = 0;
            Coords = new Vector(dim);
        }

        public List<double> GetH()
        {
            List<double> res = new List<double>(P);
            for (int i = 0; i < P; i++)
                res.Add(Coords[i]);
            return res;
        }

        public List<double> GetC()
        {
            List<double> res = new List<double>(P);
            for (int i = P; i < Coords.dim; i++)
                res.Add(Coords[i]);
            return res;
        }

        public void SetTf() 
        {
            double res = 0;
            for (int i = 0; i < P; i++)
                res += Coords[i];
            tf = res;
        }
        public double tf = 0;
        public double r_tf = 0;
        public double u_tf = 0;
        public double v_tf = 0;
        public double Fitness { set; get; }
        public Vector Coords { set; get; }
    }
}
