using System;
using System.Windows;
using Microsoft.Reporting.WinForms;
using LorealOptimiseBusiness.Lists;
using System.Net;
using LorealOptimiseGui.Enums;
using System.Collections;
using System.Collections.Generic;


namespace LorealOptimiseGui
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        private ReportType reportType;
        // private Hashtable parametres;
        private IEnumerable<ReportParameter> reportParametres;

        public Report(ReportType reportType, IEnumerable<ReportParameter> reportParametres)
        {
            InitializeComponent();
            this.reportType = reportType;
            // this.parametres = parametres;
            this.reportParametres = reportParametres;

            Loaded += new RoutedEventHandler(Report_Loaded);
        }
       

        void Report_Loaded(object sender, RoutedEventArgs e)
        {
            ReportViewer viewer = repAnimation;

            viewer.ProcessingMode = ProcessingMode.Remote;

            string username = ApplicationSettingManager.Instance.ReportUsername;
            string password = ApplicationSettingManager.Instance.ReportPassword;

            // Get a reference to the report server credentials
            ReportServerCredentials rsCredentials = viewer.ServerReport.ReportServerCredentials;

            ICredentials credentials = null;
            if (String.IsNullOrEmpty(username))
            {
                // Get a reference to the default credentials
                credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                credentials = new NetworkCredential(username, password);
            }


            // Set the credentials for the server report
            rsCredentials.NetworkCredentials = credentials;  

            viewer.ServerReport.ReportServerUrl = new Uri(ApplicationSettingManager.Instance.ReportServerURL);

            // get report path
            switch (reportType)
            {
                case ReportType.AnimationReport:
                    viewer.ServerReport.ReportPath = ApplicationSettingManager.Instance.AnimationReportPath;
                    break;
                case ReportType.AllocationReport:
                    viewer.ServerReport.ReportPath = ApplicationSettingManager.Instance.AllocationReportPath;
                    break;
                case ReportType.StoreAllocationReport:
                    viewer.ServerReport.ReportPath = ApplicationSettingManager.Instance.StoreAllocationPath;
                    break;
                case ReportType.CapacityReport:
                    viewer.ServerReport.ReportPath = ApplicationSettingManager.Instance.CapacityReportPath;
                    break;
                default:
                    break;
            }

            viewer.ServerReport.SetParameters(reportParametres);
            viewer.RefreshReport();
        }
    }
}
