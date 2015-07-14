using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Data;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using LorealOptimiseGui.Lists;
using LorealOptimiseBusiness.ViewMode;
using LorealOptimiseShared.Logging;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for CreateNewProduct.xaml
    /// </summary>
    public partial class CreateNewProduct : BaseUserControl
    {
        public event EventHandler Close;

        public CreateNewProduct()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        #region Validation
        private void txtMaterialCode_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null || e.Value.ToString().Trim() == "")
            {
                e.SetError("Material code cannot be empty.");
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
                return;
            }

            if (e.Value != null && ProductManager.Instance.GetByMaterialCode(e.Value.ToString()) != null)
            {
                e.SetError("There's already a product with this material code. Input new material code not matching existing ones.");
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            }
        }

        private void txtProductDesc_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            //if (e.Value == null || e.Value.ToString().Trim() == "")
            //{
            //    e.SetError("Product description cannot be empty.");
            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            //    return;
            //}

            //if (e.Value != null && ProductManager.Instance.GetByDescription(e.Value.ToString()) != null)
            //{
            //    e.SetError("There's already a product with this product description. Input new product description not matching existing ones.");
            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            //}

        }

        private void txtNormalMultiple_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString()))
            {
                e.SetError("Invalid Normal Multiple.");
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
                return;
            }

            int v;
            if (int.TryParse(e.Value.ToString(), out v))
            {
                if (v <= 0)
                {
                    e.SetError("Invalid Normal Multiple.");
                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
                }
            }
            else
            {
                e.SetError("Invalid Normal Multiple.");
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            }
            
        }
        #endregion

        #region DataContext

        private Product newProduct = new Product();

        public Product NewProduct
        {
            get
            {
                return newProduct;
            }
        }
        public int NormalMultiple
        {
            get;
            set;
        }
        public int WarehouseMultiple
        {
            get;
            set;
        }

        #region Commands
        private ICommand saveAndNewCommand;
        private ICommand saveAndCloseCommand;
        public ICommand SaveAndNewCommand
        {
            get
            {
                if (saveAndNewCommand == null)
                {
                    saveAndNewCommand = new RelayCommand(param => CanSave(), parm => SaveAndNew());
                }
                return saveAndNewCommand;
            }
        }
        public ICommand SaveAndCloseCommand
        {
            get
            {
                if (saveAndCloseCommand == null)
                {
                    saveAndCloseCommand = new RelayCommand(param => CanSave(), parm => SaveAndClose());
                }
                return saveAndCloseCommand;
            }
        }

        private bool IsNullOrEmpty(object obj)
        {
            if(obj == null)
                return true;
            return string.IsNullOrEmpty(obj.ToString().Trim());
        }

        private bool CanSave()
        {
            if (IsNullOrEmpty(newProduct.MaterialCode) || IsNullOrEmpty(NormalMultiple))
                return false;
            int v;
            if(int.TryParse(NormalMultiple.ToString(), out v))
                if(v <= 0)
                    return false;
            return true;
        }

        private void SaveAndNew()
        {
            Save();

            newProduct = new Product();
            this.NormalMultiple = 0;
            this.WarehouseMultiple = 0;
            this.txtCIV.Text = "";
            this.txtEAN.Text = "";
            this.txtInternationalCode.Text = "";
            
            this.DataContext = null;
            this.DataContext = this;
        }

        private void SaveAndClose()
        {
            Save();
            if (Close != null)
                Close(null, null);
        }

        private void Save()
        {
            newProduct.IDDivision = LoggedUser.LoggedDivision.ID;
            newProduct.Status = "";
            newProduct.Manual = true;
            newProduct.ProcurementType = "";
            if (newProduct.EAN == null)
                newProduct.EAN = "";
            if (newProduct.InternationalCode == null)
                newProduct.InternationalCode = "";

            Multiple normalMultiple = new Multiple();
            normalMultiple.Value = NormalMultiple;
            normalMultiple.Product = newProduct;
            normalMultiple.Manual = true;

            if (WarehouseMultiple > 0 && WarehouseMultiple != NormalMultiple)
            {
                Multiple warehouseMultiple = new Multiple();
                warehouseMultiple.Value = WarehouseMultiple;
                warehouseMultiple.Product = newProduct;
                warehouseMultiple.Manual = true;
            }

            ProductManager.Instance.InsertOrUpdate(newProduct);

            MultipleManager.Instance.Refresh();
        }

        #endregion

        #endregion
    }
}
