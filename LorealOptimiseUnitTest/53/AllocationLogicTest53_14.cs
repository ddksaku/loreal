using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LorealOptimiseData;

namespace LorealOptimiseUnitTest
{
    [TestFixture]
    class AllocationLogicTest53_14 : TestBase
    {
        [Test]
        public void Allocation()
        {
            DeleteData();

            // Divisions            
            Division division1 = CreateDivision();
            Db.Divisions.InsertOnSubmit(division1);
            Db.SubmitChanges();

            // Distribution Channel            
            DistributionChannel channel1 = CreateDistributionChannel();
            Db.DistributionChannels.InsertOnSubmit(channel1);
            Db.SubmitChanges();

            // Animation Type            
            AnimationType animationType1 = CreateAnimationType(division1);
            Db.AnimationTypes.InsertOnSubmit(animationType1);
            Db.SubmitChanges();

            // Animation            
            Animation animation1 = CreateAnimation(division1, channel1, animationType1);
            Db.Animations.InsertOnSubmit(animation1);
            Db.SubmitChanges();

            // Sales Area           
            SalesArea salesArea = CreateSalesArea(channel1, division1);
            Db.SalesAreas.InsertOnSubmit(salesArea);
            Db.SubmitChanges();

            // Signature
            Signature signature = CreateSignature(division1);

            // Brand Axe
            BrandAxe brandAxe = CreateBrandAxe(signature, "BrandAxe", "BA");

            // Category
            Category category1 = CreateCategory("Category 1", division1);
            Db.Categories.InsertOnSubmit(category1);
            Db.SubmitChanges();

            // ItemType
            ItemType itemType = CreateItemType(division1);
            Db.SubmitChanges();

            // Customer Group            
            CustomerGroup group1 = CreateCustomerGroup(salesArea, "Boots", "4545");
            group1.SalesArea = salesArea;
            CustomerGroup group2 = CreateCustomerGroup(salesArea, "Debenhams", "11232");
            group2.SalesArea = salesArea;
            CustomerGroup group3 = CreateCustomerGroup(salesArea, "John Lewis", "1232");
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
            Customer customerB1 = CreateCustomer(group1, "Boots 1", "5454", division1, category1);
            Customer customerB2 = CreateCustomer(group1, "Boots 2", "4278", division1, category1);
            Customer customerB3 = CreateCustomer(group1, "Boots 3", "54615", division1, category1);
            Customer customerD1 = CreateCustomer(group2, "Debenhams 1", "879487", division1, category1);
            Customer customerD2 = CreateCustomer(group2, "Debenhams 2", "65626", division1, category1);
            Customer customerD3 = CreateCustomer(group2, "Debenhams 3", "42718", division1, category1);
            Customer customerJ1 = CreateCustomer(group3, "John Lewis 1", "54165", division1, category1);
            Customer customerJ2 = CreateCustomer(group3, "John Lewis 2", "879817", division1, category1);
            Customer customerJ3 = CreateCustomer(group3, "John Lewis 3", "65606", division1, category1);
            Db.SubmitChanges();



            // Customer Capacity
            CustomerCapacity customerCapacity11 = CreateCustomerCapacity(customerB1, animation1.AnimationType, animation1.Priority, itemType, 9999);
            CustomerCapacity customerCapacity12 = CreateCustomerCapacity(customerB2, animation1.AnimationType, animation1.Priority, itemType, 9999);
            CustomerCapacity customerCapacity13 = CreateCustomerCapacity(customerB3, animation1.AnimationType, animation1.Priority, itemType, 9999);
            CustomerCapacity customerCapacity21 = CreateCustomerCapacity(customerD1, animation1.AnimationType, animation1.Priority, itemType, 9999);
            CustomerCapacity customerCapacity22 = CreateCustomerCapacity(customerD2, animation1.AnimationType, animation1.Priority, itemType, 9999);
            CustomerCapacity customerCapacity23 = CreateCustomerCapacity(customerD3, animation1.AnimationType, animation1.Priority, itemType, 9999);
            CustomerCapacity customerCapacity31 = CreateCustomerCapacity(customerJ1, animation1.AnimationType, animation1.Priority, itemType, 9999);
            CustomerCapacity customerCapacity32 = CreateCustomerCapacity(customerJ2, animation1.AnimationType, animation1.Priority, itemType, 9999);
            CustomerCapacity customerCapacity33 = CreateCustomerCapacity(customerJ3, animation1.AnimationType, animation1.Priority, itemType, 9999);
            Db.SubmitChanges();



            // Sales
            Sale sale11 = CreateSale(customerB1, brandAxe, 100);
            Sale sale12 = CreateSale(customerB2, brandAxe, 100);
            Sale sale13 = CreateSale(customerB3, brandAxe, 100);
            Sale sale21 = CreateSale(customerD1, brandAxe, 100);
            Sale sale22 = CreateSale(customerD2, brandAxe, 100);
            Sale sale23 = CreateSale(customerD3, brandAxe, 100);
            Sale sale31 = CreateSale(customerJ1, brandAxe, 100);
            Sale sale32 = CreateSale(customerJ2, brandAxe, 100);
            Sale sale33 = CreateSale(customerJ3, brandAxe, 100);
            Db.SubmitChanges();


            // Animation Product           
            AnimationProduct animationProduct1 = CreateAnimationProduct(division1, animation1, 1, 1, category1, brandAxe, itemType);
            Db.AnimationProducts.InsertOnSubmit(animationProduct1);
            Db.SubmitChanges();


            // update allocations
            CustomerGroupAllocation group1allocation = (from p in Db.CustomerGroupAllocations
                                                        where p.CustomerGroup == group1
                                                        select p).Single();

            CustomerGroupAllocation group2allocation = (from p in Db.CustomerGroupAllocations
                                                        where p.CustomerGroup == group2
                                                        select p).Single();

            CustomerGroupAllocation group3allocation = (from p in Db.CustomerGroupAllocations
                                                        where p.CustomerGroup == group3
                                                        select p).Single();


           
            group1allocation.RetailUplift = 3;
           


            Db.SubmitChanges();

            // update allocation quantity
            AnimationProductDetail apd = (from p in Db.AnimationProductDetails
                                          select p).Single();
            apd.AllocationQuantity = 900;
            Db.SubmitChanges();






            Db.ExecuteCommand("exec dbo.uf_allocate_animationID '" + animation1.ID + "'");


            Db = new DbDataContext();



            CustomerAllocation result1 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == apd.ID &&
                                          ca.IDCustomer == customerB1.ID
                                          select ca).FirstOrDefault();

            Assert.IsNotNull(result1, "Customer allocation for customerB1 does not exists");
            Assert.AreEqual(180, result1.CalculatedAllocation);

            CustomerAllocation result2 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == apd.ID &&
                                          ca.IDCustomer == customerB2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result2, "Customer allocation for customerB2 does not exists");
            Assert.AreEqual(180, result2.CalculatedAllocation);

            CustomerAllocation result3 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == apd.ID &&
                                          ca.IDCustomer == customerB3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result3, "Customer allocation for customerB3 does not exists");
            Assert.AreEqual(180, result3.CalculatedAllocation);

            CustomerAllocation result4 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == apd.ID &&
                                          ca.IDCustomer == customerD1.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result4, "Customer allocation for customerD1 does not exists");
            Assert.AreEqual(60, result4.CalculatedAllocation);

            CustomerAllocation result5 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == apd.ID &&
                                          ca.IDCustomer == customerD2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result5, "Customer allocation for customerD2 does not exists");
            Assert.AreEqual(60, result5.CalculatedAllocation);

            CustomerAllocation result6 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == apd.ID &&
                                          ca.IDCustomer == customerD3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result6, "Customer allocation for customerD3 does not exists");
            Assert.AreEqual(60, result6.CalculatedAllocation);

            CustomerAllocation result7 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == apd.ID &&
                                          ca.IDCustomer == customerJ1.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result7, "Customer allocation for customerJ1 does not exists");
            Assert.AreEqual(60, result7.CalculatedAllocation);

            CustomerAllocation result8 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == apd.ID &&
                                          ca.IDCustomer == customerJ2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result8, "Customer allocation for customerJ2 does not exists");
            Assert.AreEqual(60, result8.CalculatedAllocation);

            CustomerAllocation result9 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == apd.ID &&
                                          ca.IDCustomer == customerJ3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result9, "Customer allocation for customerJ3 does not exists");
            Assert.AreEqual(60, result9.CalculatedAllocation);

            CustomerGroupAllocation result10 = (from cga in Db.CustomerGroupAllocations.ToList()
                                                where cga.IDAnimationProductDetail == apd.ID &&
                                                cga.IDCustomerGroup == group1.ID
                                                select cga).FirstOrDefault();
            Assert.IsNotNull(result10, "Customer allocation for customer group B does not exists");
            Assert.AreEqual(540, result10.CalculatedAllocation);

            CustomerGroupAllocation result11 = (from cga in Db.CustomerGroupAllocations.ToList()
                                                where cga.IDAnimationProductDetail == apd.ID &&
                                                cga.IDCustomerGroup == group2.ID
                                                select cga).FirstOrDefault();
            Assert.IsNotNull(result11, "Customer allocation for customer group D does not exists");
            Assert.AreEqual(180, result11.CalculatedAllocation);

            CustomerGroupAllocation result12 = (from cga in Db.CustomerGroupAllocations.ToList()
                                                where cga.IDAnimationProductDetail == apd.ID &&
                                                cga.IDCustomerGroup == group3.ID
                                                select cga).FirstOrDefault();
            Assert.IsNotNull(result12, "Customer allocation for customer group J does not exists");
            Assert.AreEqual(180, result12.CalculatedAllocation);
        }
    }
}
