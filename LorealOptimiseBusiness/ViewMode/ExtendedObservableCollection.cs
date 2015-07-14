using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace LorealOptimiseBusiness.ViewMode
{
    public class ExtendedObservableCollection<T> : ObservableCollection<T> 
    {
        public ExtendedObservableCollection(IEnumerable<T> list) : base(list)
        {
            
        }

        public ExtendedObservableCollection():base()
        {
        }

        public new void Clear()
        {
            base.Clear();

            NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(e);
        }

        public void RaiseCollectionChanged()
        {
            NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(e);            
        }
    }
}
