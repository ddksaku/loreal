using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Threading;
using LorealOptimiseBusiness.Exceptions;
using LorealOptimiseBusiness.ViewMode;
using LorealOptimiseData;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Specialized;
using LorealOptimiseBusiness.DAO;
using LorealOptimiseData.Enums;
using System.Windows;
using LorealOptimiseBusiness.Lists;
using System.Data.Linq;
using LorealOptimiseData.Exceptions;
using LorealOptimiseShared;
using LorealOptimiseShared.Logging;

namespace LorealOptimiseBusiness
{
    public class AnimationManager : BaseManager, INotifyPropertyChanged
    {
        private static AnimationManager instance;

        public static AnimationManager GetInstance()
        {
            if (instance == null)
            {
                instance = new AnimationManager();
            }

            return instance;
        }

        private RetailerType defaultRetailerType;
        public RetailerType DefaultRetailerType
        {
            get
            {
                if (defaultRetailerType == null)
                {
                    defaultRetailerType = RetailerTypeManager.Instance.GetAll().OrderByDescending(rt => rt.Default).FirstOrDefault();
                }

                return defaultRetailerType;
            }
        }

        private Animation animation;
        public Animation Animation
        {
            get
            {
                return animation;
            }
            set
            {
                animation = value;
                Allocations = null;

                if (value != null)
                {
                    // if (value.ID != Guid.Empty)
                    {

                        Animation.ObservableAnimationCustomerGroups.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ObservableAnimationCustomerGroups_CollectionChanged);
                        Animation.ObservableAnimationCustomerGroups.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ObservableAnimationCustomerGroups_CollectionChanged);

                        LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Opening an animation", TaskStatus.InProgress, "Loading Products"));
                        Animation.ObservableProducts.CollectionChanged -= ObservableProducts_CollectionChanged;
                        Animation.ObservableProducts.CollectionChanged += ObservableProducts_CollectionChanged;
                        //ProductManager.Instance.Refresh();
                        //ProductManager.Instance.GetAll();

                        // calculating retailer type allocations for animation products
                        bNeedRecalculateRetailerTypeAllocations = true;

                        //CustomerGroups.Clear();
                        ResetCustomerGroups();

                        foreach (AnimationCustomerGroup acg in Animation.ObservableAnimationCustomerGroups)
                        {
                            acg.PropertyChanged += AnimationCustomerGroup_PropertyChanged;
                        }

                        // LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Opening an animation", TaskStatus.InProgress, "Calculating dynamic values for Procurement Plan"));
                        RecalculateProcurementPlan();
                    }
                }
                
                PropertyChanged(this, new PropertyChangedEventArgs("Animation"));
                PropertyChanged(this, new PropertyChangedEventArgs("AnimationEntity"));

                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Opening an animation", TaskStatus.InProgress, "Loading UI controls"));
            }
        }

        private AnimationAllocations allocations;
        public AnimationAllocations Allocations
        {
            get
            {
                return allocations;
            }
            set
            {
                allocations = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Allocations"));
            }
        }

        private void RecalculateProcurementPlan()
        {
            var grouped =
                Animation.AnimationCustomerGroups.Where(acg => acg.IDRetailerType != Guid.Empty).GroupBy(acg => acg.IDRetailerType).Select(
                    g => new ProcurementPlanAnimation
                             {
                                 RetailerType =
                                     RetailerTypeManager.Instance.GetAll(true).Where(rt => rt.ID == g.Key).First
                                     ().Name,
                                 OnCounterDate = g.Min(acg => acg.OnCounterDate),
                                 ComponentDeadline = g.Min(acg => acg.PLVComponentDate),
                                 DeliveryDeadline = g.Min(acg => acg.PLVDeliveryDate),
                                 StockDeadline = g.Min(acg => acg.StockDate),
                                 AllocationQuantity = 1234
                             });

            ProcurementPlan = new ExtendedObservableCollection<ProcurementPlanAnimation>(grouped);
        }

        private AnimationManager()
        {
            this.CustomerGroups = new ExtendedObservableCollection<CustomerGroup>();
            CustomerGroups.CollectionChanged += new NotifyCollectionChangedEventHandler(CustomerGroups_CollectionChanged);

            AnimationProductDetailManager.Instance.EntityChanged += new EntityChangedEventHandler<AnimationProductDetail>(AnimationProductDetailManager_EntityChanged);
            CustomerGroupAllocationManager.Instance.EntityChanged += new EntityChangedEventHandler<CustomerGroupAllocation>(CustomerGroupAllocationManager_EntityChanged);
            CustomerAllocationManager.Instance.EntityChanged += new EntityChangedEventHandler<CustomerAllocation>(CustomerAllocationManager_EntityChanged);
        }

        void CustomerAllocationManager_EntityChanged(object sender, CustomerAllocation entity)
        {
            if (Allocations != null)
            {
                Allocations.UpdateTotalFixedAllocation();
            }
        }

        void CustomerGroupAllocationManager_EntityChanged(object sender, CustomerGroupAllocation entity)
        {
            if (Allocations != null)
            {
                Allocations.UpdateTotalFixedAllocation();
            }
        }

        void AnimationProductDetailManager_EntityChanged(object sender, AnimationProductDetail entity)
        {
            if (entity.AnimationProduct != null && entity.AnimationProduct.Animation != null && entity.AnimationProduct.IDAnimation == Animation.ID)
            {
                entity.AnimationProduct.CalculateTotals();

                foreach (AnimationProduct ap in Animation.AnimationProducts)
                {
                    ap.TotalAnimationQuantity = -1;
                }
            }
        }

        void ObservableProducts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ItemGroups.Clear();
        }

        void CustomerGroups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Products.Clear();
        }

        void ObservableAnimationCustomerGroups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AnimationCustomerGroup newAcg = e.NewItems[0] as AnimationCustomerGroup;
                newAcg.PropertyChanged += new PropertyChangedEventHandler(AnimationCustomerGroup_PropertyChanged);

                CustomerGroups.Remove(CustomerGroups.Single(cg => cg.ID == newAcg.IDCustomerGroup));
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //CustomerGroup cg = GetCustomerGroupByID((e.OldItems[0] as AnimationCustomerGroup).IDCustomerGroup);
                CustomerGroup cg = (e.OldItems[0] as AnimationCustomerGroup).CustomerGroup;

                if (cg.IncludeInSystem)
                {
                    CustomerGroups.Add(cg);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ResetCustomerGroups();
            }

            RecalculateProcurementPlan();
        }

        void AnimationCustomerGroup_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.StartsWith("Modified") == false)
            {
                RecalculateProcurementPlan();
                bNeedRecalculateRetailerTypeAllocations = true;
            }
        }

        private ExtendedObservableCollection<Animation> allAnimations;
        public ExtendedObservableCollection<Animation> AllAnimations
        {
            get
            {
                if (allAnimations == null)
                {
                    allAnimations = new ExtendedObservableCollection<Animation>(Db.Animations.Where(a => a.IDDivision == LoggedUser.LoggedDivision.ID).OrderByDescending(a => a.SalesDrive.Year).ThenByDescending(a=>a.SalesDrive.Name).ThenBy(a => a.OnCounterDate));
                }

                return allAnimations;
            }
            set
            {
                allAnimations = value;
                PropertyChanged(this, new PropertyChangedEventArgs("AllAnimations"));
            }
        }

        public Animation GetByID(Guid id)
        {
            return Db.Animations.Where(a => a.ID == id).FirstOrDefault();
        }

        public CustomerGroup GetCustomerGroupByID(Guid id)
        {
            return CustomerGroupManager.Instance.GetById(id);
        }

        public AnimationProductDetail GetAnimationProductDetailById(Guid id)
        {
            return Db.AnimationProductDetails.SingleOrDefault(apd => apd.ID == id);
        }

        public bool NeedToRecreateAllocation()
        {
            Animation originalAnimation = Db.Animations.GetOriginalEntityState(Animation);
            if (originalAnimation != null && (originalAnimation.StatusName == "Cleared" || originalAnimation.StatusName == "Closed"))
            {
                return true;
            }
            return false;
        }

        public void RecreateAllocations()
        {
            LongTaskExecutor recreateAllocationsExecutor = new LongTaskExecutor("Recreating allocations");
            recreateAllocationsExecutor.DoWork +=new DoWorkEventHandler(recreateAllocationsExecutor_DoWork);
            recreateAllocationsExecutor.Run();
        }


        void recreateAllocationsExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            // call DB trigger to create/remove allocations
            Db.up_recreateAllocationsAnimation(Animation.ID, true);

            // refresh allocations to UI
            if (this.Allocations != null)
            {
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs(string.Empty, TaskStatus.InProgress, "Refreshing allocations"));
                this.Allocations.RefreshAllocations();
            }

            // recalculate capacities
            this.RecalculateAnimationProduct(RecalculationType.CalculateTotal);
        }

        private void recreateAllocationsForProduct(AnimationProduct animationProduct)
        {
            LongTaskExecutor recreateAllocationsExecutor = new LongTaskExecutor("Recreating allocations");
            recreateAllocationsExecutor.DoWork += new DoWorkEventHandler(recreateAllocationsForProductExecutor_DoWork);
            recreateAllocationsExecutor.Run(animationProduct);
        }

        void recreateAllocationsForProductExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            AnimationProduct animationProduct =e.Argument as AnimationProduct;

            if (animationProduct == null)
            {
                throw new ArgumentException("AnimationProduct can not be null");
            }

            // call DB trigger to create/remove allocations
            Db.up_recreateAllocationsAnimationProduct(animationProduct.ID, true);

            // refresh allocations to UI
            if (this.Allocations != null)
            {
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs(string.Empty, TaskStatus.InProgress, "Refreshing allocations"));
                this.Allocations.RefreshAllocations();
            }

            // recalculate capacities
            this.RecalculateAnimationProduct(RecalculationType.CalculateTotal, animationProduct);
        }

        private void submitDBChanges()
        {
            submitDBChanges_DoWork(null, null);
            //LongTaskExecutor submitDBChangesExecutor = new LongTaskExecutor("Saving ...");
            //submitDBChangesExecutor.DoWork += new DoWorkEventHandler(submitDBChanges_DoWork);
            //submitDBChangesExecutor.Run();
        }

        void submitDBChanges_DoWork(object sender, DoWorkEventArgs e)
        {
            Db.SubmitChanges();
        }

        public ObservableCollection<ItemGroup> ItemGroups
        {
            get
            {
                if (Animation != null)
                {
                    IEnumerable<ItemGroup> itemGroups =
                        Animation.AnimationProducts.Select(ap => ap.ItemGroup).Distinct();

                    return
                        new ObservableCollection<ItemGroup>(itemGroups);
                }

                return new ObservableCollection<ItemGroup>();
            }
        }

        public ExtendedObservableCollection<AnimationProduct> Products
        {
            get
            {
                if (Animation != null)
                {
                    IEnumerable<AnimationProduct> products =
                        Animation.AnimationProducts.Select(ap => ap).Distinct();

                    return
                        new ExtendedObservableCollection<AnimationProduct>(products);
                }

                return new ExtendedObservableCollection<AnimationProduct>();
            }
        }

        public ExtendedObservableCollection<AnimationProductDetail> ProductOnCountry
        {
            get
            {
                if (Animation != null)
                {
                    //IEnumerable<AnimationProductDetail> products =
                    //    Animation.AnimationProducts.Select(ap => ap).SelectMany(apd => apd.AnimationProductDetails).Distinct();

                    IEnumerable<AnimationProductDetail> products = Animation.ObservableProductDetails.Distinct();

                    return
                        new ExtendedObservableCollection<AnimationProductDetail>(products);
                }

                return new ExtendedObservableCollection<AnimationProductDetail>();
            }
        }

        public ExtendedObservableCollection<CustomerGroup> AllocationCustomerGroups
        {
            get
            {
                if (Animation != null)
                {
                    IEnumerable<CustomerGroup> customers =
                        Animation.AnimationCustomerGroups.Select(acg => acg).Select(cg => cg.CustomerGroup).Distinct();

                    return
                        new ExtendedObservableCollection<CustomerGroup>(customers);
                }

                return new ExtendedObservableCollection<CustomerGroup>();
            }
        }

        private ExtendedObservableCollection<CustomerGroup> customerGroups;
        public ExtendedObservableCollection<CustomerGroup> CustomerGroups
        {
            get
            {
                return customerGroups;
            }
            set
            {
                customerGroups = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CustomerGroups"));
                }
            }
        }

        public bool IsSelectedCustomerGroup
        {
            get;
            set;
        }

        public bool IsSelectedAnimationCustomerGroup
        {
            get;
            set;
        }

        public AnimationProduct SelectedAnimationProduct
        {
            get;
            set;
        }

        public IEnumerable<AnimationCustomerGroup> SelectedAnimationCustomerGroups
        {
            get;
            set;
        }

        private AnimationProductDetail selectedProductDetail;
        public AnimationProductDetail SelectedProductDetail
        {
            get
            {
                return selectedProductDetail;
            }
            set
            {
                selectedProductDetail = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedProductDetail"));
            }
        }

        public IEnumerable<CustomerGroup> SelectedCustomerGroups
        {
            get;
            set;
        }

        public void InsertOrUpdate(Animation entity)
        {
            bool isNewAnimation = false;
            Animation originalAnimation = null;

            if (entity.ID == Guid.Empty)
            {
                isNewAnimation = true;
                entity.IDDivision = LoggedUser.Division.ID;
                Db.Animations.InsertOnSubmit(entity);
            }
            else
            {
                originalAnimation = Db.Animations.GetOriginalEntityState(entity);
            }

            DbDataContext.GetInstance().CommandTimeout = Utility.RunAllocationCommandTimeOut;
            try
            {
                Db.SubmitChanges();
            }
            finally
            {
                DbDataContext.GetInstance().CommandTimeout = Utility.SqlCommandTimeOut;
            }

            if (isNewAnimation == false && originalAnimation != null)
            {
                if (originalAnimation.IDPriority != entity.IDPriority || originalAnimation.IDAnimationType != entity.IDAnimationType)
                {
                    DbDataContext.MakeNewInstance();
                    this.Animation = GetByID(entity.ID);
                }
                else
                {
                    // still need to get changes from database for non-changed values and update UI
                    Db.Refresh(RefreshMode.OverwriteCurrentValues, Animation);
                }
            }
            else
            {
                // still need to get changes from database for non-changed values and update UI
                Db.Refresh(RefreshMode.OverwriteCurrentValues, Animation);
            }

            // still need to get changes from database for non-changed values and update UI
            //Db.Refresh(RefreshMode.OverwriteCurrentValues, Animation);

            PropertyChanged(this, new PropertyChangedEventArgs("Animation"));

            if (isNewAnimation)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("AnimationEntity"));
            }
        }

        public void Delete(Animation entity)
        {
            if (entity.AnimationCustomerGroups != null)
            {
                Db.AnimationCustomerGroups.DeleteAllOnSubmit(entity.AnimationCustomerGroups);
                entity.AnimationCustomerGroups.Clear();
            }

            if (entity.AnimationProducts != null)
            {
                Db.AnimationProducts.DeleteAllOnSubmit(entity.AnimationProducts);
                entity.AnimationProducts.Clear();
            }

            Db.Animations.DeleteOnSubmit(entity);
            Db.SubmitChanges();
        }

        public void ProductInsertUpdate(AnimationProduct entity)
        {
            if (entity.IDBrandAxe == Guid.Empty)
            {
                entity.IDBrandAxe = null;
            }
            if (entity.IDCategory == Guid.Empty)
            {
                entity.IDCategory = null;
            }
            if (entity.IDSignature == Guid.Empty)
            {
                entity.IDSignature = null;
            }
            if (entity.IDItemType == Guid.Empty)
            {
                entity.IDItemType = null;
            }
            if (entity.IDMultipleNormal == Guid.Empty)
            {
                entity.IDMultipleNormal = null;
            }
            if (entity.IDMultipleWarehouse == Guid.Empty)
            {
                entity.IDMultipleWarehouse = null;
            }

            if (entity.ID == Guid.Empty)
            {
                Db.AnimationProducts.InsertOnSubmit(entity);
            }

            try
            {
                bool isInsert = entity.ID == Guid.Empty;

                submitDBChanges();
                //Db.SubmitChanges();

                if (isInsert)
                {
                    //TV: Allocations are recreated in trigger
                    recreateAllocationsForProduct(entity);
                }
            }
            catch (SqlException exc)
            {
                SqlException sqlExc = exc as SqlException;
                if (sqlExc.Number == 50000 && sqlExc.Class == 16)
                {
                    AnimationProduct originalEntity = new DbDataContext().AnimationProducts.Single(ap => ap.ID == entity.ID);
                    if (originalEntity != null)
                    {
                        switch(sqlExc.State)
                        {
                            case 28:
                                entity.CleanEntityRef("IDItemType");
                                entity.IDItemType = originalEntity.IDItemType;

                                entity.CleanEntityRef("IDSignature");
                                entity.IDSignature = originalEntity.IDSignature;

                                entity.CleanEntityRef("IDBrandAxe");
                                entity.IDBrandAxe = originalEntity.IDBrandAxe;

                                entity.CleanEntityRef("IDCategory");
                                entity.IDCategory = originalEntity.IDCategory;
                                break;
                            case 30: case 32: case 33: case 34: case 35:
                                entity.CleanEntityRef("IDMultipleNormal");
                                entity.IDMultipleNormal = originalEntity.IDMultipleNormal;
                                entity.IDMultipleWarehouse = originalEntity.IDMultipleWarehouse;
                                break;
                        }
                        
                        Db.SubmitChanges();
                    }
                    else
                    {
                        ProductDelete(entity);
                        Animation.AnimationProducts.Remove(entity);
                    }
                }
                throw;
            }
            catch (LorealChangeConflictException exc)
            {
                ResolveChangeConflict(exc);
            }
            catch (Exception exc)
            {
                if (entity.ID == Guid.Empty)
                {
                    Db.AnimationProducts.DeleteOnSubmit(entity);
                }
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("TableViewExceptionSql", Utility.GetExceptionsMessages(exc)));
            }
        }

        public void RecalculateAnimationProduct(RecalculationType calcType, AnimationProduct entity = null)
        {
            if (calcType == RecalculationType.CalculateTotal)
            {
                LongTaskExecutor recalculationExecutor = new LongTaskExecutor("Recalculating capacities ...");
                recalculationExecutor.DoWork += new DoWorkEventHandler(recalculationExecutor_DoWork);
                recalculationExecutor.Run(entity);
            }
            else if (calcType == RecalculationType.CalculateActiveAnimations)
            {
                LongTaskExecutor calcActiveAnimationExecutor = new LongTaskExecutor("Recalculating active animations ...");
                calcActiveAnimationExecutor.DoWork += new DoWorkEventHandler(calcActiveAnimationExecutor_DoWork);
                calcActiveAnimationExecutor.Run();
            }
            else if (calcType == RecalculationType.CalculateTotalCapacity && entity != null)
            {
                LongTaskExecutor calcTotalCapacityExecutor = new LongTaskExecutor("Recalculating total capacity for " + entity.ProductIdentifier);
                calcTotalCapacityExecutor.DoWork += new DoWorkEventHandler(calcTotalCapacityExecutor_DoWork);
                calcTotalCapacityExecutor.Run();
            }
            else if (calcType == RecalculationType.CalculateProductRecieved && entity != null)
            {
                LongTaskExecutor calcTotalCapacityExecutor = new LongTaskExecutor("Recalculating product recieved  ");
                calcTotalCapacityExecutor.DoWork += new DoWorkEventHandler(calcProductRecievedExecutor_DoWork);
                calcTotalCapacityExecutor.Run();
            }
            else if (calcType == RecalculationType.CalculateTotalAnimationQuantity)
            {
                Animation.AnimationProducts.ToList().ForEach(ap => ap.TotalAnimationQuantity = -1);
            }
        }

        void calcProductRecievedExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Animation != null)
            {
                if (Animation != null && e.Argument != null && e.Argument as AnimationProduct != null)
                {
                    (e.Argument as AnimationProduct).CalculateProductRecieved();
                }                
            }
        }

        void recalculationExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Animation != null)
            {
                LongTaskExecutor executor = sender as LongTaskExecutor;
                Animation.RecalculateAnimationProduct(e.Argument as AnimationProduct, executor);
            }
        }

        void calcTotalCapacityExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Animation != null && e.Argument != null && e.Argument as AnimationProduct != null)
            {
                (e.Argument as AnimationProduct).CalculateTotalCapacity();
            }
        }

        void calcActiveAnimationExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Animation != null)
            {
                foreach (AnimationProduct ap in Animation.AnimationProducts)
                {
                    ap.CalculateActiveAnimations();
                }
            }
        }

        private bool bNeedRecalculateRetailerTypeAllocations = false;
        public void RecalculateRetailerTypeAllocations()
        {
            if (bNeedRecalculateRetailerTypeAllocations)
            {
                ISingleResult<up_GetRetailerTypeResult> results = Db.up_GetRetailerType(Animation.ID);
                foreach (up_GetRetailerTypeResult result in results)
                {
                    AnimationProduct ap = Animation.ObservableProducts.FirstOrDefault(itm => itm.ID == result.IDAnimationProduct);
                    if (ap != null)
                    {
                        ap.SetRetailerTypeAllocation(result.RetailerType, result.CalculatedAllocation);
                    }
                }

                bNeedRecalculateRetailerTypeAllocations = false;
            }
        }

        public void ProductDelete(AnimationProduct entity)
        {
            try
            {
                Db.AnimationProducts.DeleteOnSubmit(entity);
                Db.SubmitChanges();

                RecalculateAnimationProduct(RecalculationType.CalculateActiveAnimations);
                RecalculateAnimationProduct(RecalculationType.CalculateTotalAnimationQuantity);
                Animation.ObservableProductDetails = null;
            }
            catch (Exception exc)
            {
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("TableViewExceptionSql", Utility.GetExceptionsMessages(exc)));
            }
        }

        private void SetAnimationCustomerGroupOverrides(AnimationCustomerGroup acg)
        {
            AnimationCustomerGroup original = Db.AnimationCustomerGroups.GetOriginalEntityState(acg);

            if (original.SAPDespatchCode != acg.SAPDespatchCode)
            {
                acg.OverridenFlags |= (int)AnimationCustomerGroupOverrides.SapDespatchCode;
            }

            if (original.OnCounterDate != acg.OnCounterDate)
            {
                acg.OverridenFlags |= (int)AnimationCustomerGroupOverrides.OnCounterDate;
            }

            if (original.PLVComponentDate != acg.PLVComponentDate)
            {
                acg.OverridenFlags |= (int)AnimationCustomerGroupOverrides.PLVComponentDate;
            }

            if (original.PLVDeliveryDate != acg.PLVDeliveryDate)
            {
                acg.OverridenFlags |= (int)AnimationCustomerGroupOverrides.PLVDeliveryDate;
            }

            if (original.StockDate != acg.StockDate)
            {
                acg.OverridenFlags |= (int)AnimationCustomerGroupOverrides.StockDate;
            }
        }

        public void CustomerGroupInsertUpdate(AnimationCustomerGroup entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.AnimationCustomerGroups.InsertOnSubmit(entity);
                entity.OverridenFlags = (int)AnimationCustomerGroupOverrides.None;
            }

            if (entity.OverridenFlags == null)
            {
                entity.OverridenFlags = (int)AnimationCustomerGroupOverrides.None;
            }

            SetAnimationCustomerGroupOverrides(entity);

            entity.Animation = Animation;

            try
            {
                Db.SubmitChanges();
            }
            catch(SqlException sqlExc)
            {
                if (sqlExc.Number == 50000 && sqlExc.Class == 16 && sqlExc.State == 31)
                {
                    if (sqlExc.Errors.Count > 0)
                    {
                        MessageBox.Show(sqlExc.Errors[0].Message);
                    }
                    else
                    {
                        MessageBox.Show(sqlExc.Message);
                    }
                }
                else
                    MessageBox.Show(SystemMessagesManager.Instance.GetMessage("TableViewExceptionSql", Utility.GetExceptionsMessages(sqlExc)));

                Db.AnimationCustomerGroups.DeleteOnSubmit(entity);
                Db.SubmitChanges();

                throw;
            }
        }

        private ExtendedObservableCollection<ProcurementPlanAnimation> procurementPlan;
        public ExtendedObservableCollection<ProcurementPlanAnimation> ProcurementPlan
        {
            get
            {
                if (Animation != null && procurementPlan == null)
                {
                    RecalculateProcurementPlan();
                }

                return procurementPlan;
            }
            set
            {
                procurementPlan = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ProcurementPlan"));
            }
        }

        private void ResetCustomerGroups()
        {
            CustomerGroups = new ExtendedObservableCollection<CustomerGroup>(CustomerGroupManager.Instance.GetAll().Where(c => c.SalesArea.IDDivision == LoggedUser.Division.ID && c.IncludeInSystem &&
                (Animation.ObservableAnimationCustomerGroups.Select(acg => acg.IDCustomerGroup).Contains(c.ID) == false)).OrderBy(c => c.SalesArea.Name).ThenBy(c => c.Name));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Commands

        #region Include
        private ICommand includeCommand;
        public ICommand IncludeCommand
        {
            get
            {
                if (includeCommand == null)
                {
                    includeCommand = new RelayCommand(param => CanInclude(), param => Include());
                }

                return includeCommand;
            }
        }

        private ICommand includeAllCommand;
        public ICommand IncludeAllCommand
        {
            get
            {
                if (includeAllCommand == null)
                {
                    includeAllCommand = new RelayCommand(param => CanIncludeAll(), param => IncludeAll());
                }
                return includeAllCommand;
            }
        }

        private void Include()
        {
            LongTaskExecutor includeExecutor = new LongTaskExecutor("Including selected customer groups");
            includeExecutor.TaskFinished += new EventHandler(includeExecutor_TaskFinished);
            includeExecutor.DoWork += new DoWorkEventHandler(includeExecutor_DoWork);
            includeExecutor.Run();
        }

        void includeExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            include(this.SelectedCustomerGroups.ToList(), sender as LongTaskExecutor);
        }

        void includeExecutor_TaskFinished(object sender, EventArgs e)
        {
            AfterIncludeOrRemove();
        }

        void AfterIncludeOrRemove()
        {
            RecalculateAnimationProduct(RecalculationType.CalculateTotal);

            Animation.ObservableProductDetails = null;

            // refresh these animation product details
            Db.Refresh(RefreshMode.OverwriteCurrentValues, Animation.ObservableProductDetails);
            
            Allocations = null;

            bNeedRecalculateRetailerTypeAllocations = true;
        }

        private void IncludeAll()
        {
            LongTaskExecutor includeAllExecutor = new LongTaskExecutor("Including all customer groups into animation");
            includeAllExecutor.TaskFinished += new EventHandler(includeAllExecutor_TaskFinished);
            includeAllExecutor.DoWork += new DoWorkEventHandler(includeAllExecutor_DoWork);
            includeAllExecutor.Run();
        }

        void includeAllExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            include(CustomerGroups.ToList(), sender as LongTaskExecutor);
        }

        void includeAllExecutor_TaskFinished(object sender, EventArgs e)
        {
            AfterIncludeOrRemove();
        }

        private void include(List<CustomerGroup> groups, LongTaskExecutor executor)
        {
            try
            {

                for (int i = 0; i < groups.Count; i++)
                {
                    AnimationCustomerGroup cga = new AnimationCustomerGroup();
                    cga.IDCustomerGroup = groups[i].ID;
                    cga.IDAnimation = Animation.ID;

                    cga.OnCounterDate = Animation.OnCounterDate.HasValue? Animation.OnCounterDate.Value:DateTime.Now;
                    cga.PLVComponentDate = Animation.PLVComponentDate.HasValue? Animation.PLVComponentDate.Value:DateTime.Now;
                    cga.PLVDeliveryDate = Animation.PLVDeliveryDate.HasValue? Animation.PLVDeliveryDate.Value:DateTime.Now;
                    cga.StockDate = Animation.StockDate.HasValue? Animation.StockDate.Value:DateTime.Now;
                    cga.IncludeInAllocation = true;

                    // set the default retailer type
                    if (DefaultRetailerType != null)
                        cga.IDRetailerType = DefaultRetailerType.ID;

                    // set the default date for SAP Promotion Despatch Code
                    if (Animation != null)
                        cga.SAPDespatchCode = Animation.SAPDespatchCode;

                    // try to insert into DB if it is valid.
                    string errorMessage;
                    if (cga.IsValid(out errorMessage))
                    {
                        CustomerGroupInsertUpdate(cga);
                    }
                    else
                    {
                        continue;
                    }

                    Animation.ObservableAnimationCustomerGroups.Add(cga);

                    if (executor != null)
                    {
                        executor.SendProgressMessage(groups[i].Name + " is included into the animation.");
                    }
                }

                executor.SendProgressMessage("Recreating allocations");
                LongTaskExecutor.DoEvents();

                Db.up_recreateAllocationsAnimation(Animation.ID, false);
               
            }
            catch (SqlException sqlExc)
            {
                if (sqlExc.Number == 50000 && sqlExc.Class == 16 && sqlExc.State == 31)
                {
                    DbDataContext.MakeNewInstance();
                    this.Animation = GetByID(this.Animation.ID);
                }
                else
                {
                    MessageBox.Show(SystemMessagesManager.Instance.GetMessage("TableViewExceptionSql", Utility.GetExceptionsMessages(sqlExc)));
                }
            }

        }

        private bool CanInclude()
        {
            if (DefaultRetailerType == null)
            {
                IncludeErrorMessage = "No retailer type exists";
                return false;
            }

            return IsSelectedCustomerGroup && Animation != null && Animation.ID != Guid.Empty;
        }

        private bool CanIncludeAll()
        {
            return customerGroups.Count > 0 && Animation != null && Animation.ID != Guid.Empty;
        }

        #endregion

        #region Remove

//        private void PreProcessBeforeRemove(AnimationCustomerGroup acg)
//        {
//            // delete customer allocations
//            string caDeleteString = @"Delete from CustomerAllocation
//                                      Where IDAnimationProductDetail in (Select ID from AnimationProductDetail Where IDAnimationProduct in (Select ID from AnimationProduct Where IDAnimation={0}) )
//                                      AND (Select IDCustomerGroup from Customer as C Where C.ID = IDCustomer) = {1}";
//            int rows = Db.ExecuteCommand(caDeleteString, acg.IDAnimation, acg.IDCustomerGroup);

//            // delete customer group allocations
//            string cgaDeleteString = @"Delete from CustomerGroupAllocation
//                                      Where IDAnimationProductDetail in (Select ID from AnimationProductDetail Where IDAnimationProduct in (Select ID from AnimationProduct Where IDAnimation={0}) )
//                                      AND IDCustomerGroup = {1}";
//            rows = Db.ExecuteCommand(cgaDeleteString, acg.IDAnimation, acg.IDCustomerGroup);
                
//        }

        private void Remove()
        {
            LongTaskExecutor removeExecutor = new LongTaskExecutor("Removing a customer group");
            removeExecutor.TaskFinished += new EventHandler(removeExecutor_TaskFinished);
            removeExecutor.DoWork += new DoWorkEventHandler(removeExecutor_DoWork);
            removeExecutor.Run();
        }

        void removeExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            remove(SelectedAnimationCustomerGroups.ToList(), sender as LongTaskExecutor);
        }

        void removeExecutor_TaskFinished(object sender, EventArgs e)
        {
            AfterIncludeOrRemove();
        }

        LongTaskExecutor removeAllExecutor;
        private void RemoveAll()
        {
            removeAllExecutor = new LongTaskExecutor("Removing all customer groups from animation");
            removeAllExecutor.TaskFinished += new EventHandler(removeAllExecutor_TaskFinished);
            removeAllExecutor.DoWork += new DoWorkEventHandler(removeAllExecutor_DoWork);
            removeAllExecutor.Run();
        }

        void removeAllExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            removeAll(animation.ID, sender as LongTaskExecutor);
        }

        void removeAllExecutor_TaskFinished(object sender, EventArgs e)
        {
            AfterIncludeOrRemove();
        }

        private void removeAll(Guid idAnimation, LongTaskExecutor executor)
        {
            try
            {
                executor.SendProgressMessage("Deleting allocations");
                Db.ExecuteCommand("DELETE FROM AnimationCustomerGroup WHERE IDAnimation = {0}", idAnimation);
                animation.ObservableAnimationCustomerGroups.Clear();
            }
            catch (Exception exc)
            {
                Logger.Log(exc.ToString(), LogLevel.Error);
                MessageBox.Show(exc.Message);
            }
        }

        private void remove(List<AnimationCustomerGroup> animationCustomerGroups, LongTaskExecutor executor)
        {
            try
            {
                string errorMsg = string.Empty;
                for (int i = animationCustomerGroups.Count - 1; i >= 0; i--)
                {
                    AnimationCustomerGroup animationCustomerGroupToDelete = animationCustomerGroups[i];
                    string customerGroupName = animationCustomerGroupToDelete.CustomerGroup.Name;

                    animation.ObservableAnimationCustomerGroups.Remove(animationCustomerGroupToDelete);
                    if (animationCustomerGroupToDelete.ID != Guid.Empty)
                    {
                        Db.AnimationCustomerGroups.DeleteOnSubmit(animationCustomerGroupToDelete);
                    }

                    if (executor != null)
                    {
                        executor.SendProgressMessage(customerGroupName + " is removed from the animation");
                    }

                    Db.SubmitChanges();
                }

                //executor.SendProgressMessage("Deleting allocations");
                //Db.SubmitChanges();
            }
            catch (Exception exc)
            {
                Logger.Log(exc.ToString(), LogLevel.Error);
                MessageBox.Show(exc.Message);
            }

        }

        private bool CanRemove()
        {
            return IsSelectedAnimationCustomerGroup;
        }

        private bool CanRemoveAll()
        {
            if (Animation != null)
            {
                return Animation.ObservableAnimationCustomerGroups.Count > 0;
            }
            return false;
        }

        public event Action<string> RemoveFinished;

        private ICommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                {
                    removeCommand = new RelayCommand(param => CanRemove(), param => Remove());
                }

                return removeCommand;
            }
        }

        private ICommand removeAllCommand;
        public ICommand RemoveAllCommand
        {
            get
            {
                if (removeAllCommand == null)
                {
                    removeAllCommand = new RelayCommand(param => CanRemoveAll(), param => RemoveAll());
                }
                return removeAllCommand;
            }
        }

        #endregion

        #region Create New Animation

        private string savingErrorMessage;
        public string SavingErrorMessage
        {
            get
            {
                return savingErrorMessage;
            }
            set
            {
                savingErrorMessage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SavingErrorMessage"));
            }
        }

        private string includeErrorMessage;
        public string IncludeErrorMessage
        {
            get
            {
                return includeErrorMessage;
            }
            set
            {
                includeErrorMessage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IncludeErrorMessage"));
            }
        }

        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(param => CanSave(), param => Save());
                }

                return saveCommand;
            }
        }

        private bool CanSave()
        {
            if (Animation != null)
            {
                string errorMessage = string.Empty;
                bool f = Animation.IsValid(out errorMessage);
                SavingErrorMessage = errorMessage;
                return f;
            }

            return true;
        }

        public void Save()
        {
            LongTaskExecutor saveAnimationExecutor = new LongTaskExecutor("Saving animation");
            saveAnimationExecutor.DoWork += new DoWorkEventHandler(saveAnimationExecutor_DoWork);
            saveAnimationExecutor.Run();
        }

        void saveAnimationExecutor_DoWork(object sender, DoWorkEventArgs e)
        {
            bool bNew = animation.ID == Guid.Empty;
            bool existAnyChangesForRecalc = false;
            string lastSavedStatusName = string.Empty;
            if (Animation.ID != Guid.Empty)
            {
                lastSavedStatusName = Db.Animations.GetOriginalEntityState(Animation).StatusName;
            }

            // Preprocessing before saving
            ModifiedMemberInfo[] changedMembers = Db.Animations.GetModifiedMembers(Animation);
            foreach (ModifiedMemberInfo info in changedMembers)
            {
                if (info.Member.Name == "IDSalesDrive" || info.Member.Name == "IDPriority" || info.Member.Name == "IDAnimationType")
                {
                    existAnyChangesForRecalc = true;
                    break;
                }
            }

            try
            {
                InsertOrUpdate(Animation);
                if (bNew)
                {
                    //MessageBox.Show("Animation was successfully created.");
                    MessageBox.Show(SystemMessagesManager.Instance.GetMessage("AnimationCreated"));
                }
                else
                {
                    //MessageBox.Show("Animation was successfully updated");
                    MessageBox.Show(SystemMessagesManager.Instance.GetMessage("AnimationUpdated"));
                }
            }
            catch (SqlException exc)
            {
                // trigger
                if (exc.Number == 50000 && exc.Class == 16)
                {
                    // Animation originalEntity = new DbDataContext().Animations.Single(ani => ani.ID == animation.ID);
                    Animation originalEntity = Db.Animations.GetOriginalEntityState(Animation);
                    if (originalEntity != null)
                    {
                        animation.CleanEntityRef("IDAnimationType");
                        animation.CleanEntityRef("IDPriority");

                        animation.IDAnimationType = originalEntity.IDAnimationType;
                        animation.AnimationType = Db.AnimationTypes.FirstOrDefault(at => at.ID == animation.IDAnimationType);

                        animation.IDPriority = originalEntity.IDPriority;
                        animation.Priority = Db.Priorities.FirstOrDefault(p => p.ID == animation.IDPriority);

                        Db.SubmitChanges();
                    }

                    if (exc.Errors.Count > 0)
                    {
                        MessageBox.Show(exc.Errors[0].Message);
                    }
                    else
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
                else
                {
                    throw;
                }
                return;
            }
            catch (LorealChangeConflictException exc)
            {
                ResolveChangeConflict(exc);

                if (exc.IsConflictOnField == false)
                {
                    //MessageBox.Show("Animation was successfully updated");
                    MessageBox.Show(SystemMessagesManager.Instance.GetMessage("AnimationUpdated"));
                }

                PropertyChanged(this, new PropertyChangedEventArgs("Animation"));
                return;
            }

            // do recalculation only when needed
            if (existAnyChangesForRecalc)
            {
                RecalculateAnimationProduct(RecalculationType.CalculateTotal);
            }

            AllAnimations = null;

            if (lastSavedStatusName != "Closed" && Animation.StatusName == "Closed")
            {
                BackgroundWorker bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                bgWorker.RunWorkerAsync();
            }

            if (lastSavedStatusName != "Draft" && Animation.StatusName == "Draft")
            {
                // refresh allocations
                if (this.Allocations != null)
                {
                    this.Allocations.RefreshAllocations();
                }
            }
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Db.up_createOrderExport(Animation.ID);            
        }

        private void ResolveChangeConflict(LorealChangeConflictException exc)
        {
            if (exc.IsConflictOnField)
            {
                //ServiceLocator is bettter approach to show error message. MessageBox should not be used directly from Business layer
                MessageBox.Show(exc.Message);
            }
        }

        private ICommand openAllocationCommand;
        public ICommand OpenAllocationCommand
        {
            get
            {
                if (openAllocationCommand == null)
                {
                    openAllocationCommand = new RelayCommand(param => CanOpenAllocation(), param => OpenAllocation());
                }

                return openAllocationCommand;
            }
        }

        public bool CanOpenAllocation()
        {
            return true;
        }

        public void OpenAllocation()
        {
            Allocations = new AnimationAllocations(SelectedProductDetail);
        }

        #endregion

        #region Previous & Next Product Detail

        private ICommand previousProudctCommand;
        public ICommand PreviousProductCommand
        {
            get
            {
                if (previousProudctCommand == null)
                {
                    previousProudctCommand = new RelayCommand(param => CanNavigateProducts(), param => PreviousProduct());
                }

                return previousProudctCommand;
            }
        }

        private ICommand nextProductCommand;
        public ICommand NextProductCommand
        {
            get
            {
                if (nextProductCommand == null)
                {
                    nextProductCommand = new RelayCommand(param => CanNavigateProducts(), param => NextProduct());
                }

                return nextProductCommand;
            }
        }

        private bool CanNavigateProducts()
        {
            return animation != null && animation.ObservableProductDetails != null && animation.ObservableProductDetails.Count != 0;
        }

        private void PreviousProduct()
        {
            int index = animation.ObservableProductDetails.IndexOf(selectedProductDetail);
            if (index >= 0)
            {
                int prevIndex = index - 1;
                if (prevIndex < 0) 
                    prevIndex = animation.ObservableProductDetails.Count - 1;
                if (index != prevIndex)
                {
                    selectedProductDetail = animation.ObservableProductDetails[prevIndex];
                    OpenAllocation();
                }
            }
        }

        private void NextProduct()
        {
            int index = animation.ObservableProductDetails.IndexOf(selectedProductDetail);
            if (index >= 0)
            {
                int nextIndex = (index + 1) % animation.ObservableProductDetails.Count;
                if (index != nextIndex)
                {
                    selectedProductDetail = animation.ObservableProductDetails[nextIndex];
                    OpenAllocation();
                }
            }
        }

        #endregion

        #endregion
    }
}
