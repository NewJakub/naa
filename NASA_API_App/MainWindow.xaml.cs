using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NASA_API_App.APIServices;
using NASA_API_App.Models;
namespace NASA_API_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Asteroids> AsteroidList{ get; set; }
        public StringViewModel stringView = new StringViewModel();
        private readonly APIHandler NasaApiHandler;
        public MainWindow()
        {
            InitializeComponent();
            
            AsteroidList = new ObservableCollection<Asteroids>();
            NasaApiHandler = new APIHandler();

            CallData(AsteroidList);
            

            
            YourListBox.ItemsSource = AsteroidList;
            DataContext = stringView;
            
        }

        public static async void CallData(ObservableCollection<Asteroids> astList)
        {
            APIHandler NasaApiHandler = new APIHandler();
            var data = await NasaApiHandler.GetAsteroidsAsync(DateTime.UtcNow.ToString("yyyy-MM-dd"), DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd"), "PlgKdthvA7rmL9RpBz2i91Y6Nfy9A5LqtDoh3eKt");
            
            foreach (Asteroids asteroid in data)
            {
                astList.Add(asteroid);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            MenuContainer.Content = settings;
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            AsteroidList.Clear();
            CallData(AsteroidList);
        }

        private void YourListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedItem = e.AddedItems[0];

                // Create and configure the UserControl
                var detailsControl = new DetailsControl
                {
                    DataContext = selectedItem
                };

                // Subscribe to the CloseRequested event
                detailsControl.CloseRequested += (s, args) =>
                {
                    DetailsContentControl.Content = null; // Clear the UserControl
                };

                // Display the UserControl
                DetailsContentControl.Content = detailsControl;
            }
        }
    }
}