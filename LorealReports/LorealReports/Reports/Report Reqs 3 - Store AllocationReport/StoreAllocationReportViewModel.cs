using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports;
using LorealReports.Reports.Base;

namespace LorealReports.Reports
{
    public class StoreAllocationReportViewModel : BaseViewModel
    {
        public StoreAllocationReportViewModel(Guid divisionId)
            : base(divisionId)
        {
        }

        protected override IReport CreateReport()
        {
            throw new NotImplementedException();
        }
    }
}
