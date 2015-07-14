using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace LorealReports.Reports.ValueConverters
{
    public class MultiSelectedConverter<TObject> : IValueConverter where TObject : EntityObject, IEntity
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<TObject> collection = parameter as IEnumerable<TObject>;
            return collection != null ? GetItemFromCollectionById((string)value, collection) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = string.Empty;

            var coll = (IEnumerable<object>)value;
            List<TObject> colelction = new List<TObject>();
            foreach (object o in coll)
            {
                colelction.Add(o as TObject);
            }

            foreach (TObject item in colelction)
            {
                result = string.IsNullOrEmpty(result) ? (item).StringId : result + "," + (item).StringId;
            }
            return result;
        }

        IEnumerable<TObject> GetItemFromCollectionById(string id, IEnumerable<TObject> collection)
        {
            var result = new List<TObject>();
            var col = id.Split(',');
            foreach (string s in col)
            {
                foreach (var item in collection)
                {
                    if (item.StringId == s)
                        result.Add(item);
                }
            }
            return result;
        }
    }
}
