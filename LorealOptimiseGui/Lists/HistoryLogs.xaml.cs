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
using System.Collections.ObjectModel;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using LorealOptimiseBusiness.ViewMode;
using System.Collections;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for EventLogs.xaml
    /// </summary>
    public partial class HistoryLogs : BaseListUserControl<HistoryLogManager, HistoryLog>
    {
        public HistoryLogs()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Refresh();
            HistoryLogManager.Instance.Refresh2();
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            grdHistoryLogs.BeginDataUpdate();
            grdHistoryLogs.SortInfo.Clear();
            grdHistoryLogs.SortInfo.Add(new GridSortInfo("TableName"));
            grdHistoryLogs.SortInfo.Add(new GridSortInfo("ModifiedDate", ListSortDirection.Descending));
            grdHistoryLogs.EndDataUpdate();

            Data = new ExtendedObservableCollection<HistoryLog>(HistoryLogManager.Instance.GetAll(txtTableName.Text, (DateTime?)startDate.EditValue, (DateTime?)endDate.EditValue));
            DataContext = Data;

            this.Cursor = Cursors.Arrow;
        }

        private void txtTableName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Cursor = Cursors.Wait;

                grdHistoryLogs.BeginDataUpdate();
                grdHistoryLogs.SortInfo.Clear();
                grdHistoryLogs.SortInfo.Add(new GridSortInfo("TableName"));
                grdHistoryLogs.SortInfo.Add(new GridSortInfo("ModifiedDate", ListSortDirection.Descending));
                grdHistoryLogs.EndDataUpdate();

                Data = new ExtendedObservableCollection<HistoryLog>(HistoryLogManager.Instance.GetAll(txtTableName.Text, (DateTime?)startDate.EditValue, (DateTime?)endDate.EditValue));
                DataContext = Data;

                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
