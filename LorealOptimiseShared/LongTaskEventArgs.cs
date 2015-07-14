using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseShared
{
    public enum TaskStatus
    {
        Started,
        InProgress,
        Finished
    }

    public delegate void LongTaskEventHandler(object sender, LongTaskEventArgs args);

    public class LongTaskEventArgs
    {
        public string TaskName
        {
            get; private set;
        }

        public TaskStatus Status
        {
            get;
            private set;
        }

        public string ProgressMessage
        {
            get;
            private set;
        }

        public LongTaskEventArgs(string taskName, TaskStatus status)
        {
            this.TaskName = taskName;
            this.Status = status;
        }

        public LongTaskEventArgs(string taskName, TaskStatus status, string progressMsg)
        {
            this.TaskName = taskName;
            this.Status = status;
            this.ProgressMessage = progressMsg;
        }
    }
}
