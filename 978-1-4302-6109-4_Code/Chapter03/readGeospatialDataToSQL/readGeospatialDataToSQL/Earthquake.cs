using System;
using Microsoft.Maps.MapControl.WPF;

namespace readGeospatialDataToSQL
{
    public class Earthquake
    {
        public DateTime When { get; set; }
        public Location Location { get; set; }
        public float Depth { get; set; }
        public float Magnitude { get; set; }
        public string MagType { get; set; }
        public int NbStation { get; set; }
        public int Gap { get; set; }
        public float Distance { get; set; }
        public float RMS { get; set; }
        public string Source { get; set; }
        public string EventID { get; set; }
        public float Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Earthquake(DateTime when, Location where, float depth, float magnitude, string magType,
            int nbStation, int gap, float distance, float rms, string source, string eventId, float version,
            string title, string description = "")

        {
            When = when;
            Location = where;
            Depth = depth;
            Magnitude = magnitude;
            MagType = magType;
            NbStation = nbStation;
            Gap = gap;
            Distance = distance;
            RMS = rms;
            Source = source;
            EventID = eventId;
            Version = version;
            Title = title;
            Description = description;
        }
    }
}
