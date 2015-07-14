using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LorealOptimiseBusiness;
using LorealOptimiseBusiness.Exceptions;
using LorealOptimiseBusiness.ViewMode;
using LorealOptimiseData;
using LorealOptimiseGui.Controls;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Data;
using System.ComponentModel;
using System.Data.SqlTypes;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : BaseUserControl
    {
        // UserManager manager = UserManager.Instance;
        UsersViewModel viewModel = new UsersViewModel();
        UserRole NewItemRow = null; 

        public Users()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(Users_Loaded);

                this.DataContext = viewModel;

                TableViewEventHandlers<User> eventsOnUsers = new TableViewEventHandlers<User>(grdUsers,
                                                                                              viewModel.UserManager, true);
                TableViewEventHandlers<UserRole> eventsOnRoles = new TableViewEventHandlers<UserRole>(grdRoles,
                                                                                      viewModel, true);
                eventsOnRoles.AssignEvents();

                eventsOnUsers.AssignEvents();
            }
        }

        void Users_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                viewModel.Refresh();

                cboRoles.ItemsSource = viewModel.Roles;
                cboDivisions.ItemsSource = viewModel.Divisions;

                grdUsers.BeginDataUpdate();
                grdUsers.SortInfo.Add(new GridSortInfo("Name"));
                grdUsers.EndDataUpdate();

                grdRoles.BeginDataUpdate();
                grdRoles.SortInfo.Add(new GridSortInfo("IDDivision"));
                grdRoles.SortInfo.Add(new GridSortInfo("IDRole"));
                grdRoles.EndDataUpdate();
            }
        }


        private void TableView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TableView tableView = sender as TableView;
            if (e.Key == Key.Delete && tableView.IsEditing == false)
            {
                if (MessageBox.Show(SystemMessagesManager.Instance.GetMessage("UserDelete"), "Delete a user", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    e.Handled = true;
                }
            }
        }

        private void TableView_ShownEditor(object sender, EditorEventArgs e)
        {
            TableView view = sender as TableView;
            UserRole userRole = e.Row as UserRole;
            ComboBoxEdit edit = view.ActiveEditor as ComboBoxEdit;
            if (userRole != null)
            {
                if (e.Column.FieldName == "IDRole")
                {
                    edit.ItemsSource = viewModel.Roles.Where(r => (viewModel.SelectedUser.UserRoles.Any(ur => ur.IDDivision == userRole.IDDivision && ur.IDRole == r.ID) == false) || r.ID == userRole.IDRole);
                }
                else if (e.Column.FieldName == "IDDivision")
                {
                    edit.ItemsSource = viewModel.Divisions.Where(d => (viewModel.SelectedUser.UserRoles.Any(ur => ur.IDDivision == d.ID && ur.IDRole == userRole.IDRole) == false) || d.ID == userRole.IDDivision);
                }
            }
            else
            {
                if (e.Column.FieldName == "IDRole")
                {
                    edit.ItemsSource = viewModel.Roles;
                }
                else if (e.Column.FieldName == "IDDivision")
                {
                    edit.ItemsSource = viewModel.Divisions;
                }
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            NewItemRow = grdRoles.GetRow(e.RowHandle) as UserRole;
            NewItemRow.User = viewModel.SelectedUser;

            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = viewModel;
        }

        private void TableView_RowCanceled(object sender, RowEventArgs e)
        {
            if (NewItemRow != null && e.RowHandle == GridControl.NewItemRowHandle)
            {
                NewItemRow.User.UserRoles.Remove(NewItemRow);
                NewItemRow = null;
            }
        }

        private void TableView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (viewModel.SelectedUser != null)
            {
                grdRoles.DataSource = new ExtendedObservableCollection<UserRole>(viewModel.SelectedUser.UserRoles.Where(ur => ur.IDDivision == LoggedUser.LoggedDivision.ID));
            }
        }

    }
}
