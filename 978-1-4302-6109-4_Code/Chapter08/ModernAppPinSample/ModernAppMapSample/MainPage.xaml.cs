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
using Windows.UI.Core;
using Bing.Maps;
using Bing.Maps.VenueMaps;
using Windows.Devices.Geolocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernAppMapSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        CoreDispatcher mDispatcher;
        Pushpin m_youAreHerePin;
        CustomPin m_pushPin;
        Geolocator m_geolocator;
        public MainPage()
        {
            this.InitializeComponent();
            VenueManager m = map.VenueManager;
            m.UseDefaultVenueTooltip = false;
            m.VenueEntityTapped += Venue_Tapped;

            mDispatcher = Dispatcher;
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            m_geolocator = new Geolocator();
            m_geolocator.PositionChanged += Geolocator_PositionChanged;
            m_geolocator.StatusChanged += Geolocator_StatusChanged;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Venue_Tapped(object sender, VenueEntityEventArgs e)
        {
            if (e.VenueEntity != null)
            {
                Location location = e.VenueEntity.Location;
                ShowLatLong(location);
            }
        }

        private void Map_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var position = e.GetPosition(map);
            Location location;
            map.TryPixelToLocation(position, out location);
            ShowLatLong(location);
        }

        private void Map_LandmarkTapped(object sender, LandmarkTappedEventArgs e)
        {
            if (e.Landmarks.Count != 0)
            {
                ShowLatLong(e.Landmarks[0].Location);
            }
        }

        private void ShowLatLong(Location location)
        {
            if (m_pushPin == null)
            {
                    m_pushPin = new CustomPin();
                    map.Children.Add(m_pushPin);
            }

            m_pushPin.Latitude =
                Math.Round(location.Latitude, 4).ToString();
            m_pushPin.Longitude =
                Math.Round(location.Longitude, 4).ToString();

            MapLayer.SetPosition(m_pushPin, location);
            map.SetView(location);
        }

        private void Geolocator_StatusChanged(Geolocator g, StatusChangedEventArgs e)
        {
            int i;
        }


        private async void Geolocator_PositionChanged(Geolocator g, PositionChangedEventArgs e)
        {
           await mDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
           {
               if (m_youAreHerePin == null)
               {
                   m_youAreHerePin = new Pushpin();
                   map.Children.Add(m_youAreHerePin);
               }

               Location location = new Location(e.Position.Coordinate.Latitude,
                   e.Position.Coordinate.Longitude);
               MapLayer.SetPosition(m_youAreHerePin, location);
           });
        }

    }
}
