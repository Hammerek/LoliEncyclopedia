using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Xaml.Interactions.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LoliEncycopedia
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoliGalleryPage : Page

    {
        public LoliGalleryPage()
        {
            this.InitializeComponent();
            Instance = this;
        }

        

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.OpenView(false);
        }
        public static LoliGalleryPage Instance { get; private set; }

        public void UpdateLoli(string title)
        {
            
        }
    }
}
