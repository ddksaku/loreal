using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LorealOptimiseBusiness.ViewMode;
using LorealOptimiseBusiness;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.Xpf.Grid;
using LorealOptimiseData;
using LorealOptimiseData.Enums;
using System.Collections.ObjectModel;
using LorealOptimiseBusiness.Lists;
using System.Collections;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for Divisions.xaml
    /// </summary>
    public partial class Divisions : BaseListUserControl<DivisionManager, Division>
    {
        public Divisions()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                grdDivision.View.PreviewKeyDown += new KeyEventHandler(View_PreviewKeyDown);

                AssignEvents(grdDivision);
                Loaded += new RoutedEventHandler(Divisions_Loaded);
            }
        }

        void View_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (grdDivision.View.IsEditing == false && e.Key == Key.Delete)
            {
                if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin) == false)
                {
                    e.Handled = true;
                }
            }
        }

        void Divisions_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
            {
                grdDivision.View.AllowDrop = true;
                clmCode.ReadOnly = false;
                clmName.ReadOnly = false;
                
            }
            else
            {
                
                (grdDivision.View as TableView).NewItemRowPosition = NewItemRowPosition.None;
                clmCode.ReadOnly = true;
                clmName.ReadOnly = true;                 
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                Hashtable conditions = new Hashtable();

                if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
                {  
                    return conditions;
                }
                if (LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
                {
                    conditions.Add(DivisionManager.IDDivision, LoggedUser.LoggedDivision.ID);
                }


                return conditions;
            }
        }
        
        private void LoadDivision()
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        
    }
}
