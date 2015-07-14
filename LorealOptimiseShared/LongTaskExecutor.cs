using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;
using System.Windows;

namespace LorealOptimiseShared
{
    public class LongTaskExecutor
    {
        private string taskName;

        public event EventHandler TaskStarted;
        public event EventHandler TaskFinished;
        public event DoWorkEventHandler DoWork;
        public static event LongTaskEventHandler LongTaskEvent;

        public LongTaskExecutor(string task)
        {
            taskName = task;
        }

        public void Run(object argument = null)
        {
            if (LongTaskEvent != null)
            {
                LongTaskEvent(this, new LongTaskEventArgs(taskName, TaskStatus.Started));
            }

            if (TaskStarted != null)
            {
                TaskStarted(this, new EventArgs());
            }

            if (DoWork != null)
            {
                DoWork(this, new DoWorkEventArgs(argument));
            }

            if (LongTaskEvent != null)
            {
                LongTaskEvent(this, new LongTaskEventArgs(taskName, TaskStatus.Finished));
            }

            if (TaskFinished != null)
            {
                TaskFinished(this, new EventArgs());
            }
        }

        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
        }

        public void SendProgressMessage(string msg)
        {
            LongTaskEvent(this, new LongTaskEventArgs(taskName, TaskStatus.InProgress, msg));
            DoEvents();
        }

        public static void RaiseLongTaskEvent(object sender, LongTaskEventArgs args)
        {
            LongTaskEvent(sender, args);
            DoEvents();
        }

    }
}
