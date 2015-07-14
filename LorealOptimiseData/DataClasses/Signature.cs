using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LorealOptimiseData
{
    public partial class Signature : IPrimaryKey, IDivision, ITrackChanges, IDeleted, IDeletionLimit
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;

            if (this.Manual == false)
            {
                reasonMsg = SystemMessagesManager.Instance.GetMessage("SignatureDeleteManual");
                return false;
            }

            if(this.AnimationProducts.Any(ap=>ap.Animation != null && ap.Animation.IsActive))
            {
                //reasonMsg = "This Signature cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("SignatureDelete");
                return false;
            }

            return true;
        }
    }
}
