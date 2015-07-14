using System;
using System.Collections.Generic;
using System.Deployment.Application;
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
using LorealOptimiseData;
using LorealOptimiseBusiness.Lists;
using System.Configuration;
using LorealOptimiseBusiness.ViewMode;

namespace LorealOptimiseGui
{
    /// <summary>
    /// Interaction logic for SelectDivision.xaml
    /// </summary>
    public partial class SelectDivision : Window
    {
        readonly DivisionManager manager = DivisionManager.Instance;

        public SelectDivision()
        {
            InitializeComponent();
            
            cboDivision.ItemsSource = manager.GetAllToLogged();
            cboDivision.ValueMember = "ID";
            cboDivision.DisplayMember = "Name";

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Title += " (" + ApplicationDeployment.CurrentDeployment.CurrentVersion + ")";
            }

            this.DataContext = this;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (cboDivision.SelectedItem == null)
                {
                    loginCommand = new RelayCommand(param => CanLogin(), parm => DoLogin());
                }
                return loginCommand;
            }
        }

        bool CanLogin()
        {
            return cboDivision.SelectedItem != null;
        }

        void DoLogin()
        {
            UserManager userManager = UserManager.Instance;
            
            User user = userManager.Login();

            LoggedUser.SetInstance(user, (Division)cboDivision.SelectedItem);

            this.Close();
        }
    }
}
