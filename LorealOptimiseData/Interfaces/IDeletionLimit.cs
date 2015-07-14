using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public interface IDeletionLimit
    {
        bool CanBeDeleted(out string reasonMsg, out string warning);
    }
}
