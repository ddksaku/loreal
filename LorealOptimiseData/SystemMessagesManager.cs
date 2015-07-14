using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseShared.Logging;
using LorealOptimiseData;


namespace LorealOptimiseData
{
    /// <summary>
    /// Class which handles operations with system messages.
    /// Should be located in Bussiness, but we have a lot of messages in Data assembly and move them to bussiness can have big impact on application.
    /// </summary>
    public class SystemMessagesManager 
    {
        private static SystemMessagesManager instance = null;

        public static SystemMessagesManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemMessagesManager();
                }

                return instance;
            }
        }

        private Dictionary<string, string> messages;

        private SystemMessagesManager()
        {
            messages = DbDataContext.GetInstance().SystemMessages.ToDictionary(sm => sm.Code, sm => sm.MessageContent);
        }


        private static IEnumerable<SystemMessage> all = null;
        public IEnumerable<SystemMessage> GetAll()
        {
            if (all == null)
            {
                all = DbDataContext.GetInstance().SystemMessages.ToList();
            }
            return all;
        }


        public string GetMessage(string code, params string[] parameters)
        {
            if (messages.ContainsKey(code) == false)
            {
                Logger.Log(string.Format("System messages does not contain code '{0}'", code), LogLevel.Info, LogFilePrefix.Info);
                return "[" + code + "]";
            }

            StringBuilder sb = new StringBuilder(messages[code]);

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    sb = sb.Replace("&p" + (i+1), parameters[i]);
                }
            }

            sb = sb.Replace("&newLineChar", Environment.NewLine);
            sb = sb.Replace("\\n", Environment.NewLine);

            return sb.ToString();
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
