﻿#pragma checksum "..\..\..\Pages\Packages.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E0A6FCCB6DE682618A3C0B3B073AEB49"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AssetManager.Common;
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


namespace AssetManager.Pages {
    
    
    /// <summary>
    /// Packages
    /// </summary>
    public partial class Packages : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\Pages\Packages.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PackageTypes;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Pages\Packages.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox PackagesList;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Pages\Packages.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ImportPackage;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Pages\Packages.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeletePackage;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Pages\Packages.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Preview;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Pages\Packages.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Tipp;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\Pages\Packages.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Export;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AssetManager;component/pages/packages.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\Packages.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.PackageTypes = ((System.Windows.Controls.ComboBox)(target));
            
            #line 19 "..\..\..\Pages\Packages.xaml"
            this.PackageTypes.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.PackageTypes_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.PackagesList = ((System.Windows.Controls.ListBox)(target));
            
            #line 20 "..\..\..\Pages\Packages.xaml"
            this.PackagesList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.PackagesList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ImportPackage = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\Pages\Packages.xaml"
            this.ImportPackage.Click += new System.Windows.RoutedEventHandler(this.ImportPackage_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DeletePackage = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\Pages\Packages.xaml"
            this.DeletePackage.Click += new System.Windows.RoutedEventHandler(this.DeletePackage_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Preview = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.Tipp = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.Export = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\Pages\Packages.xaml"
            this.Export.Click += new System.Windows.RoutedEventHandler(this.Export_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

