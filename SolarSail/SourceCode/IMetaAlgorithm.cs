using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSail
{
    abstract public class IMetaAlgorithm
    {
        protected int populationNumber = 0;
        public abstract Agent CalculateResult(int populationNumber, params object[] list);
    }
}
