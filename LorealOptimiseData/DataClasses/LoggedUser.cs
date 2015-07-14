using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData.Enums;

namespace LorealOptimiseData
{
    /// <summary>
    /// Class with informatins about currently logged user
    /// </summary>
    public sealed class LoggedUser
    {
        #region Properties
        /// <summary>
        /// Domain login name
        /// </summary>
        public string LoginName
        {
            get;
            set;
        }

        /// <summary>
        /// Name of logged user
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Division
        /// </summary>
        public Division Division
        {
            get;
            set;
        }

        /// <summary>
        /// Logged User
        /// </summary>
        public User User
        {
            get;
            set;
        }

        /// <summary>
        /// List of roles, which user belongs to in specified Division
        /// </summary>
        public IEnumerable<Role> Roles
        {
            get;
            set;
        }

        public RoleEnum RolesEnum
        {
            get
            {
                RoleEnum roles = RoleEnum.NoRole;

                foreach (Role r in Roles)
                {
                    switch (r.Name)
                    {
                        case "Client Care":
                            roles |= RoleEnum.ClientCare;
                            break;
                        case "Division Administrator":
                            roles |= RoleEnum.DivisionAdmin;
                            break;
                        case "Finance":
                            roles |= RoleEnum.Finance;
                            break;
                        case "Marketing":
                            roles |= RoleEnum.Marketing;
                            break;
                        case "NAMs":
                            roles |= RoleEnum.NAMs;
                            break;
                        case "Sales":
                            roles |= RoleEnum.NAMs;
                            break;
                        case "Logistics":
                            roles |= RoleEnum.Logistics;
                            break;
                        case "Read Only":
                            roles |= RoleEnum.ReadOnly;
                            break;
                        case "System Administrator":
                            roles |= RoleEnum.SystemAdmin;
                            break;
                    }
                }

                return roles;
            }
        }

        public string RoleNames
        {
            get
            {
                string roles = String.Empty;
                foreach (Role r in Roles)
                {
                    roles += r.Name + ";";
                }

                return roles;
            }
        }
        #endregion

        private static LoggedUser instance;
        private static readonly object padlock = new object();

        /// <summary>
        /// Returns true, if user is logged. Otherwise returns false
        /// </summary>
        public static bool IsLogged
        {
            get
            {
                return instance != null;
            }
        }

        /// <summary>
        /// Get instance of logged user. SetInstance method must be called before getting instance of logged user. 
        /// </summary>
        /// <param name="user">User entity</param>
        /// <param name="division">Division of the user. User can be assigned to more than one division, but can work with application only with one division</param>
        /// <returns>Return instance of Logged user</returns>
        public static LoggedUser GetInstance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    throw new ArgumentException("Set instance must be called before accessing LoggedUser");
                }

                return instance;
            }
        }

        /// <summary>
        /// Returns Division, on which user logged in
        /// </summary>
        public static Division LoggedDivision
        {
            get
            {
                if (LoggedUser.IsLogged == true)
                {
                    //return DbDataContext.GetInstance().Divisions.SingleOrDefault(d => d.ID == LoggedUser.GetInstance().Division.ID);
                    return instance.Division;
                }
                return null;
            }
        }

        /// <summary>
        /// Sets intance of logged user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="division"></param>
        public static void SetInstance(User user, Division division)
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    try
                    {
                        instance = new LoggedUser(user, division);
                    }
                    catch (Exception exc)
                    {
                        System.Windows.MessageBox.Show(exc.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Creates new instance of Logged user from user and division entity. Assigns also Roles property
        /// </summary>
        /// <param name="user"></param>
        /// <param name="division"></param>
        private LoggedUser(User user, Division division)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "User can not be null");
            }

            if (division == null)
            {
                throw new ArgumentNullException("division", "Division for user can not be null");
            }

            LoginName = user.LoginName;
            Name = user.Name;
            Division = division;
            User = user;

            Roles = DbDataContext.GetInstance().UserRoles.Where(ur => ur.Division.ID == division.ID && ur.User.LoginName == user.LoginName).Select(ur => ur.Role).ToList();
        }

        public bool IsInRole(RoleEnum role)
        {
            if ((RolesEnum & role) == role)
                return true;
            else
                return false;
        }
    }
}
