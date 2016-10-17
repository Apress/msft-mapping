using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.Maps.MapControl.WPF;
using MvcEarthquakeMap.Models;

namespace MvcEarthquakeMap.Controllers
{
    public class EarthquakeController : Controller
    {
        //
        // GET: /Earthquake/

        public ActionResult Index()
        {
            List<Earthquake> quakes = GetLocations();
            return View(quakes);
        }

        /// <summary>
        /// Determines how empty lines are interpreted when reading CSV files.
        /// These values do not affect empty lines that occur within quoted fields
        /// or empty lines that appear at the end of the input file.
        /// Originally taken from 
        ///   http://www.blackbeltcoder.com/Articles/files/reading-and-writing-csv-files-in-c
        /// by Jonathan Wood
        /// Refactored for the book by eliminating the CSV writer, becase we're only interested
        /// in reading.
        /// </summary>
        public enum EmptyLineBehavior
        {
            /// <summary>
            /// Empty lines are interpreted as a line with zero columns.
            /// </summary>
            NoColumns,
            /// <summary>
            /// Empty lines are interpreted as a line with a single empty column.
            /// </summary>
            EmptyColumn,
            /// <summary>
            /// Empty lines are skipped over as though they did not exist.
            /// </summary>
            Ignore,
            /// <summary>
            /// An empty line is interpreted as the end of the input file.
            /// </summary>
            EndOfFile,
        }

        public class CSVFileReader : IDisposable
        {
            /// <summary>
            /// These are special characters in CSV files. If a column contains any
            /// of these characters, the entire column is wrapped in double quotes.
            /// </summary>
            protected char[] SpecialChars = new char[] { ',', '"', '\r', '\n' };

            // Indexes into SpecialChars for characters with specific meaning
            private const int DelimiterIndex = 0;
            private const int QuoteIndex = 1;

            /// <summary>
            /// Gets/sets the character used for column delimiters.
            /// </summary>
            public char Delimiter
            {
                get { return SpecialChars[DelimiterIndex]; }
                set { SpecialChars[DelimiterIndex] = value; }
            }

            /// <summary>
            /// Gets/sets the character used for column quotes.
            /// </summary>
            public char Quote
            {
                get { return SpecialChars[QuoteIndex]; }
                set { SpecialChars[QuoteIndex] = value; }
            }


            // Private members
            private StreamReader Reader;
            private string CurrLine;
            private int CurrPos;
            private EmptyLineBehavior EmptyLineBehavior;

            /// <summary>
            /// Initializes a new instance of the CsvFileReader class for the
            /// specified stream.
            /// </summary>
            /// <param name="stream">The stream to read from</param>
            /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
            public CSVFileReader(Stream stream,
                EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
            {
                Reader = new StreamReader(stream);
                EmptyLineBehavior = emptyLineBehavior;
            }

            /// <summary>
            /// Initializes a new instance of the CsvFileReader class for the
            /// specified file path.
            /// </summary>
            /// <param name="path">The name of the CSV file to read from</param>
            /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
            public CSVFileReader(string path,
                EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
            {
                Reader = new StreamReader(path);
                EmptyLineBehavior = emptyLineBehavior;
            }

            /// <summary>
            /// Reads a row of columns from the current CSV file. Returns false if no
            /// more data could be read because the end of the file was reached.
            /// </summary>
            /// <param name="columns">Collection to hold the columns read</param>
            public bool ReadRow(List<string> columns)
            {
                // Verify required argument
                if (columns == null)
                    throw new ArgumentNullException("columns");

            ReadNextLine:
                // Read next line from the file
                CurrLine = Reader.ReadLine();
                CurrPos = 0;
                // Test for end of file
                if (CurrLine == null)
                    return false;
                // Test for empty line
                if (CurrLine.Length == 0)
                {
                    switch (EmptyLineBehavior)
                    {
                        case EmptyLineBehavior.NoColumns:
                            columns.Clear();
                            return true;
                        case EmptyLineBehavior.Ignore:
                            goto ReadNextLine;
                        case EmptyLineBehavior.EndOfFile:
                            return false;
                    }
                }

                // Parse line
                string column;
                int numColumns = 0;
                while (true)
                {
                    // Read next column
                    if (CurrPos < CurrLine.Length && CurrLine[CurrPos] == Quote)
                        column = ReadQuotedColumn();
                    else
                        column = ReadUnquotedColumn();
                    // Add column to list
                    if (numColumns < columns.Count)
                        columns[numColumns] = column;
                    else
                        columns.Add(column);
                    numColumns++;
                    // Break if we reached the end of the line
                    if (CurrLine == null || CurrPos == CurrLine.Length)
                        break;
                    // Otherwise skip delimiter
                    Debug.Assert(CurrLine[CurrPos] == Delimiter);
                    CurrPos++;
                }
                // Remove any unused columns from collection
                if (numColumns < columns.Count)
                    columns.RemoveRange(numColumns, columns.Count - numColumns);
                // Indicate success
                return true;
            }

            /// <summary>
            /// Reads a quoted column by reading from the current line until a
            /// closing quote is found or the end of the file is reached. On return,
            /// the current position points to the delimiter or the end of the last
            /// line in the file. Note: CurrLine may be set to null on return.
            /// </summary>
            private string ReadQuotedColumn()
            {
                // Skip opening quote character
                Debug.Assert(CurrPos < CurrLine.Length && CurrLine[CurrPos] == Quote);
                CurrPos++;

                // Parse column
                StringBuilder builder = new StringBuilder();
                while (true)
                {
                    while (CurrPos == CurrLine.Length)
                    {
                        // End of line so attempt to read the next line
                        CurrLine = Reader.ReadLine();
                        CurrPos = 0;
                        // Done if we reached the end of the file
                        if (CurrLine == null)
                            return builder.ToString();
                        // Otherwise, treat as a multi-line field
                        builder.Append(Environment.NewLine);
                    }

                    // Test for quote character
                    if (CurrLine[CurrPos] == Quote)
                    {
                        // If two quotes, skip first and treat second as literal
                        int nextPos = (CurrPos + 1);
                        if (nextPos < CurrLine.Length && CurrLine[nextPos] == Quote)
                            CurrPos++;
                        else
                            break;  // Single quote ends quoted sequence
                    }
                    // Add current character to the column
                    builder.Append(CurrLine[CurrPos++]);
                }

                if (CurrPos < CurrLine.Length)
                {
                    // Consume closing quote
                    Debug.Assert(CurrLine[CurrPos] == Quote);
                    CurrPos++;
                    // Append any additional characters appearing before next delimiter
                    builder.Append(ReadUnquotedColumn());
                }
                // Return column value
                return builder.ToString();
            }

            /// <summary>
            /// Reads an unquoted column by reading from the current line until a
            /// delimiter is found or the end of the line is reached. On return, the
            /// current position points to the delimiter or the end of the current
            /// line.
            /// </summary>
            private string ReadUnquotedColumn()
            {
                int startPos = CurrPos;
                CurrPos = CurrLine.IndexOf(Delimiter, CurrPos);
                if (CurrPos == -1)
                    CurrPos = CurrLine.Length;
                if (CurrPos > startPos)
                    return CurrLine.Substring(startPos, CurrPos - startPos);
                return String.Empty;
            }

            // Propagate Dispose to StreamReader
            public void Dispose()
            {
                Reader.Dispose();
            }
        }

        private List<Earthquake> GetLocations()
        {
            List<Earthquake> quakeData = new List<Earthquake>();
            WebClient client = new WebClient();
            Uri quakeDataURL = new Uri("http://earthquake.usgs.gov/earthquakes/feed/v0.1/summary/2.5_day.csv");
            string quakeDataFile = "C:/Users/carmenau/SkyDrive/Book/Code/MvcEarthquakeMap/MvcEarthquakeMap/quake.csv";
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
                    quakeData.Add(new Earthquake(when,
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

            return quakeData;
        }

    }
}
