using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LorealOptimiseData;

namespace LorealOptimiseUnitTest
{
    [TestFixture]
    class AllocationCalculation : TestBase
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

            // Customer Group            
            CustomerGroup group1 = CreateCustomerGroup(salesArea, "CG1", "4545");
            group1.SalesArea = salesArea;
            CustomerGroup group2 = CreateCustomerGroup(salesArea, "CG2", "1232");
            group2.SalesArea = salesArea;
            Db.CustomerGroups.InsertOnSubmit(group1);
            Db.CustomerGroups.InsertOnSubmit(group2);
            Db.SubmitChanges();

            // Animation Customer Group
            AnimationCustomerGroup animationGroup1 = CreateAnimationCustomerGroup(animation1, group1, division1);
            AnimationCustomerGroup animationGroup2 = CreateAnimationCustomerGroup(animation1, group2, division1);
            Db.AnimationCustomerGroups.InsertOnSubmit(animationGroup1);
            Db.AnimationCustomerGroups.InsertOnSubmit(animationGroup2);
            Db.SubmitChanges();

            // Customers            
            Customer customer1 = CreateCustomer(group1, "Customer 1", "5454", division1);
            Customer customer2 = CreateCustomer(group1, "Customer 2", "4278", division1);
            Customer customer3 = CreateCustomer(group1, "Customer 3", "5465", division1);
            Customer customer4 = CreateCustomer(group2, "Customer 4", "87987", division1);
            Customer customer5 = CreateCustomer(group2, "Customer 5", "6566", division1);
            Db.Customers.InsertOnSubmit(customer1);
            Db.Customers.InsertOnSubmit(customer2);
            Db.Customers.InsertOnSubmit(customer3);
            Db.Customers.InsertOnSubmit(customer4);
            Db.Customers.InsertOnSubmit(customer5);
            Db.SubmitChanges();

            // Animation Product           
            AnimationProduct animationProduct1 = CreateAnimationProduct(division1, animation1, 6, 36);
            AnimationProduct animationProduct2 = CreateAnimationProduct(division1, animation1, 3, 3);
            Db.AnimationProducts.InsertOnSubmit(animationProduct1);
            Db.AnimationProducts.InsertOnSubmit(animationProduct2);
            Db.SubmitChanges();

            // Animation Product Details
            //AnimationProductDetail animationProductDetail1 = CreateAnimationProductDetail(animationProduct1, salesArea, 1200);
            AnimationProductDetail animationProductDetail2 = CreateAnimationProductDetail(animationProduct1, salesArea, 1300);
            //AnimationProductDetail animationProductDetail3 = CreateAnimationProductDetail(animationProduct2, salesArea, 1500);
            //AnimationProductDetail animationProductDetail4 = CreateAnimationProductDetail(animationProduct2, salesArea, 2000);
            //Db.AnimationProductDetails.InsertOnSubmit(animationProductDetail1);
            Db.AnimationProductDetails.InsertOnSubmit(animationProductDetail2);
            //Db.AnimationProductDetails.InsertOnSubmit(animationProductDetail3);
            //Db.AnimationProductDetails.InsertOnSubmit(animationProductDetail4);
            Db.SubmitChanges();

            // Customer Capacity
            CustomerCapacity customerCapacity11 = CreateCustomerCapacity(customer1, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 450);
            CustomerCapacity customerCapacity12 = CreateCustomerCapacity(customer2, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 360);
            CustomerCapacity customerCapacity13 = CreateCustomerCapacity(customer3, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 530);
            CustomerCapacity customerCapacity14 = CreateCustomerCapacity(customer4, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 50);
            CustomerCapacity customerCapacity15 = CreateCustomerCapacity(customer5, animation1.AnimationType, animation1.Priority, animationProduct1.ItemType, 1520);
            CustomerCapacity customerCapacity21 = CreateCustomerCapacity(customer1, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 450);
            CustomerCapacity customerCapacity22 = CreateCustomerCapacity(customer2, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 360);
            CustomerCapacity customerCapacity23 = CreateCustomerCapacity(customer3, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 530);
            CustomerCapacity customerCapacity24 = CreateCustomerCapacity(customer4, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 50);
            CustomerCapacity customerCapacity25 = CreateCustomerCapacity(customer5, animation1.AnimationType, animation1.Priority, animationProduct2.ItemType, 1520);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity11);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity12);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity13);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity14);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity15);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity21);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity22);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity23);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity24);
            Db.CustomerCapacities.InsertOnSubmit(customerCapacity25);
            Db.SubmitChanges();

            // Customer Group Allocation
            //CustomerGroupAllocation customerGroupAllocation11 = CreateCustomerGroupAllocation(group1, animationProductDetail1, 500);
            
             // warehouse account
            //CustomerGroupItemType customerGroupItemType = CreateCustomerGroupItemType(group1, animationProduct1.ItemType, true);
            CustomerGroupAllocation customerGroupAllocation21 = CreateCustomerGroupAllocation(group1, animationProductDetail2, 500);
            CustomerGroupAllocation customerGroupAllocation22 = CreateCustomerGroupAllocation(group2, animationProductDetail2, 500);
            //Db.CustomerGroupAllocations.InsertOnSubmit(customerGroupAllocation11);
            Db.CustomerGroupAllocations.InsertOnSubmit(customerGroupAllocation21);
            Db.CustomerGroupAllocations.InsertOnSubmit(customerGroupAllocation22);
            Db.SubmitChanges();

            // Customer allocation
            
            CustomerAllocation customerAllocation21 = CreateCustomerAllocation(customer1, animationProductDetail2, 100);
            customerAllocation21.RetailUplift = 0.8;
            CustomerAllocation customerAllocation22 = CreateCustomerAllocation(customer2, animationProductDetail2, 200);
            customerAllocation22.RetailUplift = 0.9;
            CustomerAllocation customerAllocation23 = CreateCustomerAllocation(customer3, animationProductDetail2);
            customerAllocation23.RetailUplift = 0.8;
            CustomerAllocation customerAllocation24 = CreateCustomerAllocation(customer4, animationProductDetail2, 200);
            CustomerAllocation customerAllocation25 = CreateCustomerAllocation(customer5, animationProductDetail2, 200);
           // CustomerAllocation customerAllocation31 = CreateCustomerAllocation(customer1, animationProductDetail3, 100);
           // CustomerAllocation customerAllocation35 = CreateCustomerAllocation(customer5, animationProductDetail3, 400);
            Db.CustomerAllocations.InsertOnSubmit(customerAllocation21);
            Db.CustomerAllocations.InsertOnSubmit(customerAllocation22);
            Db.CustomerAllocations.InsertOnSubmit(customerAllocation23);
            Db.CustomerAllocations.InsertOnSubmit(customerAllocation24);
            Db.CustomerAllocations.InsertOnSubmit(customerAllocation25);
            //Db.CustomerAllocations.InsertOnSubmit(customerAllocation31);
           // Db.CustomerAllocations.InsertOnSubmit(customerAllocation35);
            Db.SubmitChanges();
            
            // Sales
            Sale sale11 = CreateSale(customer1, animationProduct1.BrandAxe, 900);
            Sale sale21 = CreateSale(customer2, animationProduct1.BrandAxe, 550);
            Sale sale31 = CreateSale(customer3, animationProduct1.BrandAxe, 150);
            Sale sale41 = CreateSale(customer4, animationProduct1.BrandAxe, 300);
            Sale sale51 = CreateSale(customer5, animationProduct1.BrandAxe, 900);
            Sale sale12 = CreateSale(customer1, animationProduct2.BrandAxe, 720);
            Sale sale22 = CreateSale(customer2, animationProduct2.BrandAxe, 480);
            Sale sale32 = CreateSale(customer3, animationProduct2.BrandAxe, 120);
            Sale sale42 = CreateSale(customer4, animationProduct2.BrandAxe, 450);
            Sale sale52 = CreateSale(customer5, animationProduct2.BrandAxe, 560);

            Db.Sales.InsertOnSubmit(sale11);
            Db.Sales.InsertOnSubmit(sale21);
            Db.Sales.InsertOnSubmit(sale31);
            Db.Sales.InsertOnSubmit(sale41);
            Db.Sales.InsertOnSubmit(sale51);
            Db.Sales.InsertOnSubmit(sale12);
            Db.Sales.InsertOnSubmit(sale22);
            Db.Sales.InsertOnSubmit(sale32);
            Db.Sales.InsertOnSubmit(sale42);
            Db.Sales.InsertOnSubmit(sale52);
            Db.SubmitChanges();

            Db.ExecuteCommand("exec dbo.uf_allocate_animationID '" + animation1.ID + "'");
           // var result = Db.uf_allocate_animationID(animation1.ID);


            /*
            Db = new DbDataContext();
            
            List<CustomerAllocation> allCustomerAllocation = Db.CustomerAllocations.ToList();
            CustomerAllocation result1 = (from ca in allCustomerAllocation
                                         where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                         ca.IDCustomer == customer1.ID
                                         select ca).FirstOrDefault();

            Assert.IsNotNull(result1, "Customer allocation for customer1 does not exists");
            Assert.AreEqual(282, result1.CalculatedAllocation);

            CustomerAllocation result2 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customer2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result2, "Customer allocation for customer2 does not exists");
            Assert.AreEqual(168, result2.CalculatedAllocation);

            CustomerAllocation result3 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customer3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result3, "Customer allocation for customer3 does not exists");
            Assert.AreEqual(42, result3.CalculatedAllocation);

            CustomerAllocation result4 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customer4.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result4, "Customer allocation for customer4 does not exists");
            Assert.AreEqual(48, result4.CalculatedAllocation);

            CustomerAllocation result5 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail1.ID &&
                                          ca.IDCustomer == customer5.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result5, "Customer allocation for customer5 does not exists");
            Assert.AreEqual(648, result5.CalculatedAllocation);

            CustomerGroupAllocation result6 = (from cga in Db.CustomerGroupAllocations.ToList()
                                               where cga.IDAnimationProductDetail == animationProductDetail1.ID &&
                                               cga.IDCustomerGroup == group1.ID
                                               select cga).FirstOrDefault();
            Assert.IsNotNull(result6, "Customer group allocation of animationProductDetail1 for customerGroup1 does not exists");
            Assert.AreEqual(492, result6.CalculatedAllocation);

            CustomerGroupAllocation result7 = (from cga in Db.CustomerGroupAllocations.ToList()
                                               where cga.IDAnimationProductDetail == animationProductDetail1.ID &&
                                               cga.IDCustomerGroup == group2.ID
                                               select cga).FirstOrDefault();
            Assert.IsNotNull(result7, "Customer group allocation of animationProductDetail1 for customerGroup2 does not exists");
            Assert.AreEqual(696, result7.CalculatedAllocation);

            */

            /*
            CustomerAllocation result8 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customer1.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result8, "Customer allocation of animationProductDetail1 for customer1 does not exists");
            Assert.AreEqual(96, result8.CalculatedAllocation);

            CustomerAllocation result9 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customer2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result8, "Customer allocation of animationProductDetail1 for customer2 does not exists");
            Assert.AreEqual(198, result9.CalculatedAllocation);

            CustomerAllocation result10 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customer3.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result10, "Customer allocation of animationProductDetail1 for customer3 does not exists");
            Assert.AreEqual(204, result10.CalculatedAllocation);

            CustomerAllocation result11 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customer4.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result11, "Customer allocation of animationProductDetail1 for customer4 does not exists");
            Assert.AreEqual(48, result11.CalculatedAllocation);

            CustomerAllocation result12 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail2.ID &&
                                          ca.IDCustomer == customer5.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result12, "Customer allocation of animationProductDetail1 for customer5 does not exists");
            Assert.AreEqual(198, result12.CalculatedAllocation);

            CustomerGroupAllocation result13 = (from cga in Db.CustomerGroupAllocations.ToList()
                                               where cga.IDAnimationProductDetail == animationProductDetail2.ID &&
                                               cga.IDCustomerGroup == group1.ID
                                               select cga).FirstOrDefault();
            Assert.IsNotNull(result13, "Customer group allocation of animationProductDetail1 for customerGroup1 does not exists");
            Assert.AreEqual(498, result13.CalculatedAllocation);

            CustomerGroupAllocation result14 = (from cga in Db.CustomerGroupAllocations.ToList()
                                               where cga.IDAnimationProductDetail == animationProductDetail2.ID &&
                                               cga.IDCustomerGroup == group2.ID
                                               select cga).FirstOrDefault();
            Assert.IsNotNull(result14, "Customer group allocation of animationProductDetail1 for customerGroup2 does not exists");
            Assert.AreEqual(246, result14.CalculatedAllocation);

            

            CustomerAllocation result15 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                          ca.IDCustomer == customer1.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result15, "Customer allocation of animationProductDetail3 for customer1 does not exists");
            Assert.AreEqual(99, result15.CalculatedAllocation);

            CustomerAllocation result16 = (from ca in Db.CustomerAllocations.ToList()
                                          where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                          ca.IDCustomer == customer2.ID
                                          select ca).FirstOrDefault();
            Assert.IsNotNull(result16, "Customer allocation of animationProductDetail3 for customer2 does not exists");
            Assert.AreEqual(360, result16.CalculatedAllocation);

            CustomerAllocation result17 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customer3.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result17, "Customer allocation of animationProductDetail3 for customer3 does not exists");
            Assert.AreEqual(258, result17.CalculatedAllocation);

            CustomerAllocation result18 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customer4.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result18, "Customer allocation of animationProductDetail3 for customer4 does not exists");
            Assert.AreEqual(48, result18.CalculatedAllocation);

            CustomerAllocation result19 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail3.ID &&
                                           ca.IDCustomer == customer5.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result19, "Customer allocation of animationProductDetail3 for customer5 does not exists");
            Assert.AreEqual(399, result19.CalculatedAllocation);

            CustomerGroupAllocation result20 = (from cga in Db.CustomerGroupAllocations.ToList()
                                                where cga.IDAnimationProductDetail == animationProductDetail3.ID &&
                                                cga.IDCustomerGroup == group1.ID
                                                select cga).FirstOrDefault();
            Assert.IsNotNull(result20, "Customer group allocation of animationProductDetail3 for customerGroup1 does not exists");
            Assert.AreEqual(717, result20.CalculatedAllocation);

            CustomerGroupAllocation result21 = (from cga in Db.CustomerGroupAllocations.ToList()
                                                where cga.IDAnimationProductDetail == animationProductDetail3.ID &&
                                                cga.IDCustomerGroup == group2.ID
                                                select cga).FirstOrDefault();
            Assert.IsNotNull(result21, "Customer group allocation of animationProductDetail3 for customerGroup2 does not exists");
            Assert.AreEqual(447, result21.CalculatedAllocation);
            



            CustomerAllocation result22 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail4.ID &&
                                           ca.IDCustomer == customer1.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result22, "Customer allocation of animationProductDetail4 for customer1 does not exists");
            Assert.AreEqual(450, result22.CalculatedAllocation);

            CustomerAllocation result23 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail4.ID &&
                                           ca.IDCustomer == customer2.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result23, "Customer allocation of animationProductDetail4 for customer2 does not exists");
            Assert.AreEqual(360, result23.CalculatedAllocation);

            CustomerAllocation result24 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail4.ID &&
                                           ca.IDCustomer == customer3.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result24, "Customer allocation of animationProductDetail4 for customer3 does not exists");
            Assert.AreEqual(159, result24.CalculatedAllocation);

            CustomerAllocation result25 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail4.ID &&
                                           ca.IDCustomer == customer4.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result25, "Customer allocation of animationProductDetail4 for customer4 does not exists");
            Assert.AreEqual(48, result25.CalculatedAllocation);

            CustomerAllocation result26 = (from ca in Db.CustomerAllocations.ToList()
                                           where ca.IDAnimationProductDetail == animationProductDetail4.ID &&
                                           ca.IDCustomer == customer5.ID
                                           select ca).FirstOrDefault();
            Assert.IsNotNull(result26, "Customer allocation of animationProductDetail4 for customer5 does not exists");
            Assert.AreEqual(774, result26.CalculatedAllocation);

            CustomerGroupAllocation result27 = (from cga in Db.CustomerGroupAllocations.ToList()
                                                where cga.IDAnimationProductDetail == animationProductDetail4.ID &&
                                                cga.IDCustomerGroup == group1.ID
                                                select cga).FirstOrDefault();
            Assert.IsNotNull(result27, "Customer group allocation of animationProductDetail4 for customerGroup1 does not exists");
            Assert.AreEqual(969, result27.CalculatedAllocation);

            CustomerGroupAllocation result28 = (from cga in Db.CustomerGroupAllocations.ToList()
                                                where cga.IDAnimationProductDetail == animationProductDetail4.ID &&
                                                cga.IDCustomerGroup == group2.ID
                                                select cga).FirstOrDefault();
            Assert.IsNotNull(result28, "Customer group allocation of animationProductDetail4 for customerGroup2 does not exists");
            Assert.AreEqual(822, result28.CalculatedAllocation);

            */

        }
    }
}
