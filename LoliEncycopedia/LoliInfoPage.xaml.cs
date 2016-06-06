﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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



namespace LoliEncycopedia
{
    /// <summary>
    /// Page about loli information. 
    /// </summary>
    public sealed partial class LoliInfoPage : Page
    {
        public LoliInfoPage()
        {
            InitializeComponent();
            Instance = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        public void UpdateLoli(LoliInfo loliinfo)
        {
            if (loliinfo == null)
            {
                Loli_Gallery.IsEnabled = false;
            }
            else
            {
                Loli_Gallery.IsEnabled = true;
                LoliInfo = loliinfo;
                Loli_Anime.Text = loliinfo.Anime;
                Loli_Name.Text = loliinfo.Name;
                Loli_Age.Text = loliinfo.Age == 0 ? "N/A" : loliinfo.Age.ToString();
                Loli_Height.Text = loliinfo.Heigth == 0 ? "N/A" : (loliinfo.Heigth + " cm");
                Loli_Weight.Text = loliinfo.Weigth == 0 ? "N/A" : (loliinfo.Weigth + " kg");
                Loli_Chest.Text = loliinfo.ChestSize == 0 ? "N/A" : (loliinfo.ChestSize + " cm");
                Loli_Waist.Text = loliinfo.WaistSize == 0 ? "N/A" : (loliinfo.WaistSize + " cm");
                Loli_Hip.Text = loliinfo.HipSize == 0 ? "N/A" : (loliinfo.HipSize + " cm");
            }
        }

        private void Loli_Gallery_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.OpenGalleryView(true);
        }
        public LoliInfo LoliInfo { get; set; }
        public static LoliInfoPage Instance { get; private set; }
    }
}
