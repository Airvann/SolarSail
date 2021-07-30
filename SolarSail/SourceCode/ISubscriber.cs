using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSail.SourceCode
{
    public interface ISubscriber
    {
        void Update(string text);
    }
}
