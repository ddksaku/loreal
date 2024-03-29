﻿#pragma checksum "..\..\..\Lists\Products.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7EF43744D4D5CF9040601543EF3A5A96"
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
    /// Products
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class Products : LorealOptimiseGui.Lists.BaseListUserControl<LorealOptimiseBusiness.Lists.ProductManager, LorealOptimiseData.Product>, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 21 "..\..\..\Lists\Products.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblTitle;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Lists\Products.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.TextEdit txtMaterialCodeFilter;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Lists\Products.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.TextEdit txtDescriptionFilter;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Lists\Products.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.TextEdit txtProcurementTypeFilter;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\Lists\Products.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFilter;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Lists\Products.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridControl grdProducts;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Lists\Products.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings cboDivisions;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\Lists\Products.xaml"
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
            System.Uri resourceLocater = new System.Uri("/LorealOptimiseGui;component/lists/products.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Lists\Products.xaml"
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
            this.txtMaterialCodeFilter = ((DevExpress.Xpf.Editors.TextEdit)(target));
            
            #line 37 "..\..\..\Lists\Products.xaml"
            this.txtMaterialCodeFilter.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtMaterialCodeFilter_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtDescriptionFilter = ((DevExpress.Xpf.Editors.TextEdit)(target));
            
            #line 41 "..\..\..\Lists\Products.xaml"
            this.txtDescriptionFilter.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtMaterialCodeFilter_KeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtProcurementTypeFilter = ((DevExpress.Xpf.Editors.TextEdit)(target));
            
            #line 45 "..\..\..\Lists\Products.xaml"
            this.txtProcurementTypeFilter.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtMaterialCodeFilter_KeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnFilter = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\Lists\Products.xaml"
            this.btnFilter.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.grdProducts = ((DevExpress.Xpf.Grid.GridControl)(target));
            return;
            case 7:
            this.cboDivisions = ((DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings)(target));
            return;
            case 8:
            this.colOperation = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 11:
            
            #line 117 "..\..\..\Lists\Products.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).InitNewRow += new DevExpress.Xpf.Grid.InitNewRowEventHandler(this.TableView_InitNewRow);
            
            #line default
            #line hidden
            
            #line 117 "..\..\..\Lists\Products.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).ShowingEditor += new DevExpress.Xpf.Grid.ShowingEditorEventHandler(this.TableView_ShowingEditor);
            
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
            case 9:
            
            #line 102 "..\..\..\Lists\Products.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnReplace_Click);
            
            #line default
            #line hidden
            
            #line 102 "..\..\..\Lists\Products.xaml"
            ((System.Windows.Controls.Button)(target)).Initialized += new System.EventHandler(this.btnReplace_Initialized);
            
            #line default
            #line hidden
            break;
            case 10:
            
            #line 103 "..\..\..\Lists\Products.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnEditMultiples_Click);
            
            #line default
            #line hidden
            
            #line 103 "..\..\..\Lists\Products.xaml"
            ((System.Windows.Controls.Button)(target)).Initialized += new System.EventHandler(this.btnReplace_Initialized);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

