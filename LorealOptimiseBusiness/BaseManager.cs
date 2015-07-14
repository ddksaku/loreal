using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LorealOptimiseBusiness.Exceptions;
using LorealOptimiseData;
using System.Security.Principal;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Windows;

namespace LorealOptimiseBusiness
{
    /// <summary>
    /// Base class for all managers class
    /// </summary>
    public abstract class BaseManager
    {
        protected BaseManager()
        {
                
        }

        protected BaseManager(DbDataContext dbDataContext)
        {
            db = dbDataContext;
        }

        public void SendLongTaskStart(string name)
        {
            
        }

        public void SendLongTaskEnd(string name)
        {
            
        }

        private static DbDataContext db;
        /// <summary>
        /// Datacontext used by manager classes. One datacontext will be used during application run
        /// </summary>
        protected DbDataContext Db
        {
            get
            {
                //if (db == null)
                //{
                //    db = new DbDataContext();

                //    if (db.Connection is SqlConnection)
                //    {
                //        (db.Connection as SqlConnection).InfoMessage += new SqlInfoMessageEventHandler(BaseManager_InfoMessage);
                //    }
                //}

                return DbDataContext.GetInstance();
            }
        }

        void BaseManager_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            //MessageBox should not be used directly from business
            MessageBox.Show(e.Message);
        }


        /// <summary>
        /// Login name of user who is logged in windows
        /// </summary>
        protected string LoggedUserName
        {
            get
            {
                if (WindowsIdentity.GetCurrent() == null)
                {
                    throw new LorealException("No user is logged into windows.");
                }

                return WindowsIdentity.GetCurrent().Name;
            }
        }

        /// <summary>
        /// LoggedUser intance containing detailed information about logged user
        /// </summary>
        public LoggedUser LoggedUser
        {
            get
            {
                return LoggedUser.GetInstance();
            }
        }

        protected Expression<Func<T, bool>> DivisionFilter<T>() where T : IDivision
        {
            return d => d.Division.ID == LoggedUser.Division.ID && d.Division.Deleted == false;
        }

        /// <summary>
        /// Checks whether s is not empty and whether the length is less than maxLength.
        /// If Validation is not successfull, LorealValidationException is thrown.
        /// </summary>
        /// <param name="s">String to chec.k</param>
        /// <param name="maxLength">Max length os s.</param>
        /// <param name="propertyName">Property name which is being checked.</param>
        protected void CheckStringNonEmpty(string s, int maxLength, string propertyName)
        {
            if (String.IsNullOrEmpty(s))
            {
                throw new LorealValidationException("{0} can not be empty", propertyName);
            }

            if (s.Length > maxLength)
            {
                throw new LorealValidationException("{0} - length exceeded. Maximum allowed length is {1}", propertyName, maxLength);
            }
        }

        /// <summary>
        /// Checks whether value is assignable to enumType.
        /// If value is not defined for enumType, LorealValidationException is thrown.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Enum value which is being assigned.</param>
        /// <param name="propertyName">Property name which is being checked.</param>
        protected void CheckEnumValue(Type enumType, int value, string propertyName)
        {
            if (Enum.IsDefined(enumType, value) == false)
            {
                throw new LorealValidationException("{0} contains invalid value", propertyName);
            }
        }

        /// <summary>
        /// Checks whether date is assignable to SqlDateTime type.
        /// If not, LorealValidationException is thrown.
        /// </summary>
        /// <param name="date">Value to check.</param>
        /// <param name="propertyName">Property name which is being checked.</param>
        protected void CheckDateTime(DateTime date, string propertyName)
        {
            if (date < SqlDateTime.MinValue || date > SqlDateTime.MaxValue)
            {
                throw new LorealValidationException("{0} does not contain valid date [{1}]", propertyName, date.ToString());
            }
        }

        /// <summary>
        /// Checks whether parentTable is not null or parentReferenceKey is not empty Guid.
        /// </summary>
        /// <param name="parentTable"></param>
        /// <param name="parentReferenceKey"></param>
        /// <param name="propertyName">Property name which is being checked.</param>
        protected void CheckParentTableReference(object parentTable, Guid parentReferenceKey, string propertyName)
        {
            if (parentTable == null || parentReferenceKey == Guid.Empty)
            {
                throw new LorealValidationException("No {0} is assigned");
            }
        }

    }
}
