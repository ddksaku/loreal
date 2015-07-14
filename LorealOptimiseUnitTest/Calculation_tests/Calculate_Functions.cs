using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LorealOptimiseData;

namespace LorealOptimiseUnitTest
{
    [TestFixture]
    class CalculateTotals_Test : TestBase
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
            group2.IncludeInSystem = false;
            CustomerGroup group3 = CreateCustomerGroup(salesArea, "John Lewis", "132");
            group3.SalesArea = salesArea;
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
            Db.Customers.InsertOnSubmit(customerB1);
            Db.Customers.InsertOnSubmit(customerB2);
            Db.Customers.InsertOnSubmit(customerB3);
            Db.Customers.InsertOnSubmit(customerB1W);
            Db.Customers.InsertOnSubmit(customerB2W);
            Db.Customers.InsertOnSubmit(customerB3W);
            Db.Customers.InsertOnSubmit(customerD1);
            Db.Customers.InsertOnSubmit(customerD2);
            Db.Customers.InsertOnSubmit(customerD3);
            Db.Customers.InsertOnSubmit(customerD1W);
            Db.Customers.InsertOnSubmit(customerD2W);
            Db.Customers.InsertOnSubmit(customerJ1);
            Db.Customers.InsertOnSubmit(customerJ2);
            Db.Customers.InsertOnSubmit(customerJ3);
            Db.Customers.InsertOnSubmit(customerJ1W);
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
            Sale sale31X = CreateSale(customerJ1, brandAxeX, 100);
            Sale sale32X = CreateSale(customerJ2, brandAxeX, 100);
            Sale sale33X = CreateSale(customerJ3, brandAxeX, 100);
            Db.Sales.InsertOnSubmit(sale11X);
            Db.Sales.InsertOnSubmit(sale12X);
            Db.Sales.InsertOnSubmit(sale13X);
            Db.Sales.InsertOnSubmit(sale21X);
            Db.Sales.InsertOnSubmit(sale22X);
            Db.Sales.InsertOnSubmit(sale23X);
            Db.Sales.InsertOnSubmit(sale31X);
            Db.Sales.InsertOnSubmit(sale32X);
            Db.Sales.InsertOnSubmit(sale33X);
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
            Db.Sales.InsertOnSubmit(sale11Y);
            Db.Sales.InsertOnSubmit(sale12Y);
            Db.Sales.InsertOnSubmit(sale13Y);
            Db.Sales.InsertOnSubmit(sale21Y);
            Db.Sales.InsertOnSubmit(sale22Y);
            Db.Sales.InsertOnSubmit(sale23Y);
            Db.Sales.InsertOnSubmit(sale31Y);
            Db.Sales.InsertOnSubmit(sale32Y);
            Db.Sales.InsertOnSubmit(sale33Y);
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
            Db.Sales.InsertOnSubmit(sale11Z);
            Db.Sales.InsertOnSubmit(sale12Z);
            Db.Sales.InsertOnSubmit(sale13Z);
            Db.Sales.InsertOnSubmit(sale21Z);
            Db.Sales.InsertOnSubmit(sale22Z);
            Db.Sales.InsertOnSubmit(sale23Z);
            Db.Sales.InsertOnSubmit(sale31Z);
            Db.Sales.InsertOnSubmit(sale32Z);
            Db.Sales.InsertOnSubmit(sale33Z);
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
         //   AnimationProduct animationProduct2 = CreateAnimationProduct(division1, animation1, 5, 50, signature);
           // AnimationProduct animationProduct3 = CreateAnimationProduct(division1, animation1, 5, 50, brandAxeX, signature);
            Db.AnimationProducts.InsertOnSubmit(animationProduct1);
           // Db.AnimationProducts.InsertOnSubmit(animationProduct2);
           // Db.AnimationProducts.InsertOnSubmit(animationProduct3);
            Db.SubmitChanges();

            // AnimationProductDetaail
            AnimationProductDetail animationProductDetail1 = CreateAnimationProductDetail(animationProduct1, salesArea, 150);
            animationProductDetail1.BDCQuantity = 250;
            animationProductDetail1.ForecastProcQuantity = 150;
            animationProductDetail1.RRP = 10;            
            AnimationProductDetail animationProductDetail2 = CreateAnimationProductDetail(animationProduct1, salesArea2, 500);
            AnimationProductDetail animationProductDetail3 = CreateAnimationProductDetail(animationProduct1, salesArea2);
            animationProductDetail3.BDCQuantity = 100;
            animationProductDetail3.ForecastProcQuantity = 50;
            animationProductDetail3.RRP = 200;
            AnimationProductDetail animationProductDetail4 = CreateAnimationProductDetail(animationProduct1, salesArea, 300);
            animationProductDetail4.RRP = 250;
            Db.SubmitChanges();

            // CustomerGroupAllocation
            CustomerGroupAllocation customerGroupAllocation1 = CreateCustomerGroupAllocation(group1, animationProductDetail1);
            customerGroupAllocation1.RetailUplift = 10;
            CustomerGroupAllocation customerGroupAllocation2 = CreateCustomerGroupAllocation(group2, animationProductDetail1);
            customerGroupAllocation2.RetailUplift = 5;
            CustomerGroupAllocation customerGroupAllocation3 = CreateCustomerGroupAllocation(group3, animationProductDetail1);
            customerGroupAllocation3.RetailUplift = 2;
            Db.SubmitChanges();

            //CustomerAllocations
            CustomerAllocation customerAllocation1 = CreateCustomerAllocation(customerB1, animationProductDetail1);
            customerAllocation1.RetailUplift = 0;
            CustomerAllocation customerAllocation2 = CreateCustomerAllocation(customerD1, animationProductDetail1);
            customerAllocation2.RetailUplift = 3;
            CustomerAllocation customerAllocation3 = CreateCustomerAllocation(customerJ1, animationProductDetail1);
            customerAllocation3.RetailUplift = 5;


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
            CustomerCategory customerCategory1 = CreateCustomerCategory(customerB1W, category6);
            CustomerCategory customerCategory2 = CreateCustomerCategory(customerB2W, category6);
            CustomerCategory customerCategory3 = CreateCustomerCategory(customerB3W, category6);
            CustomerCategory customerCategory4 = CreateCustomerCategory(customerB1, category1);
            CustomerCategory customerCategory5 = CreateCustomerCategory(customerB1, category2);
            CustomerCategory customerCategory6 = CreateCustomerCategory(customerB2, category3);
            CustomerCategory customerCategory7 = CreateCustomerCategory(customerD1W, category6);
            CustomerCategory customerCategory8 = CreateCustomerCategory(customerD2W, category6);
            CustomerCategory customerCategory9 = CreateCustomerCategory(customerD1, category1);
            CustomerCategory customerCategory10 = CreateCustomerCategory(customerD1, category4);
            CustomerCategory customerCategory11 = CreateCustomerCategory(customerD3, category3);
            CustomerCategory customerCategory12 = CreateCustomerCategory(customerD3, category2);
            CustomerCategory customerCategory13 = CreateCustomerCategory(customerJ2, category5);
            CustomerCategory customerCategory14 = CreateCustomerCategory(customerJ2, category4);
            CustomerCategory customerCategory15 = CreateCustomerCategory(customerJ3, category1);
            Db.SubmitChanges();
          
            // CustomerGroupItemTypes
            CustomerGroupItemType cgit1 = CreateCustomerGroupItemType(group1, animationProduct1.ItemType, customerB1W);
          //  CustomerGroupItemType cgit2 = CreateCustomerGroupItemType(group1, animationProduct2.ItemType, customerB2W);
            //CustomerGroupItemType cgit3 = CreateCustomerGroupItemType(group1, animationProduct3.ItemType, customerB3W);
            CustomerGroupItemType cgit4 = CreateCustomerGroupItemType(group2, animationProduct1.ItemType, customerD1W);
            //CustomerGroupItemType cgit5 = CreateCustomerGroupItemType(group2, animationProduct2.ItemType, customerD2W);
            CustomerGroupItemType cgit6 = CreateCustomerGroupItemType(group3, animationProduct1.ItemType, customerJ1W);
            Db.SubmitChanges();

          
          
            int? totalCapacity = null;
            int? totalBDCQuantity = null;
            int? totalForecastQuantity = null;
            int? totalAllocationQuantity = null;
            int? totalAllocationUK = null;
            int? totalAllocationROI = null;
            int? rrpUK = null;
            int? rrpROI = null;
            double? listPriceUK = null;
            double? listPriceROI = null;
            int? receivedToDate = null;
            int? activeAnimations = null;

            totalCapacity = Db.calculate_TotalCapacity(animationProduct1.ID);
            totalBDCQuantity = Db.calculate_TotalBDCQuantity(animationProduct1.ID);
            totalForecastQuantity = Db.calculate_TotalForecastQuantity(animationProduct1.ID);
            totalAllocationQuantity = Db.calculate_TotalAllocation(animationProduct1.ID);
            totalAllocationUK = Db.calculate_TotalAllocationUK(animationProduct1.ID, division1.ID);
            totalAllocationROI = Db.calculate_TotalAllocationROI(animationProduct1.ID, division1.ID);
            rrpUK = Db.calculate_RRP_UK(animationProduct1.ID, division1.ID);
            rrpROI = Db.calculate_RRP_ROI(animationProduct1.ID, division1.ID);
            listPriceUK = Db.calculate_ListPriceUK(animationProduct1.ID, division1.ID);
            listPriceROI = Db.calculate_ListPriceROI(animationProduct1.ID, division1.ID);
            receivedToDate = Db.calculate_ProductReceived(animationProduct1.ID);
            activeAnimations = Db.calculate_ActiveAnimations(animationProduct1.ID);
            

            Assert.AreEqual(54700, totalCapacity);
            Assert.AreEqual(350, totalBDCQuantity);
            Assert.AreEqual(200, totalForecastQuantity);
            Assert.AreEqual(950, totalAllocationQuantity);
            Assert.AreEqual(450, totalAllocationUK);
            Assert.AreEqual(500, totalAllocationROI);
            Assert.AreEqual(260, rrpUK);
            Assert.AreEqual(200, rrpROI);
            Assert.AreEqual(920, listPriceUK);
            Assert.AreEqual(0, listPriceROI);
            Assert.AreEqual(700, receivedToDate);
            Assert.AreEqual(1, activeAnimations);


            // include group Debenhams in system
            group2.IncludeInSystem = true;
            Db.SubmitChanges();
            totalCapacity = Db.calculate_TotalCapacity(animationProduct1.ID);
            Assert.AreEqual(86250, totalCapacity);

            
            // exclude brandaxe for J1W customer
            CustomerBrandExclusion cbe1 = CreateCustomerBrandExclusion(customerJ1W, brandAxeX);
            Db.SubmitChanges();
            totalCapacity = Db.calculate_TotalCapacity(animationProduct1.ID);
            Assert.AreEqual(70250, totalCapacity);


            pr4.Month = 12;
            Db.SubmitChanges();
            receivedToDate = Db.calculate_ProductReceived(animationProduct1.ID);
            Assert.AreEqual(1500, receivedToDate);

            pr3.Year = 2008;
            Db.SubmitChanges();
            receivedToDate = Db.calculate_ProductReceived(animationProduct1.ID);
            Assert.AreEqual(1100, receivedToDate);

        }
    }
}
