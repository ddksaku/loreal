using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using LorealOptimiseBusiness;
using LorealOptimiseData;

namespace LorealOptimiseGui
{
    public class BaseUserControl : UserControl
    {
        private ListManager listManager;
        protected ListManager ListManager
        {
            get
            {
                if (listManager == null)
                {
                    listManager = new ListManager();
                }

                return listManager;
            }
        }

        public BaseUserControl()
        {
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                CheckLoggedUser();
            }
        }

        private void CheckLoggedUser()
        {
            if (LoggedUser.IsLogged == false)
            {
                UserManager userManager = UserManager.Instance;

                int divisionCount = 0;
                User loggedUser = userManager.Login(out divisionCount);

                if (divisionCount > 1)
                {
                    SelectDivision divisions = new SelectDivision();
                    divisions.ShowDialog();

                    if (LoggedUser.IsLogged == false)
                    {
                        System.Environment.Exit(0);
                    }
                }
                else
                {
                    LoggedUser.SetInstance(loggedUser, loggedUser.UserRoles[0].Division);
                }

            }
        }
    }
}
