using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LorealOptimiseData.Enums;
using System.Linq.Expressions;

namespace LorealOptimiseData
{
    public partial class Product : IPrimaryKey, IDivision, ITrackChanges, IDeletionLimit
    {
        private int confirmedQuantity = -1;
        public int ConfirmedQuantity
        {
            get
            {
                if (confirmedQuantity == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    confirmedQuantity = ProductConfirmeds.Sum(
                        p =>
                        p.Current + p.Month1 + p.Month2 + p.Month3 + p.Month4 + p.Month5 + p.Month6 + p.Month7 + p.Month8 +
                        p.Month9 + p.Month10 + p.Month11 + p.Reliquat);
                }

                return confirmedQuantity;
            }
        }

        private int reliquat = -1;
        public int Reliquat
        {
            get
            {
                if (reliquat == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    reliquat = ProductConfirmeds.ToList().FirstOrDefault().Reliquat;
                }

                return reliquat;
            }
        }

        #region Month Properties

        public int month = -1;
        public int Month
        {
            get
            {
                if (month == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month = ProductConfirmeds.ToList().FirstOrDefault().Current;
                }

                return month;
            }
        }

        public int month1 = -1;
        public int Month1
        {
            get
            {
                if (month1 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month1 = ProductConfirmeds.ToList().FirstOrDefault().Month1;
                }

                return month1;
            }
        }

        public int month2 = -1;
        public int Month2
        {
            get
            {
                if (month2 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month2 = ProductConfirmeds.ToList().FirstOrDefault().Month2;
                }

                return month2;
            }
        }

        public int month3 = -1;
        public int Month3
        {
            get
            {
                if (month3 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month3 = ProductConfirmeds.ToList().FirstOrDefault().Month3;
                }

                return month3;
            }
        }

        public int month4 = -1;
        public int Month4
        {
            get
            {
                if (month4 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month4 = ProductConfirmeds.ToList().FirstOrDefault().Month4;
                }

                return month4;
            }
        }

        public int month5 = -1;
        public int Month5
        {
            get
            {
                if (month5 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month5 = ProductConfirmeds.ToList().FirstOrDefault().Month5;
                }

                return month5;
            }
        }

        public int month6 = -1;
        public int Month6
        {
            get
            {
                if (month6 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month6 = ProductConfirmeds.ToList().FirstOrDefault().Month6;
                }

                return month6;
            }
        }

        public int month7 = -1;
        public int Month7
        {
            get
            {
                if (month7 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month7 = ProductConfirmeds.ToList().FirstOrDefault().Month7;
                }

                return month7;
            }
        }

        public int month8 = -1;
        public int Month8
        {
            get
            {
                if (month8 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month8 = ProductConfirmeds.ToList().FirstOrDefault().Month8;
                }

                return month8;
            }
        }

        public int month9 = -1;
        public int Month9
        {
            get
            {
                if (month9 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month9 = ProductConfirmeds.ToList().FirstOrDefault().Month9;
                }

                return month9;
            }
        }

        public int month10 = -1;
        public int Month10
        {
            get
            {
                if (month10 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month10 = ProductConfirmeds.ToList().FirstOrDefault().Month10;
                }

                return month10;
            }
        }

        public int month11 = -1;
        public int Month11
        {
            get
            {
                if (month11 == -1)
                {
                    if (ProductConfirmeds.Count == 0)
                    {
                        return 0;
                    }

                    month11 = ProductConfirmeds.ToList().FirstOrDefault().Month11;
                }

                return month11;
            }
        }

        #endregion

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (Manual == true && this.AnimationProducts.Count == 0)
                return true;

            //reasonMsg = "This product cannot be deleted because it is not a dummy product or it is attached to some animations.";
            reasonMsg = SystemMessagesManager.Instance.GetMessage("ProductDelete");

            return false;
        }
    }
}
