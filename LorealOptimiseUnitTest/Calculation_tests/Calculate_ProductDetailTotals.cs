using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LorealOptimiseData;

namespace LorealOptimiseUnitTest
{
    [TestFixture]
    class Calculate_ProductDetailTotals : TestBase
    {
        [Test]
        public void Allocation()
        {
            DeleteData();            
            
            // Divisions            
            Division division1 = CreateDivision();
            division1.Name = "DIVISIO X";
            Db.Divisions.InsertOnSubmit(division1);
            Db.SubmitChanges();

            // Distribution Channel            
            DistributionChannel channel1 = CreateDistributionChannel();
            channel1.Name = "CHANNEL X";
            channel1.Code = "02";
            Db.DistributionChannels.InsertOnSubmit(channel1);
            Db.SubmitChanges();

            // Animation Type            
            AnimationType animationType1 = CreateAnimationType(division1, "PromotionX");
            AnimationType animationType2 = CreateAnimationType(division1, "LaunchX");
            Db.AnimationTypes.InsertOnSubmit(animationType1);
            Db.AnimationTypes.InsertOnSubmit(animationType2);
            Db.SubmitChanges();
            

            // Priorities
            Priority priorityA = CreatePriority(division1, "A");
            Priority priorityB = CreatePriority(division1, "B");
            Priority priorityC = CreatePriority(division1, "C");
            Db.Priorities.InsertOnSubmit(priorityA);
            Db.Priorities.InsertOnSubmit(priorityB);
            Db.Priorities.InsertOnSubmit(priorityC);
            Db.SubmitChanges();

            // Animation            
            Animation animation1 = CreateAnimation(division1, channel1, animationType1);
            animation1.Code = "xxxxxxx";
            Db.Animations.InsertOnSubmit(animation1);
            Db.SubmitChanges();

            // Sales Area           
            SalesArea salesArea = CreateSalesArea(channel1, division1, "0300");
            salesArea.Name = "SALES AREA UK";
            salesArea.RRPToListPriceRate = 2;
            SalesArea salesArea2 = CreateSalesArea(channel1, division1, "0350");
            salesArea2.RRPToListPriceRate = 0;
            salesArea2.Name = "SALES AREA ROI";

            Db.SalesAreas.InsertOnSubmit(salesArea);
            Db.SubmitChanges();

            // Product
            Product product =  CreateProduct(division1, "QWERTY");
            Db.SubmitChanges();

            // Product received
            ProductReceived pr1 = CreateProductReceived(product, 05, 2010, 200);
            ProductReceived pr2 = CreateProductReceived(product, 12, 2009, 100);
            ProductReceived pr3= CreateProductReceived(product, 01, 2010, 400);
            ProductReceived pr4 = CreateProductReceived(product, 05, 2009, 800);
            Db.SubmitChanges();


            // Signature
            Signature signature = CreateSignature(division1);
            Db.Signatures.InsertOnSubmit(signature);
            Db.SubmitChanges();

            // Customer Group            
            CustomerGroup group1 = CreateCustomerGroup(salesArea, "Boots", "45XX45");
            group1.SalesArea = salesArea;        
            CustomerGroup group2 = CreateCustomerGroup(salesArea, "Debenhams", "11232");
            group2.SalesArea = salesArea;
            CustomerGroup group3 = CreateCustomerGroup(salesArea, "John Lewis", "132");
            group3.SalesArea = salesArea2;
            Db.CustomerGroups.InsertOnSubmit(group1);
            Db.CustomerGroups.InsertOnSubmit(group2);
            Db.CustomerGroups.InsertOnSubmit(group3);
            Db.SubmitChanges();

            // Animation Customer Group
            AnimationCustomerGroup animationGroup1 = CreateAnimationCustomerGroup(animation1, group1, division1);
            AnimationCustomerGroup animationGroup2 = CreateAnimationCustomerGroup(animation1, group2, division1);
            AnimationCustomerGroup animationGroup3 = CreateAnimationCustomerGroup(animation1, group3, division1);
            Db.AnimationCustomerGroups.InsertOnSubmit(animationGroup1);
            Db.AnimationCustomerGroups.InsertOnSubmit(animationGroup2);
            Db.AnimationCustomerGroups.InsertOnSubmit(animationGroup3);
            Db.SubmitChanges();
                        

            // Customers     
            Customer customerB1 = CreateCustomer(group1, "Boots 1", "5454", division1);
            customerB1.IncludeInSystem = false;
            Customer customerB2 = CreateCustomer(group1, "Boots 2", "4278", division1);
            Customer customerB3 = CreateCustomer(group1, "Boots 3", "54615", division1);
            customerB3.IDSalesArea_AllocationSalesArea = salesArea2.ID;
            Customer customerB1W = CreateCustomer(group1, "Boots Warehouse 1", "54754", division1);
            Customer customerB2W = CreateCustomer(group1, "Boots Warehouse 2", "34278", division1);
            Customer customerB3W = CreateCustomer(group1, "Boots Warehouse 3", "54644515", division1);
            Customer customerD1 = CreateCustomer(group2, "Debenhams 1", "879487", division1);
            Customer customerD2 = CreateCustomer(group2, "Debenhams 2", "65626", division1);
            Customer customerD1W = CreateCustomer(group2, "Debenhams Warehouse 1", "8797487", division1);
            Customer customerD2W = CreateCustomer(group2, "Debenhams Warehouse 2", "656256", division1);
            Customer customerD3 = CreateCustomer(group2, "Debenhams 3", "42718", division1);
            Customer customerJ1 = CreateCustomer(group3, "John Lewis 1", "54165", division1);
            Customer customerJ2 = CreateCustomer(group3, "John Lewis 2", "879817", division1);
            Customer customerJ3 = CreateCustomer(group3, "John Lewis 3", "65606", division1);
            Customer customerJ1W = CreateCustomer(group3, "John Lewis Warehouse 1", "540165", division1);
            Db.SubmitChanges();

            // BrandAxes
            BrandAxe brandAxeX = CreateBrandAxe(signature, "BrandAxeX X", "12X3");
            BrandAxe brandAxeY = CreateBrandAxe(signature, "BrandAxe XY", "11X23");
            BrandAxe brandAxeZ = CreateBrandAxe(signature, "BrandAxeX Z", "142X3");           
            Db.BrandAxes.InsertOnSubmit(brandAxeX);
            Db.BrandAxes.InsertOnSubmit(brandAxeY);
            Db.BrandAxes.InsertOnSubmit(brandAxeZ);          
            Db.SubmitChanges();

            // Sales - X
            Sale sale11X = CreateSale(customerB1, brandAxeX, 100);
            Sale sale12X = CreateSale(customerB2, brandAxeX, 100);
            Sale sale13X = CreateSale(customerB3, brandAxeX, 100);
            Sale sale21X = CreateSale(customerD1, brandAxeX, 100);
            Sale sale22X = CreateSale(customerD2, brandAxeX, 100);
            Sale sale23X = CreateSale(customerD3, brandAxeX, 100);
            //Sale sale31X = CreateSale(customerJ1, brandAxeX, 100);
            Sale sale32X = CreateSale(customerJ2, brandAxeX, 100);
            Sale sale33X = CreateSale(customerJ3, brandAxeX, 100);          
            Db.SubmitChanges();

            // Sales - Y
            Sale sale11Y = CreateSale(customerB1, brandAxeY, 100);
            Sale sale12Y = CreateSale(customerB2, brandAxeY, 100);
            Sale sale13Y = CreateSale(customerB3, brandAxeY, 100);
            Sale sale21Y = CreateSale(customerD1, brandAxeY, 100);
            Sale sale22Y = CreateSale(customerD2, brandAxeY, 100);
            Sale sale23Y = CreateSale(customerD3, brandAxeY, 100);
            Sale sale31Y = CreateSale(customerJ1, brandAxeY, 100);
            Sale sale32Y = CreateSale(customerJ2, brandAxeY, 100);
            Sale sale33Y = CreateSale(customerJ3, brandAxeY, 100);            
            Db.SubmitChanges();

            // Sales - Z
            Sale sale11Z = CreateSale(customerB1, brandAxeZ, 100);
            Sale sale12Z = CreateSale(customerB2, brandAxeZ, 100);
            Sale sale13Z = CreateSale(customerB3, brandAxeZ, 100);
            Sale sale21Z = CreateSale(customerD1, brandAxeZ, 100);
            Sale sale22Z = CreateSale(customerD2, brandAxeZ, 100);
            Sale sale23Z = CreateSale(customerD3, brandAxeZ, 100);
            Sale sale31Z = CreateSale(customerJ1, brandAxeZ, 100);
            Sale sale32Z = CreateSale(customerJ2, brandAxeZ, 100);
            Sale sale33Z = CreateSale(customerJ3, brandAxeZ, 100);           
            Db.SubmitChanges();

            // Categories
            Category category1 = CreateCategory("Consultant", division1);
            Category category2 = CreateCategory("VIP", division1);
            Category category3 = CreateCategory("Non-Consultant", division1);
            Category category4 = CreateCategory("Flagship", division1);
            Category category5 = CreateCategory("SkinExpert", division1);
            Category category6 = CreateCategory("Warehouse", division1);
            Db.Categories.InsertOnSubmit(category1);
            Db.Categories.InsertOnSubmit(category2);
            Db.Categories.InsertOnSubmit(category3);
            Db.Categories.InsertOnSubmit(category4);
            Db.Categories.InsertOnSubmit(category5);
            Db.Categories.InsertOnSubmit(category6);
            Db.SubmitChanges();

            // Animation Product           
            AnimationProduct animationProduct1 = CreateAnimationProduct(product, division1, animation1, 5, 50, brandAxeX, signature);
            animationProduct1.Category = category1;
            animationProduct1.ConfirmedMADMonth = new DateTime(2009, 12, 19);       
            Db.SubmitChanges();

            // AnimationProductDetaail
            AnimationProductDetail animationProductDetail1 = CreateAnimationProductDetail(animationProduct1, salesArea, 150);
            animationProductDetail1.BDCQuantity = 250;
            animationProductDetail1.ForecastProcQuantity = 150;
            animationProductDetail1.RRP = 10;
            Db.SubmitChanges();

         

            // Customer Capacity - AnimationProduct1
            CustomerCapacity customerCapacity11 = CreateCustomerCapacity(customerB1, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            CustomerCapacity customerCapacity12 = CreateCustomerCapacity(customerB2, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 200);
            CustomerCapacity customerCapacity13 = CreateCustomerCapacity(customerB3, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 400);
            CustomerCapacity customerCapacity21 = CreateCustomerCapacity(customerD1, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 800);
            CustomerCapacity customerCapacity22 = CreateCustomerCapacity(customerD2, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 1600);
            CustomerCapacity customerCapacity23 = CreateCustomerCapacity(customerD3, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 150);
            CustomerCapacity customerCapacity31 = CreateCustomerCapacity(customerJ1, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 300);
            CustomerCapacity customerCapacity32 = CreateCustomerCapacity(customerJ2, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 600);
            CustomerCapacity customerCapacity33 = CreateCustomerCapacity(customerJ3, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 1200);
            CustomerCapacity customerCapacity11W = CreateCustomerCapacity(customerB1W, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 11000);
            CustomerCapacity customerCapacity12W = CreateCustomerCapacity(customerB2W, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 12000);
            CustomerCapacity customerCapacity13W = CreateCustomerCapacity(customerB3W, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 13000);
            CustomerCapacity customerCapacity21W = CreateCustomerCapacity(customerD1W, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 14000);
            CustomerCapacity customerCapacity22W = CreateCustomerCapacity(customerD2W, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 15000);
            CustomerCapacity customerCapacity23W = CreateCustomerCapacity(customerJ1W, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 16000);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity11);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity12);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity13);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity21);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity22);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity23);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity31);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity32);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity33);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity11W);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity12W);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity13W);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity21W);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity22W);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity23W);
            Db.SubmitChanges();

           

            // CustomerCategorie
            CustomerCategory customerCategory1 = CreateCustomerCategory(customerB1W, category1);
            CustomerCategory customerCategory2 = CreateCustomerCategory(customerB2W, category1);
            CustomerCategory customerCategory3 = CreateCustomerCategory(customerB3W, category1);
            CustomerCategory customerCategory4 = CreateCustomerCategory(customerB1, category1);
            CustomerCategory customerCategory5 = CreateCustomerCategory(customerB2, category1);
            CustomerCategory customerCategory6 = CreateCustomerCategory(customerB3, category1);
            CustomerCategory customerCategory7 = CreateCustomerCategory(customerD1W, category1);
            CustomerCategory customerCategory8 = CreateCustomerCategory(customerD2W, category1);
            CustomerCategory customerCategory9 = CreateCustomerCategory(customerD1, category1);
            CustomerCategory customerCategory10 = CreateCustomerCategory(customerD2, category1);
            CustomerCategory customerCategory11 = CreateCustomerCategory(customerD3, category1);
            CustomerCategory customerCategory12 = CreateCustomerCategory(customerJ1, category1);
            CustomerCategory customerCategory13 = CreateCustomerCategory(customerJ2, category1);
            CustomerCategory customerCategory14 = CreateCustomerCategory(customerJ3, category1);
        
            Db.SubmitChanges();
          
            // CustomerGroupItemTypes
            CustomerGroupItemType cgit1 = CreateCustomerGroupItemType(group1, animationProduct1.ItemType, customerB1W);       
            CustomerGroupItemType cgit4 = CreateCustomerGroupItemType(group2, animationProduct1.ItemType, customerD1W);
            CustomerGroupItemType cgit6 = CreateCustomerGroupItemType(group3, animationProduct1.ItemType, customerJ1W);
            Db.SubmitChanges();
            
            int? total = 0;
            total = Db.calculate_ProductDetailTotals(animationProductDetail1.ID);

  
            //Db = new DbDataContext();            
            Assert.AreEqual(4550, total);
            
            // add customer B2 in another Division
            customerB2.IDSalesArea_AllocationSalesArea = salesArea2.ID;           
            Db.SubmitChanges();
            int? total2 = 0;
            total2 = Db.calculate_ProductDetailTotals(animationProductDetail1.ID);         
            Assert.AreEqual(4350, total2);
            
            // exclude customer D2 from system
            customerD2.IncludeInSystem = false;
            Db.SubmitChanges();
            int? total3 = 0;
            total3 = Db.calculate_ProductDetailTotals(animationProductDetail1.ID);
            Assert.AreEqual(2750, total3);

            // add customer J3 into another cataegory
            customerCategory14.Category = category2;
            Db.SubmitChanges();
            int? total4 = 0;
            total4 = Db.calculate_ProductDetailTotals(animationProductDetail1.ID);
            Assert.AreEqual(1550, total4);

            // pri vlozeni Salu se nekotroluji Customer Allocationu, delat to ani nebudeme
            // add Sale for J1 customer
            /*
            Sale sale31X = CreateSale(customerJ1, brandAxeX, 100);
            Db.SubmitChanges();
            int? total5 = 0;
            total5 = Db.calculate_ProductDetailTotals(animationProductDetail1.ID);
            Assert.AreEqual(1850, total5);
            */

            // add INclude false for group J
            group2.IncludeInSystem = false;
            Db.SubmitChanges();
            int? total6 = 0;
            total6 = Db.calculate_ProductDetailTotals(animationProductDetail1.ID);
            Assert.AreEqual(600, total6);
            
        }
    }
}
