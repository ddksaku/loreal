using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class Role : IPrimaryKey, ITrackChanges
    {
        public bool IsDivisionOrSystemAdmin()
        {
            return (this.Name == "Division Administrator" || this.Name == "System Administrator");
        }
    }
}
