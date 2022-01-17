using System;
using System.Collections.Generic;
using MetaheuristicHelper;


namespace MetaheuristicHelper
{
    public class AgentFrame
    {
        public readonly Agent agent;
        public readonly int sectionsCount;
        public readonly int splineCoeff;
        public readonly double brightnessSolarSail;
        public readonly Orbit targetOrbit;
        public readonly OdeSolver.OdeSolver odeSolver;
        public readonly double oDE_Solver_Step;
        public AgentFrame(Orbit targetOrbit, OdeSolver.OdeSolver odeSolver, double oDE_Solver_Step,
                          double brightnessSolarSail, int sectionsCount, int splineCoeff, List<double> c, List<double> h) 
        {
            this.targetOrbit = targetOrbit;
            this.odeSolver = odeSolver;
            this.oDE_Solver_Step = oDE_Solver_Step;
            this.brightnessSolarSail = brightnessSolarSail;

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
