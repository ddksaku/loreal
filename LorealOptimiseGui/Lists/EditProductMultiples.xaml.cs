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
using System.ComponentModel;
using LorealOptimiseData;
using LorealOptimiseBusiness;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for EditProductMultiples.xaml
    /// </summary>
    public partial class EditProductMultiples : BaseUserControl
    {
        private EditProductMultiplesManager manager;

        public EditProductMultiples(Product p)
        {
            InitializeComponent();

            manager = EditProductMultiplesManager.Instance;
            manager.EditEntity = p;

            TableViewEventHandlers<Multiple> eventHandler = new TableViewEventHandlers<Multiple>(grdMultiples, manager);
            eventHandler.AssignEvents();

            Loaded += new RoutedEventHandler(EditProductMultiples_Loaded);
        }

        void EditProductMultiples_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                this.DataContext = manager;
            }
        }

        private void TableView_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            Multiple m = manager.Data.Single(mm => mm.ID == Guid.Empty);
            m.Manual = true;
            m.IDProduct = manager.EditEntity.ID;
        }

    }
}
