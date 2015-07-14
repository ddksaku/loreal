using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using LorealOptimiseShared.Logging;
using LorealOptimiseShared;
using LorealOptimiseData;
using log4net.Appender;
using log4net;
using System.IO;
using System.Data.SqlClient;

namespace LorealOptimiseGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DateTime lastExceptionTime;

        public App()
        {
            IAppender[] appenders = LogManager.GetLogger(typeof(App)).Logger.Repository.GetAppenders();
            if (appenders.Length > 0 && appenders[0] is FileAppender)
            {
                FileAppender appender = (FileAppender)appenders[0];

                string dirName = "LorealOptimiseLog\\";

                appender.File = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), dirName) + "\\";
                appender.ActivateOptions();
            }

            Logger.assembly = Assembly.GetExecutingAssembly();
            log4net.Config.XmlConfigurator.Configure();

            //Logger.Log("Connection string:" + DbDataContext.GetInstance().Connection.ConnectionString, LogLevel.Info);
        
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Log(e.Exception.ToString(), LogLevel.Error);

            SqlException sqlExc = e.Exception as SqlException;
            if (sqlExc != null)
            {
                if (sqlExc.Number == 1205)
                {
                    MessageBox.Show("Deadlock occured on the server. This usually means that two users are working on same animation (or entity). Try to run the operation again.");
                }

                if (sqlExc.Class == 20)
                {
                    MessageBox.Show("Sql Server is not reachable. Please contact system administator");
                    Environment.Exit(0);
                }
            }

            string message = Utility.GetExceptionsMessages(e.Exception);
            //MessageBox.Show("An error has occured in application." + Environment.NewLine + message+"\nYou may need to restart the application");
            MessageBox.Show(SystemMessagesManager.Instance.GetMessage("AppErrorRestart", message));

            if ((lastExceptionTime - DateTime.Now).Seconds < 1)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;                
            }
            lastExceptionTime = DateTime.Now;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(e.ExceptionObject.ToString(), LogLevel.Error);

            string message = Utility.GetExceptionsMessages(e.ExceptionObject as Exception);
            //MessageBox.Show("An error has occured in application."+Environment.NewLine+message);
            MessageBox.Show(SystemMessagesManager.Instance.GetMessage("AppError", message));
        }
    }
}
