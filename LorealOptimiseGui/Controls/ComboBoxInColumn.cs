using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Editors;

namespace LorealOptimiseGui.Controls
{
    public class ComboBoxInColumn : ComboBoxEdit
    {
        public ComboBoxInColumn()
            : base()
        {
            this.EditMode = EditMode.InplaceActive;
            this.StyleSettings = new ComboBoxStyleSettings();
        }

        public static DependencyProperty DependingDataIDProperty =
            DependencyProperty.Register("DependingDataID", typeof(Guid), typeof(ComboBoxEdit), new PropertyMetadata(new PropertyChangedCallback(OnDependingDataIDChanged)));

        private static void OnDependingDataIDChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ComboBoxInColumn ctrl = source as ComboBoxInColumn;
            ctrl.DependingDataID = (Guid)e.NewValue;
            if (ctrl.ChangedDependingDataID != null)
                ctrl.ChangedDependingDataID(source, e);
        }

        private Guid? depeningDataID = null;
        public Guid DependingDataID
        {
            get
            {
                if (depeningDataID == null)
                    return Guid.Empty;
                return depeningDataID.Value;
            }
            set
            {
                depeningDataID = value;
            }
        }

        public event DependencyPropertyChangedEventHandler ChangedDependingDataID;
    }

}
