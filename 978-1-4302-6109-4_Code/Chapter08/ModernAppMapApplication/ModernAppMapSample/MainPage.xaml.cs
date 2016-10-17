using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Bing.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernAppMapSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new EarthquakeViewModel();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Pushpin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement pin = sender as FrameworkElement;
            MapLayer.SetPosition(ContentPopup, MapLayer.GetPosition(pin));
            
            var location = (Earthquake)pin.Tag;

            ContentPopupText.Text = "Magnitude " + location.Magnitude;
            ContentPopupDescription.Text = location.When.ToString();
            ContentPopup.Visibility = Visibility.Visible;
            //map.Tapped += Map_Tapped;
        }

        private bool Map_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ContentPopup.Visibility == Visibility.Visible)
            {
                ContentPopup.Visibility = Visibility.Collapsed;
                return true;
            }
            else
                return false;
        }
    }
}
