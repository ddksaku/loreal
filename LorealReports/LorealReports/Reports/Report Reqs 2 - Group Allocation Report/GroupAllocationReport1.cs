using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Configuration;

namespace LorealReports.Reports
{
    public partial class GroupAllocationReport1 : DevExpress.XtraReports.UI.XtraReport
    {
        public GroupAllocationReport1()
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


           //xrPivotGrid1.Fields.Add()

            XRLabel label = new XRLabel();
            label.Width = 500;
            label.Font = new System.Drawing.Font("Verdana", 10F, FontStyle.Bold);
            //PageHeader.Controls.Add(label);

            if (rowsCount > 0)
            {
                int padding = 10;
                int tableWidth = this.PageWidth - this.Margins.Left - this.Margins.Right - padding * 2;

                XRTable dynamicTable = XRTable.CreateTable(
                                    new Rectangle(padding,    // rect X
                                                    2,          // rect Y
                                                    tableWidth, // width
                                                    40),        // height
                                                    1,          // table row count
                                                    0);         // table column count

                dynamicTable.Width = tableWidth;
                dynamicTable.Rows.FirstRow.Width = tableWidth;
                dynamicTable.Borders = DevExpress.XtraPrinting.BorderSide.All;
                dynamicTable.BorderWidth = 1;
                int i = 0;
                foreach (DataColumn dc in ds.Tables[tableName].Columns)
                {

                    XRTableCell cell = new XRTableCell();

                    XRBinding binding = new XRBinding("Text", ds, ds.Tables[tableName].Columns[i].ColumnName);
                    cell.DataBindings.Add(binding);
                    cell.CanGrow = false;
                    cell.Width = 100;
                    cell.Text = dc.ColumnName;
                    dynamicTable.Rows.FirstRow.Cells.Add(cell);
                    i++;
                }
                dynamicTable.Font = new System.Drawing.Font("Verdana", 8F);

                Detail.Controls.Add(dynamicTable);

                label.Text = string.Format("Data table: {0}", tableName);

                this.DataSource = ds;
                this.DataMember = tableName;
            }
            else
            {
                label.Text = string.Format("There's no data to display or the table doesn't exists");
            }

        }

    }
}
