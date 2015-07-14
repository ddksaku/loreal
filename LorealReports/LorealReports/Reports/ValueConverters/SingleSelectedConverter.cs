using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace LorealReports.Reports.ValueConverters
{
    public class SingleSelectedConverter<TObject> : IValueConverter where TObject : EntityObject, IEntity
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<TObject> collection = parameter as IEnumerable<TObject>;
            return collection != null ? GetItemByCollectionById((string)value, collection) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = ((TObject)value).StringId;
            return result;
        }

        TObject GetItemByCollectionById(string id, IEnumerable<TObject> collection)
        {
            foreach (var item in collection)
            {
                if (item.StringId == id)
                    return item;
            }
            return null;
        }
    }

    public interface IEntity
    {
        string StringId { get; }
    }
}
