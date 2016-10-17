using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;

namespace readGeospatialDataToSQL
{
    class GeospatialToSQL
    {
        private static List<Earthquake> _data = new List<Earthquake>();

        static void Main(string[] args)
        {
            GetRecentEarthquakes();
            insertQuakeDataToSQL();

            Console.WriteLine("Finished Getting Quake Data!");
            Console.ReadLine();
        }

        public static void GetRecentEarthquakes()
        {

            WebClient client = new WebClient();
            Uri quakeDataURL = new Uri("http://earthquake.usgs.gov/earthquakes/feed/v0.1/summary/2.5_day.csv");
            string quakeDataFile = "quake.csv";
            client.DownloadFile(quakeDataURL, quakeDataFile);
            CSVFileReader reader = new CSVFileReader(quakeDataFile);
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
                    float depth = columns[3] != "" ? Convert.ToSingle(columns[3]) : 0.0f;
                    float magnitude = columns[4] != "" ? Convert.ToSingle(columns[4]) : 0.0f;
                    string magType = columns[5];
                    int nbStation = columns[6] != "" ? Convert.ToInt16(columns[6]) : 0;
                    int gap = columns[7] != "" ? Convert.ToInt16(columns[7]) : 0;
                    float distance = columns[8] != "" ? Convert.ToSingle(columns[8]) : 0.0f;
                    float rms = columns[9] != "" ? Convert.ToSingle(columns[9]) : 0.0f;
                    string source = columns[10];
                    string eventId = columns[11];
                    float version = columns[12] != "" ? Convert.ToSingle(columns[12]) : 0.0f;
                    _data.Add(new Earthquake(when,
                                            where,
                                            depth,
                                            magnitude,
                                            magType,
                                            nbStation,
                                            gap,
                                            distance,
                                            rms,
                                            source,
                                            eventId,
                                            version,
                                            "M " + columns[4]));

                }
                else
                {
                    readHeader = true;
                }
            }
        }

        public static void insertQuakeDataToSQL()
        {
            // Provide the following information
            string userName = "mySqlDbLogin@mra4tkv5hq";
            string password = "123Bing!";
            string dataSource = "mra4tkv5hq.database.windows.net";
            string sampleDatabaseName = "EarthquakeMap";

            // Create a connection string for the sample database
            SqlConnectionStringBuilder connString2Builder;
            connString2Builder = new SqlConnectionStringBuilder();
            connString2Builder.DataSource = dataSource;
            connString2Builder.InitialCatalog = sampleDatabaseName;
            connString2Builder.Encrypt = true;
            connString2Builder.TrustServerCertificate = false;
            connString2Builder.UserID = userName;
            connString2Builder.Password = password;
            
            // Connect to the sample database and perform various operations
            var mystring = connString2Builder.ToString();
            Console.WriteLine(mystring);
            using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
            {
                string tableName = "earthquakeData";

                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                //// Create a table
                //cmd.CommandText = "CREATE TABLE " + tableName + "(" +
                //                      "DateTime datetime primary key," +
                //                      "Position geography," +
                //                      "Depth float," +
                //                      "Magnitude float," +
                //                      "MagType varchar(20)," +
                //                      "NbStation int," +
                //                      "Gap int," +
                //                      "Distance float," +
                //                      "RMS float," +
                //                      "Source varchar(20)," +
                //                      "EventID varchar(20)," +
                //                      "Version float," +
                //                      "Title varchar(20)," +
                //                      "Description varchar(30))";
                //cmd.ExecuteNonQuery();


                // delete data from table               
                cmd.CommandText = "DELETE FROM " + tableName;
                cmd.ExecuteNonQuery();
               
                string columnsToInsert = "INSERT INTO " + tableName + "(" +
                                         "DateTime," +
                                         "Position," +
                                         "Depth," +
                                         "Magnitude," +
                                         "MagType," +
                                         "NbStation," +
                                         "Gap," +
                                         "Distance," +
                                         "RMS," +
                                         "Source," +
                                         "EventID," +
                                         "Version," +
                                         "Title)";
                // INSERT data into SQL database
                foreach (var line in _data)
                {
                    cmd = conn.CreateCommand();
                    string valuesToInsert = " VALUES (" +
                                            "@value," +
                                            "geography::Point(" + line.Location.Latitude + "," + line.Location.Longitude +
                                            ", 4326), " +
                                            line.Depth + "," +
                                            line.Magnitude + ",'" +
                                            line.MagType + "'," +
                                            line.NbStation + ", " +
                                            line.Gap + ", " +
                                            line.Distance + ", " +
                                            line.RMS + ", '" +
                                            line.Source + "', '" +
                                            line.EventID + "', " +
                                            line.Version + ", '" +
                                            line.Title + "')";
                    string commandString = columnsToInsert + valuesToInsert;
                    cmd.CommandText = commandString;
                    cmd.Parameters.AddWithValue("@value", line.When);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();       
                }

                cmd.CommandText = "SELECT * FROM " + tableName;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0}, {1}, {2}", reader["DateTime"].ToString().Trim(),
                                            reader["Position"].ToString().Trim(),
                                            reader["Magnitude"].ToString().Trim());
                    }
                }
                // View the data from the table

                conn.Close();              
            }
        }
    }
}
