using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NASA_API_App
{
    /// <summary>
    /// Interaction logic for DetailsControl.xaml
    /// </summary>
    public partial class DetailsControl : UserControl
    {

        public event EventHandler CloseRequested;

        public DetailsControl()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the CloseRequested event
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
