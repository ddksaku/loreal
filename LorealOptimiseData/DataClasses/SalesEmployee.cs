using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class SalesEmployee : IPrimaryKey, ITrackChanges, IDivision
    {

        public string FullName
        {
            get
            {
                if (ID == Guid.Empty)
                    return string.Empty;
                else
                    return string.Format("{0} ({1})", Name, EmployeeNumber);
            }
        }



    }
}
