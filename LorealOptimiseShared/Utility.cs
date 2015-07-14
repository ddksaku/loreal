using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Editors;
using System.Reflection;
using System.Data.Linq.Mapping;

namespace LorealOptimiseShared
{
    public class Utility
    {
        public static string RemoveDomainName(string loginName)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                return string.Empty;
            }

            int slash = loginName.IndexOf('\\');

            if (slash < 0)
            {
                return loginName;
            }

            return loginName.Substring(slash + 1);
        }

        public static string GetExceptionsMessages(Exception exc)
        {
            if (exc == null)
            {
                return String.Empty;
            }

            string messages = String.Empty;

            while (exc != null)
            {
                messages += exc.Message + Environment.NewLine;
                exc = exc.InnerException;
            }

            return messages;
        }
       

        public static void ResetControls(DependencyObject baseControl)
        {
            foreach (Object o in LogicalTreeHelper.GetChildren(baseControl))
            {
                if (o is TextBox)
                {
                    ((TextBox)o).Text = String.Empty;
                }

                if (o is ComboBox)
                {
                    ((ComboBox)o).SelectedIndex = 0;
                }

                if (o is DateEdit)
                {
                    ((DateEdit)o).DateTime = DateTime.Now;
                }

                if (o is SpinEdit)
                {
                    ((SpinEdit)o).Text = String.Empty;
                }

                if (o is ComboBoxEdit)
                {
                    ((ComboBoxEdit)o).SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Gets the length limit for a given field on a LINQ object ... or zero if not known
        /// </summary>
        /// <remarks>
        /// You can use the results from this method to dynamically 
        /// set the allowed length of an INPUT on your web page to
        /// exactly the same length as the length of the database column.  
        /// Change the database and the UI changes just by
        /// updating your DBML and recompiling.
        /// </remarks>
        public static int GetLengthLimit(ColumnAttribute ca)
        {
            if (ca == null)
            {
                return int.MaxValue;
            }

            int dblenint = 0;   // default value = we can't determine the length

            string dbtype = ca.DbType;

            if (dbtype.StartsWith("NChar") || dbtype.StartsWith("NVarChar"))
            {
                int index1 = dbtype.IndexOf("(");
                int index2 = dbtype.IndexOf(")");
                string dblen = dbtype.Substring(index1 + 1, index2 - index1 - 1);
                if (dblen == "MAX")
                {
                    return int.MaxValue;
                }
                int.TryParse(dblen, out dblenint);
            }

            return dblenint;
        }

        #region Vadliation
        public static bool IsValid(object o, string propertyName, object propertyValue, out string errorMesssage)
        {
            errorMesssage = string.Empty;
            if (o.GetType().GetProperties().Any(p => p.Name == propertyName) == false)
                return false;

            PropertyInfo property = o.GetType().GetProperties().Where(p => p.Name == propertyName).FirstOrDefault();
            return IsValidProperty(property, propertyValue, out errorMesssage);
        }

        public static bool IsValid(object o, out string errorMessage, out string firstInvalidColumn)
        {
            foreach (PropertyInfo property in o.GetType().GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(false);
                if (attributes.Length > 0 && attributes[0].GetType() == typeof(ColumnAttribute))
                {
                    object propertyValue = property.GetValue(o, null);
                    if (IsValidProperty(property, propertyValue, out errorMessage) == false)
                    {
                        firstInvalidColumn = property.Name;
                        return false;
                    }
                }
            }

            firstInvalidColumn = string.Empty;
            errorMessage = String.Empty;
            return true;
        }

        public static bool IsValid(object o, out string errorMessage)
        {
            foreach (PropertyInfo property in o.GetType().GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(false);
                if (attributes.Length > 0 && attributes[0].GetType() == typeof(ColumnAttribute))
                {
                    object propertyValue = property.GetValue(o, null);
                    if (IsValidProperty(property, propertyValue, out errorMessage) == false)
                    {
                        return false;
                    }
                }
            }

            errorMessage = String.Empty;
            return true;
        }

        private static bool IsValidProperty(PropertyInfo property, object propertyValue, out string errorMessage)
        {
            object[] attributes = property.GetCustomAttributes(false);
            if (attributes.Length > 0 && attributes[0].GetType() == typeof(ColumnAttribute))
            {
                ColumnAttribute ca = attributes[0] as ColumnAttribute;

                if (ca.CanBeNull == false)
                {
                    if (propertyValue == null)
                    {
                        errorMessage = string.Format("Value of {0} should not be empty. ", property.Name);
                        return false;
                    }

                    if (property.PropertyType == typeof(String))
                    {
                        if (String.IsNullOrEmpty(propertyValue as String))
                        {
                            errorMessage = string.Format("Value of {0} should not be empty. ", property.Name);
                            return false;
                        }
                    }
                }

                if (property.PropertyType == typeof(String))
                {
                    string value = propertyValue as String;

                    //Following columns can always be empty. Not nice way, but easy :)
                    //if (property.Name != "ModifiedBy" && property.Name.Contains("Comments") == false)
                    //{
                    //    if (String.IsNullOrEmpty(value))
                    //    {
                    //        errorMessage = string.Format("Value of {0} should not be empty.", property.Name);
                    //        return false;
                    //    }
                    //}

                    int maxLength = GetLengthLimit(ca);
                    if (value != null && value.Length > maxLength)
                    {
                        errorMessage = string.Format("Max lenght[{0}]",maxLength) + " of {0} was exceeded. ";
                        return false;
                    }
                }

                if (property.PropertyType == typeof(DateTime))
                {
                    DateTime minDate = new DateTime(2000, 1, 1);

                    DateTime dt = (DateTime)propertyValue;
                    if (dt < minDate)
                    {
                        errorMessage = string.Format("Please enter valid date [MinDate={0}]", minDate.ToString()) + " for {0} .";
                        return false;
                    }
                }

                if (property.PropertyType == typeof(Guid))
                {
                    Guid guid = (Guid)propertyValue;

                    if (guid == Guid.Empty && property.Name != "ID")
                    {
                        errorMessage = "Value of {0} should not be empty. ";
                        return false;
                    }
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        #endregion

        public readonly static int SqlCommandTimeOut = 2400;    // 40 minutes
        public readonly static int RunAllocationCommandTimeOut = 3600;  // 60 minutes
    }
}
