using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace LorealOptimiseShared
{
    public static class ExtendedMethods
    {
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                observableCollection.Add(item);
            }
        }

        public static bool IsValidDate(this String candidate)
        {
            bool isValidDate = true;
            try
            {
                DateTime.Parse(candidate);
            }
            catch (FormatException)
            {
                isValidDate = false;
            }

            return isValidDate;
        }

        private static Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
        public static bool IsValidGuid(this String candidate)
        {
            bool isValid = false;

            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    isValid = true;
                }
            }

            return isValid;            
        }
    }
}
