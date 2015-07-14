using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseData.Enums;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Data;
using System.Collections;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : BaseListUserControl<ProductManager, Product>
    {
        public Products()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                AllowRefreshing = false;
                Loaded += new RoutedEventHandler(Products_Loaded);
                AssignEvents(grdProducts);

                if (grdProducts.View.AllowEditing == false)
                {
                    colOperation.Visible = false;
                }

                // getting sales status with blank
                //Regex regex = new Regex("([A-Z][a-z]*)");
                //string[] enumNames = Enum.GetNames(typeof(ProductSalesStatus));
                //for(int i = 0;i < enumNames.Length; i++)
                //{
                //    enumNames[i] = regex.Replace(enumNames[i], "$0 ");
                //}
                //cboSalesStatus.ItemsSource = enumNames;
            }
        }

        public Products(bool neededNewContext)
            : this()
        {
            this.NeededNewDBContext = neededNewContext;
        }

        protected override Hashtable Filters
        {
            get 
            {
                Hashtable conditions = new Hashtable();

                if (txtDescriptionFilter.Text != String.Empty)
                {
                    conditions.Add(ProductManager.Description, txtDescriptionFilter.Text);
                }

                if (txtMaterialCodeFilter.Text != String.Empty)
                {
                    conditions.Add(ProductManager.MaterialCode, txtMaterialCodeFilter.Text);
                }

                if (txtProcurementTypeFilter.Text != String.Empty)
                {
                    conditions.Add(ProductManager.ProcurementType, txtProcurementTypeFilter.Text);
                }

                return conditions;
            }
        }

        void Products_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            { 
                cboDivisions.ItemsSource = new Division[] { LoggedUser.LoggedDivision };
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            this.Data.Single(p => p.ID == Guid.Empty).Manual = true;   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void TableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            Product p = e.Row as Product;
            if (p != null && p.Manual == false )
            {
                e.Cancel = true;
            }
        }

        private void txtMaterialCodeFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Refresh();
            }
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            if (grdProducts.View.FocusedRow != null)
            {
                int rowHandle = grdProducts.View.FocusedRowHandle;

                MergeProduct mergeControl = new MergeProduct(grdProducts.View.FocusedRow as Product);
                PopupWindow mergetDialog = new PopupWindow("Merge Product");
                mergeControl.Close += new EventHandler(mergetDialog.CloseWindowEvent);
                mergetDialog.AddControl(mergeControl);
                mergetDialog.ShowDialog();

                if (mergeControl.RemovedSAPProduct != null)
                {
                    int pos = this.Data.IndexOf(mergeControl.RemovedSAPProduct);
                    if (pos >= 0)
                        (grdProducts.View as TableView).DeleteRow(grdProducts.GetRowHandleByListIndex(pos));
                }
            }
        }

        private void btnReplace_Initialized(object sender, EventArgs e)
        {
            EditGridCellData gridCellData = (sender as Button).DataContext as EditGridCellData;
            if (gridCellData != null)
            {
                RowTypeDescriptor rowType = gridCellData.Data as RowTypeDescriptor;
                if (rowType != null)
                {
                    if (rowType.RowHandle.Value == GridControl.NewItemRowHandle)
                    {
                        // new row 
                        (sender as FrameworkElement).Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void btnEditMultiples_Click(object sender, RoutedEventArgs e)
        {
            if (grdProducts.View.FocusedRow != null)
            {
                int rowHandle = grdProducts.View.FocusedRowHandle;

                EditProductMultiples editMultiplesControl = new EditProductMultiples(grdProducts.View.FocusedRow as Product);
                PopupWindow editMultiplesDialog = new PopupWindow("Edit Multiples");
                editMultiplesDialog.AddControl(editMultiplesControl);
                editMultiplesDialog.ShowDialog();
            }
        }
       
    }
}
