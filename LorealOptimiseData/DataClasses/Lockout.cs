using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class Lockout
    {
        public string ScheduleRange
        {
            get
            {
                return string.Format("{0} - {1}", this.Start, this.End);
            }
        }
    }
}
