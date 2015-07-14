using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using LorealOptimiseBusiness.Exceptions;
using LorealOptimiseShared;


namespace LorealOptimiseBusiness.Lists
{
    public class ApplicationSettingManager : BaseManager, IListManager<ApplicationSetting>
    {
        public event EntityChangedEventHandler<ApplicationSetting> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        public const string IDDivision = "IDDivision";

        private static ApplicationSettingManager instance = null;
        public static ApplicationSettingManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApplicationSettingManager();
                }
                return instance;
            }
        }

        public string ReportServerURL
        {
            get
            {
                ApplicationSetting reportURL = ApplicationSettingManager.Instance.GetAll().Where(c => c.SettingKey == "ReportServerURL").FirstOrDefault();

                if (reportURL == null)
                {
                    throw new LorealException("Application setting for Report URL doesn't exist.");
                }
                return reportURL.SettingValue;
            }
        }

        public string AnimationReportPath
        {
            get
            {
                ApplicationSetting reportDirectory = ApplicationSettingManager.Instance.GetAll().Where(c => c.SettingKey == "AnimationReportPath").FirstOrDefault();

                if (reportDirectory == null)
                {
                    throw new LorealException("Application setting for Report Directory doesn't exist.");
                }

                return reportDirectory.SettingValue;
            }
        }
        public string AllocationReportPath
        {
            get
            {
                ApplicationSetting reportDirectory = ApplicationSettingManager.Instance.GetAll().Where(c => c.SettingKey == "AllocationReportPath").FirstOrDefault();

                if (reportDirectory == null)
                {
                    throw new LorealException("Application setting for Report Directory doesn't exist.");
                }

                return reportDirectory.SettingValue;
            }
        }

        public string StoreAllocationPath
        {
            get
            {
                ApplicationSetting reportDirectory = ApplicationSettingManager.Instance.GetAll().Where(c => c.SettingKey == "StoreAllocationPath").FirstOrDefault();

                if (reportDirectory == null)
                {
                    throw new LorealException("Application setting for Report Directory doesn't exist.");
                }

                return reportDirectory.SettingValue;
            }
        }

        public string CapacityReportPath
        {
            get
            {
                ApplicationSetting reportDirectory = ApplicationSettingManager.Instance.GetAll().Where(c => c.SettingKey == "CapacityReportPath").FirstOrDefault();

                if (reportDirectory == null)
                {
                    throw new LorealException("Application setting for Report Directory doesn't exist.");
                }

                return reportDirectory.SettingValue;
            }
        }



        public string ReportUsername
        {
            get
            {
                ApplicationSetting reportUsername = Instance.GetAll().Where(c => c.SettingKey == "ReportUser").FirstOrDefault();

                if (reportUsername == null)
                {
                    throw new LorealException("Application setting for Report Username doesn't exist.");
                }

                return reportUsername.SettingValue;
            }

        }

        public string ReportPassword
        {
            get
            {
                ApplicationSetting reportPassword = ApplicationSettingManager.Instance.GetAll().Where(c => c.SettingKey == "ReportPassword").FirstOrDefault();

                if (reportPassword == null)
                {
                    throw new LorealException("Application setting for Report Password doesn't exist.");
                }
                return reportPassword.SettingValue;
            }
        }

        public bool ShowValidationButton
        {
            get
            {
                ApplicationSetting showValidationButton = ApplicationSettingManager.Instance.GetAll().Where(c => c.SettingKey == "ShowValidationButton").FirstOrDefault();

                if (showValidationButton == null)
                {
                    return false;
                }

                bool value = false;
                if (bool.TryParse(showValidationButton.SettingValue, out value))
                {
                    return value;
                }
                else
                {
                    return false;
                }
            }
        }

        //public int SqlCommandTimeOut
        //{
        //    get
        //    {
        //        ApplicationSetting sqlCommandTimeOut = ApplicationSettingManager.Instance.GetAll().Where(c => c.SettingKey == "SqlCommandTimeOut").FirstOrDefault();
        //        if (sqlCommandTimeOut == null)
        //        {
        //            throw new LorealException("Application setting for Sql Command TimeOut doesn't exist.");
        //        }
        //        int result = 30;
        //        int.TryParse(sqlCommandTimeOut.SettingValue, out result);
        //        return result;
        //    }
        //}

        //public int RunAllocationCommandTimeOut
        //{
        //    get
        //    {
        //        ApplicationSetting runAllocationCommandTimeOut = ApplicationSettingManager.Instance.GetAll().Where(c => c.SettingKey == "RunAllocationCommandTimeOut").FirstOrDefault();
        //        if (runAllocationCommandTimeOut == null)
        //        {
        //            throw new LorealException("Application setting for Sql RunAllocation Command TimeOut doesn't exist.");
        //        }
        //        int result = 30;
        //        int.TryParse(runAllocationCommandTimeOut.SettingValue, out result);
        //        return result;
        //    }
        //}

        private static IEnumerable<ApplicationSetting> all = null;
        public IEnumerable<ApplicationSetting> GetAll()
        {
            if (all == null)
            {
                all = Db.ApplicationSettings.ToList();
            }
            return all;
        }

        public IEnumerable<ApplicationSetting> GetFiltered(Hashtable conditions)
        {

            var result = Db.ApplicationSettings.AsQueryable();

            if (conditions != null)
            {
                Guid divisionID = Guid.Empty;

                if (conditions.ContainsKey(IDDivision) && conditions[IDDivision].ToString() != Guid.Empty.ToString() && conditions[IDDivision] != null && conditions[IDDivision].ToString().IsValidGuid())
                {
                    divisionID = (Guid)conditions[IDDivision];
                    result = result.Where(c => c.IDDivision == divisionID);
                }
            }

            return result.ToList();

        }

        public void InsertOrUpdate(ApplicationSetting entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.ApplicationSettings.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();

            all = null;
        }

        public void Delete(ApplicationSetting entity)
        {
            Db.ApplicationSettings.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
