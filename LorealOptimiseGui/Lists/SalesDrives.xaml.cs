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
using LorealOptimiseGui.Enums;
using Microsoft.Reporting.WinForms;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for SalesDrives.xaml
    /// </summary>
    public partial class SalesDrives : BaseListUserControl<SalesDriveManager, SalesDrive>
    {
        public SalesDrives()
            : base()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(SalesDrives_Loaded);
                AssignEvents(grdSalesDrives);
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        void SalesDrives_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                cboDivisions.ItemsSource = new Division[] { LoggedUser.LoggedDivision };
            }
        }

        private void colYear_Validate(object sender, GridCellValidationEventArgs e)
        {
            int year = 0;
            if (int.TryParse(e.Value.ToString(), out year))
            {
                if (year < 1)
                    e.SetError("Invalid Year.");
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                grdSalesDrives.SetCellValue(e.RowHandle, grdSalesDrives.Columns["Year"], DateTime.Now.Year);
            }
            catch (Exception exc)
            {
                //MessageBox.Show("An error occured when setting 'Year' value for a new row:" + LorealOptimiseShared.Utility.GetExceptionsMessages(exc));
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("SalesDriveExceptionNewRow", LorealOptimiseShared.Utility.GetExceptionsMessages(exc)));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            if (grdSalesDrives.View.FocusedRow != null)
            {
               
                SalesDrive salesDrive = (LorealOptimiseData.SalesDrive)(grdSalesDrives.View.FocusedRow);
                List<string> selectedValues = new List<string>();

                foreach (Guid i in SalesDriveManager.Instance.GetAnimationsBySalesDrive(salesDrive).ToArray())
                {
                    selectedValues.Add(i.ToString());
                }

                ReportParameter[] p = new ReportParameter[3];
                 
                ReportParameter rp = new ReportParameter("SalesDrive", (salesDrive.ID).ToString());
                ReportParameter rp2 = new ReportParameter("Animation", selectedValues.ToArray());
                ReportParameter rp3 = new ReportParameter("DivisionID", LoggedUser.LoggedDivision.ID.ToString());

                p[0] = rp;
                p[1] = rp2;
                p[2] = rp3;

                Report reportCtrl = new Report(ReportType.AnimationReport, p);
                reportCtrl.Width = 800;
                reportCtrl.Height = 600;
                reportCtrl.Show();
            }

        }

    }
}
