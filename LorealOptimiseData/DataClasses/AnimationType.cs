using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AnimationType : IPrimaryKey, IDivision, ITrackChanges, IDeletionLimit
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.Animations.Any(a => a.IsActive))
            {
                //reasonMsg = "This Animation Type cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("AnimationTypeDelete");
                return false;
            }
            //warning = "If you delete this animation type, then all capacities for this animation type will also be deleted. Do you want to proceed?";
            warning = SystemMessagesManager.Instance.GetMessage("AnimationTypeDeleteWarning");
            return true;
        }
    }
}
