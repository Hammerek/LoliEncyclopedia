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
            Arigato.SelectionChanged += clickInfo;
            LoliInfo.Navigate(typeof(LoliInfoPage));
            var task = GetHarem();
        }

        private void clickInfo(object sender, SelectionChangedEventArgs e)
        {

            Debug.WriteLine("loli");
            var lb = sender as ListBox;
            var selLoli = (KeyValuePair<string, string>)lb.SelectedItem;
            var loliname = selLoli.Key;
            if (!LoliInfoDatabase.ContainsLoli(loliname))
            {
                Debug.WriteLine("Nie mozna znalezc danej " + loliname);
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
                    if (!LoliInfoDatabase.ContainsLoli(para.Key))
                    {
                        LoliInfoDatabase.AddLoliInfo(para.Value);
                    }
                }
            }
            Arigato.ItemsSource = LoliInfoDatabase.GetHarem();
        }
    }
    public class ImageBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var s = (KeyValuePair<string, string>)value;
            return new Uri("ms-appx:///Assets/LoliPictures/" + s.Value + ".png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }

}
