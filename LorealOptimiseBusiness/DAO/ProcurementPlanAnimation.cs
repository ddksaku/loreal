using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseBusiness.DAO
{
    public class ProcurementPlanAnimation
    {
        public string RetailerType
        { get; set; }

        public DateTime OnCounterDate
        {
            get; set;
        }

        public DateTime DeliveryDeadline
        {
            get; set;
        }

        public DateTime ComponentDeadline
        { get; set; }

        public DateTime StockDeadline
        {
            get; set;
        }

        public int AllocationQuantity
        { get; set; }
    }
}
