﻿#pragma checksum "C:\Users\Jaxbot\Documents\Visual Studio 2010\Projects\dynamicBackground\dynamicBackground\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D6688149D287C7C13264455F8DC30A82"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace dynamicBackground {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Image image1;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.RadioButton radioButton1;
        
        internal System.Windows.Controls.RadioButton radioButton2;
        
        internal System.Windows.Controls.StackPanel slideshowPanel;
        
        internal System.Windows.Controls.ScrollViewer slideshowScroller;
        
        internal System.Windows.Controls.StackPanel slideshowImages;
        
        internal System.Windows.Controls.Slider slider1;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Controls.TextBlock txtMinutes;
        
        internal System.Windows.Controls.Button button1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/dynamicBackground;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.image1 = ((System.Windows.Controls.Image)(this.FindName("image1")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.radioButton1 = ((System.Windows.Controls.RadioButton)(this.FindName("radioButton1")));
            this.radioButton2 = ((System.Windows.Controls.RadioButton)(this.FindName("radioButton2")));
            this.slideshowPanel = ((System.Windows.Controls.StackPanel)(this.FindName("slideshowPanel")));
            this.slideshowScroller = ((System.Windows.Controls.ScrollViewer)(this.FindName("slideshowScroller")));
            this.slideshowImages = ((System.Windows.Controls.StackPanel)(this.FindName("slideshowImages")));
            this.slider1 = ((System.Windows.Controls.Slider)(this.FindName("slider1")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.txtMinutes = ((System.Windows.Controls.TextBlock)(this.FindName("txtMinutes")));
            this.button1 = ((System.Windows.Controls.Button)(this.FindName("button1")));
        }
    }
}
