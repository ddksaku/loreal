using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.Data;
using DevExpress.XtraReports.UI;

namespace LorealReports.Reports
{
    public partial class GroupAllocationReportOld : DevExpress.XtraReports.UI.XtraReport
    {
        public GroupAllocationReportOld()
        {
            InitializeComponent();

            string tableName = "REP2";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable(tableName);
            ds.Tables.Add(dt);

            string queryString = "[dbo].[rep_GroupAllocationReport]";

            string connectionString = ConfigurationManager.ConnectionStrings["LorealReports.Properties.Settings.LorealConnectionString"].ConnectionString;

            int rowsCount = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(queryString, connection);
                    rowsCount = adapter.Fill(ds, tableName);
                }
                catch (SqlException) { }
            }

            //this.DataSource = ds;
            xrPivot.DataSource = ds;
            bool b = false;
            foreach (DataColumn dc in ds.Tables[tableName].Columns)
            {
                if (dc.ColumnName != "ID" && dc.ColumnName != "ID1")
                {
                    var col = new PivotGridField()
                                  {
                                      FieldName = dc.ColumnName,
                                      Caption = dc.ColumnName,
                                      Area = b ? PivotArea.DataArea : PivotArea.RowArea
                                  };

                    xrPivot.Fields.Add(col);


                }
                if (dc.ColumnName == "Multiple")
                {
                    b = true;
                }
            }



            ParametersRequestSubmit += GroupAllocationReport_ParametersRequestSubmit;
        }

        void GroupAllocationReport_ParametersRequestSubmit(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {

        }

        private void xrPivot_FieldValueDisplayText(object sender, PivotFieldDisplayTextEventArgs e)
        {

        }

    }
}
