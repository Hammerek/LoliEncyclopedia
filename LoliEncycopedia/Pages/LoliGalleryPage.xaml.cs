using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using LoliEncyclopedia.Sources;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LoliEncyclopedia.Pages
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
