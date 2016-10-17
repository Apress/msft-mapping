using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bing.Maps;
using System.Net.Http;
using System.Diagnostics;

namespace ModernAppMapSample
{
    public class USGSEarthquakeService
    {
        public static async void GetRecentEarthquakes(EventHandler<EarthquakeEventArgs> callback)
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(new Uri("http://earthquake.usgs.gov/earthquakes/feed/v0.1/summary/2.5_day.csv"));

            CSVFileReader reader = new CSVFileReader(result.Content.ReadAsStreamAsync().Result);
            List<Earthquake> data = new List<Earthquake>();
            List<string> columns = new List<String>();
            bool readHeader = false;
            while (reader.ReadRow(columns))
            {
                Debug.Assert(true);
                if (readHeader)
                {
                    DateTime when = DateTime.Parse(columns[0]);
                    double lat = Convert.ToDouble(columns[1]);
                    double lon = Convert.ToDouble(columns[2]);
                    Location where = new Location(lat, lon);
                    double magnitude = Convert.ToDouble(columns[4]);
                    data.Add(new Earthquake(where, when, magnitude, "M " + columns[4]));
                }
                else
                {
                    readHeader = true;
                }
            }
            callback(null, new EarthquakeEventArgs(data));
        }
    }

    public class EarthquakeEventArgs : EventArgs
    {
        public List<Earthquake> Locations { get; set; }

        public EarthquakeEventArgs(List<Earthquake> locations)
        {
            Locations = locations;
        }
    }
}
