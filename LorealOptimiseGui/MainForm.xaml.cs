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
using LorealOptimiseData;
using LorealOptimiseData.Enums;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using LorealOptimiseGui.Enums;
using LorealOptimiseShared;
using Microsoft.Reporting.WinForms;
using System.ComponentModel;
using System.Data.SqlClient;
using LorealOptimiseBusiness.Lists;
using System.Windows.Threading;
using System.Globalization;
using System.Threading;

namespace LorealOptimiseGui
{
    /// <summary>
    /// Interaction logic for MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        Dictionary<MenuItem, Control> controls = new Dictionary<MenuItem, Control>();
        Dictionary<MenuItem, Control> adminSubMenus = new Dictionary<MenuItem, Control>();
        Dictionary<MenuItem, Control> ssSubMenus = new Dictionary<MenuItem, Control>(); // store & sales sub menus

        private Cursor WaitingCursor = Cursors.Wait;
        private Cursor NormalCursor = Cursors.Arrow;

        public MainForm()
        {
            try
            {
                InitializeComponent();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            }
            catch(SqlException sqlExc)
            {
                throw sqlExc;
            }

            IEnumerable<MenuItem> sortedMenuItems = itmGeneralLists.Items.Cast<MenuItem>().OrderBy(m => m.Header);
            int index = 0;
            foreach (MenuItem mi in sortedMenuItems)
            {
                itmGeneralLists.Items.Remove(mi);
                itmGeneralLists.Items.Insert(index++, mi);
            }

            grdControlArea.Children.Clear();

            Loaded += new RoutedEventHandler(MainForm_Loaded);
            Activated += new EventHandler(MainForm_Activated);

            LongTaskExecutor.LongTaskEvent += new LongTaskEventHandler(LongTaskExecutor_LongTaskEvent);
            DbDataContext.InfoMessageReceived += new DbDataContext.InfoMessageReceivedEventHandler(DbDataContext_InfoMessageReceived);
        }

        void MainForm_Activated(object sender, EventArgs e)
        {
            if (progressWnd != null && progressWnd.IsVisible)
            {
                // progressWnd.Activate();
            }
        }

        void DbDataContext_InfoMessageReceived(object sender, string message, IEnumerable<byte> states)
        {
            if (states.Any(s => s == 100))
            {
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Creating allocations", TaskStatus.InProgress, message));
            }
            else if (states.Any(s => s == 200))
            {
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Including customer groups", TaskStatus.InProgress, message));
            }
            else if (states.Any(s => s == 101))
            {
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Including customer groups", TaskStatus.InProgress, message));
            }
            else
            {
                MessageBox.Show(message);
            }

            if (states.Any(s => s == 140))
            {
                MessageBox.Show(message);
            }
        }

        private LongTaskProgressWindow progressWnd = null;
        void LongTaskExecutor_LongTaskEvent(object sender, LongTaskEventArgs args)
        {
            switch (args.Status)
            {
                case TaskStatus.Started:
                    this.Cursor = WaitingCursor;
                    if (progressWnd == null)
                    {
                        progressWnd = new LongTaskProgressWindow();
                    }
                    else
                    {
                        progressWnd.Reference++;
                    }

                    this.grdMain.IsEnabled = false;
                    progressWnd.ProgressMessage = args.TaskName + " - start";
                    progressWnd.Owner = this;
                    progressWnd.Show();
                    
                    break;
                case TaskStatus.Finished:
                    if (progressWnd != null)
                    {
                        progressWnd.Reference--;
                    }

                    if (progressWnd != null)
                    {
                        progressWnd.ProgressMessage = args.TaskName + " - end";
                        if (progressWnd.Reference == 0)
                        {
                            progressWnd.Close();
                            progressWnd.Owner = null;
                            progressWnd = null;

                            this.Activate();
                            this.grdMain.IsEnabled = true;
                        }
                        this.Cursor = NormalCursor;
                    }
                    break;
                case TaskStatus.InProgress:
                    if (progressWnd != null)
                    {
                        progressWnd.ProgressMessage = args.ProgressMessage;
                    }

                    break;
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            progressWnd.ShowDialog();
        }

        void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            // ProductManager.Instance.GetAllInBackground();

            controls.Add(itmAddAnimation, ucAnimation);
            controls.Add(itmViewAnimation, ucAnimationList);

            controls.Add(itmAdministration, ucAdminTab);
            controls.Add(itmStoresSales, ucStoresSalesTab);
            controls.Add(itmRunAllocations, ucRunAllocations);

            ucAnimationList.NewAnimation += new EventHandler(ucAnimationList_NewAnimation);
            ucAnimationList.EditAnimation += new LorealOptimiseGui.Controls.AnimationList.EditAnimationHandler(ucAnimationList_EditAnimation);

            itmAddAnimation.Click += new RoutedEventHandler(itmAddAnimation_Click);

            foreach (KeyValuePair<MenuItem, Control> values in controls)
            {
                values.Key.Click += new RoutedEventHandler(Key_Click);
            }

            // Stores And Sales Menu
            InitStoresAndSalesSubMenus();

            itmSystemAdmin.Visibility = Visibility.Collapsed;
            itmDivisionAdmin.Visibility = Visibility.Collapsed;
            itmAllocations.Visibility = Visibility.Collapsed;

            if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
            {
                itmSystemAdmin.Visibility = Visibility.Visible;
            }
            if (LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
            {
                itmDivisionAdmin.Visibility = Visibility.Visible;
                itmAllocations.Visibility = Visibility.Visible;
            }
            itmAdministration.Visibility = (itmSystemAdmin.Visibility == Visibility.Visible || itmDivisionAdmin.Visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;

            InitAdminSubMenus();

            Key_Click(itmViewAnimation, null);
        }

        void itmAddAnimation_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ucAnimation.NewAnimation();
            this.Cursor = Cursors.Arrow;
        }

        private void menuAllocationReport_Click(object sender, RoutedEventArgs e)
        {
            ReportParameter[] p = new ReportParameter[1];
            ReportParameter rp = new ReportParameter("DivisionID", LoggedUser.LoggedDivision.ID.ToString());

            p[0] = rp;

            Report reportCtrl = new Report(ReportType.AllocationReport, p);
            reportCtrl.Width = 800;
            reportCtrl.Height = 600;
            reportCtrl.Show();
        }

        private void menuStoreAllocationReport_Click(object sender, RoutedEventArgs e)
        {
            ReportParameter[] p = new ReportParameter[1];
            ReportParameter rp = new ReportParameter("divisionID", LoggedUser.LoggedDivision.ID.ToString());

            p[0] = rp;

            Report reportCtrl = new Report(ReportType.StoreAllocationReport, p);
            reportCtrl.Width = 800;
            reportCtrl.Height = 600;
            reportCtrl.Show();
        }

        void ucAnimationList_EditAnimation(object sender, Animation animation)
        {
            LongTaskExecutor animationOpener = new LongTaskExecutor("Opening the animation - " + animation.Name);
            animationOpener.DoWork += new DoWorkEventHandler(animationOpener_DoWork);
            animationOpener.Run(animation);
        }

        void animationOpener_DoWork(object sender, DoWorkEventArgs e)
        {
            AnimationManager.GetInstance().Animation = e.Argument as Animation;
            DisplayControl(ucAnimation);

            LongTaskExecutor.DoEvents();
        }

        void ucAnimationList_NewAnimation(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Wait;

            ucAnimation.NewAnimation();
            DisplayControl(ucAnimation);
            LongTaskExecutor.DoEvents();

            this.Cursor = Cursors.Arrow;
        }

        void animationCreator_DoWork(object sender, DoWorkEventArgs e)
        {
            ucAnimation.NewAnimation();
            DisplayControl(ucAnimation);

            LongTaskExecutor.DoEvents();
        }

        private void DisplayControl(Control visibleControl)
        {
            grdControlArea.Children.Clear();
            grdControlArea.Children.Add(visibleControl);
        }

        void Key_Click(object sender, RoutedEventArgs e)
        {
            MenuItem itm = sender as MenuItem;
            grdControlArea.Children.Clear();
            grdControlArea.Children.Add(controls[itm]);
            controls[itm].Visibility = System.Windows.Visibility.Visible;
        }

        #region Store & Sales Menu
        void InitStoresAndSalesSubMenus()
        {
            ssSubMenus.Add(itmSSCustomerGroups, ucStoresSalesTab.ucCustomerGroups);
            ssSubMenus.Add(itmCustomerStores, ucStoresSalesTab.ucCustomerStores);
            ssSubMenus.Add(itmSales, ucStoresSalesTab.ucSales);
            ssSubMenus.Add(itmBrandExclusions, ucStoresSalesTab.ucCustomerBrandExclusions);
            ssSubMenus.Add(itmCapacities, ucStoresSalesTab.ucCustomerCapacities);

            foreach (KeyValuePair<MenuItem, Control> control in ssSubMenus)
            {
                control.Key.Click += new RoutedEventHandler(StoresAndSalesSubMenu_Click);
            }
        }

        void StoresAndSalesSubMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem itm = sender as MenuItem;
            ucStoresSalesTab.VisibleList = ssSubMenus[itm];
        }

        #endregion

        #region Administration Menu
        void InitAdminSubMenus()
        {
            // assigning events to sub menus
            adminSubMenus.Add(itmDivisions, ucAdminTab.ucDivisions);
            adminSubMenus.Add(itmDivisionsSys, ucAdminTab.ucDivisions);
            adminSubMenus.Add(itmSalesAreas, ucAdminTab.ucSalesAreas);
            adminSubMenus.Add(itmSalesAreasSys, ucAdminTab.ucSalesAreas);
            adminSubMenus.Add(itmAppSettingsSys, ucAdminTab.ucApplicationSettings);
            adminSubMenus.Add(itmAppSettings, ucAdminTab.ucApplicationSettings);
            adminSubMenus.Add(itmHistoryLog, ucAdminTab.ucHistoryLogs);
            adminSubMenus.Add(itmUsers, ucAdminTab.ucUsers);
            adminSubMenus.Add(itmUsersInRoles, ucAdminTab.ucUsersInRole);
            adminSubMenus.Add(itmSystemMessages, ucAdminTab.ucSystemMessages);

            adminSubMenus.Add(itmUsersAndRoles, ucAdminTab.ucUsers);
            adminSubMenus.Add(itmUsersAndRolesSys, ucAdminTab.ucUsers);
            adminSubMenus.Add(itmEventLog, ucAdminTab.ucEventLogs);
            adminSubMenus.Add(itmAuditAlerts, ucAdminTab.ucAuditAlerts);

            adminSubMenus.Add(itmSalesOrganizations, ucAdminTab.ucSalesOrganizations);
            adminSubMenus.Add(itmDistributionChannels, ucAdminTab.ucDistributionChannels);
            adminSubMenus.Add(itmProducts, ucAdminTab.ucProducts);
            adminSubMenus.Add(itmSignatures, ucAdminTab.ucSignatures);
            adminSubMenus.Add(itmBrandAxes, ucAdminTab.ucBrandAxes);
            
            adminSubMenus.Add(itmCustomerGroupItemTypes, ucAdminTab.ucCustomerGroupItemTypes);
            adminSubMenus.Add(itmCustomerGroups, ucAdminTab.ucCustomerGroups);
            adminSubMenus.Add(itmItemGroups, ucAdminTab.ucItemGroups);
            adminSubMenus.Add(itmItemTypes, ucAdminTab.ucItemTypes);
            adminSubMenus.Add(itmOrderTypes, ucAdminTab.ucOrderTypes);
            adminSubMenus.Add(itmSalesDrives, ucAdminTab.ucSalesDrives);
            adminSubMenus.Add(itmRetailerTypes, ucAdminTab.ucRetailerTypes);
            adminSubMenus.Add(itmStoreCategories, ucAdminTab.ucCategories);
            adminSubMenus.Add(itmAnimationTypes, ucAdminTab.ucAnimationTypes);
            adminSubMenus.Add(itmAnimationPriorities, ucAdminTab.ucPriorities);

            foreach (KeyValuePair<MenuItem, Control> values in adminSubMenus)
            {
                values.Key.Click += new RoutedEventHandler(AdminSubMenu_Click);
            }
        }

        void AdminSubMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem itm = sender as MenuItem;
            ucAdminTab.VisibleList = adminSubMenus[itm];
        }
        #endregion

        private void mnuMain_MouseEnter(object sender, MouseEventArgs e)
        {
            // mnuMain.Focus();
        }
    }
}
