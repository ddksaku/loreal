﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LorealOptimiseData;

namespace LorealOptimiseUnitTest
{
    [TestFixture]
    class AllocationLogicTest55_28 : TestBase
    {
        [Test]
        public void Allocation()
        {
            DeleteData();

            /*
            // Divisions            
            Division division1 = CreateDivision();
            Db.Divisions.InsertOnSubmit(division1);
            Db.SubmitChanges();

            // Signature
            Signature signature = CreateSignature(division1);
            Signature signature2 = CreateSignature(division1);
            Db.Signatures.InsertOnSubmit(signature);
            Db.Signatures.InsertOnSubmit(signature2);
            Db.SubmitChanges();

            // Distribution Channel            
            DistributionChannel channel1 = CreateDistributionChannel();
            Db.DistributionChannels.InsertOnSubmit(channel1);
            Db.SubmitChanges();

            // Animation Type            
            AnimationType animationType1 = CreateAnimationType(division1, "Promotion");
            AnimationType animationType2 = CreateAnimationType(division1, "Launch");
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

            // Sales Area           
            SalesArea salesArea = CreateSalesArea(channel1, division1);
            Db.SalesAreas.InsertOnSubmit(salesArea);
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

            // Categories
            Category category1 = CreateCategory("Consultant", division1);
            Category category2 = CreateCategory("VIP", division1);
            Category category3 = CreateCategory("Non-Consultant", division1);
            Category category4 = CreateCategory("Flagship", division1);
            Category category5 = CreateCategory("SkinExpert", division1);
            Db.Categories.InsertOnSubmit(category1);
            Db.Categories.InsertOnSubmit(category2);
            Db.Categories.InsertOnSubmit(category3);
            Db.Categories.InsertOnSubmit(category4);
            Db.Categories.InsertOnSubmit(category5);
            Db.SubmitChanges();

            // Customers            
            Customer customerB1 = CreateCustomer(group1, "Boots 1", "5454", division1);
            Customer customerB2 = CreateCustomer(group1, "Boots 2", "4278", division1);
            Customer customerB3 = CreateCustomer(group1, "Boots 3", "54615", division1);
            Customer customerD1 = CreateCustomer(group2, "Debenhams 1", "879487", division1);
            Customer customerD2 = CreateCustomer(group2, "Debenhams 2", "65626", division1);
            Customer customerD3 = CreateCustomer(group2, "Debenhams 3", "42718", division1);
            Customer customerJ1 = CreateCustomer(group3, "John Lewis 1", "54165", division1);
            Customer customerJ2 = CreateCustomer(group3, "John Lewis 2", "879817", division1);
            Customer customerJ3 = CreateCustomer(group3, "John Lewis 3", "65606", division1);
            Db.Customers.InsertOnSubmit(customerB1);
            Db.Customers.InsertOnSubmit(customerB2);
            Db.Customers.InsertOnSubmit(customerB3);
            Db.Customers.InsertOnSubmit(customerD1);
            Db.Customers.InsertOnSubmit(customerD2);
            Db.Customers.InsertOnSubmit(customerD3);
            Db.Customers.InsertOnSubmit(customerJ1);
            Db.Customers.InsertOnSubmit(customerJ2);
            Db.Customers.InsertOnSubmit(customerJ3);
            Db.SubmitChanges();

            // CustomerCategorie
            CustomerCategory customerCategory1 = CreateCustomerCategory(customerB1, category1);
            CustomerCategory customerCategory2 = CreateCustomerCategory(customerB1, category2);
            CustomerCategory customerCategory3 = CreateCustomerCategory(customerB2, category3);
            CustomerCategory customerCategory4 = CreateCustomerCategory(customerD1, category1);
            CustomerCategory customerCategory5 = CreateCustomerCategory(customerD1, category4);
            CustomerCategory customerCategory6 = CreateCustomerCategory(customerD3, category3);
            CustomerCategory customerCategory7 = CreateCustomerCategory(customerD3, category2);
            CustomerCategory customerCategory8 = CreateCustomerCategory(customerJ2, category4);
            CustomerCategory customerCategory9 = CreateCustomerCategory(customerJ2, category5);
            CustomerCategory customerCategory10 = CreateCustomerCategory(customerJ3, category1);
            Db.SubmitChanges();

            // All category
            Db.ExecuteCommand("exec [up_insertMissingAllCategory]");
            Db.SubmitChanges();

            // Category ALL
            Category categoryAll = (from p in Db.Categories
                                    where p.Name == "All"
                                    select p).Single();

            // BrandAxes
            BrandAxe brandAxeX = CreateBrandAxe(signature, "BrandAxe X", "123");
            BrandAxe brandAxeY = CreateBrandAxe(signature, "BrandAxe Y", "1123");
            BrandAxe brandAxeZ = CreateBrandAxe(signature, "BrandAxe Z", "1423");
            BrandAxe brandAxeQ = CreateBrandAxe(signature2, "Brand Axe Q", "456");
            Db.BrandAxes.InsertOnSubmit(brandAxeX);
            Db.BrandAxes.InsertOnSubmit(brandAxeY);
            Db.BrandAxes.InsertOnSubmit(brandAxeZ);
            Db.BrandAxes.InsertOnSubmit(brandAxeQ);
            Db.SubmitChanges();

            // Sales - X
            Sale sale11X = CreateSale(customerB1, brandAxeX, 100);
            Sale sale12X = CreateSale(customerB2, brandAxeX, 300);
            Sale sale13X = CreateSale(customerB3, brandAxeX, 200);
            Sale sale21X = CreateSale(customerD1, brandAxeX, 200);
            Sale sale22X = CreateSale(customerD2, brandAxeX, 600);
            Sale sale23X = CreateSale(customerD3, brandAxeX, 400);
            Sale sale31X = CreateSale(customerJ1, brandAxeX, 150);
            Sale sale32X = CreateSale(customerJ2, brandAxeX, 250);
            Sale sale33X = CreateSale(customerJ3, brandAxeX, 350);
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
            Sale sale11Y = CreateSale(customerB1, brandAxeY, 200);
            Sale sale12Y = CreateSale(customerB2, brandAxeY, 100);
            Sale sale13Y = CreateSale(customerB3, brandAxeY, 300);
            Sale sale21Y = CreateSale(customerD1, brandAxeY, 200);
            Sale sale22Y = CreateSale(customerD2, brandAxeY, 400);
            Sale sale23Y = CreateSale(customerD3, brandAxeY, 600);
            Sale sale31Y = CreateSale(customerJ1, brandAxeY, 250);
            Sale sale32Y = CreateSale(customerJ2, brandAxeY, 0);
            Sale sale33Y = CreateSale(customerJ3, brandAxeY, 150);
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
            Sale sale11Z = CreateSale(customerB1, brandAxeZ, 300);
            Sale sale12Z = CreateSale(customerB2, brandAxeZ, 200);
            Sale sale13Z = CreateSale(customerB3, brandAxeZ, 100);
            Sale sale21Z = CreateSale(customerD1, brandAxeZ, 600);
            Sale sale22Z = CreateSale(customerD2, brandAxeZ, 0);
            Sale sale23Z = CreateSale(customerD3, brandAxeZ, 0);
            Sale sale31Z = CreateSale(customerJ1, brandAxeZ, 0);
            Sale sale32Z = CreateSale(customerJ2, brandAxeZ, 250);
            Sale sale33Z = CreateSale(customerJ3, brandAxeZ, 150);
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

            // Sales Q
            Sale sale11Q = CreateSale(customerB1, brandAxeQ, 100);
            Sale sale12Q = CreateSale(customerB2, brandAxeQ, 500);
            Sale sale13Q = CreateSale(customerB3, brandAxeQ, 1000);
            Sale sale21Q = CreateSale(customerD1, brandAxeQ, 300);
            Sale sale22Q = CreateSale(customerD2, brandAxeQ, 700);
            Sale sale23Q = CreateSale(customerD3, brandAxeQ, 0);
            Sale sale31Q = CreateSale(customerJ1, brandAxeQ, 0);
            Sale sale32Q = CreateSale(customerJ2, brandAxeQ, 0);
            Sale sale33Q = CreateSale(customerJ3, brandAxeQ, 0);
            Db.Sales.InsertOnSubmit(sale11Q);
            Db.Sales.InsertOnSubmit(sale12Q);
            Db.Sales.InsertOnSubmit(sale13Q);
            Db.Sales.InsertOnSubmit(sale21Q);
            Db.Sales.InsertOnSubmit(sale22Q);
            Db.Sales.InsertOnSubmit(sale23Q);
            Db.Sales.InsertOnSubmit(sale31Q);
            Db.Sales.InsertOnSubmit(sale32Q);
            Db.Sales.InsertOnSubmit(sale33Q);
            Db.SubmitChanges();

            // Animation            
            Animation animation1 = CreateAnimation(division1, channel1, animationType1, priorityA);
            Db.Animations.InsertOnSubmit(animation1);
            Db.SubmitChanges();           

            

            // Animation Customer Group
            AnimationCustomerGroup animationGroup1 = CreateAnimationCustomerGroup(animation1, group1, division1);
            AnimationCustomerGroup animationGroup2 = CreateAnimationCustomerGroup(animation1, group2, division1);
            AnimationCustomerGroup animationGroup3 = CreateAnimationCustomerGroup(animation1, group3, division1);
            Db.AnimationCustomerGroups.InsertOnSubmit(animationGroup1);
            Db.AnimationCustomerGroups.InsertOnSubmit(animationGroup2);
            Db.AnimationCustomerGroups.InsertOnSubmit(animationGroup3);
            Db.SubmitChanges();



            // Animation Product           
            AnimationProduct animationProduct1 = CreateAnimationProduct(division1, animation1, 1, 1, brandAxeX, signature);
            animationProduct1.Category = categoryAll;
            AnimationProduct animationProduct2 = CreateAnimationProduct(division1, animation1, 3, 1, signature2);
            animationProduct2.Category = category4;
            AnimationProduct animationProduct3 = CreateAnimationProduct(division1, animation1, 6, 1, brandAxeY, signature);
            animationProduct3.Category = category1;
            Db.AnimationProducts.InsertOnSubmit(animationProduct1);
            Db.AnimationProducts.InsertOnSubmit(animationProduct2);
            Db.AnimationProducts.InsertOnSubmit(animationProduct3);
            Db.SubmitChanges();     

           


            // Customer Capacity - AnimationProduct1
            CustomerCapacity customerCapacity11 = CreateCustomerCapacity(customerB1, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            CustomerCapacity customerCapacity12 = CreateCustomerCapacity(customerB2, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            CustomerCapacity customerCapacity13 = CreateCustomerCapacity(customerB3, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            CustomerCapacity customerCapacity21 = CreateCustomerCapacity(customerD1, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            CustomerCapacity customerCapacity22 = CreateCustomerCapacity(customerD2, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            CustomerCapacity customerCapacity23 = CreateCustomerCapacity(customerD3, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            CustomerCapacity customerCapacity31 = CreateCustomerCapacity(customerJ1, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            CustomerCapacity customerCapacity32 = CreateCustomerCapacity(customerJ2, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            CustomerCapacity customerCapacity33 = CreateCustomerCapacity(customerJ3, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 100);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity11);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity12);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity13);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity21);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity22);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity23);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity31);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity32);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity33);
            Db.SubmitChanges();

            // Customer Capacity - AnimationProduct2
            CustomerCapacity customerCapacity211 = CreateCustomerCapacity(customerB1, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 100);
            CustomerCapacity customerCapacity212 = CreateCustomerCapacity(customerB2, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 100);
            CustomerCapacity customerCapacity213 = CreateCustomerCapacity(customerB3, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 100);
            CustomerCapacity customerCapacity221 = CreateCustomerCapacity(customerD1, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 100);
            CustomerCapacity customerCapacity222 = CreateCustomerCapacity(customerD2, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 100);
            CustomerCapacity customerCapacity223 = CreateCustomerCapacity(customerD3, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 100);
            CustomerCapacity customerCapacity231 = CreateCustomerCapacity(customerJ1, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 100);
            CustomerCapacity customerCapacity232 = CreateCustomerCapacity(customerJ2, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 100);
            CustomerCapacity customerCapacity233 = CreateCustomerCapacity(customerJ3, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 100);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity211);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity212);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity213);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity221);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity222);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity223);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity231);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity232);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity233);
            Db.SubmitChanges();

            // Customer Capacity - AnimationProduct1
            CustomerCapacity customerCapacity311 = CreateCustomerCapacity(customerB1, animation1.AnimationType, animation1.Priority, animationProduct3.ItemType, 100);
            CustomerCapacity customerCapacity312 = CreateCustomerCapacity(customerB2, animation1.AnimationType, animation1.Priority, animationProduct3.ItemType, 100);
            CustomerCapacity customerCapacity313 = CreateCustomerCapacity(customerB3, animation1.AnimationType, animation1.Priority, animationProduct3.ItemType, 100);
            CustomerCapacity customerCapacity321 = CreateCustomerCapacity(customerD1, animation1.AnimationType, animation1.Priority, animationProduct3.ItemType, 100);
            CustomerCapacity customerCapacity322 = CreateCustomerCapacity(customerD2, animation1.AnimationType, animation1.Priority, animationProduct3.ItemType, 100);
            CustomerCapacity customerCapacity323 = CreateCustomerCapacity(customerD3, animation1.AnimationType, animation1.Priority, animationProduct3.ItemType, 100);
            CustomerCapacity customerCapacity331 = CreateCustomerCapacity(customerJ1, animation1.AnimationType, animation1.Priority, animationProduct3.ItemType, 100);
            CustomerCapacity customerCapacity332 = CreateCustomerCapacity(customerJ2, animation1.AnimationType, animation1.Priority, animationProduct3.ItemType, 100);
            CustomerCapacity customerCapacity333 = CreateCustomerCapacity(customerJ3, animation1.AnimationType, animation1.Priority, animationProduct3.ItemType, 100);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity311);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity312);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity313);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity321);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity322);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity323);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity331);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity332);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity333);
            Db.SubmitChanges();

            // Animation Product Details            
            AnimationProductDetail animationProductDetail1 = (from p in Db.AnimationProductDetails
                                                              where p.AnimationProduct == animationProduct1
                                                              select p).Single();
            animationProductDetail1.AllocationQuantity = 180;

            AnimationProductDetail animationProductDetail2 = (from p in Db.AnimationProductDetails
                                                              where p.AnimationProduct == animationProduct2
                                                              select p).Single();
            animationProductDetail2.AllocationQuantity = 180;

            AnimationProductDetail animationProductDetail3 = (from p in Db.AnimationProductDetails
                                                              where p.AnimationProduct == animationProduct3
                                                              select p).Single();
            animationProductDetail3.AllocationQuantity = 180;
            Db.SubmitChanges();


            // update group allocations
            CustomerGroupAllocation groupAllocationBoots1 = (from p in Db.CustomerGroupAllocations
                                                            where p.CustomerGroup == group1 && p.AnimationProductDetail == animationProductDetail1
                                                            select p).Single();
            groupAllocationBoots1.ManualFixedAllocation = 90;

            CustomerGroupAllocation groupAllocationBoots2 = (from p in Db.CustomerGroupAllocations
                                                             where p.CustomerGroup == group1 && p.AnimationProductDetail == animationProductDetail2
                                                             select p).Single();
            groupAllocationBoots2.ManualFixedAllocation = 90;

            CustomerGroupAllocation groupAllocationBoots3 = (from p in Db.CustomerGroupAllocations
                                                             where p.CustomerGroup == group1 && p.AnimationProductDetail == animationProductDetail3
                                                             select p).Single();
            groupAllocationBoots3.ManualFixedAllocation = 90;

            CustomerGroupAllocation groupAllocationDeb1 = (from p in Db.CustomerGroupAllocations
                                                           where p.CustomerGroup == group2 && p.AnimationProductDetail == animationProductDetail1
                                                           select p).Single();
            groupAllocationDeb1.RetailUplift = 10;

            CustomerGroupAllocation groupAllocationDeb2 = (from p in Db.CustomerGroupAllocations
                                                           where p.CustomerGroup == group2 && p.AnimationProductDetail == animationProductDetail2
                                                           select p).Single();
            groupAllocationDeb1.RetailUplift = 10;

            CustomerGroupAllocation groupAllocationDeb3 = (from p in Db.CustomerGroupAllocations
                                                           where p.CustomerGroup == group2 && p.AnimationProductDetail == animationProductDetail3
                                                           select p).Single();
            groupAllocationDeb1.RetailUplift = 10;

            Db.SubmitChanges();


            // update customer allocation
            CustomerAllocation customerAllocationB11 = (from p in Db.CustomerAllocations
                                                        where p.Customer == customerB1 && p.AnimationProductDetail == animationProductDetail1
                                                        select p).Single();
            customerAllocationB11.RetailUplift = 6;

            CustomerAllocation customerAllocationB12 = (from p in Db.CustomerAllocations
                                                        where p.Customer == customerB1 && p.AnimationProductDetail == animationProductDetail2
                                                        select p).Single();
            customerAllocationB12.RetailUplift = 6;

            CustomerAllocation customerAllocationB13 = (from p in Db.CustomerAllocations
                                                        where p.Customer == customerB1 && p.AnimationProductDetail == animationProductDetail3
                                                        select p).Single();
            customerAllocationB13.RetailUplift = 6;



            CustomerAllocation customerAllocationD11 = (from p in Db.CustomerAllocations
                                                        where p.Customer == customerD1 && p.AnimationProductDetail == animationProductDetail1
                                                        select p).Single();
            customerAllocationD11.FixedAllocation = 30;

            CustomerAllocation customerAllocationD12 = (from p in Db.CustomerAllocations
                                                        where p.Customer == customerD1 && p.AnimationProductDetail == animationProductDetail2
                                                        select p).Single();
            customerAllocationD12.FixedAllocation = 30;

            CustomerAllocation customerAllocationD13 = (from p in Db.CustomerAllocations
                                                        where p.Customer == customerD1 && p.AnimationProductDetail == animationProductDetail3
                                                        select p).Single();
            customerAllocationD13.FixedAllocation = 30;

            CustomerAllocation customerAllocationJ21 = (from p in Db.CustomerAllocations
                                                        where p.Customer == customerJ2 && p.AnimationProductDetail == animationProductDetail1
                                                        select p).Single();
            customerAllocationJ21.RetailUplift = 3;

            CustomerAllocation customerAllocationJ22 = (from p in Db.CustomerAllocations
                                                        where p.Customer == customerJ2 && p.AnimationProductDetail == animationProductDetail2
                                                        select p).Single();
            customerAllocationJ22.RetailUplift = 3;

            CustomerAllocation customerAllocationJ23 = (from p in Db.CustomerAllocations
                                                        where p.Customer == customerJ2 && p.AnimationProductDetail == animationProductDetail3
                                                        select p).Single();
            customerAllocationJ23.RetailUplift = 3;
            Db.SubmitChanges();

           

            Db.ExecuteCommand("exec dbo.uf_allocate_animationID '" + animation1.ID + "'");

            
            Db = new DbDataContext();
            
            
            CustomerAllocation result1 = (from ca in Db.CustomerAllocations.ToList()
                                         where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                         ca.IDCustomer == customerB1.ID
                                         select ca).FirstOrDefault();

            Assert.IsNotNull(result1, "Customer allocation for customerB1 does not exists");
            Assert.AreEqual(50, result1.CalculatedAllocation);

            CustomerAllocation result2 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customerB2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result2, "Customer allocation for customerB2 does not exists");
            Assert.AreEqual(24, result2.CalculatedAllocation);

            CustomerAllocation result3 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customerB3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result3, "Customer allocation for customerB3 does not exists");
            Assert.AreEqual(16, result3.CalculatedAllocation);

            CustomerAllocation result4 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customerD1.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result4, "Customer allocation for customerD1 does not exists");
            Assert.AreEqual(30, result4.CalculatedAllocation);

            CustomerAllocation result5 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customerD2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result5, "Customer allocation for customerD2 does not exists");
            Assert.AreEqual(34, result5.CalculatedAllocation);

            CustomerAllocation result6 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customerD3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result6, "Customer allocation for customerD3 does not exists");
            Assert.AreEqual(21, result6.CalculatedAllocation);

            CustomerAllocation result7 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customerJ1.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result7, "Customer allocation for customerJ1 does not exists");
            Assert.AreEqual(0, result7.CalculatedAllocation);

            CustomerAllocation result8 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customerJ2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result8, "Customer allocation for customerJ2 does not exists");
            Assert.AreEqual(4, result8.CalculatedAllocation);
            
            CustomerAllocation result9 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customerJ3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result9, "Customer allocation for customerJ3 does not exists");
            Assert.AreEqual(1, result9.CalculatedAllocation);
            
            
            


            CustomerAllocation result21 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customerB1.ID
                                          select ca).FirstOrDefault();

            Assert.IsNotNull(result21, "Customer allocation for customerB1 does not exists");
            Assert.AreEqual(0, result21.CalculatedAllocation);

            CustomerAllocation result22 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customerB2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result22, "Customer allocation for customerB2 does not exists");
            Assert.AreEqual(0, result22.CalculatedAllocation); // 10 in excel

            CustomerAllocation result23 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customerB3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result23, "Customer allocation for customerB3 does not exists");
            Assert.AreEqual(0, result23.CalculatedAllocation); //20 in excel

            CustomerAllocation result24 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customerD1.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result24, "Customer allocation for customerD1 does not exists");
            Assert.AreEqual(30, result24.CalculatedAllocation);

            CustomerAllocation result25 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customerD2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result25, "Customer allocation for customerD2 does not exists");
            Assert.AreEqual(0, result25.CalculatedAllocation);

            CustomerAllocation result26 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customerD3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result26, "Customer allocation for customerD3 does not exists");
            Assert.AreEqual(0, result26.CalculatedAllocation); 

            CustomerAllocation result27 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customerJ1.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result27, "Customer allocation for customerJ1 does not exists");
            Assert.AreEqual(0, result27.CalculatedAllocation); 

            CustomerAllocation result28 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customerJ2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result28, "Customer allocation for customerJ2 does not exists");
            Assert.AreEqual(0, result28.CalculatedAllocation);

            CustomerAllocation result29 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customerJ3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result29, "Customer allocation for customerJ3 does not exists");
            Assert.AreEqual(0, result29.CalculatedAllocation);


            
            
            CustomerAllocation result321 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customerB1.ID
                                           select ca).FirstOrDefault();

            Assert.IsNotNull(result321, "Customer allocation for customerB1 does not exists");
            Assert.AreEqual(90, result321.CalculatedAllocation);

            CustomerAllocation result322 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customerB2.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result322, "Customer allocation for customerB2 does not exists");
            Assert.AreEqual(0, result322.CalculatedAllocation);

            CustomerAllocation result323 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customerB3.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result323, "Customer allocation for customerB3 does not exists");
            Assert.AreEqual(0, result323.CalculatedAllocation);

            CustomerAllocation result324 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customerD1.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result324, "Customer allocation for customerD1 does not exists");
            Assert.AreEqual(30, result324.CalculatedAllocation); 

            CustomerAllocation result325 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customerD2.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result325, "Customer allocation for customerD2 does not exists");
            Assert.AreEqual(0, result325.CalculatedAllocation);

            CustomerAllocation result326 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customerD3.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result326, "Customer allocation for customerD3 does not exists");
            Assert.AreEqual(0, result326.CalculatedAllocation);

            CustomerAllocation result327 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customerJ1.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result327, "Customer allocation for customerJ1 does not exists");
            Assert.AreEqual(0, result327.CalculatedAllocation);

            CustomerAllocation result328 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customerJ2.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result328, "Customer allocation for customerJ2 does not exists");
            Assert.AreEqual(0, result328.CalculatedAllocation); 

            CustomerAllocation result329 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customerJ3.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result329, "Customer allocation for customerJ3 does not exists");
            Assert.AreEqual(60, result329.CalculatedAllocation);

           
        */

        }
    }
}
