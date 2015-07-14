using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using LorealOptimiseBusiness.Exceptions;
using LorealOptimiseData;
using System.Windows.Input;
using LorealOptimiseShared;
using System.ComponentModel;
using System.Windows.Data;
using LorealOptimiseBusiness.Lists;
using System.Collections.Specialized;
using LorealOptimiseData.Enums;

namespace LorealOptimiseBusiness.ViewMode
{
    public class UsersViewModel : BaseManager, IModify<UserRole>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EntityChangedEventHandler<UserRole> EntityChanged;

        private ListManager listManager = new ListManager();

        private UserManager userManager = new UserManager();
        public UserManager UserManager
        {
            get
            {
                return userManager;
            }
        }

        private ExtendedObservableCollection<User> users;
        public ExtendedObservableCollection<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Users"));
            }
        }

        public IEnumerable<Role> Roles
        {
            get; set;
        }

        public IEnumerable<Division> Divisions
        {
            get; set;
        }

        public string LoginName
        {
            get; set;
        }

        public UsersViewModel()
        {
        }

        public void Refresh()
        {
            DbDataContext.MakeNewInstance();

            Users = new ExtendedObservableCollection<User>(Db.Users.OrderBy(u => u.Name));
            
            Roles = RoleManager.Instance.GetAll();

            Divisions = DivisionManager.Instance.GetAll().Where(d => d.ID == LoggedUser.Division.ID);

            //if (LoggedUser.IsInRole(RoleEnum.SystemAdmin))
            //{
            //    Divisions = DivisionManager.Instance.GetAll();
            //}
            //else
            //{
            //    Divisions = DivisionManager.Instance.GetAll().Where(d => d.ID == LoggedUser.Division.ID);
            //}
        }

        private User selectedUser;
        public User SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));
            }
        }

        #region Insert
        private ICommand insertCommand;
        public ICommand InsertCommand
        {
            get
            {
                if (insertCommand == null)
                {
                    insertCommand = new RelayCommand(param => CanInsert(), param => Insert());
                }

                return insertCommand;
            }
        }

        public bool CanInsert()
        {
            return !String.IsNullOrEmpty(LoginName);
        }

        public void Insert()
        {
            string name;
            string email;

            bool exists = false;

            User existsInDb = Db.Users.Where(user => user.LoginName == LoginName).FirstOrDefault();

            if (existsInDb != null)
            {
                System.Windows.MessageBox.Show(string.Format("User with login name {0} already exists in system", LoginName));
                return;
            }

            try
            {
                exists = ExistsInAD(LoginName, out name, out email);
            }
            catch (Exception exc)
            {
                System.Windows.MessageBox.Show(exc.Message);
                return;
            }

            if (exists == false)
            {
                // throw new LorealValidationException("User {0} does not exists in active directory. Please check typed login name.");
                string msg = string.Format("User '{0}' does not exists in active directory. Please check typed login name.", LoginName);
                System.Windows.MessageBox.Show(msg);
                return;
            }

            User u = new User();
            u.EmailAddress = email;
            u.LoginName = LoginName;
            u.Name = name;

            Db.Users.InsertOnSubmit(u);
            Db.SubmitChanges();

            Users.Add(u);
        }
        #endregion

        #region Assign role

        private ICommand assignRoleCommand;
        public ICommand AssignRoleCommand
        {
            get
            {
                if (assignRoleCommand == null)
                {
                    assignRoleCommand = new RelayCommand(param => AssignRole());
                }

                return assignRoleCommand;
            }
            set
            {
                assignRoleCommand = value;
            }
        }

        public void AssignRole()
        {
            if (SelectedUser != null)
            {
                
            }
        }
        #endregion

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

        #region IModify<UserRole> Members

        public void InsertOrUpdate(UserRole entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.UserRoles.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();
        }

        public void Delete(UserRole entity)
        {
            if (entity.IDDivision != LoggedUser.LoggedDivision.ID && !LoggedUser.IsInRole(RoleEnum.SystemAdmin))
            {
                System.Windows.MessageBox.Show("Can not delete user from different division");
                return;
            }

            Db.UserRoles.DeleteOnSubmit(entity);
            Db.SubmitChanges();
        }

        #endregion
    }
}
