using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LorealOptimiseData.Enums;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using System.Collections;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for ApplicationSettings.xaml
    /// </summary>
    public partial class ApplicationSettings : BaseListUserControl<ApplicationSettingManager, ApplicationSetting>
    {
        public ApplicationSettings()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                AssignEvents(grdApplicationSettings);       
            }
        }

      
        protected override Hashtable Filters
        {
            get 
            {
                Hashtable conditions = new Hashtable();

                if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
                {
                    conditions.Add(ApplicationSettingManager.IDDivision, Guid.Empty);
                }
                else if (LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
                {
                    conditions.Add(ApplicationSettingManager.IDDivision, LoggedUser.LoggedDivision.ID);
                }

                return conditions;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }


        private void btnInsertLockout_Click(object sender, RoutedEventArgs e)
        {
            Lockouts lockoutControl = new Lockouts();
            PopupWindow addSalesDialog = new PopupWindow("Insert a new lock out schedule");
            addSalesDialog.Width = 460;
            addSalesDialog.Height = 310;
            lockoutControl.Close += new EventHandler(addSalesDialog.CloseWindowEvent);
            addSalesDialog.AddControl(lockoutControl);
            addSalesDialog.ShowDialog();
        }
    }
}
