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
using LoliEncycopedia;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;
using SQLite.Net.Platform.WinRT;
using SQLite.Net;


namespace LoliEncycopedia
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
            LoliListView.SelectionChanged += LoliListView_SelectionChanged;
            FileHelper.PrepareDirectories();
            OpenView(true);
            OpenView(false);
            Current = this;
            var task = GetHarem();
        }

        private void LoliListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = sender as GridView;
            if (lb.SelectedItem == null)
            {
                if (LoliInfoPage.Instance != null)
                {
                    LoliInfoPage.Instance.UpdateLoli(null);
                }
                return;
            }
            var loliname = (string)lb.SelectedItem;
            if (LoliInfoDatabase.ContainsLoli(loliname))
            {
                var loliinfo = LoliInfoDatabase.GetLoliInfo(loliname);
                LoliInfoPage.Instance.UpdateLoli(loliinfo);
                LoliGalleryPage.Instance.UpdateLoli(loliinfo.Title, loliinfo.Name);
            }
            else
            {
                Debug.WriteLine("Cannot find " + loliname);
            }
        }

        private async Task GetHarem()
        {
            try
            {
                var list = await WebHelper.GetLatestLoliInfos();
                if (list != null && list.Count > 0)
                {
                    foreach (var para in list)
                    {
                        var loliTitle = para.Key;
                        var loliInfo = para.Value;
                        loliInfo.Title = loliTitle;

                        if (!LoliInfoDatabase.ContainsLoli(para.Key))
                        {
                            LoliInfoDatabase.AddLoliInfo(loliInfo);
                        }
                        else
                        {
                            LoliInfoDatabase.UpdateLoliInfo(loliInfo);
                        }
                    }
                    await GetIcons();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("No internet connection. " + e);
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                LoliListView.ItemsSource = LoliInfoDatabase.GetLoliTitles();
                MainPageInstance.IsEnabled = true;
            });
        }

        private async Task GetIcons()
        {
            var list = LoliInfoDatabase.GetLoliTitles();
            foreach (var loliName in list)
            {
                if (!string.IsNullOrEmpty(loliName))
                {
                    try
                    {
                        await WebHelper.DownloadIconImage(loliName);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public void OpenView(bool galleryOpen)
        {
            if (galleryOpen)
            {
                LoliInfo.Navigate(typeof(LoliGalleryPage));
                LoliListView_SelectionChanged(LoliListView, null);
            }
            else
            {
                LoliInfo.Navigate(typeof(LoliInfoPage));
                LoliListView_SelectionChanged(LoliListView, null);
            }
        }

        public static MainPage Current { get; private set; }
    }

    public class IconImageBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var s = value as string;
            var path = FileHelper.IconDirectory.Path;
            Uri output;
            return Uri.TryCreate(path + "/" + s + ".png", UriKind.Absolute, out output) ? output : MainPage.LoliNotFoundUri;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }

    public class GalleryImageBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var s = (KeyValuePair<string, string>)value;
            var path = FileHelper.GalleriesDirectory.Path;
            Uri output;
            return Uri.TryCreate(s.Value, UriKind.Absolute, out output) ? output : MainPage.LoliNotFoundUri;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }
}
