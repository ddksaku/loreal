using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Xpf.Printing;
using DevExpress.XtraReports;
using LorealReports.DataAccess;


namespace LorealReports.Reports.Base
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected LorealEntities db = new LorealEntities();

        protected Division UserDivision { get; set; }

        XtraReportPreviewModel reportPreviewModel;
        public XtraReportPreviewModel ReportPreviewModel
        {
            get { return reportPreviewModel; }
        }

        public BaseViewModel(Guid divisionId)
        {
            UserDivision = db.Division.FirstOrDefault(x => x.ID == divisionId);
            CreateReportCore(delegate(IReport report)
                                    {
                                        reportPreviewModel = CreateReportPreviewModel(report);
                                    });
        }

        void CreateReportCore(Action<IReport> assignReport)
        {
            IReport report = CreateReport();
            assignReport(report);
        }

        protected abstract IReport CreateReport();

        protected virtual XtraReportPreviewModel CreateReportPreviewModel(IReport report)
        {
            var rr = new XtraReportPreviewModel(report);
            return new XtraReportPreviewModel(report);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
