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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public string apiKey;
        public Settings()
        {
            InitializeComponent();
            apiKey = "PlgKdthvA7rmL9RpBz2i91Y6Nfy9A5LqtDoh3eKt";
            txtbox.Text = apiKey;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            
        }

        private void txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            apiKey = txtbox.Text;
        }

        public void SetApiKey(string apiKey, TextBlock text)
        {


        }
    }
}
