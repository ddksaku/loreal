using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.DirectoryServices;
using System.Data.SqlClient;
using LorealOptimiseShared;
using LorealOptimiseBusiness.Exceptions;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseShared.Logging;
using LorealOptimiseData.Enums;

namespace LorealOptimiseBusiness
{
    /// <summary>
    /// Contains business logic for operations with logged user
    /// </summary>
    public class UserManager : BaseManager, IModify<User>
    {
        public event EntityChangedEventHandler<User> EntityChanged;

        // private ObservableCollection<User> users = null;

        public UserManager()
        {
            
        }

        public UserManager(DbDataContext dbDataContext)
            : base(dbDataContext)
        {

        }


        /// <summary>
        /// Check if database's table User contains login credentials for currently logged user.
        /// If user does not exists or user does not belong to any role, LorealException is thrown
        /// </summary>
        /// <returns>Returns new instance of User class</returns>
        public User Login()
        {
            User user = null;

            try
            {
                user = DbDataContext.GetInstance().Users.Where(u => u.LoginName == LoggedUserName).FirstOrDefault();
            }
            catch (SqlException exc)
            {
                Logger.Log("Can not login into the application. Problem with sql server." + exc.ToString(), LogLevel.Error, LogFilePrefix.Db);
                Logger.Log("Connection string:"+DbDataContext.GetInstance().Connection.ConnectionString, LogLevel.Error);
                System.Windows.MessageBox.Show(string.Format("Can not login into the application. Problem with sql server ({0}). Please contact system administrator", exc.Message));
                Environment.Exit(0);
            }

            if (user == null)
            {
                // throw new LorealLoginException("User '{0}' does not exists in database. If you want to login, please contact system administrator.", LoggedUserName);
                string msg = string.Format("User '{0}' does not exists in database. If you want to login, please contact system administrator.", LoggedUserName);
                Logger.Log(msg, LogLevel.Error, LogFilePrefix.Db);
                System.Windows.MessageBox.Show(msg);
                Environment.Exit(0);
            }

            if (user.UserRoles.Count() == 0)
            {
                // throw new LorealLoginException("User '{0}' does not belong to any role. Please contact system administrator", LoggedUserName);
                string msg = string.Format("User '{0}' does not belong to any role. Please contact system administrator", LoggedUserName);
                Logger.Log(msg, LogLevel.Error, LogFilePrefix.Db);
                System.Windows.MessageBox.Show(msg);
                Environment.Exit(0);
            }

            Lockout lockout = LockoutManager.Instance.GetCurrentLockout();
            if (lockout != null)
            {
                string msg = SystemMessagesManager.Instance.GetMessage("SystemIsLockedOut");

                if (user.UserRoles.Select(ur => ur.Role).Count(r => r.Name == "System Administrator") > 0)
                {
                    // If the user has the "System Administrator" role
                    msg += " Do you want to unlock a system?";
                    if (System.Windows.MessageBox.Show(msg, string.Empty, System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                    {
                        LockoutManager.Instance.UnlockCurrentLock();
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(msg);
                }

                Environment.Exit(0);
            }

            return user;
        }

        /// <summary>
        /// Check if database's table User contains login credentials for currently logged user.
        /// If user does not exists or user does not belong to any role, LorealException is thrown
        /// </summary>
        /// <param name="divisionCount">Contains number of division which currently logged user belongs to</param>
        /// <returns>Return new instance of User class</returns>
        public User Login(out int divisionCount)
        {
            User user = Login();

            divisionCount = user.UserRoles.Where(ur => ur.Division.Deleted == false).Select(ur => ur.IDDivision).Distinct().Count();

            return user;
        }

        /// <summary>
        /// Returns true if user exists in AD, otherwise false
        /// </summary>
        /// <param name="login"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ExistsInAD(string login, out string name, out string email)
        {
            name = String.Empty;
            email = String.Empty;

            if (String.IsNullOrEmpty(login))
            {
                return false;
            }

            using (DirectorySearcher search = new DirectorySearcher())
            {
                search.Filter = string.Format("(sAMAccountName={0})", Utility.RemoveDomainName(login));


                SearchResult result = search.FindOne();
                if (result != null)
                {
                    if (result.Properties["mail"].Count == 0)
                    {
                        throw new LorealValidationException("User {0} does have email address assigned in AD", login);
                    }

                    if (result.Properties["displayname"].Count == 0)
                    {
                        throw new LorealValidationException("User {0} does not have display name assigned in AD", login);
                    }

                    name = result.Properties["displayname"][0].ToString();
                    email = result.Properties["mail"][0].ToString();
                    return true;
                }
            }

            return false;
        }

        private static UserManager instance = null;
        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserManager();
                }
                return instance;
            }
        }

        private static IEnumerable<User> all = null;
        public IEnumerable<User> GetAll()
        {
            if (all == null)
            {
                all = Db.Users;
            }
            return all;
        }

        public void InsertUser(string login)
        {
            string name = String.Empty;
            string email = String.Empty;

            bool exists = ExistsInAD(login, out name, out email);

            if (exists == false)
            {
                throw new LorealValidationException(string.Format("User {0} does not exist in active directory. Please check typed login name.", login));
            }

            User u = new User();
            u.EmailAddress = email;
            u.LoginName = login;
            u.Name = name;
            
            Db.Users.InsertOnSubmit(u);
            Db.SubmitChanges();

            all = null;
        }

        public IEnumerable<UserRole> GetRolesToUser(Guid guid)
        {
            return Db.UserRoles.Where(ur => ur.ID == guid);
        }

        #region IModify<User> Members

        public void InsertOrUpdate(User entity)
        {
            ;
        }

        public void Delete(User entity)
        {
            if (entity.UserRoles.Where(ur => ur.IDDivision != LoggedUser.Division.ID).Count() > 0 && !LoggedUser.IsInRole(RoleEnum.SystemAdmin))
            {
                System.Windows.MessageBox.Show("Can not delete user from different division");
                return;
            }

            Db.UserRoles.DeleteAllOnSubmit(entity.UserRoles);
            Db.Users.DeleteOnSubmit(entity);
            Db.SubmitChanges();

            all = null;
        }

        public void Refresh()
        {
            all = null;
        }

        #endregion
    }
}
