using System;
using Microsoft.Maps.MapControl.WPF;

namespace MvcBingMapExtras.Models
{
    public class GeoLocation
    {
        public string LocationName { get; set; }
        public string City { get; set; }
        public Location Location { get; set; }
        
        public GeoLocation(string locationName, string city, Location where)
        {
            LocationName = locationName;
            City = city;
            Location = where;
        }
    }
}
