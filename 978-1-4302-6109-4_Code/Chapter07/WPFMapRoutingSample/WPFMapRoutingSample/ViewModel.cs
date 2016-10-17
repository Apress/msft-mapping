using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.Maps.MapControl.WPF;

namespace WPFMapRoutingSample
{
    public class ViewModel: INotifyPropertyChanged
    {
        public ICommand CalculateRouteCommand { get; private set; }    
        private string _from = "Sunnyvale, CA";
        public string From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged("From");
            }
        }

        private string _to = "201 Castro St Ste 1, Mountain View, CA";
        public string To
        {
            get { return _to; }
            set
            {
                _to = value;
                 OnPropertyChanged("To");
            }
        }

        private BingRouteService.RouteResult _routeResult;
        public BingRouteService.RouteResult RouteResult
        {
            get { return _routeResult; }
            set
            {
                _routeResult = value;
                OnPropertyChanged("RouteResult");
            }
        }

        private ObservableCollection<Waypoint> _waypoints;
        public ObservableCollection<Waypoint> Waypoints
        {
            get { return _waypoints; }
            set
            {
                _waypoints = value;
                OnPropertyChanged("Waypoints");
            }
        }

        public ViewModel()
        {
            CalculateRouteCommand = new DelegateCommand(CalculateRoute);
        }

        private void CalculateRoute()
        {
            var from = GeocodeAddress(From);
            var to = GeocodeAddress(To);

            CalculateRoute(from, to);
        }

        private BingGeocodeService.GeocodeResult GeocodeAddress(string address)
        {
            BingGeocodeService.GeocodeResult result = null;

            using (BingGeocodeService.GeocodeServiceClient client = 
                new BingGeocodeService.GeocodeServiceClient("CustomBinding_IGeocodeService"))
            {
                BingGeocodeService.GeocodeRequest request = new BingGeocodeService.GeocodeRequest();
                request.Credentials = new Credentials() 
                { 
                        ApplicationId = (App.Current.Resources["MyCredentials"] as 
                            ApplicationIdCredentialsProvider).ApplicationId 
                };
                request.Query = address;
                result = client.Geocode(request).Results[0];
            }

            return result;
        }

        private void CalculateRoute(BingGeocodeService.GeocodeResult from, BingGeocodeService.GeocodeResult to)
        {
            using (BingRouteService.RouteServiceClient client = 
                new BingRouteService.RouteServiceClient("CustomBinding_IRouteService"))
            {
                BingRouteService.RouteRequest request = new BingRouteService.RouteRequest();
                request.Credentials = new Credentials() 
                { 
                    ApplicationId = (App.Current.Resources["MyCredentials"] as 
                        ApplicationIdCredentialsProvider).ApplicationId 
                };
                request.Waypoints = new BingRouteService.Waypoint[2];
                request.Waypoints[0] = ConvertGeocodeResultToWaypoint(from);
                request.Waypoints[1] = ConvertGeocodeResultToWaypoint(to);

                request.Options = new BingRouteService.RouteOptions();
                request.Options.RoutePathType = BingRouteService.RoutePathType.Points;

                RouteResult = client.CalculateRoute(request).Result;
            }
            MakeWaypoints();
        }

        private void MakeWaypoints()
        {
            Waypoints = new ObservableCollection<Waypoint>();

            foreach (BingRouteService.ItineraryItem item in RouteResult.Legs[0].Itinerary)
            {
                Waypoints.Add(new Waypoint()
                {
                    Description = GetDirectionText(item),
                    Location = new Location(item.Location.Latitude, item.Location.Longitude)
                });
            }
        }

        private static string GetDirectionText(BingRouteService.ItineraryItem item)
        {
            string direction = item.Text;
            Regex regex = new Regex("<(.|\n)*?>");
            direction = regex.Replace(direction, string.Empty);
            return direction;
        }

        private BingRouteService.Waypoint ConvertGeocodeResultToWaypoint(BingGeocodeService.GeocodeResult result)
        {
            BingRouteService.Waypoint waypoint = new BingRouteService.Waypoint();
            waypoint.Description = result.DisplayName;
            waypoint.Location = new BingRouteService.Location()
                {
                    Latitude = result.Locations[0].Latitude,
                    Longitude = result.Locations[0].Longitude
                };
            return waypoint;
        }

        public event PropertyChangedEventHandler PropertyChanged;

       protected virtual void OnPropertyChanged(string propertyName)
       {
            if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
       }
    }
}
