using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealReports.Reports.ValueConverters;

namespace LorealReports.DataAccess
{
    public partial class ItemGroup : IEntity
    {
        public string StringId
        {
            get { return ID.ToString(); }
        }
    }
}
