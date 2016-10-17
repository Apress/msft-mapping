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
using Microsoft.Maps.MapControl.WPF;


namespace WPFMapApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new EarthquakeViewModel();
        }

        private void Pushpin_MouseEnter(object sender, MouseEventArgs e)
        {
            FrameworkElement pin = sender as FrameworkElement;
            MapLayer.SetPosition(ContentPopup, MapLayer.GetPosition(pin));
            MapLayer.SetPositionOffset(ContentPopup, new Point(20, -20));

            var location = (Earthquake)pin.Tag;

            ContentPopupText.Text = "Magnitude " + location.Magnitude;
            ContentPopupDescription.Text = location.When.ToString();
            ContentPopup.Visibility = Visibility.Visible;
        }

        private void Pushpin_MouseLeave(object sender, MouseEventArgs e)
        {
            ContentPopup.Visibility = Visibility.Collapsed;
        }
    }
}
