using System;
using System.Collections.Generic;
using getEarthquakeDataApp.ServiceReference1;

namespace getEarthquakeDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1Client client = null;

            try
            {
                client = new Service1Client();
                var test = client.GetEarthquakeData();
                var data = new List<Earthquake>();
                foreach (var earthquake in test)
                {
                    var when = earthquake.When;
                    var where = earthquake.Location;
                    var magnitude = earthquake.Magnitude;
                    var depth = earthquake.Depth;
                    var magType = earthquake.MagType;
                    var nbStation = earthquake.NbStation;
                    var gap = earthquake.Gap;
                    var distance = earthquake.Distance;
                    var rms = earthquake.RMS;
                    var source = earthquake.Source;
                    var eventId = earthquake.EventID;
                    var version = earthquake.Version;
                    var title = earthquake.Title;
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}", when, where, magnitude, depth, magType, nbStation, gap, distance, rms, source, eventId, version, title);
                    data.Add(new Earthquake(when, where, magnitude, depth, magType, nbStation, gap, distance, rms, source, eventId, version, title));
                }

                var test2 = client.GetEarthquakeDataBBox(-145, 0, -75, 45);
                var data2 = new List<Earthquake>();
                foreach (var earthquake in test2)
                {
                    var when = earthquake.When;
                    var where = earthquake.Location;
                    var magnitude = earthquake.Magnitude;
                    var depth = earthquake.Depth;
                    var magType = earthquake.MagType;
                    var nbStation = earthquake.NbStation;
                    var gap = earthquake.Gap;
                    var distance = earthquake.Distance;
                    var rms = earthquake.RMS;
                    var source = earthquake.Source;
                    var eventId = earthquake.EventID;
                    var version = earthquake.Version;
                    var title = earthquake.Title;
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}", when, where, magnitude, depth, magType, nbStation, gap, distance, rms, source, eventId, version, title);
                    data2.Add(new Earthquake(when, where, magnitude, depth, magType, nbStation, gap, distance, rms, source, eventId, version, title));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception encounter: {0}", e.Message);
            }
            finally
            {
                Console.WriteLine("Done!");
                Console.ReadLine();
                if (null != client)
                {
                    client.Close();
                }
            }
        }
    }
}
