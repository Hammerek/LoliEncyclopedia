using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace LoliEncyclopedia
{
    public sealed partial class LoliPage : Page
    {
        public LoliPage()
        {
            InitializeComponent();
            LoliListView.SelectionChanged += LoliListView_SelectionChanged;
            OpenGalleryView(true);
            OpenGalleryView(false);
            var secondRun = Instance != null;
            Instance = this;
            if (!secondRun)
            {
                var task = GetHarem();
            }
            else
            {
                UpdateLoliListView();
            }
        }

        private async void LoliListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            if (e != null)
            {
                if (IsGalleryOpen)
                {
                    await WebHelper.DownloadLoliGallery(loliname);
                }
                else
                {
                    var task = WebHelper.DownloadLoliGallery(loliname);
                }
            }
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

        private bool IsGalleryOpen { get; set; }

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
            UpdateLoliListView();
        }

        private void UpdateLoliListView()
        {
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
             {
                 LoliListView.ItemsSource = LoliInfoDatabase.GetLoliTitles();
                 MainPage.Instance.IsEnabled = true;
             });
        }

        private async Task GetIcons()
        {
            var list = LoliInfoDatabase.GetLoliTitles();
            var iconHashes = await WebHelper.GetLatestIconHashes();
            foreach (var loliTitle in list)
            {
                if (!string.IsNullOrEmpty(loliTitle))
                {
                    try
                    {
                        if (await FileHelper.IconFileExistsAsync(loliTitle))
                        {
                            var currentFileHash = await FileHelper.GetIconFileHash(loliTitle);
                            var newFileHash = iconHashes[loliTitle + ".png"];
                            if (!currentFileHash.Equals(newFileHash))
                            {
                                await WebHelper.DownloadIconImage(loliTitle);
                            }
                        }
                        else
                        {
                            await WebHelper.DownloadIconImage(loliTitle);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public async void OpenGalleryView(bool galleryOpen)
        {
            IsGalleryOpen = galleryOpen;
            if (galleryOpen)
            {
                var loliname = (string)LoliListView.SelectedItem;
                if (loliname != null)
                {
                    await WebHelper.DownloadLoliGallery(loliname);
                }
                LoliInfo.Navigate(typeof(LoliGalleryPage));
                LoliListView_SelectionChanged(LoliListView, null);
            }
            else
            {
                LoliInfo.Navigate(typeof(LoliInfoPage));
                LoliListView_SelectionChanged(LoliListView, null);
            }
        }

        public static LoliPage Instance { get; set; }
    }

    public class IconImageBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var s = value as string;
            var path = FileHelper.IconDirectory.Path;
            Uri output;
            var r1 = Uri.TryCreate(path + "/" + s + ".png", UriKind.Absolute, out output);
            var task = Task.Run(async () =>
              {
                  var r2 = await FileHelper.IconFileExistsAsync(s);
                  return (r1 && r2) ? output : MainPage.LoliNotFoundUri;
              });
            return task.Result;
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
