using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.SqlServer.Types;

namespace WCFServiceWebRole1
{

[ServiceBehavior(
    IncludeExceptionDetailInFaults = true
  )]
    public class Service1 : IService1
    {
        public List<Earthquake> GetEarthquakeDataBBox(double TLLong, double TLLat, double BRLong, double BRLat)
        {
            string tableName = "earthquakeData";
            var selectString = "SELECT DateTime, " +
                                 "Position, " +
                                 "Magnitude, " +
                                 "Depth, " +
                                 "MagType, " +
                                 "NbStation, " +
                                 "Gap, " +
                                 "Distance, " +
                                 "RMS, " +
                                 "Source, " +
                                 "EventID, " +
                                 "Version, " +
                                 "Title FROM " + tableName;

            string bboxString = "POLYGON((" +
                           TLLong + " " + TLLat + ", " +
                           BRLong + " " + TLLat + ", " +
                           BRLong + " " + BRLat + ", " +
                           TLLong + " " + BRLat + ", " +
                           TLLong + " " + TLLat +
                            "))";
            var setGeometry = "DECLARE @g geography; SET @g=geography::STGeomFromText('" + bboxString + "', 4326); ";
            var queryFilter = " WHERE Position.Filter(@g)=1";

            var queryString = setGeometry + selectString + queryFilter;
            return GetEarthquakesFromSql(queryString);
        }

        public List<Earthquake> GetEarthquakesFromSql(String queryString)
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
            using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = queryString;

                var data = new List<Earthquake>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var when = (DateTime)reader.GetValue(0);
                        var position = SqlGeography.Deserialize(reader.GetSqlBytes(1));
                        var where = new Location((double)position.Lat, (double)position.Long);
                        var magnitude = (float) (double) reader.GetValue(2);
                        var depth = (float)(double)reader.GetValue(3);
                        var magType = (string)reader.GetValue(4);
                        var nbStation = (int)reader.GetValue(5);
                        var gap = (int)reader.GetValue(6);
                        var distance = (float)(double)reader.GetValue(7);
                        var rms = (float)(double)reader.GetValue(8);
                        var source = (string)reader.GetValue(9);
                        var eventId = (string)reader.GetValue(10);
                        var version = (float)(double)reader.GetValue(11);
                        var title = (string)reader.GetValue(12);

                        data.Add(new Earthquake(when,
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
                                                title));
                    }
                }

                conn.Close();

                return data;
            }
        }

        public List<Earthquake> GetEarthquakeData()
        {
            string tableName = "earthquakeData";
            var queryString = "SELECT DateTime, " +
                                 "Position, " +
                                 "Magnitude, " +
                                 "Depth, " +
                                 "MagType, " +
                                 "NbStation, " +
                                 "Gap, " +
                                 "Distance, " +
                                 "RMS, " +
                                 "Source, " +
                                 "EventID, " +
                                 "Version, " +
                                 "Title FROM " + tableName;
            return GetEarthquakesFromSql(queryString);
        }

    }
}
