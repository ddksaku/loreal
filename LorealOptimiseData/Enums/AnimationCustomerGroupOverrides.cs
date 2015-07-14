using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData.Enums
{
    [Flags]
    public enum AnimationCustomerGroupOverrides
    {
        None = 0,
        SapDespatchCode = 1,
        OnCounterDate = 2,
        PLVDeliveryDate = 4,
        PLVComponentDate = 8,
        StockDate = 16
    }
}
