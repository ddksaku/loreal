﻿#pragma checksum "..\..\..\Lists\RetailerTypes.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E3DCBDC1FD2CF2025EA633B351C3C069"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    /// RetailerTypes
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class RetailerTypes : LorealOptimiseGui.Lists.BaseListUserControl<LorealOptimiseBusiness.Lists.RetailerTypeManager, LorealOptimiseData.RetailerType>, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\Lists\RetailerTypes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblTitle;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Lists\RetailerTypes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRefresh;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Lists\RetailerTypes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridControl grdRetailerTypes;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Lists\RetailerTypes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings cboDivisions;
        
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
            System.Uri resourceLocater = new System.Uri("/LorealOptimiseGui;component/lists/retailertypes.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Lists\RetailerTypes.xaml"
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
            
            #line 20 "..\..\..\Lists\RetailerTypes.xaml"
            this.btnRefresh.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.grdRetailerTypes = ((DevExpress.Xpf.Grid.GridControl)(target));
            return;
            case 4:
            this.cboDivisions = ((DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
