﻿#pragma checksum "..\..\..\..\Controls\Animation\CustomerGroupAllocation.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0EE8AD853643586120E719678054B5AF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Xpf.Data;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.Themes;
using LorealOptimiseGui;
using LorealOptimiseGui.Controls;
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


namespace LorealOptimiseGui.Controls {
    
    
    /// <summary>
    /// CustomerGroupAllocation
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class CustomerGroupAllocation : LorealOptimiseGui.BaseUserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\..\Controls\Animation\CustomerGroupAllocation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridControl grdCustomerGroupAllocations;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Controls\Animation\CustomerGroupAllocation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn clmFixedAllocation;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\Controls\Animation\CustomerGroupAllocation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridColumn clmRetailUplift;
        
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
            System.Uri resourceLocater = new System.Uri("/LorealOptimiseGui;component/controls/animation/customergroupallocation.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\Animation\CustomerGroupAllocation.xaml"
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
            this.grdCustomerGroupAllocations = ((DevExpress.Xpf.Grid.GridControl)(target));
            return;
            case 2:
            
            #line 29 "..\..\..\..\Controls\Animation\CustomerGroupAllocation.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).ShowingEditor += new DevExpress.Xpf.Grid.ShowingEditorEventHandler(this.TableView_ShowingEditor);
            
            #line default
            #line hidden
            
            #line 29 "..\..\..\..\Controls\Animation\CustomerGroupAllocation.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).CellValueChanged += new DevExpress.Xpf.Grid.CellValueChangedEventHandler(this.TableView_CellValueChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.clmFixedAllocation = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            case 4:
            this.clmRetailUplift = ((DevExpress.Xpf.Grid.GridColumn)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

