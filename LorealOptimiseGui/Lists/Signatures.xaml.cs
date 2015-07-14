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

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for Signatures.xaml
    /// </summary>
    public partial class Signatures : BaseListUserControl<SignatureManager, Signature>
    {
        public Signatures()
            : base()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(Signatures_Loaded);
                AssignEvents(grdSignatures);
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        void Signatures_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                cboDivisions.ItemsSource = new Division[] { LoggedUser.LoggedDivision };
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
                Signature entity = e.Row as Signature;
                if (Data.Any(s => s.Code == e.Value.ToString() && s.ID != entity.ID))
                {
                    e.SetError("There's already a signature with the same code.");
                }
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            Signature signature = this.Data.Single(s => s.ID == Guid.Empty);
            signature.Manual = true;
        }

    }
}
