using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DevExpress.Xpf.Grid;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using System.Collections;
using LorealOptimiseBusiness.ViewMode;
using System.Linq.Expressions;

namespace LorealOptimiseGui.Lists
{
    public abstract class BaseListUserControl<T,U> : BaseUserControl where T : IListManager<U>, new()
                                                        where U : IPrimaryKey, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected abstract Hashtable Filters
        {
            get;
        }

        private ExtendedObservableCollection<U> data;
        protected ExtendedObservableCollection<U> Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;

                //To make this working we need to assign this class as datacontext, not this property
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Data"));
                }
            }
        }
        protected IListManager<U> Manager;
        
        public BaseListUserControl()
        {
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(BaseListUserControl_Loaded);

                Manager = new T();
            }
        }

        protected bool AllowRefreshing = true;
        protected bool NeededNewDBContext = true;
        void BaseListUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                if (NeededNewDBContext)
                {
                    DbDataContext.MakeNewInstance(Manager.LoadOptions);
                }

                if (!(AllowRefreshing == false && Filters.Count == 0 ))
                {
                    Refresh();
                }
                else
                {
                    Data = new ExtendedObservableCollection<U>();
                    DataContext = Data;
                }
            }
        }

        protected void AssignEvents(GridControl gridControl, bool isAvailableOnlyForDivisionAdmin = false)
        {
            TableViewEventHandlers<U> eventHandler = new TableViewEventHandlers<U>(gridControl, Manager);
            eventHandler.AssignEvents(isAvailableOnlyForDivisionAdmin);

            // set the cellstyle to grid
            object style = this.FindResource("CellNormalStyle");
            if (style != null && (style as Style) != null)
            {
                gridControl.View.CellStyle = style as Style;
            }

        }

        protected void Refresh()
        {
            this.Cursor = Cursors.Wait;
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));

            Manager.Refresh();

            Data = new ExtendedObservableCollection<U>(Manager.GetFiltered(Filters));

            DataContext = Data;

            this.Cursor = Cursors.Arrow;
        }

    }
}
