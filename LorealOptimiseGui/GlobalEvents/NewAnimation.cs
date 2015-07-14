using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseGui.GlobalEvents
{
    public delegate void NewAnimationHandler(object sender, EventArgs e);

    public class GlobalEvents
    {
        public static event NewAnimationHandler NewAnimation;
    }
}
