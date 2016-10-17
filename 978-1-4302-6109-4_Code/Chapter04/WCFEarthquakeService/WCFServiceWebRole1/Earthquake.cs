using System;
using System.Runtime.Serialization;
using Microsoft.Maps.MapControl.WPF;

namespace WCFServiceWebRole1
{
    [DataContract]
    public class Earthquake
    {
        [DataMember]
        public DateTime When { get; set; }
        [DataMember]
        public Location Location { get; set; }
        [DataMember]
        public float Depth { get; set; }
        [DataMember]
        public float Magnitude { get; set; }
        [DataMember]
        public string MagType { get; set; }
        [DataMember]
        public int NbStation { get; set; }
        [DataMember]
        public int Gap { get; set; }
        [DataMember]
        public float Distance { get; set; }
        [DataMember]
        public float RMS { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string EventID { get; set; }
        [DataMember]
        public float Version { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }

        public Earthquake()
        {

        }

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
