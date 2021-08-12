using System;
using System.Collections.Generic;

namespace MetaheuristicHelper
{
    public class AgentFrame
    {
        public readonly Agent agent;
        public readonly int sectionsCount;
        public readonly int splineCoeff;

        public AgentFrame(int sectionsCount, int splineCoeff, List<double> c, List<double> h) 
        {
            int dim = c.Count + h.Count;
            this.sectionsCount = sectionsCount;
            this.splineCoeff = splineCoeff;
            agent = new Agent(dim);

            for (int i = 0; i < agent.P; i++)
                agent.Coords[i] = h[i];
            for (int i = agent.P; i < dim; i++)
                agent.Coords[i] = c[i - agent.P];
        }
    }
}
