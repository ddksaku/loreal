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
using LorealOptimiseBusiness.Lists;
using System.ComponentModel;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using System.Linq.Expressions;

namespace AnimationProductGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private members

        private DbDataContext Db = new DbDataContext();

        private Division division;
        private Animation animation;
        private int count;

        private List<Product> products;
        private int productsCount;

        private List<ItemType> itemtypes;
        private int itemtypesCount;

        private List<ItemGroup> itemgroups;
        private int itemgroupsCount;

        private List<Signature> signatures;
        private int signaturesCount;

        private List<BrandAxe> brandaxesWithSales;
        private List<BrandAxe> brandaxes;
        private int brandaxesCount;

        private List<Category> categories;
        private int categoriesCount;

        private List<Sale> sales;

        #endregion

        protected Expression<Func<T, bool>> DivisionFilter<T>() where T : IDivision
        {
            return d => d.Division.ID == division.ID && d.Division.Deleted == false;
        }

        public MainWindow()
        {
            InitializeComponent();

            cboDivisions.ItemsSource = Db.Divisions.OrderBy(d=>d.Name).ToList();

            brandaxes = Db.BrandAxes.ToList();
        }

        private void cboDivisions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            division = cboDivisions.SelectedItem as Division;

            // get animations
            cboAnimations.ItemsSource = Db.Animations.Where(ani => ani.IDDivision == division.ID && ani.Status == 1).OrderBy(ani=>ani.Name).ToList();

            // get lists
            products = Db.Products.Where(DivisionFilter<Product>()).ToList();
            productsCount = products.Count - 1;

            itemgroups = Db.ItemGroups.Where(DivisionFilter<ItemGroup>()).ToList();
            itemgroupsCount = itemgroups.Count - 1;

            itemtypes = Db.ItemTypes.Where(DivisionFilter<ItemType>()).ToList();

            // sales = Db.Sales.Where(s=>s.Customer.SalesArea.IDDivision == division.ID && s.BrandAxe.Signature.IDDivision == division.ID).ToList();
            brandaxesWithSales = brandaxes.Where(ba => ba.Signature.IDDivision == division.ID && Db.Sales.Any(sa => sa.IDBrandAxe == ba.ID)).ToList();
            signatures = brandaxesWithSales.Select(ba => ba.Signature).ToList();
            signaturesCount = signatures.Count - 1;
            

            categories = Db.Categories.Where(DivisionFilter<Category>()).ToList();
            categoriesCount = categories.Count - 1;
        }

        private void cboAnimations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            animation = cboAnimations.SelectedItem as Animation;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            count = 10;
            int.TryParse(txtCount.Text, out count);

            
            
            generateAnimationProducts();
        }

        // generator
        void generateAnimationProducts()
        {
            if (animation == null)
            {
                MessageBox.Show("Select an animation.");
                return;
            }

            if (count == 0)
            {
                MessageBox.Show("Input count of animation products correctly.");
                return;
            }

            List<ItemType> reducedItemTypes = itemtypes.Where(it => Db.CustomerCapacities.Any(cc => cc.Capacity > 0 && cc.IDAnimationType == animation.IDAnimationType && cc.IDPriority == animation.IDPriority && cc.Customer.SalesArea.IDDivision == division.ID && cc.Customer.IncludeInSystem == true)).ToList();
            itemtypesCount = reducedItemTypes.Count - 1;

            if (itemtypesCount < 0)
            {
                MessageBox.Show("There is no proper Item Type for this animation.");
                return;
            }
            if (productsCount < 0)
            {
                MessageBox.Show("There is no product for this animation.");
                return;
            }
            if (itemgroupsCount < 0)
            {
                MessageBox.Show("There is no Item Group for this animation.");
                return;
            }
            if (categoriesCount < 0)
            {
                MessageBox.Show("There is no Category for this animation.");
                return;
            }
            if (brandaxesWithSales.Count == 0)
            {
                MessageBox.Show("There is no proper Brand/Axes for this animation.");
                return;
            }
            
            LoggedUser.SetInstance(Db.Users.Where(u=>u.Name.StartsWith("Beth COOK")).FirstOrDefault(), Db.Divisions.Where(d=>d.Name.StartsWith("Prestige")).FirstOrDefault());

            this.Cursor = Cursors.Wait;
            Random random = new Random();
            int counter = count;
            while (counter > 0)
            {
                AnimationProduct ap = new AnimationProduct();

                ap.IDAnimation = animation.ID;
                ap.IDItemGroup = itemgroups[random.Next(itemgroupsCount)].ID;
                ap.IDProduct = products[random.Next(productsCount)].ID;
                ap.IDItemType = reducedItemTypes[random.Next(itemtypesCount)].ID;
                ap.IDCategory = categories[random.Next(categoriesCount)].ID;
                ap.IDSignature = signatures[random.Next(signaturesCount)].ID;

                List<BrandAxe> reducedbrandaxes = brandaxesWithSales.Where(b => b.IDSignature == ap.IDSignature).ToList();
                brandaxesCount = reducedbrandaxes.Count - 1;
                if (brandaxesCount < 0)
                {
                    continue;
                }
                ap.IDBrandAxe = reducedbrandaxes[random.Next(brandaxesCount)].ID;
                ap.ConfirmedMADMonth = DateTime.Now;

                Db.AnimationProducts.InsertOnSubmit(ap);
                counter--;
            }

            Db.SubmitChanges();

            this.Cursor = Cursors.Arrow;

            MessageBox.Show(string.Format("Generated {0} animation products successfully", count));
        }

    }
}
