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
using System.Collections.ObjectModel;
using LorealOptimiseData;
using LorealOptimiseBusiness.Lists;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for MergeProduct.xaml
    /// </summary>
    public partial class MergeProduct : BaseUserControl
    {
        private Product dummyProduct = null;
        private Product SAPProduct = null;

        public event EventHandler Close = null;

        public MergeProduct(Product p)
        {
            InitializeComponent();

            dummyProduct = p;

            txtDescription.Text = p.Description;
            txtMaterialCode.Text = p.MaterialCode;

            this.DataContext = ProductManager.Instance.GetAll().Where(pp => pp.Manual == false);
            cboDivisions.ItemsSource = DivisionManager.Instance.GetAll();
        }

        // result of Merging
        private Product removedSAPProduct = null;
        public Product RemovedSAPProduct
        {
            get
            {
                return removedSAPProduct;
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (grdProducts.View.FocusedRow != null)
            {
                SAPProduct = grdProducts.GetFocusedRow() as Product;
            }
        }

        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {
            if (SAPProduct == null)
            {
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("MergeProductSelectProduct"));
                return;
            }

            if (SAPProduct.AnimationProducts.Any(ap=>ap.Animation != null && ap.Animation.IsActive) == true)
            {
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("MergeProductAttachedToLiveAnimation"));
                return;
            }

            try
            {
                dummyProduct.Manual = false;
                dummyProduct.MaterialCode = SAPProduct.MaterialCode;
                dummyProduct.Description = SAPProduct.Description;
                dummyProduct.InternationalCode = SAPProduct.InternationalCode;
                dummyProduct.EAN = SAPProduct.EAN;
                dummyProduct.Status = SAPProduct.Status;
                dummyProduct.ProcurementType = SAPProduct.ProcurementType;
                dummyProduct.CIV = SAPProduct.CIV;
                dummyProduct.Stock = SAPProduct.Stock;
                dummyProduct.StockLessPipe = SAPProduct.StockLessPipe;
                dummyProduct.InTransit = SAPProduct.InTransit;
                

                DbDataContext.GetInstance().up_mergeProducts(dummyProduct.ID, SAPProduct.ID);
                MultipleManager.Instance.Refresh();
                ProductManager manager = ProductManager.Instance;

                // insert
                manager.InsertOrUpdate(dummyProduct);

                // delete
                removedSAPProduct = SAPProduct;
                manager.Delete(SAPProduct);

                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("MergeProductMerged"), "Merge Product");

                btnClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            catch (Exception exc)
            {
                removedSAPProduct = null;
                //MessageBox.Show("An error occured when replacing the dummy store with a SAP store: " + LorealOptimiseShared.Utility.GetExceptionsMessages(exc));
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("MergeProductException", LorealOptimiseShared.Utility.GetExceptionsMessages(exc)));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Close != null)
            {
                Close(this, new EventArgs());
            }
        }
    }
}
