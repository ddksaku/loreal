using System.Collections.Generic;
using System.Collections;
using System.Data.Linq;

namespace LorealOptimiseBusiness.Lists
{
    public interface IListManager<T> : IModify<T>
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> GetFiltered(Hashtable conditions);

        DataLoadOptions LoadOptions
        {
            get;
        }

        void Refresh();
    }
}
