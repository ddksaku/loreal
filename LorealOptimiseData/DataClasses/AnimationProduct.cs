using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using LorealOptimiseShared;
using DevExpress.XtraEditors.DXErrorProvider;

namespace LorealOptimiseData
{
    public partial class AnimationProduct : IPrimaryKey, ITrackChanges, ICleanEntityRef, IDXDataErrorInfo
    {
        partial void OnCreated()
        {
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(AnimationProduct_PropertyChanged);
        }

        void AnimationProduct_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ConfirmedMADMonth")
            {
                CalculateTotals();
            }
        }

        partial void OnLoaded()
        {
            ;
        }

        #region Calculated Properties
        private int? totalCapacity = null;
        private int? totalBDCQuantity = null;
        private int? totalForecastQuantity = null;
        private int? totalAllocationQuantity = null;
        private int? totalAllocationUK = null;
        private int? totalAllocationROI = null;
        private double? rrpUK = null;
        private double? rrpROI = null;
        private double? listPriceUK = null;
        private double? listPriceROI = null;
        private int? receivedToDate = null;
        private int? activeAnimations = null;

        public void CalculateTotals()
        {
            int? calcActiveAnimations = null;
            //if we set it to -1, value is not calculated on sql server. we can calculate as sum of uk and roi allocations
            totalAnimationQuantity = -1;

            DbDataContext.GetInstance().CalculateTotals(ID, LoggedUser.GetInstance().Division.ID,
                ref totalCapacity,
                ref totalBDCQuantity,
                ref totalForecastQuantity,
                ref totalAllocationQuantity,
                ref totalAllocationUK,
                ref totalAllocationROI,
                ref rrpUK,
                ref rrpROI,
                ref listPriceUK,
                ref listPriceROI,
                ref receivedToDate,
                ref calcActiveAnimations);

            ActiveAnimations = calcActiveAnimations.Value;

            CalculateDuplicateProduct();
        }

        public void CalculateProductRecieved()
        {
            int? productRecieved = DbDataContext.GetInstance().calculate_ProductReceived(this.ID);

            if (productRecieved.HasValue)
            {
                receivedToDate = productRecieved.Value;
            }
            else
            {
                receivedToDate = 0;
            }
        }

        public void CalculateActiveAnimations()
        {
            int? calcActiveAnimations = DbDataContext.GetInstance().calculate_ActiveAnimations(this.ID);
            this.ActiveAnimations = calcActiveAnimations.Value;
            CalculateDuplicateProduct();
        }

        public void CalculateTotalCapacity()
        {
            totalCapacity = DbDataContext.GetInstance().calculate_TotalCapacity(this.ID);

            foreach (AnimationProductDetail apd in Animation.ObservableProductDetails.Where(apd => apd.IDAnimationProduct == this.ID).ToList())
            {
                apd.MaximumCapacity = -1;

                foreach (CustomerAllocation ca in apd.CustomerAllocations)
                {
                    ca.MaximumCapacity = -1;
                }

                foreach (CustomerGroupAllocation cga in apd.CustomerGroupAllocations)
                {
                    cga.MaximumCapacity = -1;
                }

            }
        }

        public void CalculateDuplicateProduct()
        {
            if (this.Animation != null)
                DuplicateProduct = this.Animation.AnimationProducts.Where(p => p.IDProduct == IDProduct).Count();
        }

        public int ActiveAnimations
        {
            get
            {
                if (activeAnimations == null)
                {
                    CalculateTotals();
                }

                return activeAnimations.Value;
            }
            set
            {
                activeAnimations = value;
                SendPropertyChanged("ActiveAnimations");
            }
        }

        public int TotalCapacity
        {
            get
            {
                if (totalCapacity == null)
                {
                    CalculateTotals();
                }

                return totalCapacity.Value;
            }
            set
            {
                totalCapacity = value;
                SendPropertyChanged("TotalCapacity");
            }
        }

        public int TotalBDCQuantity
        {
            get
            {
                if (totalBDCQuantity == null)
                {
                    CalculateTotals();
                }

                return totalBDCQuantity.Value;
            }
            set
            {
                totalBDCQuantity = value;
                SendPropertyChanged("TotalBDCQuantity");
            }
        }

        public int TotalForecastQuantity
        {
            get
            {
                if (totalForecastQuantity == null)
                {
                    CalculateTotals();
                }

                return totalForecastQuantity.Value;
            }
            set
            {
                totalForecastQuantity = value;
                SendPropertyChanged("TotalForecastQuantity");
            }
        }

        public int TotalAllocationQuantity
        {
            get
            {
                //if (totalAllocationQuantity == null)
                //{
                //    CalculateTotals();
                //}

                //return totalAllocationQuantity.Value;

                return AllocationQuantityROI + AllocationQuantityUK;
            }
        }

        public decimal TotalCostValue
        {
            get
            {
                if (Product != null)
                    return Product.CIV * TotalForecastQuantity;
                return 0;
            }
        }

        public int AllocationQuantity
        {
            get
            {
                return AnimationProductDetails.Sum(a => a.AllocationQuantity) ?? 0;
            }
        }

        public int AllocationQuantityUK
        {
            get
            {
                if (totalAllocationUK == null)
                {
                    CalculateTotals();
                }

                return totalAllocationUK.Value;
            }
        }

        public int AllocationQuantityROI
        {
            get
            {
                if (totalAllocationROI == null)
                {
                    CalculateTotals();
                }

                return totalAllocationROI.Value;
            }
        }

        public double UKListPrice
        {
            get
            {
                //TODO: Calculate as sum of UK List from AnimationProductDetail
                //AnimationProductDetails.Sum(apd => apd.ListPrice);
                if (listPriceUK == null)
                {
                    CalculateTotals();
                }

                return listPriceUK.Value;
            }
        }

        public double ROIListPrice
        {
            get
            {
                //TODO: Calculate as sum of UK List from AnimationProductDetail
                if (listPriceROI == null)
                {
                    CalculateTotals();
                }

                return listPriceROI.Value;
            }
        }

        public double UKRrp
        {
            get
            {
                if (rrpUK == null)
                {
                    CalculateTotals();
                }

                return rrpUK.Value;
            }
        }

        public double ROIRrp
        {
            get
            {
                if (rrpROI == null)
                {
                    CalculateTotals();
                }

                return rrpROI.Value;
            }
        }

        private int duplicateProduct = -1;
        public int DuplicateProduct
        {
            get
            {
                if (duplicateProduct == -1 && Animation != null)
                {

                }

                return duplicateProduct;
            }
            set
            {
                duplicateProduct = value;
                SendPropertyChanged("DuplicateProduct");
            }
        }

        private int totalAnimationQuantity = -1;
        public int TotalAnimationQuantity
        {
            get
            {
                if (totalAnimationQuantity == -1 && Animation != null)
                {
                    totalAnimationQuantity =
                        Animation.AnimationProducts.Where(p => p.IDProduct == IDProduct).Sum(
                            p => p.TotalAllocationQuantity);
                }

                return totalAnimationQuantity;
            }
            set
            {
                totalAnimationQuantity = value;
                SendPropertyChanged("TotalAnimationQuantity");
            }
        }

        public int ReceivedToDate
        {
            get
            {
                if (receivedToDate == null)
                {
                    CalculateTotals();
                }

                return receivedToDate.Value;
            }
        }

        public string BrandAxeName
        {
            get
            {
                if (BrandAxe != null)
                {
                    return BrandAxe.FullName;
                }

                if (this.IDBrandAxe != null)
                {
                    BrandAxe brandaxe = DbDataContext.GetInstance().BrandAxes.SingleOrDefault(b => b.ID == this.IDBrandAxe.Value);
                    if (brandaxe != null)
                    {
                        return brandaxe.FullName;
                    }
                }

                return String.Empty;
            }
        }

        public string NormalMultiple
        {
            get
            {
                if (this.Multiple != null)
                {
                    return this.Multiple.Value.ToString();
                }

                if (this.IDMultipleNormal != null)
                {
                    LorealOptimiseData.Multiple multiple = DbDataContext.GetInstance().Multiples.SingleOrDefault(m => m.ID == this.IDMultipleNormal.Value);
                    if (multiple != null)
                    {
                        return multiple.Value.ToString();
                    }
                }
                return string.Empty;
            }
        }

        public string WarehouseMultiple
        {
            get
            {
                if (this.Multiple1 != null)
                {
                    return this.Multiple1.Value.ToString();
                }

                if (this.IDMultipleWarehouse != null)
                {
                    LorealOptimiseData.Multiple multiple = DbDataContext.GetInstance().Multiples.SingleOrDefault(m => m.ID == this.IDMultipleWarehouse.Value);
                    if (multiple != null)
                    {
                        return multiple.Value.ToString();
                    }
                }
                return string.Empty;
            }
        }

        #endregion

        private Dictionary<string, int> retailerTypeAllocation;
        /// <summary>
        /// string = retailer type name
        /// int = sum of allocation for customers assigned to retailer type name
        /// </summary>
        public Dictionary<string, int> RetailerTypeAllocation
        {
            get
            {
                if (retailerTypeAllocation == null)
                {
                    retailerTypeAllocation = new Dictionary<string, int>();
                }

                return retailerTypeAllocation;
            }
        }

        public void SetRetailerTypeAllocation(string retailerType, int? calculatedAllocation)
        {
            if (calculatedAllocation.HasValue)
            {
                if (RetailerTypeAllocation.ContainsKey(retailerType) == false)
                    RetailerTypeAllocation.Add(retailerType, calculatedAllocation.Value);
                else
                    RetailerTypeAllocation[retailerType] = calculatedAllocation.Value;
            }
        }

        public string ProductIdentifier
        {
            get
            {
                if (ItemGroup != null)
                {
                    return Product.MaterialCode + " - " + ItemGroup.Name;
                }
                return Product.MaterialCode;
            }
        }

        public string ForOrdering
        {
            get
            {
                if (ItemType != null)
                {
                    return ItemType.IncludeInSAPOrders ? "Y" : "N";
                }

                return String.Empty;
            }
        }

        private string materialCode = null;
        public string MaterialCode
        {
            get
            {
                if (materialCode == null)
                {
                    if (Product != null)
                        materialCode = Product.MaterialCode;
                    else
                        materialCode = "";
                }
                return materialCode;
            }
            set
            {
                materialCode = value;
            }
        }

        private string description = null;
        public string Description
        {
            get
            {
                if (description == null)
                {
                    if (Product != null)
                        description = Product.Description;
                    else
                        description = "";
                }
                return description;
            }
            set
            {
                description = value;
            }
        }

        private string confirmedMadMonthString = string.Empty;
        public string ConfirmedMadMonthString
        {
            get
            {
                if (confirmedMadMonthString == string.Empty)
                {
                    confirmedMadMonthString = ConfirmedMADMonth.ToString("MM.yyyy");
                }
                return confirmedMadMonthString;
            }
            set
            {
                confirmedMadMonthString = value;
            }
        }

        public string Multipliers
        {
            get
            {
                string text = String.Empty;

                if (Multiple != null)
                {
                    text += Multiple.Value.ToString();
                }

                if (Multiple1 != null)
                {
                    text += "/" + Multiple1.Value.ToString();
                }

                return text;
            }
        }

        public bool IsValid(out string errorMessage, out string firstInvalidColumn)
        {
            return Utility.IsValid(this, out errorMessage, out firstInvalidColumn);
        }

        public void CleanEntityRef(string fieldName)
        {
            if (fieldName == "IDMultipleNormal")
            {
                this._Multiple = default(EntityRef<Multiple>);
            }
            else if (fieldName == "IDMultipleWarehouse")
            {
                this._Multiple1 = default(EntityRef<Multiple>);
            }
            else if (fieldName == "IDItemType")
            {
                this._ItemType = default(EntityRef<ItemType>);
            }
            else if (fieldName == "IDCategory")
            {
                this._Category = default(EntityRef<Category>);
            }
            else if (fieldName == "IDItemGroup")
            {
                this._ItemGroup = default(EntityRef<ItemGroup>);
            }
            else if (fieldName == "IDBrandAxe")
            {
                this._BrandAxe = default(EntityRef<BrandAxe>);
            }
            else if (fieldName == "IDSignature")
            {
                this._Signature = default(EntityRef<Signature>);
            }
            else if (fieldName == "IDProduct")
            {
                this._Product = default(EntityRef<Product>);
            }
        }

        void IDXDataErrorInfo.GetPropertyError(string propertyName, ErrorInfo info)
        {
            switch (propertyName)
            {
                case "IDItemGroup":
                    if (this.IDItemGroup == Guid.Empty)
                    {
                        info.ErrorType = ErrorType.Critical;
                        //info.ErrorText = string.Format("Item Group cannot be empty.");
                        info.ErrorText = SystemMessagesManager.Instance.GetMessage("AnimationProductEmptyItemGroup");
                    }
                    break;
                case "MaterialCode":
                case "Description":
                    if (this.IDProduct == Guid.Empty)
                    {
                        info.ErrorType = ErrorType.Critical;
                        //info.ErrorText = string.Format("Product cannot be empty.");
                        info.ErrorText = SystemMessagesManager.Instance.GetMessage("AnimationProductEmptyProduct");
                    }
                    break;
            }
        }
        void IDXDataErrorInfo.GetError(ErrorInfo info)
        {

        }
    }
}
