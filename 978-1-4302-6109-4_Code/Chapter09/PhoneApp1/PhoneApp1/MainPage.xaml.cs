using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using Microsoft.Phone.Maps.Toolkit;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        EarthquakeViewModel m_model;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            m_model = new EarthquakeViewModel(this);
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "ApplicationID";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "AuthenticationToken";
        }

        public void UpdateDataBinding()
        {
            MapExtensions.GetChildren(map).OfType<MapItemsControl>().First().ItemsSource = m_model.Earthquakes;
        }

        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}