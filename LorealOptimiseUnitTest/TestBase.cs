using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using NUnit.Framework;

namespace LorealOptimiseUnitTest
{
    public class TestBase
    {
        private string ticks
        {
            get
            {
                return DateTime.Now.Ticks.ToString();
            }
        }

        private DbDataContext db;
        protected DbDataContext Db
        {
            get
            {
                if (db == null)
                {
                    db = new DbDataContext();
                }

                return db;
            }
            set
            {
                db = value;
            }
        }

        protected Division Division
        {
            get;
            set;
        }

        protected User User
        {
            get;
            set;
        }

        protected Role Role
        {
            get;
            set;
        }

        protected UserRole UserRole
        {
            get;
            set;
        }

        [SetUp]
        public void Init()
        {
            // delete 
            Db.UserRoles.DeleteAllOnSubmit(Db.UserRoles); Db.SubmitChanges();
            Db.Users.DeleteAllOnSubmit(Db.Users); Db.SubmitChanges();
            Db.Roles.DeleteAllOnSubmit(Db.Roles); Db.SubmitChanges();

            Division = CreateDivision();
            Role = CreateRole("Role");
            User = CreateUser("User", true);
            UserRole = CreateUserRole(Division, Role, User);

            LoggedUser.SetInstance(User, Division);
        }

        protected Division CreateDivision()
        {
            Division division = new Division();
            division.ID = Guid.NewGuid();
            division.AllocateByAxe = TestHelper.GetRandomBool();
            division.AllocateByBrand = TestHelper.GetRandomBool();
            division.Code = ticks.Substring(ticks.Length-10, 10);
            division.Deleted = TestHelper.GetRandomBool();
            division.Name = "DivisionName" + ticks;

            return division;
        }

        protected UserRole CreateUserRole(Division division, Role role, User user)
        {
            UserRole userRole = new UserRole();
            userRole.Division = division;
            userRole.Role = role;
            userRole.User = user;

            return userRole;
        }

        protected User CreateUser(string userName, bool insertIntoDb)
        {
            User user = new User();
            user.EmailAddress = userName + "@memos.cz";
            user.LoginName = userName;
            user.Name = userName;

            if (insertIntoDb)
            {
                Db.Users.InsertOnSubmit(user);
                Db.SubmitChanges();
            }

            return user;
        }

        protected Role CreateRole(string userRole)
        {
            Role role = new Role();
            role.Name = userRole;
            return role;            
        }

        protected Category CreateCategory(string name, Division division)
        {
            Category category = new Category();
            category.ID = Guid.NewGuid();
            category.Name = name;
            category.Division = division;

            return category;
        }

        protected CustomerCategory CreateCustomerCategory(Customer customer, Category category)
        {
            CustomerCategory cuCa = new CustomerCategory();
            cuCa.ID = Guid.NewGuid();
            cuCa.Customer = customer;
            cuCa.Category = category;

            return cuCa;
        }

        protected User CreateUser(bool insertIntoDb)
        {
            return CreateUser(ticks, insertIntoDb);
        }

        protected void AssignUserToDivisions(User user, params Division[] divisions)
        {
            Role role = new Role();
            role.Name = "System administrator";
            
            db.Roles.InsertOnSubmit(role);
            db.SubmitChanges();

            foreach (Division division in divisions)
            {
                UserRole userRole = new UserRole();
                userRole.Division = division;
                userRole.Role = role;
                userRole.User = user;

                db.UserRoles.InsertOnSubmit(userRole);
            }

            db.SubmitChanges();
        }

        protected AnimationType CreateAnimationType(Division division)
        {
            AnimationType animationType = new AnimationType();
            animationType.ID = Guid.NewGuid();
            animationType.Deleted = false;
            animationType.Name = "AnimationType1";
            animationType.Division = division;
            
            return animationType;
        }

        protected AnimationType CreateAnimationType(Division division, String name)
        {
            AnimationType animationType = new AnimationType();
            animationType.ID = Guid.NewGuid();
            animationType.Deleted = false;
            animationType.Name = name;
            animationType.Division = division;

            return animationType;
        }

        protected DistributionChannel CreateDistributionChannel()
        {
            DistributionChannel channel = new DistributionChannel();
            channel.Name = "DistributionChannel1";
            channel.ID = Guid.NewGuid();
            channel.Deleted = false;
            channel.Code = "1234";

            return channel;
        }

        protected Priority CreatePriority(Division division)
        {
            Priority priority = new Priority();
            priority.ID = Guid.NewGuid();
            priority.Name = "Priority1";
            priority.Division = division;
            priority.Deleted = false;

            return priority;
        }

        protected Priority CreatePriority(Division division, String name)
        {
            Priority priority = new Priority();
            priority.ID = Guid.NewGuid();
            priority.Name = name;
            priority.Division = division;
            priority.Deleted = false;

            return priority;
        }

        protected SalesDrive CreateSalesDrive(Division division)
        {
            SalesDrive salesDrive = new SalesDrive();
            salesDrive.ID = Guid.NewGuid();
            salesDrive.Division = division;
            salesDrive.Name = "SalesDrive1";
            salesDrive.Deleted = false;

            return salesDrive;
        }

        protected Animation CreateAnimation(Division division, DistributionChannel channel, AnimationType animationType)
        {

            Animation animation = new Animation();
            animation.ID = Guid.NewGuid();
            animation.AnimationType = animationType;
            animation.Code = "01234";
            animation.Name = "Animation1";
            animation.DateClosed = null;
            animation.DateSAPOrderCreated = null;
            animation.SAPDespatchCode = "6454";
            animation.DefaultCustomerReference = "1234";
            animation.DistributionChannel = channel;
            animation.Priority = CreatePriority(division);
            animation.SalesDrive = CreateSalesDrive(division);
            animation.Status = 1;
            animation.RequestedDeliveryDate = DateTime.Now;
            animation.OnCounterDate = DateTime.Now;
            animation.PLVDeliveryDate = DateTime.Now;
            animation.PLVComponentDate = DateTime.Now;
            animation.StockDate = DateTime.Now;
            animation.IDDivision = division.ID;

            return animation;            
        }

        protected Animation CreateAnimation(Division division, DistributionChannel channel, AnimationType animationType, Priority priority)
        {

            Animation animation = new Animation();
            animation.ID = Guid.NewGuid();
            animation.AnimationType = animationType;
            animation.Code = "01234";
            animation.Name = "Animation1";
            animation.DateClosed = null;
            animation.DateSAPOrderCreated = null;
            animation.SAPDespatchCode = "6454";
            animation.DefaultCustomerReference = "1234";
            animation.DistributionChannel = channel;
            animation.Priority = priority;
            animation.SalesDrive = CreateSalesDrive(division);
            animation.Status = 1;
            animation.RequestedDeliveryDate = DateTime.Now;
            animation.OnCounterDate = DateTime.Now;
            animation.PLVDeliveryDate = DateTime.Now;
            animation.PLVComponentDate = DateTime.Now;
            animation.StockDate = DateTime.Now;

            return animation;
        }

        protected ItemType CreateItemType(Division division)
        {
            ItemType itemType = new ItemType();
            itemType.ID = Guid.NewGuid();
            itemType.Name = "ItemType1";
            itemType.Division = division;
            itemType.IncludeInSAPOrders = true;
            itemType.RRPAvailable = true;

            return itemType;

        }

        protected ItemGroup CreateItemGroup(Division division)
        {
            ItemGroup itemGroup = new ItemGroup();
            itemGroup.ID = Guid.NewGuid();
            itemGroup.Name = "ItemGroup1";
            itemGroup.Division = division;

            return itemGroup;
        }

        protected Product CreateProduct(Division division)
        {
            Product product = new Product();
            product.ID = Guid.NewGuid();
            product.Division = division;
            product.MaterialCode = "1345";
            product.Description = "Product Description";
            product.InternationalCode = "International 12345";
            product.EAN = "123456789";
            product.Status = "Active";
            product.Manual = false;
            product.ProcurementType = "Procurement type";
            product.CIV = 999;
            product.Stock = 100;
            product.StockLessPipe = 50;
            product.InTransit = 50;

            return product;

        }

        protected Product CreateProduct(Division division, string materialCode)
        {
            Product product = new Product();
            product.ID = Guid.NewGuid();
            product.Division = division;
            product.MaterialCode = materialCode;
            product.Description = "Product Description";
            product.InternationalCode = "International 12345";
            product.EAN = "123456789";
            product.Status = "Active";
            product.Manual = false;
            product.ProcurementType = "Procurement type";
            product.CIV = 999;
            product.Stock = 100;
            product.StockLessPipe = 50;
            product.InTransit = 50;

            return product;

        }

        protected Signature CreateSignature(Division division)
        {
            Signature signature = new Signature();
            signature.ID = Guid.NewGuid();
            signature.Division = division;
            signature.Name = "Signature1";
            signature.Code = "456";
            signature.Deleted = false;

            return signature;
        }

        protected CustomerBrandExclusion CreateCustomerBrandExclusion(Customer customer, BrandAxe brand)
        {
            CustomerBrandExclusion cbe = new CustomerBrandExclusion();
            cbe.ID = Guid.NewGuid();
            cbe.Customer = customer;
            cbe.BrandAxe = brand;
            cbe.Excluded = true;

            return cbe;
        }

        protected BrandAxe CreateBrandAxe(Signature signature)
        {
            BrandAxe brandAxe = new BrandAxe();
            brandAxe.ID = Guid.NewGuid();
            brandAxe.Signature = signature;
            brandAxe.Name = "BrandAxe1";
            brandAxe.Code = "789";
            brandAxe.Brand = true;
            brandAxe.Deleted = false;

            return brandAxe;
        }

        protected BrandAxe CreateBrandAxe(Signature signature, String name, String code)
        {
            BrandAxe brandAxe = new BrandAxe();
            brandAxe.ID = Guid.NewGuid();
            brandAxe.Signature = signature;
            brandAxe.Name = name;
            brandAxe.Code = code;
            brandAxe.Brand = true;
            brandAxe.Deleted = false;

            return brandAxe;
        }

        protected Multiple CreateMultiple(Product product, int multipleValue)
        {
            Multiple multiple = new Multiple();
            multiple.ID = Guid.NewGuid();
            multiple.Product = product;
            multiple.Value = multipleValue;
            multiple.Deleted = false;
            multiple.Manual = false;

            return multiple;
        }

        protected RetailerType CreateRetailerType(Division division)
        {
            RetailerType retailer = new RetailerType();
            retailer.ID = Guid.NewGuid();
            retailer.Division = division;
            retailer.Name = "Reatiler1";
            retailer.Default = true;
            retailer.Deleted = false;

            return retailer;

        }

        protected AnimationCustomerGroup CreateAnimationCustomerGroup(Animation animation, CustomerGroup group, Division division)
        {
            AnimationCustomerGroup animationGroup = new AnimationCustomerGroup();
            animationGroup.ID = Guid.NewGuid();
            animationGroup.Animation = animation;
            animationGroup.CustomerGroup = group;
            animationGroup.IncludeInAllocation = true;
            animationGroup.OnCounterDate = DateTime.Now;
            animationGroup.PLVComponentDate = DateTime.Now;
            animationGroup.PLVDeliveryDate = DateTime.Now;
            animationGroup.StockDate = DateTime.Now;
            animationGroup.RetailerType = CreateRetailerType(division);

            return animationGroup;
        }

        protected AnimationProduct CreateAnimationProduct(Division division, Animation animation, int normalMultiple, int warehouseMultiple)
        {
            AnimationProduct animationProduct = new AnimationProduct();
            animationProduct.ID = Guid.NewGuid();
            animationProduct.Animation = animation;
            animationProduct.ItemType = CreateItemType(division);
            animationProduct.ItemGroup = CreateItemGroup(division);
            animationProduct.Product = CreateProduct(division);
            animationProduct.Signature = CreateSignature(division);
            animationProduct.BrandAxe = CreateBrandAxe(animationProduct.Signature);
            if (normalMultiple == warehouseMultiple)
            {
                Multiple multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple = multiple;
                animationProduct.Multiple1 = multiple;
            }
            else
            {
                animationProduct.Multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple1 = CreateMultiple(animationProduct.Product, warehouseMultiple);
            }            
            animationProduct.OnCAS = true;
            animationProduct.ConfirmedMADMonth = DateTime.Now;
            animationProduct.StockRisk = true;
            animationProduct.DeliveryRisk = true;
            animationProduct.SortOrder = 1;

            return animationProduct;
        }

        protected AnimationProduct CreateAnimationProduct(Division division, Animation animation, int normalMultiple, int warehouseMultiple, Category category)
        {
            AnimationProduct animationProduct = new AnimationProduct();
            animationProduct.ID = Guid.NewGuid();
            animationProduct.Animation = animation;
            animationProduct.ItemType = CreateItemType(division);
            animationProduct.ItemGroup = CreateItemGroup(division);
            animationProduct.Product = CreateProduct(division);
            animationProduct.Signature = CreateSignature(division);
            animationProduct.BrandAxe = CreateBrandAxe(animationProduct.Signature);
            if (normalMultiple == warehouseMultiple)
            {
                Multiple multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple = multiple;
                animationProduct.Multiple1 = multiple;
            }
            else
            {
                animationProduct.Multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple1 = CreateMultiple(animationProduct.Product, warehouseMultiple);
            }
            animationProduct.OnCAS = true;
            animationProduct.ConfirmedMADMonth = DateTime.Now;
            animationProduct.StockRisk = true;
            animationProduct.DeliveryRisk = true;
            animationProduct.SortOrder = 1;
            animationProduct.Category = category;

            return animationProduct;
        }

        protected AnimationProduct CreateAnimationProduct(Division division, Animation animation, int normalMultiple, int warehouseMultiple, Category category, BrandAxe brandAxe, ItemType itemType)
        {
            AnimationProduct animationProduct = new AnimationProduct();
            animationProduct.ID = Guid.NewGuid();
            animationProduct.Animation = animation;
            animationProduct.ItemType = itemType;
            animationProduct.ItemGroup = CreateItemGroup(division);
            animationProduct.Product = CreateProduct(division);
            animationProduct.Signature = CreateSignature(division);
            if (normalMultiple == warehouseMultiple)
            {
                Multiple multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple = multiple;
                animationProduct.Multiple1 = multiple;
            }
            else
            {
                animationProduct.Multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple1 = CreateMultiple(animationProduct.Product, warehouseMultiple);
            }
            animationProduct.OnCAS = true;
            animationProduct.ConfirmedMADMonth = DateTime.Now;
            animationProduct.StockRisk = true;
            animationProduct.DeliveryRisk = true;
            animationProduct.SortOrder = 1;
            animationProduct.Category = category;
            animationProduct.BrandAxe = brandAxe;

            return animationProduct;
        }

        protected AnimationProduct CreateAnimationProduct(Division division, Animation animation, int normalMultiple, int warehouseMultiple, ItemType itemType)
        {
            AnimationProduct animationProduct = new AnimationProduct();
            animationProduct.ID = Guid.NewGuid();
            animationProduct.Animation = animation;
            animationProduct.ItemType = itemType;
            animationProduct.ItemGroup = CreateItemGroup(division);
            animationProduct.Product = CreateProduct(division);
            animationProduct.Signature = CreateSignature(division);
            animationProduct.BrandAxe = CreateBrandAxe(animationProduct.Signature);
            if (normalMultiple == warehouseMultiple)
            {
                Multiple multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple = multiple;
                animationProduct.Multiple1 = multiple;
            }
            else
            {
                animationProduct.Multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple1 = CreateMultiple(animationProduct.Product, warehouseMultiple);
            }
            animationProduct.OnCAS = true;
            animationProduct.ConfirmedMADMonth = DateTime.Now;
            animationProduct.StockRisk = true;
            animationProduct.DeliveryRisk = true;
            animationProduct.SortOrder = 1;

            return animationProduct;
        }


        protected AnimationProduct CreateAnimationProduct(Division division, Animation animation, int normalMultiple, int warehouseMultiple, BrandAxe brandAxe, Signature signature)
        {
            AnimationProduct animationProduct = new AnimationProduct();
            animationProduct.ID = Guid.NewGuid();
            animationProduct.Animation = animation;
            animationProduct.ItemType = CreateItemType(division);
            animationProduct.ItemGroup = CreateItemGroup(division);
            animationProduct.Product = CreateProduct(division);
            animationProduct.Signature = signature;
            animationProduct.BrandAxe = brandAxe;
            if (normalMultiple == warehouseMultiple)
            {
                Multiple multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple = multiple;
                animationProduct.Multiple1 = multiple;
            }
            else
            {
                animationProduct.Multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple1 = CreateMultiple(animationProduct.Product, warehouseMultiple);
            }            
            animationProduct.OnCAS = true;
            animationProduct.ConfirmedMADMonth = DateTime.Now;
            animationProduct.StockRisk = true;
            animationProduct.DeliveryRisk = true;
            animationProduct.SortOrder = 1;

            return animationProduct;
        }

        protected void DeleteData()
        {
            Db.VersionSnapshots.DeleteAllOnSubmit(Db.VersionSnapshots); Db.SubmitChanges();
            Db.Versions.DeleteAllOnSubmit(Db.Versions); Db.SubmitChanges();
            Db.AuditAlerts.DeleteAllOnSubmit(Db.AuditAlerts); Db.SubmitChanges();
            Db.Sales.DeleteAllOnSubmit(Db.Sales); Db.SubmitChanges();
            Db.CustomerAllocations.DeleteAllOnSubmit(Db.CustomerAllocations); Db.SubmitChanges();
            Db.CustomerGroupAllocations.DeleteAllOnSubmit(Db.CustomerGroupAllocations); Db.SubmitChanges();
            Db.CustomerCapacities.DeleteAllOnSubmit(Db.CustomerCapacities); Db.SubmitChanges();
            Db.AnimationProductDetails.DeleteAllOnSubmit(Db.AnimationProductDetails); Db.SubmitChanges();
            Db.AnimationProducts.DeleteAllOnSubmit(Db.AnimationProducts); Db.SubmitChanges();
            Db.CustomerCategories.DeleteAllOnSubmit(Db.CustomerCategories); Db.SubmitChanges();
            Db.CustomerBrandExclusions.DeleteAllOnSubmit(Db.CustomerBrandExclusions); Db.SubmitChanges();
            Db.CustomerGroupItemTypes.DeleteAllOnSubmit(Db.CustomerGroupItemTypes); Db.SubmitChanges();
            Db.Customers.DeleteAllOnSubmit(Db.Customers); Db.SubmitChanges();
            Db.AnimationCustomerGroups.DeleteAllOnSubmit(Db.AnimationCustomerGroups); Db.SubmitChanges();
            Db.CustomerGroups.DeleteAllOnSubmit(Db.CustomerGroups); Db.SubmitChanges();
            Db.SalesAreas.DeleteAllOnSubmit(Db.SalesAreas); Db.SubmitChanges();
            Db.Animations.DeleteAllOnSubmit(Db.Animations); Db.SubmitChanges();
            Db.AnimationTypes.DeleteAllOnSubmit(Db.AnimationTypes); Db.SubmitChanges();
            Db.DistributionChannels.DeleteAllOnSubmit(Db.DistributionChannels); Db.SubmitChanges();
            Db.ItemGroups.DeleteAllOnSubmit(Db.ItemGroups); Db.SubmitChanges();
            Db.ItemTypes.DeleteAllOnSubmit(Db.ItemTypes); Db.SubmitChanges();
            Db.Priorities.DeleteAllOnSubmit(Db.Priorities); Db.SubmitChanges();
            Db.Multiples.DeleteAllOnSubmit(Db.Multiples); Db.SubmitChanges();
            Db.ProductConfirmeds.DeleteAllOnSubmit(Db.ProductConfirmeds); Db.SubmitChanges();
            Db.ProductReceiveds.DeleteAllOnSubmit(Db.ProductReceiveds); Db.SubmitChanges();
            Db.Products.DeleteAllOnSubmit(Db.Products); Db.SubmitChanges();
            Db.UserRoles.DeleteAllOnSubmit(Db.UserRoles); Db.SubmitChanges();
            Db.BrandAxes.DeleteAllOnSubmit(Db.BrandAxes); Db.SubmitChanges();
            Db.Signatures.DeleteAllOnSubmit(Db.Signatures); Db.SubmitChanges();
            Db.OrderTypes.DeleteAllOnSubmit(Db.OrderTypes); Db.SubmitChanges();
            Db.RetailerTypes.DeleteAllOnSubmit(Db.RetailerTypes); Db.SubmitChanges();
            Db.Categories.DeleteAllOnSubmit(Db.Categories); Db.SubmitChanges();
            Db.SalesDrives.DeleteAllOnSubmit(Db.SalesDrives); Db.SubmitChanges();
            Db.SalesEmployees.DeleteAllOnSubmit(Db.SalesEmployees); Db.SubmitChanges();
            Db.ApplicationSettings.DeleteAllOnSubmit(Db.ApplicationSettings); Db.SubmitChanges();
            Db.Divisions.DeleteAllOnSubmit(Db.Divisions); Db.SubmitChanges();
           
            
        }

        protected AnimationProduct CreateAnimationProduct(Product product, Division division, Animation animation, int normalMultiple, int warehouseMultiple, BrandAxe brandAxe, Signature signature)
        {
            AnimationProduct animationProduct = new AnimationProduct();
            animationProduct.ID = Guid.NewGuid();
            animationProduct.Animation = animation;
            animationProduct.ItemType = CreateItemType(division);
            animationProduct.ItemGroup = CreateItemGroup(division);
            animationProduct.Product = product;
            animationProduct.Signature = signature;
            animationProduct.BrandAxe = brandAxe;
            animationProduct.Multiple = CreateMultiple(animationProduct.Product, normalMultiple);
            animationProduct.Multiple1 = CreateMultiple(animationProduct.Product, warehouseMultiple);
            animationProduct.OnCAS = true;
            animationProduct.ConfirmedMADMonth = DateTime.Now;
            animationProduct.StockRisk = true;
            animationProduct.DeliveryRisk = true;
            animationProduct.SortOrder = 1;

            return animationProduct;
        }


        protected AnimationProduct CreateAnimationProduct(Division division, Animation animation, int normalMultiple, int warehouseMultiple, Signature signature)
        {
            AnimationProduct animationProduct = new AnimationProduct();
            animationProduct.ID = Guid.NewGuid();
            animationProduct.Animation = animation;
            animationProduct.ItemType = CreateItemType(division);
            animationProduct.ItemGroup = CreateItemGroup(division);
            animationProduct.Product = CreateProduct(division);
            animationProduct.Signature = signature;
            if (normalMultiple == warehouseMultiple)
            {
                Multiple multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple = multiple;
                animationProduct.Multiple1 = multiple;
            }
            else
            {
                animationProduct.Multiple = CreateMultiple(animationProduct.Product, normalMultiple);
                animationProduct.Multiple1 = CreateMultiple(animationProduct.Product, warehouseMultiple);
            }          
            animationProduct.OnCAS = true;
            animationProduct.ConfirmedMADMonth = DateTime.Now;
            animationProduct.StockRisk = true;
            animationProduct.DeliveryRisk = true;
            animationProduct.SortOrder = 1;

            return animationProduct;
        }

        protected SalesOrganization CreateSalesOrganization()
        {
            SalesOrganization salesOrganization = new SalesOrganization();
            salesOrganization.ID = Guid.NewGuid();
            salesOrganization.Name = "SalesOrganization1";
            salesOrganization.Code = "7899";
            salesOrganization.Deleted = false;

            return salesOrganization;
        }

        protected SalesOrganization CreateSalesOrganization(string code)
        {
            SalesOrganization salesOrganization = new SalesOrganization();
            salesOrganization.ID = Guid.NewGuid();
            salesOrganization.Name = "SalesOrganization1";
            salesOrganization.Code = code;
            salesOrganization.Deleted = false;

            return salesOrganization;
        }

        protected SalesArea CreateSalesArea(DistributionChannel channel, Division division)
        {
            SalesArea salesArea = new SalesArea();
            salesArea.ID = Guid.NewGuid();
            salesArea.DistributionChannel = channel;
            salesArea.SalesOrganization = CreateSalesOrganization();
            salesArea.Division = division;
            salesArea.Name = "SalesArea1";
            salesArea.Code = "454";
            salesArea.Deleted = false;

            return salesArea;
        }

        protected SalesArea CreateSalesArea(DistributionChannel channel, Division division, string SalesOrgCode)
        {
            SalesArea salesArea = new SalesArea();
            salesArea.ID = Guid.NewGuid();
            salesArea.DistributionChannel = channel;
            salesArea.SalesOrganization = CreateSalesOrganization(SalesOrgCode);
            salesArea.Division = division;
            salesArea.Name = "SalesArea1";
            salesArea.Code = "454";
            salesArea.Deleted = false;

            return salesArea;
        }

        protected ProductReceived CreateProductReceived(Product product, int month, int year, int quantity)
        {
            ProductReceived pr = new ProductReceived();
            pr.ID = Guid.NewGuid();
            pr.Product = product;
            pr.Month = month;
            pr.Year = year;
            pr.Quantity = quantity;

            return pr;
        }


        protected AnimationProductDetail CreateAnimationProductDetail(AnimationProduct animationProduct, SalesArea salesArea, int allocationQuantity)
        {
            
            AnimationProductDetail animationProductDetail = new AnimationProductDetail();
            animationProductDetail.ID = Guid.NewGuid();
            animationProductDetail.AnimationProduct = animationProduct;
            animationProductDetail.SalesArea = salesArea;
            //animationProductDetail.BDCQuantity = 10;
            animationProductDetail.AllocationQuantity = allocationQuantity;

            return animationProductDetail;

        }

        protected AnimationProductDetail CreateAnimationProductDetail(AnimationProduct animationProduct, SalesArea salesArea)
        {
            AnimationProductDetail animationProductDetail = new AnimationProductDetail();
            animationProductDetail.ID = Guid.NewGuid();
            animationProductDetail.AnimationProduct = animationProduct;
            animationProductDetail.SalesArea = salesArea;
            //animationProductDetail.BDCQuantity = 10;            

            return animationProductDetail;

        }

        protected CustomerGroupItemType CreateCustomerGroupItemType(CustomerGroup group, ItemType itemType, Customer customer)
        {
            CustomerGroupItemType customerGroupItemType = new CustomerGroupItemType();
            customerGroupItemType.ID = Guid.NewGuid();
            customerGroupItemType.CustomerGroup = group;
            customerGroupItemType.ItemType = itemType;
            customerGroupItemType.IDCustomer = customer.ID;
            customerGroupItemType.IncludeInSAPOrders = true;
            customerGroupItemType.WarehouseAllocation = true;

            return customerGroupItemType;
        }

        protected CustomerGroup CreateCustomerGroup(SalesArea salesArea, string name, string code)
        {
            CustomerGroup customerGroup = new CustomerGroup();
            customerGroup.ID = Guid.NewGuid();
            customerGroup.SalesArea = salesArea;
            customerGroup.Name = name;
            customerGroup.Code = code;
            customerGroup.IncludeInSystem = true;
            customerGroup.ShowRBMInReporting = true;
            customerGroup.IncludeInSAPOrders = true;
            customerGroup.Manual = false;

            return customerGroup;
        }

        protected Customer CreateCustomer(CustomerGroup customerGroup, string name, string code, Division division)
        {
            Customer customer = new Customer();
            customer.ID = Guid.NewGuid();
            customer.SalesEmployee = CreateSalesEmployee(division);
            customer.CustomerGroup = customerGroup;
            customer.AccountNumber = code;
            customer.Name = name;
            customer.IncludeInSystem = true;
            customer.Manual = false;

            return customer;
        }

        protected Customer CreateCustomer(CustomerGroup customerGroup, string name, string code, Division division, Category category)
        {
            Customer customer = new Customer();
            customer.ID = Guid.NewGuid();
            customer.SalesEmployee = CreateSalesEmployee(division);
            customer.CustomerGroup = customerGroup;
            customer.AccountNumber = code;
            customer.Name = name;
            customer.IncludeInSystem = true;
            customer.Manual = false;
            CreateCustomerCategory(customer, category);

            return customer;
        }

        protected Customer CreateCustomer(CustomerGroup customerGroup, string name, string code, SalesEmployee employee, Division division)
        {
            Customer customer = new Customer();
            customer.ID = Guid.NewGuid();
            customer.SalesEmployee = employee;
            customer.CustomerGroup = customerGroup;
            customer.AccountNumber = code;
            customer.Name = name;
            customer.IncludeInSystem = true;
            customer.Manual = false;

            return customer;
        }

        protected CustomerAllocation CreateCustomerAllocation(Customer customer, AnimationProductDetail animationProductDetail, int fixedAllocation)
        {
            CustomerAllocation customerAllocation = new CustomerAllocation();
            customerAllocation.ID = Guid.NewGuid();
            customerAllocation.Customer = customer;
            customerAllocation.AnimationProductDetail = animationProductDetail;
            customerAllocation.FixedAllocation = fixedAllocation;

            return customerAllocation;
        }

        protected CustomerAllocation CreateCustomerAllocation(Customer customer, AnimationProductDetail animationProductDetail)
        {
            CustomerAllocation customerAllocation = new CustomerAllocation();
            customerAllocation.ID = Guid.NewGuid();
            customerAllocation.Customer = customer;
            customerAllocation.AnimationProductDetail = animationProductDetail;            

            return customerAllocation;
        }

        protected CustomerGroupAllocation CreateCustomerGroupAllocation(CustomerGroup customerGroup, AnimationProductDetail animationProductDetail, int fixedAllocation)
        {
            CustomerGroupAllocation groupAllocation = new CustomerGroupAllocation();
            groupAllocation.ID = Guid.NewGuid();
            groupAllocation.CustomerGroup = customerGroup;
            groupAllocation.AnimationProductDetail = animationProductDetail;
            groupAllocation.ManualFixedAllocation = fixedAllocation;

            return groupAllocation;
        }

        protected CustomerGroupAllocation CreateCustomerGroupAllocation(CustomerGroup customerGroup, AnimationProductDetail animationProductDetail)
        {
            CustomerGroupAllocation groupAllocation = new CustomerGroupAllocation();
            groupAllocation.ID = Guid.NewGuid();
            groupAllocation.CustomerGroup = customerGroup;
            groupAllocation.AnimationProductDetail = animationProductDetail;
          

            return groupAllocation;
        }

        protected CustomerGroupItemType CreateCustomerGroupItemType(CustomerGroup group, ItemType itemType, bool isWarehouse)
        {
            CustomerGroupItemType customerGroupItemType = new CustomerGroupItemType();
            customerGroupItemType.CustomerGroup = group;
            customerGroupItemType.ItemType = itemType;
            customerGroupItemType.WarehouseAllocation = isWarehouse;

            return customerGroupItemType;
        }

        protected CustomerCapacity CreateCustomerCapacity(Customer customer, AnimationType animationType, Priority priority, ItemType itemType, int capacity)
        {
            CustomerCapacity cusCapacity = new CustomerCapacity();
            cusCapacity.ID = Guid.NewGuid();
            cusCapacity.Customer = customer;
            cusCapacity.AnimationType = animationType;
            cusCapacity.Priority = priority;
            cusCapacity.ItemType = itemType;
            cusCapacity.Capacity = capacity;

            return cusCapacity;
        }

        protected Sale CreateSale(Customer customer, BrandAxe brandAxe, decimal manualValue)
        {
            Sale sale = new Sale();
            sale.ID = Guid.NewGuid();
            sale.Customer = customer;
            sale.BrandAxe = brandAxe;
            sale.ManualValue = manualValue;
            sale.Date = DateTime.Now.AddDays(-7);

            return sale;
        }

        protected SalesEmployee CreateSalesEmployee(Division division)
        {
            SalesEmployee salesEmployee = new SalesEmployee();
            salesEmployee.ID = Guid.NewGuid();
            salesEmployee.Name = "Sales Employee";
            salesEmployee.EmployeeNumber = "65454";
            salesEmployee.Division = division;
 
            return salesEmployee;
        }

    }
}
