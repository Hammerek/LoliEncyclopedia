using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LoliEncyclopedia.Pages
{
    public sealed partial class About : Page
    {
        public About()
        {
            InitializeComponent();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Instance.MainFrameInstance.Navigate(typeof(LoliPage));
        }
    }
}
