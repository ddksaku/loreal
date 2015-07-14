﻿#pragma checksum "..\..\..\Lists\CustomerStores.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A66A02FCDE2289C24E6040043B6F1AE7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseGui.Controls;
using LorealOptimiseGui.Lists;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace LorealOptimiseGui.Lists {
    
    
    /// <summary>
    /// CustomerStores
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class CustomerStores : LorealOptimiseGui.Lists.BaseListUserControl<LorealOptimiseBusiness.Lists.CustomerManager, LorealOptimiseData.Customer>, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 24 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblTitle;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRefresh;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.TextEdit txtCustomer;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.TextEdit txtAccount;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ComboBoxEdit cboCustomerGroupFilter;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ComboBoxEdit cboSalesEmployeeFilter;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFilter;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGenerate;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridControl grdCustomerStores;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colSalesArea;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings cboSalesArea;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colCustomerGroup;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings cboCustomerGroup;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colAccountNumber;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colStoreName;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colSalesEmployee;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings cboSalesEmployee;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colRetailSales;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colSystemInclude;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colStoreCategory;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colSource;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\Lists\CustomerStores.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn colOperation;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LorealOptimiseGui;component/lists/customerstores.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Lists\CustomerStores.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lblTitle = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.btnRefresh = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\Lists\CustomerStores.xaml"
            this.btnRefresh.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtCustomer = ((DevExpress.Xpf.Editors.TextEdit)(target));
            
            #line 45 "..\..\..\Lists\CustomerStores.xaml"
            this.txtCustomer.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtCustomer_KeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtAccount = ((DevExpress.Xpf.Editors.TextEdit)(target));
            
            #line 49 "..\..\..\Lists\CustomerStores.xaml"
            this.txtAccount.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtCustomer_KeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.cboCustomerGroupFilter = ((DevExpress.Xpf.Editors.ComboBoxEdit)(target));
            
            #line 53 "..\..\..\Lists\CustomerStores.xaml"
            this.cboCustomerGroupFilter.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtCustomer_KeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.cboSalesEmployeeFilter = ((DevExpress.Xpf.Editors.ComboBoxEdit)(target));
            
            #line 57 "..\..\..\Lists\CustomerStores.xaml"
            this.cboSalesEmployeeFilter.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtCustomer_KeyDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnFilter = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\..\Lists\CustomerStores.xaml"
            this.btnFilter.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnGenerate = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\Lists\CustomerStores.xaml"
            this.btnGenerate.Click += new System.Windows.RoutedEventHandler(this.btnGenerate_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.grdCustomerStores = ((DevExpress.Xpf.Grid.GridControl)(target));
            return;
            case 10:
            this.colSalesArea = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 11:
            this.cboSalesArea = ((DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings)(target));
            return;
            case 12:
            this.colCustomerGroup = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 13:
            this.cboCustomerGroup = ((DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings)(target));
            return;
            case 14:
            this.colAccountNumber = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 15:
            this.colStoreName = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 16:
            this.colSalesEmployee = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 17:
            this.cboSalesEmployee = ((DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings)(target));
            return;
            case 18:
            this.colRetailSales = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 19:
            this.colSystemInclude = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 20:
            this.colStoreCategory = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 21:
            this.colSource = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 22:
            this.colOperation = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 26:
            
            #line 130 "..\..\..\Lists\CustomerStores.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).InitNewRow += new DevExpress.Xpf.Grid.InitNewRowEventHandler(this.TableView_InitNewRow);
            
            #line default
            #line hidden
            
            #line 130 "..\..\..\Lists\CustomerStores.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).ShowingEditor += new DevExpress.Xpf.Grid.ShowingEditorEventHandler(this.TableView_ShowingEditor);
            
            #line default
            #line hidden
            
            #line 130 "..\..\..\Lists\CustomerStores.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).RowCanceled += new DevExpress.Xpf.Grid.RowEventHandler(this.TableView_RowCanceled);
            
            #line default
            #line hidden
            
            #line 130 "..\..\..\Lists\CustomerStores.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).FocusedRowChanged += new DevExpress.Xpf.Grid.FocusedRowChangedEventHandler(this.TableView_FocusedRowChanged);
            
            #line default
            #line hidden
            
            #line 130 "..\..\..\Lists\CustomerStores.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).ShownEditor += new DevExpress.Xpf.Grid.EditorEventHandler(this.TableView_ShownEditor);
            
            #line default
            #line hidden
            
            #line 130 "..\..\..\Lists\CustomerStores.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).CellValueChanging += new DevExpress.Xpf.Grid.CellValueChangedEventHandler(this.TableView_CellValueChanging);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 23:
            
            #line 114 "..\..\..\Lists\CustomerStores.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnAddSales_Click);
            
            #line default
            #line hidden
            
            #line 114 "..\..\..\Lists\CustomerStores.xaml"
            ((System.Windows.Controls.Button)(target)).Initialized += new System.EventHandler(this.btnAddSales_Initialized);
            
            #line default
            #line hidden
            break;
            case 24:
            
            #line 115 "..\..\..\Lists\CustomerStores.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnReplaceAccNumber_Click);
            
            #line default
            #line hidden
            
            #line 115 "..\..\..\Lists\CustomerStores.xaml"
            ((System.Windows.Controls.Button)(target)).Initialized += new System.EventHandler(this.btnAddSales_Initialized);
            
            #line default
            #line hidden
            break;
            case 25:
            
            #line 116 "..\..\..\Lists\CustomerStores.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnSetCapacities_Click);
            
            #line default
            #line hidden
            
            #line 116 "..\..\..\Lists\CustomerStores.xaml"
            ((System.Windows.Controls.Button)(target)).Initialized += new System.EventHandler(this.btnAddSales_Initialized);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
