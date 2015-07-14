using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using LorealOptimiseData.Enums;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class User : IPrimaryKey, ITrackChanges, IDeletionLimit
    {
        private ObservableCollection<UserRole> observableUserRoles;
        public ObservableCollection<UserRole> ObservableUserRoles
        {
            get
            {
                if (observableUserRoles == null)
                {
                    observableUserRoles = new ObservableCollection<UserRole>(UserRoles);
                }

                return observableUserRoles;
            }
        }

        #region IDeletionLimit Members

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = String.Empty;
            warning = String.Empty;

            if (UserRoles.Where(ur => ur.IDDivision != LoggedUser.GetInstance().Division.ID).Count() > 0 && !LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
            {
                reasonMsg = "Can not delete user from different division";
                return false;
            }

            return true;
        }

        #endregion
    }
}
