using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class UserRole : IPrimaryKey, IDivision, ITrackChanges, ICleanEntityRef
    {
        public void CleanEntityRef(string FieldName)
        {
            if (FieldName == "IDDivision")
            {
                this._Division = default(EntityRef<Division>);
            }
            else if (FieldName == "IDRole")
            {
                this._Role = default(EntityRef<Role>);
            }
        }

        public string RoleName
        {
            get
            {
                if (this.IDRole == Guid.Empty)
                    return "";
                return DbDataContext.GetInstance().Roles.SingleOrDefault(r => r.ID == this.IDRole).Name;

            }
        }

        public string DivisionName
        {
            get
            {
                if (this.IDDivision == Guid.Empty)
                    return "";
                return DbDataContext.GetInstance().Divisions.SingleOrDefault(d => d.ID == this.IDDivision).Name;

            }
        }
    }
}
