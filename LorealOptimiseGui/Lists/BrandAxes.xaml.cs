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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using System.Collections;
using LorealOptimiseBusiness.Exceptions;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for BrandAxes.xaml
    /// </summary>
    public partial class BrandAxes : BaseListUserControl<BrandAxeManager, BrandAxe>
    {
        public BrandAxes()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(BrandAxes_Loaded);
                (grdBrandAxes.View as TableView).ShowingEditor += new ShowingEditorEventHandler(View_ShowingEditor);
                (grdBrandAxes.View as TableView).ValidateRow += new GridRowValidationEventHandler(View_ValidateRow);

                AssignEvents(grdBrandAxes);
            }
        }

        void View_ValidateRow(object sender, GridRowValidationEventArgs e)
        {
            BrandAxe ba = e.Row as BrandAxe;
            if (ba != null)
            {
                try
                {
                    (Manager as BrandAxeManager).Validate(ba);
                }
                catch (LorealValidationException exc)
                {
                    e.SetError(exc.Message);
                    e.IsValid = false;
                }
            }
        }

        void View_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            if (e.Column.FieldName == "Brand" && e.Row != null)
            {
                BrandAxe entity = e.Row as BrandAxe;
                if (entity != null)
                {
                    if (entity.Manual == false)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        void BrandAxes_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                cboSignatures.ItemsSource = SignatureManager.Instance.GetAll();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void colCode_Validate(object sender, GridCellValidationEventArgs e)
        {
            if (e.Row != null)
            {
                //BrandAxe entity = e.Row as BrandAxe;
                //if (Data.Any(b => b.Code == e.Value.ToString() && b.ID != entity.ID))
                //{
                //    e.SetError("There's relady a brand/axe with the same code.");
                //}
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            this.Data.Single(ba => ba.ID == Guid.Empty).Manual = true;
        }
    }
}
