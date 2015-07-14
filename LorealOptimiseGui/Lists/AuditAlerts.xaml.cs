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
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using System.Collections;


namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for AuditAlerts.xaml
    /// </summary>
    public partial class AuditAlerts : BaseListUserControl<AuditAlertManager, AuditAlert>
    {
        public AuditAlerts()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(AuditAlerts_Loaded);
                AssignEvents(grdAuditAlerts);
                this.AllowRefreshing = false;
            }
        }

        protected override Hashtable Filters
        {
            get
            {
                Hashtable conditions = new Hashtable();

                if (txtAuditType.Text != String.Empty)
                {
                    conditions.Add(AuditAlertManager.AlertType, txtAuditType.Text);
                }
               

                if (dateFrom.EditValue != null)
                {
                    conditions.Add(AuditAlertManager.DateCreated, dateFrom.EditValue);
                }

                return conditions;
            }
        }

        void AuditAlerts_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                cboDivisions.ItemsSource = new Division[] { LoggedUser.LoggedDivision };
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void txtAuditType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Refresh();
            }
        }


    }
}
