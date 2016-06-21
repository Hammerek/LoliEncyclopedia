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
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LoliEncyclopedia
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
            SetUpPageAnimation();
        }

        private void SetUpPageAnimation() //NOT WORKING 
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();
            var info = new ContinuumNavigationTransitionInfo();
            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            LoliPage.Instance.OpenGalleryView(false);
        }
        public static LoliGalleryPage Instance { get; private set; }

        public async void UpdateLoli(string title, string name)
        {
            var dictionary = await FileHelper.GetGalleryFiles(title);
            Loli_Name.Text = name;
            Loli_Gallery.ItemsSource = dictionary;
        }
    }
}
