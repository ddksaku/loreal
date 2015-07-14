using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LorealOptimiseShared;
using LorealOptimiseData.Enums;

namespace LorealOptimiseData
{
    public partial class Animation : IPrimaryKey, ITrackChanges, ICleanEntityRef
    {
        partial void OnLoaded()
        {
        }

        partial void OnCreated()
        {
            //condition is only for unit tests
            if (LoggedUser.IsLogged)
            {
                IDDivision = LoggedUser.GetInstance().Division.ID;
            }
        }

        public void RecalculateAnimationProduct(AnimationProduct animationProduct = null, LongTaskExecutor executor = null)
        {
            if (animationProduct != null)
            {
                animationProduct.CalculateTotals();
                animationProduct.CalculateTotalCapacity();

                if (executor != null)
                {
                    executor.SendProgressMessage("   Calculating capacities for " + animationProduct.ProductIdentifier);
                }

                return;
            }

            foreach (AnimationProduct ap in AnimationProducts)
            {
                ap.CalculateTotals();
                
                ap.CalculateTotalCapacity();

                if (executor != null)
                {
                    executor.SendProgressMessage("   Calculating capacities for " + ap.ProductIdentifier);
                }
            }
            
        }

        public void RefreshAllocations()
        {
            foreach (AnimationProduct ap in AnimationProducts)
            {
                foreach(AnimationProductDetail apd in ap.AnimationProductDetails)
                {
                    // to do,
                }
            }
        }

        #region Observable collections
        private ObservableCollection<AnimationCustomerGroup> observableAnimationCustomerGroups;
        public ObservableCollection<AnimationCustomerGroup> ObservableAnimationCustomerGroups
        {
            get
            {
                if (observableAnimationCustomerGroups == null)
                {
                    observableAnimationCustomerGroups = new ObservableCollection<AnimationCustomerGroup>(this.AnimationCustomerGroups.OrderBy(cg => cg.RetailerType.Name).ThenBy(cg => cg.CustomerGroup.Name).ThenBy(cg => cg.CustomerGroup.SalesArea.Name));
                }

                return observableAnimationCustomerGroups;
            }
            set
            {
                observableAnimationCustomerGroups = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ObservableAnimationCustomerGroups"));
            }
        }

        void Animation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if(e.PropertyName == "Product" && Animation != null)

        }

        private ObservableCollection<AnimationProduct> observableProducts;
        public ObservableCollection<AnimationProduct> ObservableProducts
        {
            get
            {
                if (observableProducts == null)
                {
                    observableProducts = new ObservableCollection<AnimationProduct>(AnimationProducts.OrderBy(ap=>ap.ItemGroup.Name).ThenBy(ap=>ap.SortOrder));
                }

                return observableProducts;
            }
            set
            {
                observableProducts = value;
                SendPropertyChanged("ObservableProducts");
            }
        }

        private ObservableCollection<AnimationProductDetail> observableProductDetails;
        public ObservableCollection<AnimationProductDetail> ObservableProductDetails
        {
            get
            {
                if (observableProductDetails == null)
                {
                    DbDataContext Db = DbDataContext.GetInstance();

                    List<AnimationProductDetail> apds = new List<AnimationProductDetail>();

                    apds = (from apd in Db.AnimationProductDetails
                            where AnimationProducts.Contains(apd.AnimationProduct)
                            select apd).ToList();

                    apds = apds.OrderBy(apd => apd.SalesArea.Name).ThenBy(apd => apd.AnimationProduct.ItemGroup.Name).ThenBy(apd => apd.AnimationProduct.SortOrder).ToList();

                    observableProductDetails = new ObservableCollection<AnimationProductDetail>(apds);
                }

                return observableProductDetails;
            }
            set
            {
                observableProductDetails = value;
                SendPropertyChanged("ObservableProductDetails");
            }
        }
        #endregion

        private bool IsEmptyOrNull(Guid? guid)
        {
            if (guid == null || guid.Value == Guid.Empty)
                return true;
            return false;
        }

        public bool IsValid(out string errorMessage)
        {
            string firstInvalidProperty;
            if (Utility.IsValid(this, out errorMessage, out firstInvalidProperty) == false)
            {
                errorMessage = string.Format(errorMessage, firstInvalidProperty);
                return false;
            }

            StringBuilder animationErrorMessage = new StringBuilder();
            StringBuilder productErrorMessage = new StringBuilder();
            string productDetailErrorMessage = String.Empty;

            if (this.Status.HasValue)
            {
                bool isValid = true;

                if (this.Status.Value >= (byte)AnimationStatus.Locked)
                {
                    #region Check Locked status
                    if (this.IDPriority == null)
                    {
                        //errorMessage = "Priority is missing.";
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationPriorityMissing"));
                        isValid = false;
                    }
                    if (this.IDAnimationType == null)
                    {
                       // errorMessage = "Animation Type is missing";
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationAnimationTypeMissing"));
                        isValid = false;
                    }
                    if (this.AnimationProducts.Count == 0)
                    {
                        //errorMessage = "There is no animation product.";
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationProductMissing"));
                        isValid = false;
                    }

                    //errorMessage = string.Format("Mandatory data is missing for animation product");
                    foreach (AnimationProduct ap in AnimationProducts)
                    {
                        //string tempErrMsg = SystemMessagesManager.Instance.GetMessage("AnimationDataMissing", ap.ProductIdentifier));
                        string tempErrMsg = SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", "normal multiple", ap.ProductIdentifier);

                        if (ap.IDItemGroup == Guid.Empty)
                        {
                            productErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", "item group", ap.ProductIdentifier));
                            isValid = false;
                        }
                        if (ap.IDProduct == Guid.Empty)
                        {
                            productErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", "product", ap.ProductIdentifier));
                            isValid = false;
                        }
                        if(IsEmptyOrNull(ap.IDSignature))
                        {
                            productErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", "signature", ap.ProductIdentifier));
                            isValid = false;
                        }
                        if(IsEmptyOrNull(ap.IDCategory))
                        {
                            productErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", "category", ap.ProductIdentifier));
                            isValid = false;
                        }
                        if (IsEmptyOrNull(ap.IDItemType))
                        {
                            productErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", "item type", ap.ProductIdentifier));
                            isValid = false;
                        }
                        if(IsEmptyOrNull(ap.IDMultipleNormal))
                        {
                            productErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", "normal multiple", ap.ProductIdentifier));
                            isValid = false;
                        }

                        if (IsEmptyOrNull(ap.IDMultipleWarehouse))
                        {
                            productErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", "warehouse multiple", ap.ProductIdentifier));
                            isValid = false;
                        }

                        // check if animation products have allocation quantity
                        //LORLPD-428
                        //if (ap.TotalAllocationQuantity == 0)
                        //{
                        //    //errorMessage = string.Format("Allocation quantity is missing for some animation products.");
                        //    productDetailErrorMessage += SystemMessagesManager.Instance.GetMessage("AnimationAllocationQuantityMissing", ap.ProductIdentifier) + Environment.NewLine;
                        //    isValid = false;
                        //}
                    }
                    #endregion
                }

                if (this.Status.Value >= (byte)AnimationStatus.Closed)
                {
                    #region Check closed status
                    if (string.IsNullOrEmpty(Code) || Code.Trim() == "")
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationCodeMissing"));
                        isValid = false;
                    }
                    
                    if (IDSalesDrive == null)
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationSalesDriveMissing"));
                        isValid = false;
                    }
                    if(StockDate == null)
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationStockDateMissing"));
                        isValid = false;
                    }
                    if(PLVDeliveryDate == null)
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationPLVDeliveryDateMissing"));
                        isValid = false;
                    }
                    if(PLVComponentDate == null)
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationPLVComponentDateMissing"));
                        isValid = false;
                    }
                    if(OnCounterDate == null)
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationClosedOnCounterDateMissing"));
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(SAPDespatchCode) || SAPDespatchCode.Trim() == "")
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationSAPDespatchCodeMissing"));
                        isValid = false;
                    }
                    if(this.RequestedDeliveryDate == null)
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationRequestedDeliveryDateMissing"));
                        isValid = false;
                    }
                    if (IDDistributionChannel_Order == null)
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationDistributionChannelMissing"));
                        isValid = false;
                    }
                    if (IDOrderType_Order == null)
                    {
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationOrderTypeMissing"));
                        isValid = false;
                    }

                    // check if all products are SAP products.
                    if (AnimationProducts.Count(ap => ap.Product.Manual == true) > 0)
                    {
                        //errorMessage = string.Format("Animation contains some dummy products.");
                        animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationDummyProducts"));
                        isValid = false;
                    }

                    // Animation Products
                    
                    foreach (AnimationProduct ap in AnimationProducts)
                    {
                        //  string tempErrMsg = SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", ap.ProductIdentifier));

                        if(string.IsNullOrEmpty(ap.BDCBookNumber) || ap.BDCBookNumber.Trim() == "")
                        {
                            productErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationProductMandatory", "BDC book number", ap.ProductIdentifier));
                            isValid = false;
                        }

                        IEnumerable<AnimationProductDetail> animationProductDetails = this.ObservableProductDetails.Where(apd => apd.IDAnimationProduct == ap.ID);
                        if (animationProductDetails.Count() == 0)
                        {
                           // errorMessage = string.Format("Animation product details are missing for some animation products.");
                            productDetailErrorMessage += SystemMessagesManager.Instance.GetMessage("AnimationNoProduct") + Environment.NewLine;
                            isValid = false;
                        }
                    }
                    #endregion
                }

                // check default on counter Date
                if (OnCounterDate == null)
                {
                    //errorMessage = "OnCounterDate is missing.";
                    animationErrorMessage.AppendLine(SystemMessagesManager.Instance.GetMessage("AnimationOnCounterDateMissing"));
                    isValid = false;
                }
                
                string delimiter = "----------------------------------------" + Environment.NewLine;
                StringBuilder concatMessage = new StringBuilder();
                concatMessage.Append(animationErrorMessage.Length > 0 ? animationErrorMessage + delimiter : String.Empty);
                concatMessage.Append(productErrorMessage.Length > 0 ? productErrorMessage + delimiter : String.Empty);
                concatMessage.Append(productDetailErrorMessage);

                errorMessage = concatMessage.ToString();

                return isValid;
            }
            
            return true;
        }

        public string StatusName
        {
            get
            {
                if (Status == null)
                {
                    return AnimationStatus.Draft.ToString();
                }

                return ((AnimationStatus)Status).ToString();
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    Status = null;
                }
                else
                {
                    Status = (byte)(AnimationStatus)Enum.Parse(typeof(AnimationStatus), value);
                }
            }
        }

        public bool IsActive
        {
            get
            {
                if (this.Status != null && this.Status.Value >= 1 && this.Status.Value <= 4)
                    return true;
                return false;
            }
        }

        public void CleanEntityRef(string fieldname)
        {
            if (fieldname == "IDAnimationType")
            {
                this._AnimationType = default(EntityRef<AnimationType>);
            }
            else if (fieldname == "IDPriority")
            {
                this._Priority = default(EntityRef<Priority>);
            }
            else if (fieldname == "IDSalesDrive")
            {
                this._SalesDrive = default(EntityRef<SalesDrive>);
            }
        }
    }
}
