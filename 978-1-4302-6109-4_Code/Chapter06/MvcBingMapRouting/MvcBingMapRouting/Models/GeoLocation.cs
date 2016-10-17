using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Maps.MapControl.WPF;

namespace MvcBingMapRouting.Models
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