#pragma checksum "Z:\Projects\NMPIB\Backup\ImageBrowser\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6E99D53E5ECF6F7A21C3E4812C1C3146"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace ImageBrowser {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ComboBox MagList;
        
        internal System.Windows.Controls.TextBox txtMagazineIssue;
        
        internal System.Windows.Controls.TextBox txtShoot;
        
        internal System.Windows.Controls.DatePicker ShootDate;
        
        internal System.Windows.Controls.TextBox txtDescription;
        
        internal System.Windows.Controls.TextBox txtKeywords;
        
        internal System.Windows.Controls.TextBlock lblPhotographer;
        
        internal System.Windows.Controls.Button btnBrowse;
        
        internal System.Windows.Controls.Image myImage;
        
        internal System.Windows.Controls.Button btnSave;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ImageBrowser;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.MagList = ((System.Windows.Controls.ComboBox)(this.FindName("MagList")));
            this.txtMagazineIssue = ((System.Windows.Controls.TextBox)(this.FindName("txtMagazineIssue")));
            this.txtShoot = ((System.Windows.Controls.TextBox)(this.FindName("txtShoot")));
            this.ShootDate = ((System.Windows.Controls.DatePicker)(this.FindName("ShootDate")));
            this.txtDescription = ((System.Windows.Controls.TextBox)(this.FindName("txtDescription")));
            this.txtKeywords = ((System.Windows.Controls.TextBox)(this.FindName("txtKeywords")));
            this.lblPhotographer = ((System.Windows.Controls.TextBlock)(this.FindName("lblPhotographer")));
            this.btnBrowse = ((System.Windows.Controls.Button)(this.FindName("btnBrowse")));
            this.myImage = ((System.Windows.Controls.Image)(this.FindName("myImage")));
            this.btnSave = ((System.Windows.Controls.Button)(this.FindName("btnSave")));
        }
    }
}
