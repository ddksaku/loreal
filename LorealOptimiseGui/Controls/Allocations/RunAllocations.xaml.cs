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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LorealOptimiseData;
using LorealOptimiseData.Enums;
using LorealOptimiseBusiness;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseShared;
using LorealOptimiseShared.Logging;
using System.Data.Linq;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for ViewAllocations.xaml
    /// </summary>
    public partial class RunAllocations : BaseUserControl
    {
        public RunAllocations()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(RunAllocations_Loaded);
        }

        void RunAllocations_Loaded(object sender, RoutedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                cboSalesDrive.ItemsSource = SalesDriveManager.Instance.GetAll();
            }
        }

        private void cboSalesDrive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSalesDrive.SelectedItem != null)
            {
                SalesDrive selectedSalesDrive = cboSalesDrive.SelectedItem as SalesDrive;
                if (selectedSalesDrive != null)
                {
                    cboeAnimations.Text = "";
                    cboeAnimations.ItemsSource = AnimationManager.GetInstance().AllAnimations.Where(a => a.IDSalesDrive == selectedSalesDrive.ID && a.Status != null && (AnimationStatus)a.Status.Value != AnimationStatus.Closed && (AnimationStatus)a.Status.Value != AnimationStatus.Cleared).OrderBy(a => a.Name);
                }
            }
        }

        private void btnRunAllocation_Click(object sender, RoutedEventArgs e)
        {
            tabResults.Items.Clear();

            if (cboeAnimations.SelectedItem != null)
            {
                Animation ani = cboeAnimations.SelectedItem as Animation;

                if (ani != null)
                {
                    // check if SAP order is created
                    if (ani.DateSAPOrderCreated != null)
                    {
                        if (MessageBox.Show(SystemMessagesManager.Instance.GetMessage("RunAllocationOrderCreated"), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            return;
                    }

                    LongTaskExecutor runAllocationExecutor = new LongTaskExecutor("Running allocation for "+ani.Name);
                    runAllocationExecutor.DoWork += new System.ComponentModel.DoWorkEventHandler(runAllocationExecutor_DoWork);
                    runAllocationExecutor.Run(ani);
                }
            }
            else
            {
                string systemMessage = SystemMessagesManager.Instance.GetMessage("RunAllocationOrderCreated");
                string warning = String.Empty;

                foreach (Animation animation in cboeAnimations.ItemsSource)
                {
                    if (animation.DateSAPOrderCreated != null)
                    {
                        warning += string.Format(systemMessage, animation.Name) + Environment.NewLine;
                    }
                }

                if (!String.IsNullOrEmpty(warning) && MessageBox.Show(warning, "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
                
                //Run for all animation in selected sales drive
                foreach (Animation animation in cboeAnimations.ItemsSource)
                {
                    if (animation != null)
                    {
                        // check if SAP order is created
                        if (animation.DateSAPOrderCreated != null)
                        {
                            if (System.Windows.MessageBox.Show(SystemMessagesManager.Instance.GetMessage("RunAllocationOrderCreated", animation.Name), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            {
                                return;
                            }
                        }

                        LongTaskExecutor runAllocationExecutor = new LongTaskExecutor("Running allocation for "+animation.Name);
                        runAllocationExecutor.DoWork += new System.ComponentModel.DoWorkEventHandler(runAllocationExecutor_DoWork);
                        runAllocationExecutor.Run(animation);
                    }
                }
            }

            if (tabResults.Items.Count > 0)
            {
                tabResults.SelectedIndex = 0;
            }
        }

        void runAllocationExecutor_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                Animation ani = e.Argument as Animation;

                ani = DbDataContext.GetInstance().Animations.FirstOrDefault(a => a.ID == ani.ID);

                DbDataContext.GetInstance().CommandTimeout = Utility.RunAllocationCommandTimeOut;

                Guid? returnValue = DbDataContext.GetInstance().uf_allocate_animationID(ani.ID, chckEnableLogging.IsChecked );

                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<LorealOptimiseData.Animation>(a => a.AnimationProducts);
                loadOptions.LoadWith<LorealOptimiseData.AnimationProduct>(ap => ap.AnimationProductDetails);
                loadOptions.LoadWith<LorealOptimiseData.AnimationProductDetail>(apd => apd.CustomerAllocations);
                loadOptions.LoadWith<LorealOptimiseData.CustomerAllocation>(apd => apd.Customer);

                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Getting results ...", TaskStatus.InProgress, "Getting results ..."));
                DbDataContext.MakeNewInstance(loadOptions);
                ani = DbDataContext.GetInstance().Animations.FirstOrDefault(a => a.ID == ani.ID);

                List<LorealOptimiseData.CustomerAllocation> logs = new List<LorealOptimiseData.CustomerAllocation>();
                foreach (AnimationProduct ap in ani.AnimationProducts)
                {
                    foreach (AnimationProductDetail apd in ap.AnimationProductDetails)
                    {
                        logs.AddRange(apd.CustomerAllocations);
                    }
                }

                //grdEventLog.DataSource = logs;
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Displaying results ...", TaskStatus.InProgress, "Displaying results ..."));
                AllocationResult resultGrid = new AllocationResult();
                resultGrid.Bind(logs, returnValue);
                TabItem ti = new TabItem();
                ti.Header = ani.Name;
                ti.Content = resultGrid;
                tabResults.Items.Add(ti);
            }
            catch (Exception exc)
            {
                Logger.Log(exc.ToString(), LogLevel.Error);
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("RunAllocationException", Utility.GetExceptionsMessages(exc)));
            }
            finally
            {
                DbDataContext.GetInstance().CommandTimeout = Utility.SqlCommandTimeOut;
            }
        }

    }
}
