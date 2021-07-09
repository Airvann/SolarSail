using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSail
{
    public abstract class IAgent 
    {
    }

    abstract public class IMetaAlgorithm
    {
        UInt64 populationNumber = 0;
        public abstract IAgent CalculateResult(UInt64 populationNumber, params object[] list);
    }
}
