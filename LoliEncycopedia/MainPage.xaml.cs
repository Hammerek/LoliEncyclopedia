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
        public MainPage()
        {
            LoliInfoDatabase.CreateDatabase();
            InitializeComponent();
            LoliListView.SelectionChanged += LoliListView_SelectionChanged;
            FileHelper.PrepareDirectories();
            LoliInfo.Navigate(typeof(LoliInfoPage));
            var task = GetHarem();
        }

        private void LoliListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = sender as GridView;
            var selLoli = (KeyValuePair<string, string>)lb.SelectedItem;
            var loliname = selLoli.Key;
            if (!LoliInfoDatabase.ContainsLoli(loliname))
            {
                Debug.WriteLine("Cannot find " + loliname);
            }
            else
            {
                var loliinfo = LoliInfoDatabase.GetLoliInfo(loliname);
                LoliInfoPage.Instance.UpdateLoli(loliinfo);
            }
        }

        private async Task GetHarem()
        {
            var list = await WebHelper.GetLatestLoliInfos();
            if (list != null && list.Count > 0)
            {
                foreach (var para in list)
                {
                    var loliTitle = para.Key;
                    var loliInfo = para.Value;
                    loliInfo.Title = loliTitle;
                    var fileName = string.Join("_", loliInfo.Title.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    if (!string.IsNullOrEmpty(loliInfo.Icon))
                    {
                        try
                        {
                            loliInfo.Icon = (await WebHelper.DownloadImage(fileName, loliInfo.Icon)).Path;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                    }
                    if (!LoliInfoDatabase.ContainsLoli(para.Key))
                    {
                        LoliInfoDatabase.AddLoliInfo(loliInfo);
                    }
                    else
                    {
                        LoliInfoDatabase.UpdateLoliInfo(loliInfo);
                    }
                }
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                LoliListView.ItemsSource = LoliInfoDatabase.GetHarem();
            });
        }
    }

    public class ImageBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var s = (KeyValuePair<string, string>)value;
            return new Uri(s.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }

}
