﻿using System;
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

        public static LoliInfoPage Instance { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        public void UpdateLoli(LoliInfo loliinfo)
        {          
            Loli_Name.Text = loliinfo.Name;
            Loli_Age.Text = loliinfo.Age.ToString();
            Loli_Height.Text = loliinfo.Heigth.ToString();
            Loli_Weight.Text = loliinfo.Heigth.ToString();
            Loli_Chest.Text = loliinfo.ChestSize.ToString();
            Loli_Waist.Text = loliinfo.WaistSize.ToString();
            Loli_Hip.Text = loliinfo.HipSize.ToString();
        }
    }
}