﻿#pragma checksum "..\..\WaterMarking.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "10D272F4F086E3203DF67EB15FF655D0"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.34209
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
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


namespace WpfSteganography {
    
    
    /// <summary>
    /// WaterMark
    /// </summary>
    public partial class WaterMark : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\WaterMarking.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WpfSteganography.WaterMark Window_WaterMarking;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\WaterMarking.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WaterMarking_exit;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\WaterMarking.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox WaterMarking_text;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\WaterMarking.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox WaterMarking_opacity;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\WaterMarking.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WaterMarking_image;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\WaterMarking.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WaterMarking_preview;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\WaterMarking.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WaterMarking_save;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\WaterMarking.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WaterMarking_preview_aft;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\WaterMarking.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WaterMarking_save_aft;
        
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
            System.Uri resourceLocater = new System.Uri("/WpfSteganography;component/watermarking.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\WaterMarking.xaml"
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
            this.Window_WaterMarking = ((WpfSteganography.WaterMark)(target));
            return;
            case 2:
            this.WaterMarking_exit = ((System.Windows.Controls.Label)(target));
            
            #line 12 "..\..\WaterMarking.xaml"
            this.WaterMarking_exit.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.WaterMarking_exit_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.WaterMarking_text = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.WaterMarking_opacity = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.WaterMarking_image = ((System.Windows.Controls.Label)(target));
            
            #line 26 "..\..\WaterMarking.xaml"
            this.WaterMarking_image.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.WaterMarking_image_MouseLeftButtonDown_1);
            
            #line default
            #line hidden
            return;
            case 6:
            this.WaterMarking_preview = ((System.Windows.Controls.Label)(target));
            
            #line 31 "..\..\WaterMarking.xaml"
            this.WaterMarking_preview.MouseEnter += new System.Windows.Input.MouseEventHandler(this.WaterMarking_preview_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 7:
            this.WaterMarking_save = ((System.Windows.Controls.Label)(target));
            
            #line 36 "..\..\WaterMarking.xaml"
            this.WaterMarking_save.MouseEnter += new System.Windows.Input.MouseEventHandler(this.WaterMarking_save_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 8:
            this.WaterMarking_preview_aft = ((System.Windows.Controls.Label)(target));
            
            #line 41 "..\..\WaterMarking.xaml"
            this.WaterMarking_preview_aft.MouseLeave += new System.Windows.Input.MouseEventHandler(this.WaterMarking_preview_aft_MouseLeave);
            
            #line default
            #line hidden
            
            #line 41 "..\..\WaterMarking.xaml"
            this.WaterMarking_preview_aft.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.WaterMarking_preview_aft_MouseLeftButtonDown_1);
            
            #line default
            #line hidden
            return;
            case 9:
            this.WaterMarking_save_aft = ((System.Windows.Controls.Label)(target));
            
            #line 46 "..\..\WaterMarking.xaml"
            this.WaterMarking_save_aft.MouseLeave += new System.Windows.Input.MouseEventHandler(this.WaterMarking_save_aft_MouseLeave);
            
            #line default
            #line hidden
            
            #line 46 "..\..\WaterMarking.xaml"
            this.WaterMarking_save_aft.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.WaterMarking_save_aft_MouseLeftButtonDown_1);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

