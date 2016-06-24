using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LoliEncyclopedia;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;
using LoliEncyclopedia.Sources;
using SQLite.Net.Platform.WinRT;
using SQLite.Net;


namespace LoliEncyclopedia
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Uri LoliNotFoundUri = new Uri("ms-appx:///Assets/LoliNotFound.jpg");
        public MainPage()
        {
            LoliInfoDatabase.CreateDatabase();
            InitializeComponent();
            FileHelper.PrepareDirectories();
            Instance = this;
            MainFrameInstance = MainFrame;
            MainFrame.Navigate(typeof (Pages.LoliPage));          
        }

        public static MainPage Instance { get; private set; }
        public  Frame MainFrameInstance { get; private set; }
    }
}
