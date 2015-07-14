using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class BrandAxe : IPrimaryKey, ITrackChanges, IDeletionLimit, ICleanEntityRef
    {
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return "";

                if (this.Brand == true)
                {
                    return "Brand - " + Name;
                }

                return "Axe - " + Name;
            }
        }

        public string FullNameWithCode
        {
            get
            {
                return FullName + " / " + Code;
            }
        }

        public string FullNameWithSignature
        {
            get
            {
                if (Signature != null)
                {
                    return FullName + " (" + Signature.Name + ")";
                }

                return String.Empty;
            }
        }

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;

            string brandaxe = (this.Brand ? "Brand" : "Axe");

            if (this.Manual == false)
            {
                reasonMsg = SystemMessagesManager.Instance.GetMessage("BrandAxeDeleteManual", brandaxe);
                return false;
            }

            if (this.AnimationProducts.Any(ap=>ap.Animation != null && ap.Animation.IsActive))
            {
                // reasonMsg = "This " + brandaxe + " cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("BrandAxeDeleteAttached", brandaxe);
                return false;
            }
            warning = SystemMessagesManager.Instance.GetMessage("BrandAxeDeleteWarning", brandaxe);
            return true;
        }

        public void CleanEntityRef(string fieldName)
        {
            if (fieldName == "IDSignature")
            {
                this._Signature = default(EntityRef<Signature>);
            }
        }
    }
}
