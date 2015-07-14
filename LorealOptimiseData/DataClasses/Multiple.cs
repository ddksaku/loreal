using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class Multiple : IPrimaryKey, ITrackChanges
    {
        public string ValueString
        {
            get
            {
                if (Value == 0)
                    return "";
                return Value.ToString();
            }
        }
    }
}
