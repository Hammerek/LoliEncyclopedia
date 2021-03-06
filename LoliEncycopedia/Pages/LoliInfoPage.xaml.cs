﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using LoliEncyclopedia.Sources;

namespace LoliEncyclopedia.Pages
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
                Loli_Description.Text = loliinfo.Description ?? "N/A";
                var animeAnimation = new DoubleAnimation
                {
                    From = LoliAnimeScroll.HorizontalOffset,
                    To = 100,
                    Duration = new Duration(TimeSpan.FromSeconds(6)),
                    EnableDependentAnimation = true
                };
                var animeStoryboard = new Storyboard
                {
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                animeStoryboard.Children.Add(animeAnimation);
                Storyboard.SetTarget(animeAnimation, LoliAnimeScroll);
                Storyboard.SetTargetProperty(animeAnimation, "ScrollViewer.HorizontalOffset");
                animeStoryboard.Begin();
            }
        }

        private void Loli_Gallery_Click(object sender, RoutedEventArgs e)
        {
            LoliPage.Instance.OpenGalleryView(true);
        }

        private void Button_About_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Instance.MainFrameInstance.Navigate(typeof(About));
        }

        public LoliInfo LoliInfo { get; set; }
        public static LoliInfoPage Instance { get; private set; }
    }
}
