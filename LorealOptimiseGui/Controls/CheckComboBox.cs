using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Editors;

namespace LorealOptimiseGui.Controls
{
    public class CheckComboBox : ComboBoxEdit
    {
        public CheckComboBox()
            : base()
        {
            this.EditMode = EditMode.InplaceActive;
        }

        public static DependencyProperty ParentDataIDProperty =
            DependencyProperty.Register("ParentDataID", typeof(Guid), typeof(ComboBoxEdit), new PropertyMetadata(new PropertyChangedCallback(OnParentDataIDChanged)));

        private static void OnParentDataIDChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            CheckComboBox ctrl = source as CheckComboBox;
            ctrl.ParentDataID = (Guid)e.NewValue;
            if (ctrl.UpdateSelectedItems != null)
                ctrl.UpdateSelectedItems(source, e);
        }

        public Guid ParentDataID
        {
            get
            {
                if (id == null)
                    return Guid.Empty;
                return id.Value;
            }
            set
            {
                id = value;
            }
        }

        public event DependencyPropertyChangedEventHandler UpdateSelectedItems;
        private Guid? id = null;
    }

}
