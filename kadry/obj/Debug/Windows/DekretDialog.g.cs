﻿#pragma checksum "..\..\..\Windows\DekretDialog.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1CCE0300DEC57740A084CB63E1959529D45F63CAE02D6D8AD1F5B74EC3A5E42E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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
using kadry.Windows;


namespace kadry.Windows {
    
    
    /// <summary>
    /// DekretDialog
    /// </summary>
    public partial class DekretDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\Windows\DekretDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid Lista;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Windows\DekretDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SymbolSrch;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\Windows\DekretDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NazwaSrch;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\Windows\DekretDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Select;
        
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
            System.Uri resourceLocater = new System.Uri("/kadry;component/windows/dekretdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\DekretDialog.xaml"
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
            this.Lista = ((System.Windows.Controls.DataGrid)(target));
            
            #line 25 "..\..\..\Windows\DekretDialog.xaml"
            this.Lista.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Lista_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SymbolSrch = ((System.Windows.Controls.TextBox)(target));
            
            #line 45 "..\..\..\Windows\DekretDialog.xaml"
            this.SymbolSrch.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SymbolSrch_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.NazwaSrch = ((System.Windows.Controls.TextBox)(target));
            
            #line 55 "..\..\..\Windows\DekretDialog.xaml"
            this.NazwaSrch.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SymbolSrch_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Select = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\Windows\DekretDialog.xaml"
            this.Select.Click += new System.Windows.RoutedEventHandler(this.Select_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

