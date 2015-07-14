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
using LorealOptimiseGui.Lists;
using LorealOptimiseBusiness;
using LorealOptimiseData;
using LorealOptimiseData.Enums;
using DevExpress.Xpf.Grid;
using System.Data.Linq;

namespace LorealOptimiseGui.Controls
{

    /// <summary>
    /// Interaction logic for AnimationList.xaml
    /// </summary>
    public partial class AnimationList : BaseUserControl
    {
        public delegate void EditAnimationHandler(object sender, Animation animation);

        public event EventHandler NewAnimation;
        public event EditAnimationHandler EditAnimation;

        private AnimationManager animationManager = AnimationManager.GetInstance();
        private bool isDivisionAdmin = LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin);

        public AnimationList()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(AnimationList_Loaded);
        }

        void AnimationList_Loaded(object sender, RoutedEventArgs e)
        {
            grdAnimations.DataContext = animationManager;

            DataLoadOptions loadOptions = new DataLoadOptions();
            loadOptions.LoadWith<Animation>(a => a.AnimationType);
            loadOptions.LoadWith<Animation>(a => a.Priority);
            loadOptions.LoadWith<Animation>(a => a.SalesDrive);

            DbDataContext.MakeNewInstance(loadOptions);
            animationManager.AllAnimations = null;
        }

        private void btnNewAnimation_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;

            if (NewAnimation != null)
            {
                NewAnimation(sender, e);
            }
        }

        private void TableView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DbDataContext.MakeNewInstance();
            int rowHandle = grdAnimations.View.GetRowHandleByMouseEventArgs(e);
            Animation animation = grdAnimations.GetRow(rowHandle) as Animation;

            if (animation != null && EditAnimation != null)
            {
                animation = AnimationManager.GetInstance().GetByID(animation.ID);
                EditAnimation(this, animation);
            }
        }

        private void TableView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TableView tableView = sender as TableView;
            if (e.Key == Key.Delete && tableView.IsEditing == false)
            {
                if (tableView.SelectedRows.Count > 0)
                {
                    Animation animation = tableView.SelectedRows[0] as Animation;

                    if (this.isDivisionAdmin == false)
                    {
                        //MessageBox.Show("Animation can be deleted only by Division Administrator.");
                        MessageBox.Show(SystemMessagesManager.Instance.GetMessage("AnimationListDeleteNoAdmin"));
                        return;
                    }

                    if (animation.Status != null)
                    {
                        byte status = animation.Status.Value;
                        bool isActiveStatus = (status == (byte)AnimationStatus.Open) || (status == (byte)AnimationStatus.Locked) || (status == (byte)AnimationStatus.Draft) || (status == (byte)AnimationStatus.Published);
                        if (isActiveStatus == false)
                        {
                            //MessageBox.Show("Closed or cleared animations cannot be deleted.");
                            MessageBox.Show(SystemMessagesManager.Instance.GetMessage("AnimationListDeleteClearedClosed"));
                            return;
                        }
                    }

                    animationManager.Delete(animation);
                    tableView.DeleteRow(tableView.GetSelectedRowHandles()[0]);
                }
            }
        }
    }
}
