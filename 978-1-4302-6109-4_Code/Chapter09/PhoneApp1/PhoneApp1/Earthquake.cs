using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.ComponentModel;

namespace PhoneApp1
{
    public class Earthquake
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Magnitude { get; set; }
        public GeoCoordinate Location { get; set; }
        public DateTime When { get; set; }
        public Earthquake(GeoCoordinate where, DateTime when, double magnitude, string title, string description = "")
        {
            Location = where;
            When = when;
            Magnitude = magnitude;
            Title = title;
            Description = description;
        }
        
    }
}
